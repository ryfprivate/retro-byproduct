using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputAction moveInput;
    public InputAction shootInput;

    public Rigidbody2D rb;
    public Animator animator;
    public Transform aimUI;

    public float moveSpeed = 5f;

    Vector2 moveVector;
    Vector2 aimVector;

    // Shooting
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;

    void OnEnable()
    {
        moveInput.Enable();
        shootInput.Enable();
    }

    void onDisable()
    {
        moveInput.Disable();
        shootInput.Disable();
    }

    void Update()
    {
        moveVector = moveInput.ReadValue<Vector2>();
        aimVector = shootInput.ReadValue<Vector2>();
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
