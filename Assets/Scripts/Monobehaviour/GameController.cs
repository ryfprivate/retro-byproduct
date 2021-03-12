using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

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
    private GameControls controls;
    private Pathfinding _pathfinding;

    private int numEnemies;

    private int mapSize = 200;

    void OnEnable()
    {
        controls.Enable();
        controls.Player.LeftClick.performed += ctx => HandleLeftClick(ctx);
        // controls.Player.RightClick.performed += ctx => HandleRightClick(ctx);
    }
    void OnDisable()
    {
        controls.Disable();
        controls.Player.LeftClick.performed -= ctx => HandleLeftClick(ctx);
        // controls.Player.RightClick.performed -= ctx => HandleRightClick(ctx);
    }

    void Awake()
    {
        Instance = this;

        controls = new GameControls();
        _pathfinding = new Pathfinding(mapSize, mapSize);
    }


    void Start()
    {
        numEnemies = 4;
        // Adds all spawn locations to a list
        // Adds all collidable tiles as obstacles in the pathfinding grid
        List<Vector3> spawnLocations = SetUpTiles();

        // Spawns all monster lairs
        SpawnLairs();

        // Spawn player on random spawn location
        Vector3 offset = new Vector3(.5f, .5f, 0);
        // Adds offset to position to spawn in middle of cell
        Vector3 playerSpawnPosition = spawnLocations[Random.Range(0, spawnLocations.Count)] + offset;
        Instantiate(playerPrefab, playerSpawnPosition, Quaternion.Euler(Vector3.zero));

        // Spawns enemies on a random spawn location
        // for (int i = 0; i < numEnemies; i++)
        // {
        //     int randomIdx = Random.Range(0, spawnLocations.Count);
        //     Vector3 enemySpawnPosition = spawnLocations[randomIdx] + offset;
        //     spawnLocations.Remove(enemySpawnPosition - offset);
        //     Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.Euler(Vector3.zero));
        // }
    }

    private void HandleLeftClick(InputAction.CallbackContext ctx)
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        _pathfinding.GetGrid().GetXY(worldPosition, out int x, out int y);
        List<PathNode> path = _pathfinding.FindPath(0, 0, x, y);
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(new Vector3(path[i].x, path[i].y) + Vector3.one * .5f, new Vector3(path[i + 1].x, path[i + 1].y) + Vector3.one * .5f, Color.green, .5f);
            }
        }
    }

    // private void HandleRightClick(InputAction.CallbackContext ctx)
    // {
    //     Vector2 screenPosition = Mouse.current.position.ReadValue();
    //     Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    //     Debug.Log(grid.GetGridObject(worldPosition));
    // }

    private List<Vector3> SetUpTiles() {
        List<Vector3> spawnLocations = new List<Vector3>();

        baseTilemap.CompressBounds();
        BoundsInt bounds = baseTilemap.cellBounds;
        Debug.Log(bounds);
        TileBase[] spawnTiles = spawnTilemap.GetTilesBlock(bounds);
        TileBase[] collidableTiles = collidableTilemap.GetTilesBlock(bounds);
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase spawnTile = spawnTiles[x + y * bounds.size.x];
                TileBase collidableTile = collidableTiles[x + y * bounds.size.x];

                if (spawnTile != null)
                {
                    // Debug.Log("x:" + x + " y:" + y + " tile:" + spawnTile.name);
                    spawnLocations.Add(new Vector3(x, y, 0));
                }

                if (collidableTile != null)
                {
                    _pathfinding.GetGrid().GetGridObject(x, y).isWalkable = false;
                }
            }
        }

        return spawnLocations;
    }

    private void SpawnLairs() {
        lairs = new GameObject[lairPrefabs.Length];
        int len = lairPrefabs.Length;
        for (int i = 0; i<len; i++) {
            Vector3 spawnLocation = lairPrefabs[i].GetComponent<LairController>().spawnLocation;
            // Vector3 spawnLocation = new Vector3(12.5f, 12.5f, 0) + new Vector3(0, 25f*i, 0);
            GameObject lair = Instantiate(lairPrefabs[i], spawnLocation, Quaternion.Euler(Vector3.zero));
            // lair.GetComponent<LairController>().spawnLocation = spawnLocation;
            lairs[i] = lair;
            
            DrawLairTiles(spawnLocation, lair);
        }
    }

    private void DrawLairTiles(Vector3 spawnLocation, GameObject lair) {
        lairTilemap.SetTile(Vector3Int.FloorToInt(spawnLocation), lairTile);

        // int lairRadius = lair.GetComponent<LairController>().lairRadius;
        // Change back to top later
        int lairRadius = 1;
        for (int x = -lairRadius; x<lairRadius+1; x++) {
            for (int y = -lairRadius; y<lairRadius+1; y++) {
                Vector3Int location = Vector3Int.FloorToInt(spawnLocation) + new Vector3Int(x, y, 0);
                lairTilemap.SetTile(location, lairTile);
            }
        }
    }

    public void Restart() {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
