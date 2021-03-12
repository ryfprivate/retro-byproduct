using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMain : MonoBehaviour
{
    public MonsterSimpleAI MonsterSimpleAI { get; private set; }
    public Rigidbody2D MonsterRigidbody2D { get; private set; }

    void Awake()
    {
        MonsterSimpleAI = GetComponent<MonsterSimpleAI>();
        MonsterRigidbody2D = GetComponent<Rigidbody2D>();
    }
}
