using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    private GameControls controls;
    Grid grid;

    private void Awake()
    {
        controls = new GameControls();
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.LeftClick.performed += ctx => HandleLeftClick(ctx);
        controls.Player.RightClick.performed += ctx => HandleRightClick(ctx);
    }

    void OnDisable()
    {
        controls.Disable();
        controls.Player.LeftClick.performed -= ctx => HandleLeftClick(ctx);
        controls.Player.RightClick.performed -= ctx => HandleRightClick(ctx);
    }

    void Start()
    {
        grid = new Grid(30, 30, 1f, new Vector3(0, 0));
    }

    void HandleLeftClick(InputAction.CallbackContext ctx)
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        grid.SetValue(worldPosition, 56);
    }

    void HandleRightClick(InputAction.CallbackContext ctx)
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        Debug.Log(grid.GetValue(worldPosition));
    }

    void Update()
    {
    }
}
