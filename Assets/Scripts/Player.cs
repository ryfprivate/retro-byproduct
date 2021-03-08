using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputAction wasd;
    public Rigidbody2D rb;
    public Animator animator;

    public float moveSpeed = 5f;

    Vector2 movement;

    void OnEnable() {
        wasd.Enable();
    }

    void onDisable() {
        wasd.Disable();
    }
    
    void Update() {
        movement = wasd.ReadValue<Vector2>();

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
