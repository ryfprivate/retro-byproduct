using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Vector2[] directions = new Vector2[]
        { new Vector2(0, 0), new Vector2(0.7f, -0.7f), new Vector2(1, 0), new Vector2(0.7f, 0.7f),
        new Vector2(0, 1), new Vector2(-0.7f, 0.7f), new Vector2(-1, 0), new Vector2(-0.7f, -0.7f) };

    public GameObject currentGraphics;
    public SpriteRenderer currentSprite;
    public Animator animator;

    public SpriteRenderer aimSprite;
    public Transform firePoint;
    public Vector2 aimVector;
    public Transform aimTransform;

    public GameObject bulletPrefab;
    public GameObject punchPrefab;

    public Vector3 moveVector;
    public float health;

    public float reloadTime = 3f;
    public bool canAttack = true;

    public float damage;

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
        Debug.Log(col);
        if (incoming.tag == "Damage")
        {
            GameObject parent = incoming.GetComponent<Damage>().GetParent();
            float damageDealt = parent.GetComponent<Character>().damage;
            if (gameObject.tag == "Monster" && parent.tag == "Monster") return;
            // Debug.Log("damage: " + damageDealt + " from " + parent);
            health -= damageDealt;
            if (health <= 0)
            {
                if (gameObject.tag == "Monster") {
                    GetComponent<MonsterMain>().LairController.Dead();
                }

                if (gameObject.tag == "Player") {
                    GameManager.Instance.Restart();
                }

                if (parent.tag == "Player") {
                    PlayerManager.Instance.IncrementKill();
                }
                Destroy(gameObject);
            }
        }
    }

    public virtual void OnStart()
    {
        health = 100f;
        // Debug.Log("character" + health);
    }

    public virtual void OnUpdate()
    {
        currentGraphics.GetComponent<Animator>().SetFloat("Horizontal", moveVector.x);
        currentGraphics.GetComponent<Animator>().SetFloat("Vertical", moveVector.y);
        currentGraphics.GetComponent<Animator>().SetFloat("Speed", moveVector.sqrMagnitude);
        currentGraphics.GetComponent<Animator>().SetFloat("AimH", aimVector.x);
        currentGraphics.GetComponent<Animator>().SetFloat("AimV", aimVector.y);

        currentGraphics.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, health / 100);
        // Debug.LogFormat("health {0}", health.ToString());
    }
}
