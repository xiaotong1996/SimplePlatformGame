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
    public abstract StatePlayer StatePlayer { get; }

    public abstract void Update();

    //public abstract void HandleInput();

}

public class IdleState : PlayerBaseState
{
    //private Player player;

    public override StatePlayer StatePlayer
    {
        get
        {
            return StatePlayer.IDLE;
        }
    }

    public override void Update()
    {
        
    }

}


public class MoveState : PlayerBaseState
{
    private Player player;

    public override StatePlayer StatePlayer
    {
        get
        {
            return StatePlayer.MOVE;
        }
    }

    public override void Update()
    {
        if (player != null)
        {
            player.Move();
        }
    }
}
