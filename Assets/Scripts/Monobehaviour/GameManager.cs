using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameControls Controls { get; private set; }

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    // Monsters
    public GameObject[] lairPrefabs;
    public GameObject[] lairs;

    public Tilemap baseTilemap;
    public Tilemap spawnTilemap;
    public Tilemap collidableTilemap;

    public Tilemap lairTilemap;
    public Tile lairTile;

    public Tilemap pathTilemap;
    public Tile pathTile;

    // Class Instances
    private Pathfinding _pathfinding;

    // Spawning
    private List<Vector3> lairLocations = new List<Vector3>();
    private List<Vector3> spawnLocations = new List<Vector3>();
    private Vector3 spawnOffset = new Vector3(.5f, .5f, 0);

    private int mapSize = 50;

    // temp
    public GameObject simpleLairPrefab;

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
        _pathfinding = new Pathfinding(mapSize, mapSize);
    }


    void Start()
    {
        // Adds all spawn locations to a list
        // Adds all collidable tiles as obstacles in the pathfinding grid
        spawnLocations = SetUpTiles();

        // Spawns all monster lairs
        // SpawnLairs();

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

    private List<Vector3> SetUpTiles()
    {
        baseTilemap.CompressBounds();
        BoundsInt bounds = baseTilemap.cellBounds;
        Debug.Log(bounds);
        // Tilemaps
        TileBase[] spawnTiles = spawnTilemap.GetTilesBlock(bounds);
        TileBase[] collidableTiles = collidableTilemap.GetTilesBlock(bounds);
        TileBase[] lairTiles = lairTilemap.GetTilesBlock(bounds);
        // Looks at every tile in grid
        // If its a spawn or collidle tile, stores it in an array
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                TileBase spawnTile = spawnTiles[x + y * mapSize];
                TileBase collidableTile = collidableTiles[x + y * mapSize];
                TileBase lairTile = lairTiles[x + y * mapSize];

                if (spawnTile != null)
                {
                    // Debug.Log("x:" + x + " y:" + y + " tile:" + spawnTile.name);
                    spawnLocations.Add(new Vector3(x, y, 0));
                }

                if (collidableTile != null)
                {
                    _pathfinding.GetGrid().GetGridObject(x, y).isWalkable = false;
                }

                if (lairTile != null)
                {
                    lairLocations.Add(new Vector3(x, y, 0));
                }
            }
        }

        return spawnLocations;
    }

    private void SpawnLairs()
    {
        int numLairs = lairLocations.Count;
        Debug.Log("num lairs " + numLairs);
        for (int i = 0; i < numLairs; i++)
        {

            Vector3 lairSpawnPosition = lairLocations[i] + spawnOffset;
            Spawn(simpleLairPrefab, lairSpawnPosition);
        }
    }

    private void SpawnPlayer()
    {
        Debug.Log("num spawn " + spawnLocations.Count);
        // Spawn player on random spawn location
        // Adds offset to position to spawn in middle of cell
        Vector3 playerSpawnPosition = spawnLocations[Random.Range(0, spawnLocations.Count)] + spawnOffset;
        Spawn(playerPrefab, playerSpawnPosition);
    }
}
