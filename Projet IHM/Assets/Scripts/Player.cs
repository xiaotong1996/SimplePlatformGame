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

/// <summary>
/// Class <c>Player</c> models main character that player controls.
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField]
    private StatePlayer statePlayerSelf;

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

    public void Move()
    {
        transform.Translate(new Vector2(MoveDirection.x, MoveDirection.y) * MoveSpeed * Time.deltaTime);
    }

    public void Run()
    {

    }

    public void Jump()
    {

    }

    void Update()
    {
        Move();
    }
}
