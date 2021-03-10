using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;

public class GameController : MonoBehaviour
{
    public GameObject playerPrefab;
    public Tilemap baseTilemap;
    public Tilemap collidableTilemap;

    // Class Instances
    private GameControls controls;
    private Pathfinding _pathfinding;

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
        // Spawn player
        Vector3 spawnPosition = new Vector3(15, 15, 0);
        Instantiate(playerPrefab, spawnPosition, Quaternion.Euler(Vector3.zero));


        // Spawn enemies
        //      Spawns enemies on the boundaries of the grid


        // Adds all collidable tiles as obstacles in the pathfinding grid
        //      Ensures the bounds are restored to the outmost tiles
        baseTilemap.CompressBounds();
        collidableTilemap.CompressBounds();
        BoundsInt bounds = baseTilemap.cellBounds;
        TileBase[] allTiles = collidableTilemap.GetTilesBlock(bounds);
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    // Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    _pathfinding.GetGrid().GetGridObject(x, y).isWalkable = false;
                }
            }
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
