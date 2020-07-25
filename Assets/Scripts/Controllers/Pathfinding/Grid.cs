using UnityEngine;
using System.Collections;
using System;

public class Grid
{
    LayerMask unwalkableMask;
    float nodeRadius;
    public Node[,] Nodes { get; protected set; }

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    int width, height;
    Vector3 worldOrigin;

    public int nearestNeighborRange;

    public Grid(Vector3 worldOrigin, int width, int height, float nodeRadius, LayerMask unwalkableMask)
    {
        this.worldOrigin = worldOrigin;
        this.width = width;
        this.height = height;
        this.unwalkableMask = unwalkableMask;
        this.nodeRadius = nodeRadius;
      
        GenerateGrid(nodeRadius, unwalkableMask);
    }

    public void GenerateGrid(float nodeRadius, LayerMask unwalkableMask)
    {
        nodeDiameter = Mathf.Max(nodeRadius * 2, 0.1f);

        gridSizeX = Mathf.RoundToInt(width / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(height / nodeDiameter);

        Nodes = new Node[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                Vector3 worldPoint = worldOrigin + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = IsWalkable(worldPoint);
                Nodes[x, y] = new Node(walkable, worldPoint);
            }
        }
    }

    bool IsWalkable(Vector3 worldPoint)
    {
        //return !Physics.CheckBox(worldPoint, Vector3.one * (nodeRadius - 0.1f), Quaternion.identity, unwalkableMask);
        return !Physics.CheckSphere(worldPoint, nodeRadius - 0.01f, unwalkableMask);

    }

    public void UpdateNodesAtGameObject(GameObject go)
    {
        Collider collider = go.GetComponent<Collider>();
        Vector3 colliderPosition = collider.transform.position;
        Vector3 colliderExtents = collider.bounds.extents;

        Vector3 colliderBottomLeft = colliderPosition - colliderExtents;
        Vector3 colliderTopRight = colliderPosition + colliderExtents;

        int[] bottomLeftNodeIndices = WorldPointToGridPos(colliderBottomLeft);
        int[] topRightNodeIndices = WorldPointToGridPos(colliderTopRight);

        for (int x = bottomLeftNodeIndices[0]; x <= topRightNodeIndices[0]; x++) {
            for (int y = bottomLeftNodeIndices[1]; y <= topRightNodeIndices[1]; y++) {

                Node n = Nodes[x, y];
                n.Update(IsWalkable(n.worldPoint));
            }
        }
    }

    public Node NodeAtWorldPoint(Vector3 worldPoint)
    {
        int[] gridPos = WorldPointToGridPos(worldPoint);
        int x = gridPos[0];
        int y = gridPos[1];
        if (x >= 0 && x < Nodes.GetLength(0) && x >= 0 && y < Nodes.GetLength(1)) {
            return Nodes[gridPos[0], gridPos[1]];
        }
        else return null;
    }

    int[] WorldPointToGridPos(Vector3 worldPoint)
    {
        float percentX = worldPoint.x / width;
        float percentY = worldPoint.y / height;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = (int)Mathf.Floor(gridSizeX * percentX);
        int y = (int)Mathf.Floor(gridSizeY * percentY);

        return new int[] { x, y };
    }
}