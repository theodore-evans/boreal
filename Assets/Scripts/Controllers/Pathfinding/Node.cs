using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPoint;

    public Node(bool walkable, Vector3 worldPoint)
    {
        this.walkable = walkable;
        this.worldPoint = worldPoint;
    }

    public void Update(bool walkable)
    {
        this.walkable = walkable;
    }
}