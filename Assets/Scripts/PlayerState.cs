

﻿using System.Collections;
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
        if (player.IsDeath)
        {
            player.StatePlayer = new DeathState(this.player);
        }
        //Input.GetKeyDown(KeyCode.Joystick1Button0)
        if (Input.GetAxisRaw("Jump") == 1)
        {
            //Debug.Log("get keyCode.Space");
            player.Jump();
           
            player.StatePlayer = new JumpState(this.player);
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

        if (player.IsDeath) 
        {
            player.StatePlayer = new DeathState(this.player);
        }
        //Debug.Log(Input.GetAxis("Jump"));
        //Debug.Log(Input.GetAxisRaw("Jump"));
        if (!player.IsOnGround && Input.GetAxisRaw("Jump") == 0)
        //if (!player.IsOnGround()&&(Input.GetKeyUp(KeyCode.Space)||Input.GetKeyUp(KeyCode.Joystick1Button0)))
        //if (Input.GetKeyUp(KeyCode.Space)||Input.GetKeyUp(KeyCode.Joystick1Button0))
        {

            if (player.WallSliding)
            {
                player.StatePlayer = new OnWallState(this.player);
            }
            else
            {

                player.StatePlayer = new OnAirState(this.player);
            }

        }
        if(player.IsOnGround)
        {
            player.StatePlayer = new IdleState(this.player);
        }
        //else
        //{
        //    Debug.Log("error");
        //    float moveDirectionX = Input.GetAxis("Horizontal");
        //    if (moveDirectionX != 0)
        //    {
        //        player.MoveDirection = new Vector2(moveDirectionX, 0);
        //        player.Move();
        //        player.StatePlayer = new MoveState(this.player);
        //    }
        //}

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
        if (player.IsDeath)
        {
            player.StatePlayer = new DeathState(this.player);
        }
        if (player.IsOnGround && Input.GetAxis("Horizontal") == 0)
        {
            //Debug.Log("player fall on ground");
            player.StatePlayer = new IdleState(this.player);
        }


        if (!player.IsOnGround)
        {
            //Debug.Log("player is on air");
            player.StatePlayer = new OnAirState(this.player);
        }

        if (Input.GetAxisRaw("Jump") == 1 && player.IsOnGround)
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
                if (player.IsOnIce)
                {

                    player.StatePlayer = new RunOnIceState(this.player);
                }
                else
                {
                    player.Run();
                    player.StatePlayer = new RunState(this.player);
                }
            }
            else
            {
                if (player.IsOnIce)
                {
                    player.StatePlayer = new MoveOnIceState(this.player);
                }
                else
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
        if (player.IsDeath)
        {
            player.StatePlayer = new DeathState(this.player);
        }
        if (player.IsOnGround && Input.GetAxis("Horizontal") == 0)
        {
            //Debug.Log("player fall on ground");
            player.StatePlayer = new IdleState(this.player);
        }
        if (Input.GetAxisRaw("Jump") == 1 && player.IsOnGround)
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
                if (player.IsOnIce)
                {
                    player.RunOnIce();
                    player.StatePlayer = new RunOnIceState(this.player);
                }
                else
                    player.Run();
            }
            else
            {
                if (player.IsOnIce)
                {
                    player.MoveOnIce();
                    player.StatePlayer = new MoveOnIceState(this.player);
                }
                else
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
        if (player.IsDeath)
        {
            player.StatePlayer = new DeathState(this.player);
        }
        float moveDirectionX = Input.GetAxis("Horizontal");
        if (player.IsOnGround && moveDirectionX == 0)
        {
            Debug.Log("Idle");
            player.StatePlayer = new IdleState(this.player);
        }

        else if (player.IsOnWall)
        {
            //Debug.Log("OnWall");
            player.StatePlayer = new OnWallState(this.player);
        }
        else if (moveDirectionX != 0)
        {
            player.MoveDirection = new Vector2(moveDirectionX, 0);
            player.MoveOnAir();
            player.StatePlayer = new MoveOnAirState(this.player);
        }
        else if (Input.GetAxisRaw("Jump") == 1)
        {
            //Debug.Log("get keyCode.Space");
            Debug.Log("cccccccccccccc");
            player.Jump();
            player.StatePlayer = new DoubleJumpState(this.player);
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
        if (player.IsDeath)
        {
            player.StatePlayer = new DeathState(this.player);
        }
        if (player.IsOnGround)
        {
            //Debug.Log("player fall on ground");
            player.StatePlayer = new IdleState(this.player);
        }
        else if (player.IsOnWall)
        {
            player.StatePlayer = new OnWallState(this.player);
        }
        else
        {
            float moveDirectionX = Input.GetAxis("Horizontal");
            if (moveDirectionX != 0)
            {
                player.MoveDirection = new Vector2(moveDirectionX, 0);
                player.MoveOnAir();
                //player.StatePlayer = new MoveState(this.player);
            }
            if (Input.GetAxisRaw("Jump") == 1 && isDoubleJumpOccured == false)
            {
                //Debug.Log("get keyCode.Space");
                player.Jump();
                isDoubleJumpOccured = true;
                player.StatePlayer = new DoubleJumpState(this.player);

            }
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
        if (player.IsDeath)
        {
            player.StatePlayer = new DeathState(this.player);
        }
        if (player.IsOnGround)
        {
            // Debug.Log("player fall on ground");
            player.StatePlayer = new IdleState(this.player);
        }
        else if (player.IsOnWall)
        {
            player.StatePlayer = new OnWallState(this.player);
        }
        else
        {
            float moveDirectionX = Input.GetAxis("Horizontal");
            if (moveDirectionX != 0)
            {
                player.MoveDirection = new Vector2(moveDirectionX, 0);
                player.MoveOnAir();
                //player.StatePlayer = new MoveState(this.player);
            }
        }

    }
}


public class WallClimbState : PlayerBaseState
{
    private Player player;


    public WallClimbState(Player player)
    {
        this.player = player;
        Debug.Log("Player in WallClimbState");
    }

    public override void Update()
    {

    }

    public override void HandleInput()
    {
        if (player.IsDeath)
        {
            player.StatePlayer = new DeathState(this.player);
        }
        Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        if (player.IsOnWall && Input.GetAxisRaw("Jump") == 0)
        {
            player.StatePlayer = new OnWallState(this.player);
        }
        else if (player.IsOnGround)
        {
            player.StatePlayer = new IdleState(this.player);
        }
        else if (!player.IsOnGround && !player.IsOnWall)
        {
            player.StatePlayer = new OnAirState(this.player);
        }
    }
}

public class WallOffState : PlayerBaseState
{
    private Player player;

    public WallOffState(Player player)
    {
        this.player = player;
        Debug.Log("Player in WallOffState");
    }

    public override void Update()
    {

    }

    public override void HandleInput()
    {
        if (player.IsDeath)
        {
            player.StatePlayer = new DeathState(this.player);
        }
        Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        if (player.IsOnGround)
        {
            player.StatePlayer = new IdleState(this.player);
        }
        else if (player.IsOnWall && Input.GetAxisRaw("Jump") == 0)
        {
            player.StatePlayer = new OnWallState(this.player);
        }
        else if (!player.IsOnGround && !player.IsOnWall)
        {
            player.StatePlayer = new OnAirState(this.player);
        }

    }
}

public class WallLeapState : PlayerBaseState
{
    private Player player;

    public WallLeapState(Player player)
    {
        this.player = player;
        Debug.Log("Player in WallLeapState");
    }

    public override void Update()
    {

    }

    public override void HandleInput()
    {
        if (player.IsDeath)
        {
            player.StatePlayer = new DeathState(this.player);
        }
        Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        if (player.IsOnGround)
        {
            player.StatePlayer = new IdleState(this.player);
        }
        else if (player.IsOnWall && Input.GetAxisRaw("Jump") == 0)
        {
            player.StatePlayer = new OnWallState(this.player);
        }
        //else if (Input.GetAxisRaw("Jump") == 1 && player.OnWallDirection() == -moveDirection.normalized.x)
        //{
        //    player.WallLeap();
        //    player.StatePlayer = new WallLeapState(this.player);
        //}

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
        
        if (player.IsDeath)
        {
            player.StatePlayer = new DeathState(this.player);
        }
        Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        Debug.Log(moveDirection.normalized.x);
        Debug.Log("b" + player.OnWallDirection);
        if (player.IsOnGround)
        {

            player.StatePlayer = new IdleState(this.player);
        }
        else if (Input.GetAxisRaw("Jump") == 1)
        {

            if (moveDirection.normalized.x == 0)
            {
                Debug.Log("in 1");
                player.WallOff();
                player.StatePlayer = new WallOffState(this.player);
            }
            //else if(player.OnWallDirection() == moveDirection.normalized.x)
            //{
            //    player.WallClimb();
            //    player.StatePlayer = new WallClimbState(this.player);
            //}
            else if (player.OnWallDirection == -moveDirection.normalized.x)
            {
                Debug.Log("in 2");
                player.WallLeap();
                player.StatePlayer = new WallLeapState(this.player);
            }
        }

        /////off the wall by wasd  TODECIDE
        else if (moveDirection.normalized.x != 0 && moveDirection.normalized.x == -player.OnWallDirection)
        {
            Debug.Log(moveDirection.normalized.x);
            Debug.Log("a" + player.OnWallDirection);
            Debug.Log("in 3");
                //    Debug.Log("oushhh");
                player.WallOff2();
                player.StatePlayer = new WallOffState(this.player);
            
        }


      

    }
}

public class MoveOnIceState : PlayerBaseState
{
    private Player player;

    public MoveOnIceState(Player player)
    {
        this.player = player;
        Debug.Log("Player in MoveOnIceState");
    }

    public override void HandleInput()
    {
        if (player.IsDeath)
        {
            player.StatePlayer = new DeathState(this.player);
        }
        if (player.IsOnGround && Input.GetAxis("Horizontal") == 0)
        {
            //Debug.Log("player fall on ground");
            player.StatePlayer = new IdleState(this.player);
        }


        if (!player.IsOnGround)
        {
            //Debug.Log("player is on air");
            player.StatePlayer = new OnAirState(this.player);
        }

        if (Input.GetAxisRaw("Jump") == 1 && player.IsOnGround)
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
                if (player.IsOnIce)
                {
                    player.RunOnIce();
                    player.StatePlayer = new RunOnIceState(this.player);
                }
                else
                {
                    player.Run();
                    player.StatePlayer = new RunState(this.player);
                }
            }
            else
            {
                if (!player.IsOnIce)
                {
                    player.StatePlayer = new MoveState(this.player);
                }
                else
                {

                    player.MoveOnIce();
                }

            }
        }

    }

    public override void Update()
    {

    }
}


