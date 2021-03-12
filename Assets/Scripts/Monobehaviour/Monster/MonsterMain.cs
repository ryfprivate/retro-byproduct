using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMain : Character
{
    public MonsterSimpleAI MonsterSimpleAI { get; private set; }
    public Rigidbody2D MonsterRigidbody2D { get; private set; }

    public LairController LairController { get; set; }

    public float moveSpeed;

    void Awake()
    {
        MonsterSimpleAI = GetComponent<MonsterSimpleAI>();
        MonsterRigidbody2D = GetComponent<Rigidbody2D>();
        moveSpeed = 3f;
    }

    void Start()
    {
        base.OnStart();
        reloadTime = 3f;
        damage = 10f;
    }

    void Update()
    {
        base.OnUpdate();
    }
}
