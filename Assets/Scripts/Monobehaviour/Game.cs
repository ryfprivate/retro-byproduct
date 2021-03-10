using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CodeMonkey.Utils;

public class Game : MonoBehaviour
{
    public GameObject playerPrefab;

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

    void Awake() {
        controls = new GameControls();
        _pathfinding = new Pathfinding(25, 25);
    }


    void Start()
    {
        Vector3 spawnPosition = new Vector3(15, 15, 0);
        Instantiate(playerPrefab, spawnPosition, Quaternion.Euler(Vector3.zero));
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
