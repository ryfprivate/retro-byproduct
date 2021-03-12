using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private PlayerMain playerMain;

    public float moveSpeed = 5f;
    private int killCount;

    void Awake()
    {
        Instance = this;
        playerMain = GetComponent<PlayerMain>();
        moveSpeed = 5f;
        killCount = 0;
    }

    void Update()
    {
        playerMain.moveVector = playerMain.controls.Player.Movement.ReadValue<Vector2>();
        playerMain.aimVector = playerMain.controls.Player.Aim.ReadValue<Vector2>();
        float angle = Vector3.SignedAngle(new Vector2(0, -1), playerMain.aimVector, Vector3.forward);
        playerMain.aimTransform.rotation = Quaternion.Euler(0, 0, angle);

        Upgrade();
    }

    void FixedUpdate()
    {
        // Player Movement
        Rigidbody2D rb = playerMain.PlayerRigidbody2D;
        rb.velocity = playerMain.moveVector * moveSpeed;
    }

    void Upgrade() {
        if (killCount > 2) {
            killCount = 0;
            Debug.Log("upgrade");
        }
    }

    public void IncrementKill() {
        killCount++;
    }
}
