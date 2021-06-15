using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {

    private Grid<PathNode> grid;
    public int x;
    public int y;

    public int gCost; // edge cost between adjacent node
    public int hCost; // heuristic cost, direct path
    public int fCost; // h + g

    public PathNode parentNode;

    public PathNode(Grid<PathNode> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public override string ToString() {
        return x + "," + y;
    }

}
