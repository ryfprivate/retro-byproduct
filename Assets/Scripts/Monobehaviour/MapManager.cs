using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap baseTilemap;

    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;

    void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    void Update()
    {
        if (PlayerManager.Instance != null)
        {
            Vector3Int gridPosition = baseTilemap.WorldToCell(PlayerManager.Instance.transform.position);
            TileBase currentTile = baseTilemap.GetTile(gridPosition);
            float walkingSpeed = dataFromTiles[currentTile].walkingSpeed;
            float poisonous = dataFromTiles[currentTile].poisonous;

            print("walking speed on: " + currentTile + walkingSpeed);
            PlayerManager.Instance.moveSpeed = PlayerManager.Instance.playerSpeed * walkingSpeed;

            if (poisonous > 0)
            {
                PlayerManager.Instance.health -= 0.1f;
            }
        }
    }
}
