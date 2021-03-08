using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputAction wasd;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;

    void OnEnable() {
        wasd.Enable();
    }

    void onDisable() {
        wasd.Disable();
    }
    
    void Update() {
        movement = wasd.ReadValue<Vector2>();
        Debug.Log(movement);
    }

    void FixedUpdate()
    {
        // Movement
        Debug.Log(movement);
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
