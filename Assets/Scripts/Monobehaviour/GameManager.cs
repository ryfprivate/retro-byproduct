using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameControls Controls { get; private set; }

    public GameObject playerPrefab;

    // Monsters
    public GameObject[] lairPrefabs;
    public GameObject[] lairs;

    // temp
    public GameObject simpleLairPrefab;
    private Vector3 spawnOffset = new Vector3(.5f, .5f, 0);

    void OnEnable()
    {
        Controls.Enable();
    }
    void OnDisable()
    {
        Controls.Disable();
    }

    void Awake()
    {
        Instance = this;

        Controls = new GameControls();
    }


    void Start()
    {
        // Spawns all monster lairs
        SpawnLairs();

        SpawnPlayer();
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public GameObject Spawn(GameObject obj, Vector3 pos)
    {
        return Instantiate(obj, pos, Quaternion.Euler(Vector3.zero));
    }

    private void SpawnLairs()
    {
        int numLairs = MapManager.Instance.lairLocations.Count;
        Debug.Log("num lairs " + numLairs);
        for (int i = 0; i < numLairs; i++)
        {

            Vector3 lairSpawnPosition = MapManager.Instance.lairLocations[i];
            Spawn(simpleLairPrefab, lairSpawnPosition);
        }
    }

    private void SpawnPlayer()
    {
        Debug.Log("num spawn " + MapManager.Instance.spawnLocations.Count);
        // Spawn player on random spawn location
        // Adds offset to position to spawn in middle of cell
        Vector3 playerSpawnPosition = MapManager.Instance.spawnLocations[Random.Range(0, MapManager.Instance.spawnLocations.Count)];
        Spawn(playerPrefab, playerSpawnPosition);
    }
}
