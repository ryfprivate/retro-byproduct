using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Damage
{
    float speed = 15f;

    IEnumerator DestroyBulletAfterTime()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(DestroyBulletAfterTime());
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
