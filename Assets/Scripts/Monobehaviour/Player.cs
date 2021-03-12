using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private enum Type
    {
        Melee,
        Bowman
    }

    public GameObject meleeForm;
    public GameObject bowmanForm;

    public SpriteRenderer aimSprite;
    public Transform aimTransform;

    public Vector2 aimVector;

    // Attacking
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;
    public float reloadTime = 0.5f;

    private bool canAttack = true;

    private Type type;

    IEnumerator Reload()
    {
        aimSprite.color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(reloadTime);
        aimSprite.color = new Color(1f, 1f, 1f, 1f);
        canAttack = true;
    }

    public void OnAwake()
    {
        SwitchToBowman();
    }

    public void OnStart()
    {
        base.OnStart();
    }

    public void OnUpdate()
    {
        base.OnUpdate();
        animator.SetFloat("AimH", aimVector.x);
        animator.SetFloat("AimV", aimVector.y);
    }

    public void Attack()
    {
        if (!canAttack) return;

        animator.SetTrigger("Attack");

        switch (type)
        {
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

    private void SwitchToMelee()
    {
        type = Type.Melee;
        bowmanForm.SetActive(false);
        meleeForm.SetActive(true);
        currentSprite = meleeForm.GetComponent<SpriteRenderer>();
        animator = meleeForm.GetComponent<Animator>();
    }

    private void SwitchToBowman()
    {
        type = Type.Bowman;
        bowmanForm.SetActive(true);
        meleeForm.SetActive(false);
        currentSprite = bowmanForm.GetComponent<SpriteRenderer>();
        animator = bowmanForm.GetComponent<Animator>();
    }
}
