using UnityEngine;

public class Node : IHeapItem<Node>
{
    public int gridX, gridY;

    public bool walkable;
    public Vector3 worldPoint;

    public int gCost, hCost;
    public int fCost { get => gCost + hCost; }

    int heapIndex;

    public int HeapIndex {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public Node parent;

    public Node(bool walkable, Vector3 worldPoint, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPoint = worldPoint;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public void Update(bool walkable)
    {
        this.walkable = walkable;
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}