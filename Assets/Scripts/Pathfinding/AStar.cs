using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

// TODO make class more dependent + encapsulated

public class AStar : MonoBehaviour, IPathfinding
{
    PathRequestManager requestManager;
    SpaceGrid<PathNode> grid;

    private void Start()
    {
        grid = GetComponent<PathGridController>().pathGrid;
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        PathNode startNode = grid.GetNodeAt(startPos);
        PathNode targetNode = grid.GetNodeAt(targetPos);

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        if (startNode.Walkable && targetNode.Walkable) {
            Heap<PathNode> openSet = new Heap<PathNode>(grid.MaxSize);
            HashSet<PathNode> closedSet = new HashSet<PathNode>();

            openSet.Add(startNode);

            while (openSet.Count > 0) {
                PathNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode) {
                    pathSuccess = true;
                    break;
                }

                foreach (PathNode neighbour in grid.GetNeighbours(currentNode.X, currentNode.Y)) {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour)) {
                        continue;
                    }

                    int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }

        yield return null;
        if (pathSuccess) {
            waypoints = RetracePath(startNode, targetNode);

        }
        requestManager.FinishProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;
        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);

        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<PathNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i - 1].X - path[i].X, path[i - 1].Y - path[i].Y);
            if (directionNew != directionOld) {
                waypoints.Add(grid.GetNodePosition(path[i]));

            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.X - nodeB.X);
        int dstY = Mathf.Abs(nodeA.Y - nodeB.Y);

        if (dstX > dstY) {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        else {
            return 14 * dstX + 10 * (dstY - dstX);
        }

    }
}
