using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 10f;

    IEnumerator DestroyBulletAfterTime()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
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
