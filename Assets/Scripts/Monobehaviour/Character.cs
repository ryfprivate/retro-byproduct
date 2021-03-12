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

    public SpriteRenderer aimSprite;
    public Transform firePoint;
    public Vector2 aimVector;
    public Transform aimTransform;
    public GameObject punchPrefab;

    public Vector3 moveVector;
    public float health;

    public float reloadTime = 3f;
    public bool canAttack = true;

    public IEnumerator Reload()
    {
        aimSprite.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(reloadTime);
        aimSprite.color = new Color(1f, 1f, 1f, 1f);
        canAttack = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject incoming = col.collider.gameObject;
        if (incoming.tag == "Damage")
        {
            health -= 10f;
        }
    }

    public virtual void OnStart()
    {
        health = 100f;
        // Debug.Log("character" + health);
    }

    public virtual void OnUpdate()
    {
        animator.SetFloat("Horizontal", moveVector.x);
        animator.SetFloat("Vertical", moveVector.y);
        animator.SetFloat("Speed", moveVector.sqrMagnitude);
        animator.SetFloat("AimH", aimVector.x);
        animator.SetFloat("AimV", aimVector.y);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        currentSprite.color = new Color(1f, 1f, 1f, health / 100);
        // Debug.LogFormat("health {0}", health.ToString());
    }
}
