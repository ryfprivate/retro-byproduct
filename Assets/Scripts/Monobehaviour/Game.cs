using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform playerParent;
    public Vector3 spawnPosition;

    void Start()
    {
        spawnPosition = new Vector3(15, 15, 0);
        Instantiate(playerPrefab, spawnPosition, playerParent.rotation, playerParent);
    }

    void Update()
    {

    }
}
