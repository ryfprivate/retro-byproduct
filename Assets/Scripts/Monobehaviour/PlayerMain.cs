﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{

    public Player Player { get; private set; }
    // public PlayerSwapAimNormal PlayerSwapAimNormal { get; private set; }
    // public PlayerMovementHandler PlayerMovementHandler { get; private set; }
    // public PlayerDodgeRoll PlayerDodgeRoll { get; private set; }

    public Rigidbody2D PlayerRigidbody2D { get; private set; }

    private void Awake()
    {
        Player = GetComponent<Player>();
        // PlayerSwapAimNormal = GetComponent<PlayerSwapAimNormal>();
        // PlayerMovementHandler = GetComponent<PlayerMovementHandler>();
        // PlayerDodgeRoll = GetComponent<PlayerDodgeRoll>();

        PlayerRigidbody2D = GetComponent<Rigidbody2D>();
    }

}
