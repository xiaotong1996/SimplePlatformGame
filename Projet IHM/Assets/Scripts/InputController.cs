﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>InputController</c> controls player's input from keyboard or handle.
/// </summary>
public class InputController : MonoBehaviour
{
    private Player player;

    Vector2 PlayerMove()
    {
        return new Vector2(Input.GetAxis("Horizontal"), 0);
    }

    void PlayerJump()
    {
        player.Jump();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        player.MoveDirection = PlayerMove();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerJump();
        }
    }
}
