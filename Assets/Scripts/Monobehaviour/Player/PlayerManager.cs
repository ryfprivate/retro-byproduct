using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Character
{
    public static PlayerManager Instance { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }

    [SerializeField]
    private GameObject[] GraphicsPrefabs;
    private int graphicsIndex = 0;

    public float playerSpeed;
    public float moveSpeed;

    private int playerLevel;
    private int killCount;

    void Awake()
    {
        Instance = this;
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        playerLevel = 0;

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

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        aimVector = mousePosition - transform.position;
        float rotationZ = Mathf.Atan2(aimVector.y, aimVector.x) * Mathf.Rad2Deg + 90;
        aimTransform.rotation = Quaternion.Euler(0, 0, rotationZ);
        // aimVector = mousePosition;
        // float angle = Vector3.SignedAngle(new Vector2(0, -1), aimVector, Vector3.forward);
        // aimTransform.rotation = Quaternion.Euler(aimVector);

        Upgrade();
    }

    void FixedUpdate()
    {
        // Player Movement
        Rigidbody2D rb = Rigidbody2D;
        rb.velocity = moveVector * moveSpeed;
    }

    public void LevelUp()
    {
        print("level up");
        playerLevel++;

        if (playerLevel > 0)
        {
            // Upgrade to next
            graphicsIndex++;
            GameObject playerPrefab = GraphicsPrefabs[graphicsIndex];
            GameObject newForm = GameManager.Instance.Spawn(playerPrefab, transform.position);
            newForm.transform.parent = gameObject.transform;
            Destroy(currentGraphics);
            currentGraphics = newForm;
        }
    }

    public void Attack()
    {
        if (!canAttack) return;

        animator.SetTrigger("Attack");

        GameObject arrow = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        arrow.GetComponent<Bullet>().SetParent(gameObject);
        Physics2D.IgnoreCollision(arrow.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        arrow.SetActive(true);

        canAttack = false;
        StartCoroutine(Reload());
    }

    void Upgrade()
    {
        // if (killCount > 2)
        // {
        //     killCount = 0;
        //     health = 100f;
        //     // Upgrades:
        //     // Increase fire rate
        //     // Increase damage
        //     // Increase move speed
        //     // Increase range 
        //     int randNum = Random.Range(0, 4);
        //     Debug.Log("upgrade " + randNum);
        //     switch (randNum)
        //     {
        //         case 0:
        //             reloadTime /= 1.1f;
        //             break;
        //         case 1:
        //             damage *= 1.1f;
        //             break;
        //         case 2:
        //             moveSpeed *= 1.1f;
        //             break;
        //         case 3:
        //             if (type == Player.Type.Melee)
        //             {
        //                 SwitchToBowman();
        //             }
        //             else
        //             {
        //                 reloadTime /= 1.1f;
        //             }
        //             break;
        //     }
        // }
    }

    public void IncrementKill()
    {
        killCount++;
    }
}
