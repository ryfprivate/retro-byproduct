using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : Character
{
    public Rigidbody2D EnemyRigidbody2D { get; private set; }

    void Awake()
    {
        base.OnAwake();
        EnemyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        base.OnStart();
    }

    void Update()
    {
        base.OnUpdate();
    }
}
