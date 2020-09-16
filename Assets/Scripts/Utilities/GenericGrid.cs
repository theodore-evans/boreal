using System;
using System.Collections.Generic;
using UnityEngine; // change Mathf methods to something decoupled from unityengine

public class GenericGrid // TODO to replace Grid class
{
    private readonly int gridSizeX;
    private readonly int gridSizeY;

    private readonly int width;
    private readonly int height;

    public INode[,] Nodes { get; protected set; }

    bool IsInBounds(int x, int y) => (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY);

    public GenericGrid(int width, int height, float nodeSpacing)
    {
        this.width = width;
        this.height = height;

        gridSizeX = Mathf.RoundToInt(width / nodeSpacing);
        gridSizeY = Mathf.RoundToInt(height / nodeSpacing);

        Nodes = new INode[gridSizeX, gridSizeY];
    }

    public List<INode> GetNeighbours(INode node)
    {
        // returns the default 8-neighbourhood
        return GetNeighbours(node, (x,y) => !(x == 0 && y == 0));
    }

    public List<INode> GetNeighbours(INode node, Func<int, int, bool> NodeMask)
    {
        List<INode> neighbours = new List<INode>();
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {

                if (NodeMask(x, y)) {
                    int checkX = node.X + x;
                    int checkY = node.Y + y;

                    if (IsInBounds(checkX, checkY)) {
                        neighbours.Add(Nodes[checkX, checkY]);
                    }
                }
            }
        }
        return neighbours;
    }

    public INode GetNodeAtPosition(Vector3 position)
    {
        float percentX = Mathf.Clamp01(position.x / width);
        float percentY = Mathf.Clamp01(position.y / height);

        int x = (int) Mathf.Floor(gridSizeX * percentX);
        int y = (int) Mathf.Floor(gridSizeY * percentY);

        return GetNodeAt(x, y);
    }

    public INode GetNodeAt(int x, int y)
    {
        if (IsInBounds(x, y)) {
            return Nodes[x, y];
        }

        else return default;
    }

    public List<INode> GetNodesInsideRect(Vector3 bottomLeft, Vector3 size)
    {
        List<INode> nodesInsideRect = new List<INode>();

        INode bottomLeftNode = GetNodeAtPosition(bottomLeft);
        INode topRightNode = GetNodeAtPosition(bottomLeft + size);

        for (int x = bottomLeftNode.X; x < topRightNode.X; x++) {
            for (int y = bottomLeftNode.Y; y < topRightNode.Y; y++) {
                nodesInsideRect.Add(Nodes[x, y]);
            }
        }

        return nodesInsideRect;
    }
}
