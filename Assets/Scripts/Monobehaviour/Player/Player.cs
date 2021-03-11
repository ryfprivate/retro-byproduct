using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    public static Player Instance { get; private set; }

    private PlayerMain playerMain;

    private GameControls controls;

    public float moveSpeed = 5f;

    private Vector2 moveVector;
    private Vector2 aimVector;

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
        controls = new GameControls();
        Instance = this;
        playerMain = GetComponent<PlayerMain>();
    }

    void Start()
    {
        base.OnStart();
        controls.Player.Shoot.performed += _ => Shoot();
    }

    void Update()
    {
        base.OnUpdate();
        moveVector = controls.Player.Movement.ReadValue<Vector2>();
        aimVector = controls.Player.Aim.ReadValue<Vector2>();
        float angle = Vector3.SignedAngle(new Vector2(0, -1), aimVector, Vector3.forward);
        aimUI.rotation = Quaternion.Euler(0, 0, angle);

        animator.SetFloat("Horizontal", moveVector.x);
        animator.SetFloat("Vertical", moveVector.y);
        animator.SetFloat("Speed", moveVector.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // Player Movement
        Rigidbody2D rb = playerMain.PlayerRigidbody2D;
        rb.MovePosition(rb.position + moveVector * moveSpeed * Time.fixedDeltaTime);
    }
}
