using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum <c>StatePlayer</c> represents player's state.
/// </summary>
public enum StatePlayer
{
    IDLE,
    DIE,
    MOVE,
    RUN,
    JUMP,
    JUMPDOUBLE,
    JUMPWALL
}


public abstract class PlayerBaseState
{
    //public abstract StatePlayer StatePlayer { get; }

    public abstract void Update();

    public abstract void HandleInput();

}

public class IdleState : PlayerBaseState
{
    private Player player;

    public IdleState(Player player)
    {
        this.player = player;
        Debug.Log("Player in IdleState");
    }


    public override void Update()
    {
        
    }

    public override void HandleInput()
    {
        //Input.GetKeyDown(KeyCode.Joystick1Button0)
        if (Input.GetAxisRaw("Jump")==1)
        {
            Debug.Log("get keyCode.Space");
            player.Jump();
            player.StatePlayer=new JumpState(this.player);
        }
        float moveDirectionX = Input.GetAxis("Horizontal");
        if (moveDirectionX != 0)
        {
            player.MoveDirection = new Vector2(moveDirectionX, 0);
            player.Move();
            player.StatePlayer = new MoveState(this.player);
        }
    }

}


public class JumpState : PlayerBaseState
{
    private Player player;

    public JumpState(Player player)
    {
        this.player = player;
        Debug.Log("Player in JumpState");
    }


    public override void Update()
    {
        
    }

    public override void HandleInput()
    {
        
        if (!player.IsOnGround()&& Input.GetAxisRaw("Jump") == 0)
        {
            //Input.GetAxisRaw("Jump") == 1
            //Debug.Log("player is on air");
            player.StatePlayer = new OnAirState(this.player);
        }
        float moveDirectionX = Input.GetAxis("Horizontal");
        if (moveDirectionX != 0)
        {
            player.MoveDirection = new Vector2(moveDirectionX, 0);
            player.Move();
            player.StatePlayer = new MoveState(this.player);
        }
    }
}


public class MoveState : PlayerBaseState
{
    private Player player;

    public MoveState(Player player)
    {
        this.player = player;
        Debug.Log("Player in MoveState");
    }


    public override void Update()
    {
        
    }

    public override void HandleInput()
    {
        if (player.IsOnGround()&& Input.GetAxis("Horizontal")==0)
        {
            //Debug.Log("player fall on ground");
            player.StatePlayer = new IdleState(this.player);
        }
        if (!player.IsOnGround())
        {
            //Debug.Log("player is on air");
            player.StatePlayer = new OnAirState(this.player);
        }
       
        if (Input.GetAxisRaw("Jump")==1&&player.IsOnGround())
        {
            //Debug.Log("get keyCode.Space");
            player.Jump();
            player.StatePlayer = new JumpState(this.player);
        }
        float moveDirectionX = Input.GetAxis("Horizontal");
        if (moveDirectionX != 0)
        {
            player.MoveDirection = new Vector2(moveDirectionX, 0);
            
            //player.StatePlayer = new MoveState(this.player);
            if (Input.GetAxis("Run") > 0)
            {
                player.Run();
                player.StatePlayer = new RunState(this.player);
            }
            else
            {
                player.Move();
            }
        }
    }
}

public class RunState : PlayerBaseState
{
    private Player player;

    public RunState(Player player)
    {
        this.player = player;
        Debug.Log("Player in RunState");
    }


    public override void Update()
    {

    }

    public override void HandleInput()
    {
        if (player.IsOnGround() && Input.GetAxis("Horizontal")==0)
        {
            Debug.Log("player fall on ground");
            player.StatePlayer = new IdleState(this.player);
        }
        if (Input.GetAxisRaw("Jump")==1&&player.IsOnGround())
        {
            //Debug.Log("get keyCode.Space");
            player.Jump();
            player.StatePlayer = new JumpState(this.player);
        }
        float moveDirectionX = Input.GetAxis("Horizontal");
        if (moveDirectionX != 0)
        {
            player.MoveDirection = new Vector2(moveDirectionX, 0);

            //player.StatePlayer = new MoveState(this.player);
            if (Input.GetAxis("Run") > 0)
            {
                player.Run();
            }
            else
            {
                player.StatePlayer = new MoveState(this.player);
            }
        }
    }
}

