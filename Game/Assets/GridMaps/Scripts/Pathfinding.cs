using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Grid<PathNode> grid;
    private List<PathNode> frontierList;
    private List<PathNode> visitedList;

    public Pathfinding(int width, int height) {
        grid = new Grid<PathNode>(width, height, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public Grid<PathNode> GetGrid() {
        return grid;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
        PathNode startNode = GetNode(startX, startY);
        PathNode endNode = GetNode(endX, endY);

        frontierList = new List<PathNode> { startNode }; // initial node
        visitedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                PathNode pathNode = GetNode(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.parentNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (frontierList.Count > 0) {
            PathNode currentNode = GetLowestFCostNode(frontierList);

            if (currentNode == endNode) { // goal reached
                return CalculatePath(endNode);
            }

            // pop from frontier, record as visited
            frontierList.Remove(currentNode);
            visitedList.Add(currentNode);

            // expand or successor function
            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)) {
                if (visitedList.Contains(neighbourNode)) continue;
                
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost) {
                    neighbourNode.parentNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!frontierList.Contains(neighbourNode)) {
                        frontierList.Add(neighbourNode);
                    }
                }

            }
        }

        // no path
        return null;
    }

    private PathNode GetNode(int x, int y) {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y)); // left
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1)); // left-down
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1)); // left-up
        }

        if (currentNode.x + 1 < grid.GetWidth()) {
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y)); // right
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1)); // right-down
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1)); // right-up
        }

        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1)) ;// down
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1)); // up

        return neighbourList;
    }

    private List<PathNode> CalculatePath(PathNode endNode) {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        
        PathNode currentNode = endNode;
        while (currentNode.parentNode != null) {
            path.Add(currentNode.parentNode);
            currentNode = currentNode.parentNode;
        }
    
        path.Reverse();
        return path;
    }

    // distance heuristics
    private int CalculateDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> nodeList) {
        PathNode lowestFCostNode = nodeList[0];
        for (int i = 1; i < nodeList.Count; i++) {
            if (nodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = nodeList[i];
            }
        }
        return lowestFCostNode;
    }
}
