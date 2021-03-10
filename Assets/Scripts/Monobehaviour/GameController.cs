using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;

public class GameController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Tilemap baseTilemap;
    public Tilemap spawnTilemap;
    public Tilemap collidableTilemap;

    // Class Instances
    private GameControls controls;
    private Pathfinding _pathfinding;

    private int numEnemies;

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
        controls = new GameControls();
        _pathfinding = new Pathfinding(25, 25);
    }


    void Start()
    {
        numEnemies = 4;
        // Adds all spawn locations to a list
        // Adds all collidable tiles as obstacles in the pathfinding grid
        List<Vector3> spawnLocations = new List<Vector3>();

        baseTilemap.CompressBounds();
        BoundsInt bounds = baseTilemap.cellBounds;
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

        // Spawn player on random spawn location
        Vector3 offset = new Vector3(.5f, .5f, 0);
        // Adds offset to position to spawn in middle of cell
        Vector3 playerSpawnPosition = spawnLocations[Random.Range(0, spawnLocations.Count)] + offset;
        Instantiate(playerPrefab, playerSpawnPosition, Quaternion.Euler(Vector3.zero));

        // Spawns enemies on a random spawn location
        for (int i = 0; i < numEnemies; i++)
        {
            Vector3 enemySpawnPosition = spawnLocations[Random.Range(0, spawnLocations.Count)] + offset;
            Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.Euler(Vector3.zero));
        }
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
}
