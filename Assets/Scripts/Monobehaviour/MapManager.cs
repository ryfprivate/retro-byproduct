using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [SerializeField]
    private Tilemap baseTilemap;
    [SerializeField]
    private Tilemap spawnTilemap;
    [SerializeField]
    private Tilemap collidableTilemap;
    [SerializeField]
    private Tilemap lairTilemap;

    // Spawning
    public List<Vector3> lairLocations = new List<Vector3>();
    public List<Vector3> spawnLocations = new List<Vector3>();

    // Tile data
    [SerializeField]
    private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;

    // Class Instances
    private Pathfinding _pathfinding;

    private int mapSize = 50;

    void Awake()
    {
        Instance = this;

        _pathfinding = new Pathfinding(mapSize, mapSize);

        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    void Start()
    {
        // Adds all spawn locations to a list
        // Adds all collidable tiles as obstacles in the pathfinding grid
        spawnLocations = SetUpTiles();
    }

    void Update()
    {
        if (PlayerManager.Instance != null)
        {
            Vector3Int gridPosition = baseTilemap.WorldToCell(PlayerManager.Instance.transform.position);
            TileBase currentTile = baseTilemap.GetTile(gridPosition);
            float walkingSpeed = dataFromTiles[currentTile].walkingSpeed;
            float poisonous = dataFromTiles[currentTile].poisonous;

            PlayerManager.Instance.moveSpeed = PlayerManager.Instance.playerSpeed * walkingSpeed;

            if (poisonous > 0)
            {
                PlayerManager.Instance.health -= 0.1f;
            }
        }
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
}
