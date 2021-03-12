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

    // Attacking
    public GameObject bulletPrefab;

    private Type type;

    public void OnAwake()
    {
        SwitchToMelee();
    }

    public override void OnStart()
    {
        base.OnStart();
        reloadTime = 1f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public void Attack()
    {
        if (!canAttack) return;

        animator.SetTrigger("Attack");

        switch (type)
        {
            case Type.Melee:
                GameObject punch = Instantiate(punchPrefab, firePoint.position, firePoint.rotation);
                Physics2D.IgnoreCollision(punch.GetComponent<Collider2D>(), GetComponent<Collider2D>());

                break;
            case Type.Bowman:
                GameObject arrow = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Physics2D.IgnoreCollision(arrow.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                arrow.SetActive(true);
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
