using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LairController : MonoBehaviour
{
    public Vector3 spawnLocation;

    public GameObject monsterPrefab;
    public GameObject[] monsters;

    public int lairRadius = 5;
    private int numMonsters = 3;

    void Awake()
    {
        monsters = new GameObject[numMonsters];
    }

    void Start()
    {
        for (int i = 0; i < numMonsters; i++)
        {
            GameObject monster = Instantiate(monsterPrefab, spawnLocation, Quaternion.Euler(Vector3.zero), transform);
        }
    }
}
