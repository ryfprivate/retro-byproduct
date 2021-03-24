using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLair : LairController
{
    public GameObject monsterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(monsterPrefab, transform.position, Quaternion.Euler(Vector3.zero));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
