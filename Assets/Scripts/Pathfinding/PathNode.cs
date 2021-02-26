public class PathNode : AbstractNode, IHeapItem<PathNode>
{
    public float Radius { get; }
    public bool Walkable { get; set; }

    public int gCost, hCost;
    internal PathNode parent;

    public int fCost { get => gCost + hCost; }

    public int heapIndex { get; set; }
    

    public PathNode(int x, int y, float radius) : base(x, y, radius)
    {
        Radius = radius;
        Walkable = false;
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