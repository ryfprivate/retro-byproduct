using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : Player
{

    public PlayerController PlayerController { get; private set; }
    // public PlayerSwapAimNormal PlayerSwapAimNormal { get; private set; }
    // public PlayerMovementHandler PlayerMovementHandler { get; private set; }
    // public PlayerDodgeRoll PlayerDodgeRoll { get; private set; }

    public Rigidbody2D PlayerRigidbody2D { get; private set; }

    public GameControls controls;

    void OnEnable()
    {
        controls.Enable();
    }

    void onDisable()
    {
        controls.Disable();
    }

    void Awake()
    {
        base.OnAwake();
        controls = new GameControls();
        PlayerController = GetComponent<PlayerController>();
        // PlayerSwapAimNormal = GetComponent<PlayerSwapAimNormal>();
        // PlayerMovementHandler = GetComponent<PlayerMovementHandler>();
        // PlayerDodgeRoll = GetComponent<PlayerDodgeRoll>();

        PlayerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start() {
        base.OnStart();
        controls.Player.Attack.performed += _ => Attack();
        reloadTime = .5f;
        damage = 30f;
    }

    void Update() {
        base.OnUpdate();
    }

}
