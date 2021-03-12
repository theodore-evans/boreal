using System;

public class PathNode : AbstractNode, IHeapItem<PathNode>
{
    public float Radius { get; }

    public float gCost, hCost, movementCost;
    public float w;
    internal PathNode parent;

    public float fCost { get => gCost + w * hCost; }
    public int heapIndex { get; set; }
    public float Altitude { get; internal set; }

    public PathNode(int x, int y, float radius, float heuristicWeight = 1) : base(x, y, radius * 2)
    {
        Radius = radius;
        w = heuristicWeight;
    }

    public int CompareTo(PathNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}