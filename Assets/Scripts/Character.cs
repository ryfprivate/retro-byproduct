using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public SpriteRenderer sprite;

    private float health;

    void Start()
    {
        health = 100f;
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        sprite.color = new Color(1f, 1f, 1f, health / 100);
        // Debug.LogFormat("health {0}", health.ToString());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        health -= 10f;
    }
}
