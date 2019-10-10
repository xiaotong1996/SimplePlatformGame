using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum <c>StateEnv</c> represents player is in which environment.
/// </summary>
public enum StateEnv
{
    ONGROUND,
    ONWALL,
    UNDERWATER,
    ONWATER,
    ONICE
}


/// <summary>
/// Class <c>Player</c> models main character that player controls.
/// </summary>
public class Player : MonoBehaviour
{
    //[SerializeField]
    //private StatePlayer statePlayer;
    [SerializeField]
    private PlayerBaseState statePlayer;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private float jumpForce;

    public StateEnv StatePlayerEnvironment { set; get; }

    public float Gravity { get => gravity; set => gravity = value; }

    public Vector2 MoveDirection { set; get; }

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    public Vector2 RunDirection { set; get; }

    public float RunSpeed { get => runSpeed; set => runSpeed = value; }

    public float JumpForce { get => jumpForce; set => jumpForce = value; }

    public PlayerBaseState StatePlayer { get => statePlayer; set => statePlayer = value; }

    //public PlayerBaseState CurrentState { get => currentState; set => currentState = value; }

    private new Rigidbody2D rigidbody;
    private Vector3 v;

    private bool isOnGround;




    public void Move()
    {
        //transform.Translate(new Vector2(MoveDirection.x, MoveDirection.y) * MoveSpeed * Time.deltaTime);
        //rigidbody.AddForce(new Vector2(MoveDirection.x, 0) * MoveSpeed , ForceMode2D.Impulse);
        v = rigidbody.velocity;
        rigidbody.velocity = new Vector3(MoveDirection.x * MoveSpeed, v.y, v.z);
    }

    public void MoveOnAir()
    {
        //transform.Translate(new Vector2(MoveDirection.x, MoveDirection.y) * MoveSpeed * Time.deltaTime);
        //rigidbody.AddForce(new Vector2(MoveDirection.x, 0) * MoveSpeed , ForceMode2D.Impulse);
        v = rigidbody.velocity;
        rigidbody.velocity = new Vector3(MoveDirection.x * MoveSpeed, v.y, v.z);
    }

    public void Idle()
    {

    }

    public void Run()
    {
        v = rigidbody.velocity;
        rigidbody.velocity = new Vector3(MoveDirection.x * RunSpeed, v.y, v.z);
    }

    public void Jump()
    {
        v = rigidbody.velocity;
        rigidbody.velocity = new Vector3(v.x, JumpForce, v.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isOnGround = true;
            Debug.Log("collision ENter");
        }
            

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isOnGround = false;
            Debug.Log("collision Exit");
        }
            
    }

    public bool IsOnGround()
    {
        return isOnGround;
    }

    public bool IsIdle()
    {
        return (rigidbody.velocity.x == 0)&& (rigidbody.velocity.y == 0) ? true : false;
    }


    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        StatePlayer = new IdleState(this);
        
    }
    void Update()
    {
        //Move(); 
        StatePlayer.HandleInput();
    }
}
