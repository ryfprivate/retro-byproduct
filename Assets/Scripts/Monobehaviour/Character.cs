using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public SpriteRenderer sprite;

    public Animator animator;
    public SpriteRenderer aimSprite;
    public Transform aimUI;

    // Shooting
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;
    public float reloadTime = 0.1f;

    private bool canShoot = true;

    private float health;

    IEnumerator Reload()
    {
        aimSprite.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(reloadTime);
        aimSprite.color = new Color(1f, 1f, 1f, 1f);
        canShoot = true;
    }

    public void OnStart()
    {
        health = 100f;
        Debug.Log("character" + health);
    }

    public void OnUpdate()
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

    public void Shoot()
    {
        if (!canShoot) return;

        animator.SetTrigger("Shoot");
        GameObject g = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        g.SetActive(true);
        canShoot = false;
        StartCoroutine(Reload());
    }
}
