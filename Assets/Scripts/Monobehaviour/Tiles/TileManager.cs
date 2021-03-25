using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap roughnessTilemap;
    [SerializeField]
    private float maxRoughness;
    [SerializeField]
    private Color maxColor, minColor, clearColor;



    private Dictionary<Vector3Int, float> roughnessTiles = new Dictionary<Vector3Int, float>();

    public void AddRoughness()
    {

    }

    private void ChangeRoughness(Vector3Int gridPosition, float changeBy)
    {
        if (!roughnessTiles.ContainsKey(gridPosition))
        {
            roughnessTiles.Add(gridPosition, 0f);
        }

        float newValue = roughnessTiles[gridPosition] + changeBy;

        if (newValue <= 0f)
        {
            roughnessTiles.Remove(gridPosition);

            roughnessTilemap.SetTileFlags(gridPosition, TileFlags.None);
            roughnessTilemap.SetColor(gridPosition, clearColor);
            roughnessTilemap.SetTileFlags(gridPosition, TileFlags.LockColor);
        }
        else
        {
            roughnessTiles[gridPosition] = Mathf.Clamp(newValue, 0f, maxRoughness);
        }
    }

    private void VisualizeRoughness()
    {
        foreach (var entry in roughnessTiles)
        {
            float roughnessPercentage = entry.Value / maxRoughness;

            Color newTileColor = maxColor * roughnessPercentage + minColor * (1f - roughnessPercentage);

            roughnessTilemap.SetTileFlags(entry.Key, TileFlags.None);
            roughnessTilemap.SetColor(entry.Key, newTileColor);
            roughnessTilemap.SetTileFlags(entry.Key, TileFlags.LockColor);
        }
    }
}
