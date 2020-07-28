using UnityEngine;
using System.Collections.Generic;
using System;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public float nodeRadius;

    public Node[,] Nodes { get; protected set; }

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    int width, height;
    Vector3 origin;

    public bool autoUpdate = true;
    public List<Node> path;

    public void Initialise(Vector3 origin, int width, int height)
    {
        this.origin = origin;
        this.width = width;
        this.height = height;

        CreateGrid();
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    public void CreateGrid()
    {
        nodeDiameter = Mathf.Max(nodeRadius * 2, 0.1f);

        gridSizeX = Mathf.RoundToInt(width / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(height / nodeDiameter);

        Nodes = new Node[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {

                Vector3 worldPoint = origin + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = IsWalkable(worldPoint);

                Nodes[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    bool IsWalkable(Vector3 worldPoint)
    {
        //return !Physics.CheckBox(worldPoint, Vector3.one * (nodeRadius - 0.1f), Quaternion.identity, unwalkableMask);
        return !Physics.CheckSphere(worldPoint, nodeRadius - 0.1f, unwalkableMask);
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {

                if (x == 0 && y == 0) continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add(Nodes[checkX, checkY]);
                }

            }
        }
        return neighbours;
    }

    public void UpdateNodesOnGameObject(GameObject go)
    {
        Vector3 bottomLeftCorner = go.transform.position;
        Vector3 size = go.GetComponent<Collider>().bounds.size;

        int[] bottomLeftNodeIndices = GetGridIndicesAtWorldPoint(bottomLeftCorner);
        int[] topRightNodeIndices = GetGridIndicesAtWorldPoint(bottomLeftCorner + size);

        for (int x = bottomLeftNodeIndices[0]; x < topRightNodeIndices[0]; x++) {
            for (int y = bottomLeftNodeIndices[1]; y < topRightNodeIndices[1]; y++) {

                Node n = Nodes[x, y];
                n.Update(IsWalkable(n.worldPoint));
            }
        }
    }

    public Node GetNodeAtWorldPoint(Vector3 worldPoint)
    {
        int[] gridPos = GetGridIndicesAtWorldPoint(worldPoint);
        int x = gridPos[0];
        int y = gridPos[1];
        if (x >= 0 && x < Nodes.GetLength(0) && x >= 0 && y < Nodes.GetLength(1)) {
            Node node = Nodes[gridPos[0], gridPos[1]];
            if (node != null) return node;
            else return null;
        }
        else return null;
    }

    int[] GetGridIndicesAtWorldPoint(Vector3 worldPoint)
    {
        float percentX = worldPoint.x / width;
        float percentY = worldPoint.y / height;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = (int)Mathf.Floor(gridSizeX * percentX);
        int y = (int)Mathf.Floor(gridSizeY * percentY);

        return new int[] { x, y };
    }

    private void OnValidate()
    {
        if (nodeRadius < 0.2f) { nodeRadius = 0.2f; }
    }
}