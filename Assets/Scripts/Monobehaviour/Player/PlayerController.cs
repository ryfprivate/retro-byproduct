using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private PlayerMain playerMain;

    public float moveSpeed = 5f;

    void Awake()
    {
        Instance = this;
        playerMain = GetComponent<PlayerMain>();
    }

    void Update()
    {
        playerMain.moveVector = playerMain.controls.Player.Movement.ReadValue<Vector2>();
        playerMain.aimVector = playerMain.controls.Player.Aim.ReadValue<Vector2>();
        float angle = Vector3.SignedAngle(new Vector2(0, -1), playerMain.aimVector, Vector3.forward);
        playerMain.aimTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FixedUpdate()
    {
        // Player Movement
        Rigidbody2D rb = playerMain.PlayerRigidbody2D;
        rb.velocity = playerMain.moveVector * moveSpeed;
    }
}
