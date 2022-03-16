using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

// TODO: Implement Block A*: file:///Users/theoevans/Downloads/4055-17651-1-PB.pdf
// and/or limited lookahead
public class AStar : MonoBehaviour, IPathfinding
{
    PathRequestManager requestManager;
    PathGridController gridController;

    internal Heap<PathNode> openSet;
    internal Cache<PathNode> closedSet;
    internal NodeGrid<PathNode> grid;

    [SerializeField] private float ascendDescendPenalty = 1f;
    [SerializeField] public float maxMovementPenalty = 5f;

    public float MaxMovementCost { get => maxMovementPenalty; }
    public bool IsWalkable(PathNode node) => node.movementPenalty < maxMovementPenalty;

    private void Start()
    {
        gridController = GetComponent<PathGridController>();
        requestManager = GetComponent<PathRequestManager>();
        grid = gridController.grid;
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        grid = gridController.grid;
        PathNode startNode = grid.GetNodeAt(startPos);
        PathNode targetNode = grid.GetNodeAt(targetPos);

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        if (IsWalkable(startNode) && IsWalkable(targetNode)) {
            openSet = new Heap<PathNode>(grid.MaxSize);
            closedSet = new Cache<PathNode>();

            openSet.Add(startNode);

            while (openSet.Count > 0) {
                PathNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode) {
                    pathSuccess = true;
                    break;
                }

                foreach (PathNode neighbour in currentNode.Neighbours) {
                    if (!IsWalkable(neighbour) || closedSet.Contains(neighbour)) {
                        continue;
                    }
                    float newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) * neighbour.movementPenalty;
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        else {
            openSet.Clear();
            closedSet.Clear();
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
                waypoints.Add(grid.GetNodeCenter(path[i]));

            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    float GetDistance(PathNode nodeA, PathNode nodeB)
    {
        float dstX = Mathf.Abs(nodeA.X - nodeB.X);
        float dstY = Mathf.Abs(nodeA.Y - nodeB.Y);
        float ascendDescendCost = ascendDescendPenalty * Mathf.Abs(nodeA.Altitude - nodeB.Altitude);

        float distance; 

        if (dstX > dstY) {
            distance = 1.4f * dstY + 1.0f * (dstX - dstY);
        }
        else {
            distance = 1.4f * dstX + 1.0f * (dstY - dstX);
        }

        return distance + ascendDescendCost;
    }
}
