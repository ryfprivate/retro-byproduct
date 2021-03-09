﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private GameControls controls;

    public Rigidbody2D rb;
    public Animator animator;
    public Transform aimUI;

    public float moveSpeed = 5f;

    Vector2 moveVector;
    Vector2 aimVector;

    // Shooting
    public Transform firePoint;
    public Transform bulletParent;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;
    public float reloadTime = 0.5f;

    bool isShooting;
    bool canShoot;

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }

    private void Awake()
    {
        controls = new GameControls();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void onDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        controls.Player.Shoot.performed += _ => StartShooting();
        controls.Player.Shoot.canceled += _ => StopShooting();
        controls.Player.Aim.started += _ => StartAiming();

        isShooting = false;
        canShoot = true;
        Debug.Log("Game STart");
    }

    void StartShooting()
    {
        isShooting = true;
        if (!canShoot) return;

        animator.SetTrigger("Shoot");
        GameObject g = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation, bulletParent);
        g.SetActive(true);
        canShoot = false;
        StartCoroutine(Reload());
    }

    void StartAiming()
    {
        Debug.Log("aiming");
    }

    void StopShooting()
    {
        isShooting = false;
    }

    void Update()
    {
        if (isShooting)
        {
            StartShooting();
        }
        moveVector = controls.Player.Movement.ReadValue<Vector2>();
        aimVector = controls.Player.Aim.ReadValue<Vector2>();
        // Debug.Log(aimVector);
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
}
