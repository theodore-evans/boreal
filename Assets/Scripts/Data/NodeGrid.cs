﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid<T> where T : AbstractGridNode
{
    private readonly Vector3 origin;
    private readonly float nodeSpacing;
    private readonly float width;
    private readonly float height;
    private readonly Vector2 epsilon = 0.0001f * Vector2.one;

    public Vector3 Origin { get => origin; }
    public int GridSizeX { get; }
    public int GridSizeY { get; }
    public int MaxSize { get => GridSizeX * GridSizeY; }

    public T[] Nodes { get; protected set; }

    bool IsInBounds(int x, int y) => !(x < 0 || x >= GridSizeX || y < 0 || y >= GridSizeY);
    int GetIndex(int x, int y) => y * GridSizeX + x;
    Func<int, int, bool> MooreNeighbourhoodMask => (maskX, maskY) => maskX == 0 && maskY == 0;

    public NodeGrid(Vector3 origin, float width, float height, float nodeSpacing)
    {
        if (width < 1 || height < 1 || nodeSpacing < 0) {
            throw new NotImplementedException("Negative tile index");
        }

        this.origin = origin;
        this.nodeSpacing = nodeSpacing;
        this.width = width;
        this.height = height;

        GridSizeX = Mathf.FloorToInt(width / nodeSpacing);
        GridSizeY = Mathf.FloorToInt(height / nodeSpacing);

        Nodes = new T[GridSizeX * GridSizeY];
    }

    private List<int> CalculateNeighbourIndices(int x, int y, Func<int, int, bool> Mask)
    {
        List<int> neighbours = new List<int>();
        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {

                if (Mask(dx, dy) == false) {
                    int checkX = x + dx;
                    int checkY = y + dy;

                    if (IsInBounds(checkX, checkY)) {
                        neighbours.Add(GetIndex(checkX, checkY));
                    }
                }
            }
        }
        return neighbours;
    }

    public IEnumerable<T> GetNeighbours(int x, int y)
    {
        List<T> neighbours = new List<T>();
        foreach (int neighbourIndex in CalculateNeighbourIndices(x, y, MooreNeighbourhoodMask)) {
                neighbours.Add(Nodes[neighbourIndex]);
            }
        return neighbours;
    }

    internal IEnumerable<T> GetNodesOnOtherGridsNode<U>(U otherNode) where U : AbstractGridNode
    {
        List<T> nodesOnNode = new List<T>();

        Vector2 bottomLeft = new Vector2(otherNode.X, otherNode.Y);

        T bottomLeftNode = GetNodeAt(bottomLeft + epsilon);
        T topRightNode = GetNodeAt(bottomLeft + otherNode.Scale * Vector2.one - epsilon);

        for (int x = bottomLeftNode.X; x <= topRightNode.X; x++) {
            for (int y = bottomLeftNode.Y; y <= topRightNode.Y; y++) {
                nodesOnNode.Add(GetNodeAt(x, y));
            }
        }

        return nodesOnNode;

    }

    public T GetNodeAt(Vector3 position)
    {
        float percentX = Mathf.Clamp01((position.x - origin.x) / width);
        float percentY = Mathf.Clamp01((position.y - origin.y) / height);

        int x = Mathf.FloorToInt(percentX * GridSizeX);
        int y = Mathf.FloorToInt(percentY * GridSizeY);

        return GetNodeAt(x, y);
    }

    public T GetNodeAt(int x, int y)
    {
        if (IsInBounds(x, y)) {
            return Nodes[y * GridSizeX + x];
        }
        else return default;
    }

    public bool InstantiateNode(T newNode)
    {
        int x = newNode.X;
        int y = newNode.Y;
        if (IsInBounds(x, y)) {
            Nodes[GetIndex(x, y)] = newNode;
            return true;
        }
        else return false;
    }

    public Vector3 GetNodeCenter(T node)
    {
        Vector3 bottomLeftCorner = GetNodePosition(node);
        return bottomLeftCorner + new Vector3(0.5f, 0.5f, 0) * node.Scale;
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
        T topRightNode = GetNodeAt(bounds.center + bounds.extents + epsilon);

        for (int x = bottomLeftNode.X; x < topRightNode.X; x++) {
            for (int y = bottomLeftNode.Y; y < topRightNode.Y; y++) {
                nodesInsideRect.Add(GetNodeAt(x, y));
            }
        }

        return nodesInsideRect;
    }
}
