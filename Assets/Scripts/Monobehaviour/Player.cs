using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private PlayerMain playerMain;

    private GameControls controls;

    public Rigidbody2D rb;
    public Animator animator;
    public Transform aimUI;
    public SpriteRenderer aimSprite;

    public float moveSpeed = 5f;

    Vector2 moveVector;
    Vector2 aimVector;

    // Shooting
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;
    public float reloadTime = 0.1f;

    bool isShooting;
    bool canShoot;

    IEnumerator Reload()
    {
        aimSprite.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(reloadTime);
        aimSprite.color = new Color(1f, 1f, 1f, 1f);
        canShoot = true;
    }

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
        controls.Player.Shoot.performed += _ => StartShooting();
        controls.Player.Shoot.canceled += _ => StopShooting();

        isShooting = false;
        canShoot = true;
    }

    void Update()
    {
        if (isShooting)
        {
            StartShooting();
        }
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
        rb.MovePosition(rb.position + moveVector * moveSpeed * Time.fixedDeltaTime);
    }

    private void StartShooting()
    {
        isShooting = true;
        if (!canShoot) return;

        animator.SetTrigger("Shoot");
        GameObject g = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        g.SetActive(true);
        canShoot = false;
        StartCoroutine(Reload());
    }

    private void StopShooting()
    {
        isShooting = false;
    }
}
