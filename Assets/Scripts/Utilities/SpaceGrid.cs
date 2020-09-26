﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGrid<T> where T : AbstractNode
{
    private readonly Vector3 origin;
    private readonly float nodeSpacing;
    private readonly float width;
    private readonly float height;

    public int GridSizeX { get; }
    public int GridSizeY { get; }
    public int MaxSize { get => GridSizeX * GridSizeY; }

    public T[,] Nodes { get; protected set; }

    bool IsInBounds(int x, int y) => (x >= 0 && x < GridSizeX && y >= 0 && y < GridSizeY);

    public SpaceGrid(Vector3 origin, float width, float height, float nodeSpacing)
    {
        this.origin = origin;
        this.nodeSpacing = nodeSpacing;
        this.width = width;
        this.height = height;

        GridSizeX = Mathf.FloorToInt(width / nodeSpacing);
        GridSizeY = Mathf.FloorToInt(height / nodeSpacing);

        Nodes = new T[GridSizeX, GridSizeY];
    }

    public List<T> GetNeighbours(int x, int y)
    {
        return GetNeighbours(x, y, (maskX, maskY) => !(maskX == 0 && maskY == 0));
    }

    public List<T> GetNeighbours(int x, int y, Func<int, int, bool> NodeMask)
    {
        List<T> neighbours = new List<T>();
        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {

                if (NodeMask(dx, dy)) {
                    int checkX = x + dx;
                    int checkY = y + dy;

                    if (IsInBounds(checkX, checkY)) {
                        neighbours.Add(GetNodeAt(checkX, checkY));
                    }
                }
            }
        }
        return neighbours;
    }

    public T GetNodeAt(Vector3 position)
    {
        float percentX = Mathf.Clamp01( (position.x - origin.x) / width );
        float percentY = Mathf.Clamp01( (position.y - origin.y) / height );

        int x = Mathf.FloorToInt(percentX * GridSizeX);
        int y = Mathf.FloorToInt(percentY * GridSizeY);

        return GetNodeAt(x, y);
    }

    public T GetNodeAt(int x, int y)
    {
        if (IsInBounds(x, y)) {
            return Nodes[x, y];
        }
        else return default;
    }

    public Vector3 GetNodePosition(int x, int y)
    {
        return new Vector3(origin.x + x * nodeSpacing, origin.y + y * nodeSpacing, origin.z);
    }

    public Vector3 GetNodePosition(T node)
    {
        return GetNodePosition(node.X, node.Y);
    }

    public List<T> GetNodesInsideBounds(Bounds bounds)
    { 
        List<T> nodesInsideRect = new List<T>();

        Vector3 epsilon = 0.0001f * Vector3.one;

        T bottomLeftNode = GetNodeAt(bounds.center - bounds.extents + epsilon);
        T topRightNode = GetNodeAt(bounds.center + bounds.extents - epsilon);

        for (int x = bottomLeftNode.X; x < topRightNode.X; x++) {
            for (int y = bottomLeftNode.Y; y < topRightNode.Y; y++) {
                nodesInsideRect.Add(Nodes[x, y]);
            }
        }

        return nodesInsideRect;
    }
}