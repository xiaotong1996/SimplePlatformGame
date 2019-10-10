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
    [SerializeField]
    private StatePlayer statePlayerSelf;
    //[SerializeField]
    //private PlayerBaseState currentState;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private float jumpForce;

    public StateEnv StatePlayerEnvironment { set; get; }

    public StatePlayer StatePlayerSelf1 { get => statePlayerSelf; set => statePlayerSelf = value; }

    public float Gravity { get => gravity; set => gravity = value; }

    public Vector2 MoveDirection { set; get; }

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    public Vector2 RunDirection { set; get; }

    public float RunSpeed { get => runSpeed; set => runSpeed = value; }

    public float JumpForce { get => jumpForce; set => jumpForce = value; }

    //public PlayerBaseState CurrentState { get => currentState; set => currentState = value; }

    private new Rigidbody2D rigidbody;
    private Vector3 v;

    private bool isOnGround;




    public void Move()
    {
        transform.Translate(new Vector2(MoveDirection.x, MoveDirection.y) * MoveSpeed * Time.deltaTime);
        //rigidbody.AddForce(new Vector2(MoveDirection.x, 0) * MoveSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }

    public void Idle()
    {

    }

    public void Run()
    {
        rigidbody.AddForce(new Vector2(MoveDirection.x, MoveDirection.y) * MoveSpeed*2 * Time.deltaTime, ForceMode2D.Impulse);
    }

    public void Jump()
    {
        v = rigidbody.velocity;
        rigidbody.velocity = new Vector3(v.x, JumpForce, v.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isOnGround = true;

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isOnGround = false;
    }

    public bool IsOnGround()
    {
        return isOnGround;
    }


    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        
    }
    void Update()
    {
        //Move();   
    }
}
