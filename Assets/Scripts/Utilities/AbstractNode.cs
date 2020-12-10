using UnityEngine;

public abstract class AbstractNode
{
    public int X { get; }
    public int Y { get; }

    protected AbstractNode(int x, int y)
    {
        X = x;
        Y = y;

    }
}