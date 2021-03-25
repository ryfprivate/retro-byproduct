using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Player
{
    public static PlayerManager Instance { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }

    public GameObject crosshair;

    public float playerSpeed;
    public float moveSpeed;
    private int killCount;

    void Awake()
    {
        base.OnAwake();
        Instance = this;
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        base.OnStart();
        GameManager.Instance.Controls.Player.Attack.performed += _ => Attack();
        playerSpeed = 5f;
        moveSpeed = playerSpeed;
        killCount = 0;
        reloadTime = .5f;
        damage = 30f;
    }

    void Update()
    {
        base.OnUpdate();
        moveVector = GameManager.Instance.Controls.Player.Movement.ReadValue<Vector2>();
        aimVector = GameManager.Instance.Controls.Player.Aim.ReadValue<Vector2>();
        float angle = Vector3.SignedAngle(new Vector2(0, -1), aimVector, Vector3.forward);
        aimTransform.rotation = Quaternion.Euler(0, 0, angle);

        Upgrade();
    }

    void FixedUpdate()
    {
        // Player Movement
        Rigidbody2D rb = Rigidbody2D;
        rb.velocity = moveVector * moveSpeed;
    }

    void Upgrade()
    {
        if (killCount > 2)
        {
            killCount = 0;
            health = 100f;
            // Upgrades:
            // Increase fire rate
            // Increase damage
            // Increase move speed
            // Increase range 
            int randNum = Random.Range(0, 4);
            Debug.Log("upgrade " + randNum);
            switch (randNum)
            {
                case 0:
                    reloadTime /= 1.1f;
                    break;
                case 1:
                    damage *= 1.1f;
                    break;
                case 2:
                    moveSpeed *= 1.1f;
                    break;
                case 3:
                    if (type == Player.Type.Melee)
                    {
                        SwitchToBowman();
                    }
                    else
                    {
                        reloadTime /= 1.1f;
                    }
                    break;
            }
        }
    }

    public void IncrementKill()
    {
        killCount++;
    }
}
