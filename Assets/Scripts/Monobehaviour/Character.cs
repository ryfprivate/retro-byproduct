using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private enum Type
    {
        Melee,
        Bowman
    }

    private Vector2[] directions = new Vector2[] 
        { new Vector2(0, 0), new Vector2(0.7f, -0.7f), new Vector2(1, 0), new Vector2(0.7f, 0.7f), 
        new Vector2(0, 1), new Vector2(-0.7f, 0.7f), new Vector2(-1, 0), new Vector2(-0.7f, -0.7f) };

    public GameObject meleeForm;
    public GameObject bowmanForm;

    public SpriteRenderer currentSprite;

    public Animator animator;
    public SpriteRenderer aimSprite;
    public Transform aimTransform;

    public Vector3 moveVector;
    public Vector2 aimVector;

    // Attacking
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;
    public float reloadTime = 0.5f;

    private bool canAttack = true;

    private float health;

    private Type type;

    IEnumerator Reload()
    {
        aimSprite.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(reloadTime);
        aimSprite.color = new Color(1f, 1f, 1f, 1f);
        canAttack = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject incoming = col.collider.gameObject;
        if (incoming.tag == "Damage") {
                    health -= 10f;
        }
    }

    public void OnAwake() {
        SwitchToBowman();
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
        animator.SetFloat("AimH", aimVector.x);
        animator.SetFloat("AimV", aimVector.y);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        currentSprite.color = new Color(1f, 1f, 1f, health / 100);
        // Debug.LogFormat("health {0}", health.ToString());
    }

    public void Attack()
    {
        if (!canAttack) return;

        animator.SetTrigger("Attack");

        switch (type) {
            case Type.Melee:

                Debug.Log("melee attack " + aimVector);

                break;
            case Type.Bowman:
                GameObject g = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                g.SetActive(true);
                break;
        }

        canAttack = false;
        StartCoroutine(Reload());
    }

    private void SwitchToMelee() {
        type = Type.Melee;
        bowmanForm.SetActive(false);
        meleeForm.SetActive(true);
        currentSprite = meleeForm.GetComponent<SpriteRenderer>();
        animator = meleeForm.GetComponent<Animator>();
    }

    private void SwitchToBowman() {
        type = Type.Bowman;
        bowmanForm.SetActive(true);
        meleeForm.SetActive(false);
        currentSprite = bowmanForm.GetComponent<SpriteRenderer>();
        animator = bowmanForm.GetComponent<Animator>();
    }
}
