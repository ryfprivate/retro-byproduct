using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int gCost;
    public int hCost;
    public int fCost;
    public int x;
    public int y;

    public PathNode prevNode;

    Grid<PathNode> grid;


    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
