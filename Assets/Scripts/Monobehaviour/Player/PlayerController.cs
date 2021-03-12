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
            playerMain.health = 100f;
            // Upgrades:
                // Increase fire rate
                // Increase damage
                // Increase move speed
                // Increase range 
            int randNum = Random.Range(0, 4);
            Debug.Log("upgrade " + randNum);
            switch (randNum) {
                case 0:
                    playerMain.reloadTime /= 1.1f;
                    break;
                case 1:
                    playerMain.damage *= 1.1f;
                    break;
                case 2:
                    moveSpeed *= 1.1f;
                    break;
                case 3:
                    if (playerMain.type == Player.Type.Melee) {
                        playerMain.SwitchToBowman();
                    } else {
                        playerMain.reloadTime /= 1.1f;
                    }
                    break;
            }
        }
    }

    public void IncrementKill() {
        killCount++;
    }
}
