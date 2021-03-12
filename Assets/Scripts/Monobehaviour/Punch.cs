using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{

    IEnumerator PunchTime()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(PunchTime());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
