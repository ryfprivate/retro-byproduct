using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LairController : MonoBehaviour
{
    public Vector3 spawnLocation;

    public GameObject monsterPrefab;
    public GameObject[] monsters;

    public int lairRadius = 12;
    private int numMonsters = 3;
    private int currentNum = 0;

    IEnumerator NewWave()
    {
        yield return new WaitForSeconds(3f);
        numMonsters += 2;
        SpawnMonsters();
    }

    void Awake()
    {
        lairRadius = 12;
        monsters = new GameObject[numMonsters];
    }

    void Start()
    {
        SpawnMonsters();
    }

    public void Dead()
    {
        currentNum--;
        Debug.Log("num left " + currentNum);
        if (currentNum <= 0)
        {
            Debug.Log("all dead");
            StartCoroutine(NewWave());
        }
    }

    void SpawnMonsters()
    {
        currentNum = numMonsters;
        for (int i = 0; i < numMonsters; i++)
        {
            GameObject monster = Instantiate(monsterPrefab, spawnLocation, Quaternion.Euler(Vector3.zero), transform);
            monster.GetComponent<MonsterMain>().LairController = this;
            monster.GetComponent<MonsterSimpleAI>().SetRoamRadius(lairRadius);
            monster.GetComponent<MonsterSimpleAI>().SetStartingPosition(spawnLocation);
            monster.GetComponent<MonsterSimpleAI>().StartRoaming();
        }
    }
}
