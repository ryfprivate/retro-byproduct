using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public GameObject parent;

    public void SetParent(GameObject newParent)
    {
        parent = newParent;
    }

    public GameObject GetParent()
    {
        return parent;
    }
}
