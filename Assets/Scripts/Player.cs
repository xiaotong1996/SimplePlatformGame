using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private float wallSlideSpeed = 1f;

    public StateEnv StatePlayerEnvironment { set; get; }

    public float Gravity { get => gravity; set => gravity = value; }

    public float WallSlideSpeed { get => wallSlideSpeed; set => wallSlideSpeed = value; }

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
    private float rayDistance = 0.1f;

    [SerializeField]
    private Vector2 wallClimbF;
    [SerializeField]
    private Vector2 wallOffF;
    [SerializeField]
    private Vector2 wallLeapF;

    private bool wallSliding = false;
    private bool isOnIce = false;
    private bool isOnGround;
    private bool isOnWall;
    private bool isDeath;
    private bool isWin;
    private bool isReborn;
    private bool isOnBelt;
    private int onWallDirection = 0; //1-->left  -1 -->right;
    public bool WallSliding { get => wallSliding; set => wallSliding = value; }
    public bool IsOnIce { get => isOnIce; set => isOnIce = value; }
    public bool IsOnGround { get => isOnGround; set => isOnGround = value; }
    public bool IsOnWall { get => isOnWall; set => isOnWall = value; }
    public int OnWallDirection { get => onWallDirection; set => onWallDirection = value; }
    private Transform startPoint;
    public Transform StartPoint { get => startPoint; set => startPoint = value; }
    
    public bool IsDeath { get => isDeath; set => isDeath = value; }
    public bool IsReborn { get => isReborn; set => isReborn = value; }
    public bool IsWin { get => isWin; set => isWin = value; }
    public bool IsOnBelt { get => isOnBelt; set => isOnBelt = value; }
    [SerializeField]
    private  string nextScene;
    public string NextScene { get => nextScene; set => nextScene = value; }
    [SerializeField]
    public void Idle()
    {

    }

    public void Reborn()
    {
        isDeath = false;
        rigidbody.velocity = new Vector2(0, 0);
        gameObject.transform.position = startPoint.position;
    }
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


    public void WallClimb()
    {
        rigidbody.velocity = new Vector2(-onWallDirection * wallClimbF.x, wallClimbF.y);
    }
    public void WallOff()
    {
        Debug.Log("walloff"); 
        rigidbody.velocity = new Vector2(-onWallDirection * wallOffF.x, wallOffF.y);
    }
    public void WallOff2()
    {
        Debug.Log("walloff2");
        rigidbody.velocity = new Vector2(-onWallDirection * wallOffF.x, 0);
    }

    public void WallLeap()
    {
        rigidbody.velocity = new Vector2(-onWallDirection * wallLeapF.x, wallLeapF.y);
    }




    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        StatePlayer = new IdleState(this);
        Physics2D.gravity = new Vector2(0, Gravity);
        startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;
    }
    void Update()
    {
        //Move(); 
        CheckIsOnGround();
        CheckIsOnWall();

        StopMove();


        WallSliding = false; //
        if (OnWallDirection != 0 && !IsOnGround && rigidbody.velocity.y < 0)
        {
            WallSliding = true;
            //Debug.Log(rigidbody.velocity.y);
            if (rigidbody.velocity.y < -WallSlideSpeed)
            {
                rigidbody.velocity = new Vector3(0, -WallSlideSpeed, 0);
            }
            //Debug.Log("dd" + rigidbody.velocity.y);
        }
        StatePlayer.HandleInput();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Conveyor belt")
        {
            Debug.Log("is on belt");

            IsOnBelt = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "StartPoint")
        {
            IsReborn = true;
        }
        if (collision.tag == "Ice")
        {
            IsOnIce = true;
        }

        if (collision.tag == "Nail")
        {
            IsDeath = true;
            IsReborn = false;
        }
        if(collision.tag == "PointFinal")
        {
            IsWin = true;
            Debug.Log("11111111111win1");
            SceneManager.LoadScene(NextScene);
            //Reborn();
        } 
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Conveyor belt")
        {
            IsOnBelt = false;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ice")
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
        else if (hit.collider != null)
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

    void StopMove()
    {
        if(IsOnGround && Input.GetAxis("Horizontal") == 0 && !IsOnBelt)
        {
            rigidbody.velocity = new Vector2(0, 0);
        }
    }

    
}
