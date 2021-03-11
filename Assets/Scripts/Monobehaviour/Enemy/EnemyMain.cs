using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Vector3 moveVector;

    void Start()
    {

    }

    void Update()
    {
        animator.SetFloat("Horizontal", moveVector.x);
        animator.SetFloat("Vertical", moveVector.y);
        animator.SetFloat("Speed", moveVector.sqrMagnitude);
    }
}
