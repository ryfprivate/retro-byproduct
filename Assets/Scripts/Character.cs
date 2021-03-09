using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private float health;

    void Start()
    {
        health = 100f;
    }

    void Update()
    {
        // Debug.LogFormat("health {0}", health.ToString());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("ouch");
    }
}