public class OnAirState : PlayerBaseState
{
    private Player player;

    public OnAirState(Player player)
    {
        this.player = player;
        Debug.Log("Player in OnAirState");
    }


    public override void Update()
    {

    }

    public override void HandleInput()
    {
        float moveDirectionX = Input.GetAxis("Horizontal");
        if (moveDirectionX != 0)
        {
            player.MoveDirection = new Vector2(moveDirectionX, 0);
            player.MoveOnAir();
            player.StatePlayer = new MoveOnAirState(this.player);
        }
        if (player.IsOnGround() && moveDirectionX == 0)
        {
            Debug.Log("player fall on ground");
            player.StatePlayer = new IdleState(this.player);
        }
        if (Input.GetAxisRaw("Jump")==1)
        {
            Debug.Log("get keyCode.Space");
            player.Jump();
            player.StatePlayer = new DoubleJumpState(this.player);
        }
        if (player.IsOnWall())
        {
            player.StatePlayer = new OnWallState(this.player);
        }
    }
}

public class MoveOnAirState : PlayerBaseState
{
    private Player player;

    private bool isDoubleJumpOccured = false;

    public MoveOnAirState(Player player)
    {
        this.player = player;
        Debug.Log("Player in MoveOnAirState");
    }


    public override void Update()
    {

    }

    public override void HandleInput()
    {
        if (player.IsOnGround())
        {
            Debug.Log("player fall on ground");
            player.StatePlayer = new IdleState(this.player);
        }
        float moveDirectionX = Input.GetAxis("Horizontal");
        if (moveDirectionX != 0)
        {
            player.MoveDirection = new Vector2(moveDirectionX, 0);
            player.MoveOnAir();
            //player.StatePlayer = new MoveState(this.player);
        }
        if (Input.GetAxisRaw("Jump")==1&&isDoubleJumpOccured==false)
        {
            Debug.Log("get keyCode.Space");
            player.Jump();
            isDoubleJumpOccured = true;
            player.StatePlayer = new DoubleJumpState(this.player);

        }
        if (player.IsOnWall())
        {
            player.StatePlayer = new OnWallState(this.player);
        }
    }
}

public class DoubleJumpState : PlayerBaseState
{
    private Player player;

    public DoubleJumpState(Player player)
    {
        this.player = player;
        Debug.Log("Player in DoubleJumpState");
    }


    public override void Update()
    {

    }

    public override void HandleInput()
    {
        if (player.IsOnGround())
        {
            Debug.Log("player fall on ground");
            player.StatePlayer = new IdleState(this.player);
        }
        float moveDirectionX = Input.GetAxis("Horizontal");
        if (moveDirectionX != 0)
        {
            player.MoveDirection = new Vector2(moveDirectionX, 0);
            player.MoveOnAir();
            //player.StatePlayer = new MoveState(this.player);
        }
        
    }
}

public class OnWallState : PlayerBaseState
{
    private Player player;

    public OnWallState(Player player)
    {
        this.player = player;
        Debug.Log("Player in OnWallState");
    }


    public override void Update()
    {

    }

    public override void HandleInput()
    {
        int onWallDirection = player.OnWallDirection();
        if (player.IsOnGround())
        {
            Debug.Log("player fall on ground");
            player.StatePlayer = new IdleState(this.player);
        }
         float moveDirectionX = Input.GetAxis("Horizontal");
       

            if (Input.GetAxisRaw("Jump")==1 && (onWallDirection == -1 && moveDirectionX > 0 || (onWallDirection == 1) && moveDirectionX < 0))
        {
              
                player.Jump();
                player.StatePlayer = new JumpState(this.player);
        }
            

            if(moveDirectionX != 0)
        {
            player.MoveDirection = new Vector2(moveDirectionX, 0);
            player.MoveOnAir();
        }
           
        if(onWallDirection == 0)
        {
            player.StatePlayer = new MoveOnAirState(this.player);
        }

        


    }
}




