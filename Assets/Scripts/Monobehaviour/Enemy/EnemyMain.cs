using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : Character
{
    public Rigidbody2D rb;
    public Vector3 moveVector;

    void Start()
    {
        base.OnStart();
    }

    void Update()
    {
        base.OnUpdate();
        animator.SetFloat("Horizontal", moveVector.x);
        animator.SetFloat("Vertical", moveVector.y);
        animator.SetFloat("Speed", moveVector.sqrMagnitude);
    }
}
