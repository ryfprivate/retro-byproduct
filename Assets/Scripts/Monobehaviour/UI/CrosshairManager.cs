using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairManager : MonoBehaviour
{
    void FixedUpdate()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = new Vector3(pos.x, pos.y, 0);
        Debug.Log(Mouse.current.position.ReadValue());
    }
}
