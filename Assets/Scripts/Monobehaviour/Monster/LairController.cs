using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LairController : MonoBehaviour
{
    public Vector3 spawnLocation;

    public GameObject monsterPrefab;
    public GameObject piratePrefab;
    public GameObject[] monsters;

    public int lairRadius = 12;
    private int numMonsters = 4;
    private float monsterMultiplier = 1f;
    private int numPirates = 1;
    private float pirateMultiplier = 1f;
    private int currentNum = 0;

    IEnumerator NewWave()
    {
        yield return new WaitForSeconds(3f);
        numMonsters = (int)(numMonsters - monsterMultiplier);
        monsterMultiplier *= 0.8f;
        numPirates = (int)(numPirates + pirateMultiplier);
        pirateMultiplier *= 1.3f;
        Debug.Log("monsters " + numMonsters + " pirates " + numPirates);
        SpawnMonsters();
    }

    void Awake()
    {
        lairRadius = 15;
        // monsters = new GameObject[(int)numMonsters];
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
        currentNum = numMonsters + numPirates;
        for (int i = 0; i < numMonsters; i++)
        {
            GameObject monster = Instantiate(monsterPrefab, spawnLocation, Quaternion.Euler(Vector3.zero), transform);
            monster.GetComponent<MonsterMain>().LairController = this;
            monster.GetComponent<MonsterSimpleAI>().SetRoamRadius(lairRadius);
            monster.GetComponent<MonsterSimpleAI>().SetStartingPosition(spawnLocation);
            monster.GetComponent<MonsterSimpleAI>().StartRoaming();
        }

        for (int i = 0; i < numPirates; i++)
        {
            GameObject pirate = Instantiate(piratePrefab, spawnLocation, Quaternion.Euler(Vector3.zero), transform);
            pirate.GetComponent<MonsterMain>().LairController = this;
            pirate.GetComponent<PirateAI>().SetRoamRadius(lairRadius);
            pirate.GetComponent<PirateAI>().SetStartingPosition(spawnLocation);
            pirate.GetComponent<PirateAI>().StartRoaming();
        }
    }
}
