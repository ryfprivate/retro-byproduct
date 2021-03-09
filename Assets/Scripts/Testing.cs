using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    GameControls controls;

    Pathfinding pathfinding;


    private void Awake()
    {
        controls = new GameControls();
    }

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

    void Start()
    {
        pathfinding = new Pathfinding(30, 30);
        // grid = new Grid<int>(30, 30, 1f, Vector3.zero, (Grid<int> g, int x, int y) => 0);
    }

    void HandleLeftClick(InputAction.CallbackContext ctx)
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        pathfinding.GetGrid().GetXY(worldPosition, out int x, out int y);
        List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(new Vector3(path[i].x, path[i].y) + Vector3.one * .5f, new Vector3(path[i + 1].x, path[i + 1].y) + Vector3.one * .5f, Color.green, .5f);
            }
        }
    }

    // void HandleRightClick(InputAction.CallbackContext ctx)
    // {
    //     Vector2 screenPosition = Mouse.current.position.ReadValue();
    //     Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    //     Debug.Log(grid.GetGridObject(worldPosition));
    // }
}
