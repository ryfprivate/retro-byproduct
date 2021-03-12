using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Vector2[] directions = new Vector2[]
        { new Vector2(0, 0), new Vector2(0.7f, -0.7f), new Vector2(1, 0), new Vector2(0.7f, 0.7f),
        new Vector2(0, 1), new Vector2(-0.7f, 0.7f), new Vector2(-1, 0), new Vector2(-0.7f, -0.7f) };

    public SpriteRenderer currentSprite;
    public Animator animator;

    public Vector3 moveVector;
    private float health;

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject incoming = col.collider.gameObject;
        if (incoming.tag == "Damage")
        {
            health -= 10f;
        }
    }

    public void OnStart()
    {
        health = 100f;
        // Debug.Log("character" + health);
    }

    public void OnUpdate()
    {
        animator.SetFloat("Horizontal", moveVector.x);
        animator.SetFloat("Vertical", moveVector.y);
        animator.SetFloat("Speed", moveVector.sqrMagnitude);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        currentSprite.color = new Color(1f, 1f, 1f, health / 100);
        // Debug.LogFormat("health {0}", health.ToString());
    }
}
