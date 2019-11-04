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
    private float moveOnAirSpeed;

    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private float moveOnIceSpeed;

    [SerializeField]
    private float runOnIceSpeed;

    [SerializeField]
    private float jumpForce;

    public StateEnv StatePlayerEnvironment { set; get; }

    public float Gravity { get => gravity; set => gravity = value; }

    public Vector2 MoveDirection { set; get; }

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float MoveOnAirSpeed { get => moveOnAirSpeed; set => moveOnAirSpeed = value; }

    public Vector2 RunDirection { set; get; }

    public float RunSpeed { get => runSpeed; set => runSpeed = value; }

    public float MoveOnIceSpeed { get => moveOnIceSpeed; set => moveOnIceSpeed = value; }

    public float RunOnIceSpeed { get => runOnIceSpeed; set => runOnIceSpeed = value; }

    public float JumpForce { get => jumpForce; set => jumpForce = value; }

    public PlayerBaseState StatePlayer { get => statePlayer; set => statePlayer = value; }

    //public PlayerBaseState CurrentState { get => currentState; set => currentState = value; }

    private new Rigidbody2D rigidbody;
    private Vector3 v;
    public float wallSlideSpeed = 1f;
    
    private bool isOnGround;
    private bool isDeath;
    public bool IsDeath { get => isDeath; set => isDeath = value; }
    private bool isOnWall;
    private int onWallDirection = 0; //1-->left  -1 -->right;
   
    private float rayDistance = 0.1f;

    public Vector2 wallClimbF;
    public Vector2 wallOffF;
    public Vector2 wallLeapF;
    private bool wallSliding = false;
    private Transform startPoint;
    public Transform StartPoint { get => startPoint; set => startPoint = value; }
    private bool isOnIce = false;

    public void Move()
    {
        //transform.Translate(new Vector2(MoveDirection.x, MoveDirection.y) * MoveSpeed * Time.deltaTime);
        //rigidbody.AddForce(new Vector2(MoveDirection.x, 0) * MoveSpeed , ForceMode2D.Impulse);
        v = rigidbody.velocity;
        rigidbody.velocity = new Vector3(MoveDirection.normalized.x * MoveSpeed, v.y, v.z);
    }

    public void MoveOnAir()
    {
        //transform.Translate(new Vector2(MoveDirection.x, MoveDirection.y) * MoveSpeed * Time.deltaTime);
        //rigidbody.AddForce(new Vector2(MoveDirection.x, 0) * MoveSpeed , ForceMode2D.Impulse);
        v = rigidbody.velocity;
        rigidbody.velocity = new Vector3(MoveDirection.normalized.x * MoveOnAirSpeed, v.y, v.z);
    }

    public void MoveOnIce()
    {
        v = rigidbody.velocity;
        rigidbody.velocity = new Vector3(MoveDirection.normalized.x * MoveOnIceSpeed, v.y, v.z);
    }

    public void RunOnIce()
    {
        v = rigidbody.velocity;
        rigidbody.velocity = new Vector3(MoveDirection.normalized.x * RunOnIceSpeed, v.y, v.z);
    }

    public void Idle()
    {

    }


    public void Run()
    {
        v = rigidbody.velocity;
        rigidbody.velocity = new Vector3(MoveDirection.normalized.x * MoveOnIceSpeed, v.y, v.z);
    }

    public void Jump()
    {
        v = rigidbody.velocity;
        rigidbody.velocity = new Vector3(v.x, JumpForce, v.z);

    }
    public void Reborn()
    {
        isDeath = false;
        gameObject.transform.position = startPoint.position;
    }
    public void WallJump()
    {
        v = rigidbody.velocity;
        float moveDirectionX = Input.GetAxis("Horizontal");
        MoveDirection = new Vector2(moveDirectionX, 0);
        MoveDirection.Normalize();
        if (MoveDirection.x > 0)
            rigidbody.velocity = new Vector3(JumpForce, JumpForce, v.z);
        else if(MoveDirection.x <0)
            rigidbody.velocity = new Vector3(-JumpForce, JumpForce, v.z);
        //else if(MoveDirection.x == 0)
        //{
        //    rigidbody.velocity = new Vector3(-onWallDirection*JumpForce, JumpForce, v.z);
        //}
    }

    public void WallClimb()
    {
        rigidbody.velocity = new Vector2(-onWallDirection*wallClimbF.x, wallClimbF.y);
    }
    public void WallOff()
    {
        rigidbody.velocity = new Vector2(-onWallDirection * wallOffF.x, wallOffF.y);
    }

    public void WallLeap()
    {
        rigidbody.velocity = new Vector2(-onWallDirection * wallLeapF.x, wallLeapF.y);
    }

    public bool IsOnGround()
    {
        return isOnGround;
    }
    public bool IsOnWall()
    {
        return isOnWall;
    }
    public int OnWallDirection()
    {
        return onWallDirection;
    }

    public bool WallSlide()
    {
        return wallSliding;
    }

    public bool IsOnIce()
    {
        return isOnIce;
    }
    //public bool IsIdle()
    //{
    //    Debug.Log(rigidbody.velocity.x);
    //    Debug.Log(rigidbody.velocity.y);

    //    return (rigidbody.velocity.x == 0)&& (rigidbody.velocity.y == 0) ? true : false;
    //}


    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        StatePlayer = new IdleState(this);
        startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;
        Physics2D.gravity = new Vector2(0, Gravity);

    }
    void Update()
    {
        //Move(); 
        CheckIsOnGround();
        CheckIsOnWall();


        
        wallSliding = false; //
        if (OnWallDirection() != 0 && !IsOnGround() && rigidbody.velocity.y < 0)
        {
            wallSliding = true;
            //Debug.Log(rigidbody.velocity.y);
            if (rigidbody.velocity.y < -wallSlideSpeed)
            {
                rigidbody.velocity = new Vector3(0, -wallSlideSpeed, 0);
            }
            //Debug.Log("dd" + rigidbody.velocity.y);
        }
        StatePlayer.HandleInput();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ice")
        {
            isOnIce = true;
        }
        if(collision.tag == "Nail")
        {
            IsDeath = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ice")
        {
            isOnIce = false;
        }
    }
    void CheckIsOnGround()
    {
        Debug.DrawRay(transform.position, Vector2.down * rayDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, 1 << LayerMask.NameToLayer("Ground"));
        if (hit.collider != null)
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;

        }
        //Debug.Log("isOnGround: " + isOnGround);
    }

    void CheckIsOnWall()
    {
        Debug.DrawRay(transform.position, Vector2.left * rayDistance, Color.green);
        Debug.DrawRay(transform.position, Vector2.right * rayDistance, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, rayDistance, 1 << LayerMask.NameToLayer("Wall"));
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.right, rayDistance, 1 << LayerMask.NameToLayer("Wall"));
        if (hit.collider == null && hit2.collider == null)
        {
            onWallDirection = 0;
            isOnWall = false;
        }
        else if(hit.collider != null)
        {
            onWallDirection = -1;
            isOnWall = true;

        }
        else if (hit2.collider != null)
        {
            onWallDirection = 1;
            isOnWall = true;

        }
        
    }
}
