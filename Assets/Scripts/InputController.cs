using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>InputController</c> controls player's input from keyboard or handle.
/// </summary>
public class InputController : MonoBehaviour
{
    private Player player;
    private FSM fsm;

   

    //Vector2 PlayerMove()
    //{
    //    return new Vector2(Input.GetAxis("Horizontal"), 0);
    //}

    void PlayerJump()
    {
        player.Jump();
    }

    private void Awake()
    {
        player = GetComponent<Player>();

        fsm = new FSM();

        FSM.FSMState idleState = new FSM.FSMState("idle");
        FSM.FSMState moveState = new FSM.FSMState("move");
        FSM.FSMState jumpState = new FSM.FSMState("jump");
        FSM.FSMState runState = new FSM.FSMState("run");
        //FSM.FSMState onAirState = new FSM.FSMState("onAir");
        //FSM.FSMState moveOnAirState = new FSM.FSMState("moveOnAir");
        //FSM.FSMState doubleJumpState = new FSM.FSMState("doubleJump");

        FSM.FSMTranslation idleToMove = new FSM.FSMTranslation(idleState, "idleToMove", moveState, player.Move);
        FSM.FSMTranslation moveToRun = new FSM.FSMTranslation(moveState, "moveToRun", runState, player.Run);
        FSM.FSMTranslation runToMove = new FSM.FSMTranslation(runState, "runToMove", moveState, player.Move);
        FSM.FSMTranslation moveToIdle = new FSM.FSMTranslation(moveState, "moveToIdle", idleState, player.Idle);
        FSM.FSMTranslation runToIdle = new FSM.FSMTranslation(runState, "runToIdle", idleState, player.Idle);
        FSM.FSMTranslation idleToJump = new FSM.FSMTranslation(idleState, "idleToJump", jumpState, player.Jump);
        FSM.FSMTranslation moveToJump = new FSM.FSMTranslation(moveState, "moveToJump", jumpState, player.Jump);
        FSM.FSMTranslation runToJump = new FSM.FSMTranslation(runState, "runToJump", jumpState, player.Jump);
        FSM.FSMTranslation jumpToIdle = new FSM.FSMTranslation(jumpState, "jumpToIdle", idleState, player.Idle);

        fsm.AddState(idleState);
        fsm.AddState(moveState);
        fsm.AddState(runState);
        fsm.AddState(jumpState);

        fsm.AddTransaltion(idleToMove);
        fsm.AddTransaltion(moveToIdle);
        fsm.AddTransaltion(moveToRun);
        fsm.AddTransaltion(runToIdle);
        fsm.AddTransaltion(runToMove);
        fsm.AddTransaltion(idleToJump);
        fsm.AddTransaltion(moveToJump);
        fsm.AddTransaltion(runToJump);
        fsm.AddTransaltion(jumpToIdle);

        fsm.Start(idleState);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(fsm.currentState.name);
        //player.MoveDirection = PlayerMove();
        float moveDirectionX = Input.GetAxis("Horizontal");
        if (moveDirectionX != 0)
        {
            player.MoveDirection = new Vector2(moveDirectionX, 0);
            fsm.HandleEvent("idleToMove");
        }
        else
        {
            fsm.HandleEvent("moveToIdle");
            fsm.HandleEvent("runToIdle");
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            fsm.HandleEvent("moveToRun");
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            fsm.HandleEvent("runToMove");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            fsm.HandleEvent("idleToJump");
            fsm.HandleEvent("moveToJump");
            fsm.HandleEvent("runToJump");
        }

        if(player.IsOnGround())
        {
            fsm.HandleEvent("jumpToIdle");
        }
    }
}
