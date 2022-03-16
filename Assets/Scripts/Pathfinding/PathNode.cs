using System;
using System.Collections.Generic;

public class PathNode : AbstractGridNode, IHeapItem<PathNode>
{
    public float Radius { get; }

    public float gCost, hCost, movementPenalty;
    public float w;
    internal PathNode parent;
    private new IEnumerable<PathNode> _neighbours;
    public IEnumerable<PathNode> Neighbours { get => _neighbours; }

    public float fCost { get => gCost < hCost ? gCost + w * hCost : (gCost + (2 * w - 1) * hCost) / w; }
    //https://webdocs.cs.ualberta.ca/~nathanst/papers/chen2021general.pdf
    //via http://theory.stanford.edu/~amitp/GameProgramming/Variations.html

    public int heapIndex { get; set; }
    public float Altitude { get; internal set; }

    public PathNode(int x, int y, float radius, IEnumerable<PathNode> neighbours, float heuristicWeight = 1) : base(x, y, radius * 2)
    {
        Radius = radius;
        w = heuristicWeight;
        _neighbours = neighbours;
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