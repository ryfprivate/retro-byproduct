using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputAction moveInput;
    public InputAction aimInput;
    public Rigidbody2D rb;
    public Animator animator;
    public Transform aimUI;

    public float moveSpeed = 5f;

    Vector2 movement;
    Vector2 aim;

    // Shooting
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;

    void OnEnable() {
        moveInput.Enable();
        aimInput.Enable();
    }

    void onDisable() {
        moveInput.Disable();
        aimInput.Disable();
    }
    
    void Update() {
        movement = moveInput.ReadValue<Vector2>();
        aim = aimInput.ReadValue<Vector2>();
        float angle = Vector3.SignedAngle(new Vector2(0, -1), aim, Vector3.forward);
        aimUI.rotation = Quaternion.Euler(0, 0, angle);

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