public class RunOnIceState : PlayerBaseState
{
    private Player player;

    public RunOnIceState(Player player)
    {
        this.player = player;
        Debug.Log("Player in RunOnIceState");
    }

    public override void HandleInput()
    {

        if (player.IsDeath)
        {
            player.StatePlayer = new DeathState(this.player);
        }
        if (player.IsOnGround && Input.GetAxis("Horizontal") == 0)
        {
            //Debug.Log("player fall on ground");
            player.StatePlayer = new IdleState(this.player);
        }
        if (Input.GetAxisRaw("Jump") == 1 && player.IsOnGround)
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
                if (!player.IsOnIce)
                {
                    player.Run();
                    player.StatePlayer = new RunState(this.player);
                }
                else
                    player.RunOnIce();
            }
            else
            {
                if (!player.IsOnIce)
                    player.StatePlayer = new MoveState(this.player);
                else
                {
                    player.StatePlayer = new MoveOnAirState(this.player);
                }
            }
        }

    }

    public override void Update()
    {

    }
}



public class DeathState : PlayerBaseState
{
    private Player player;

    public DeathState(Player player)
    {
        this.player = player;
        Debug.Log("Player in Deathdtate");
    }

    public override void HandleInput()
    {
        player.Reborn();

        
        if (player.IsReborn)
        {
            player.StatePlayer = new IdleState(this.player);
        }
    }
    public override void Update()
    {

    }
}


