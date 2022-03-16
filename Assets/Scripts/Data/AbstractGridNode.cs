using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGridNode : INode
{
    public int X { get; }
    public int Y { get; }
    public float Scale { get; }

    internal IEnumerable<INode> _neighbours;
    private int _hashCode;

    public override int GetHashCode()
    {
        return _hashCode;
    }

    protected AbstractGridNode(int x, int y, float scale)
    {
        if (x < 0 || y < 0) {
            throw new NotImplementedException("Negative node index");
        }
        X = x;
        Y = y;
        Scale = scale;
        _hashCode = GenerateHashCode(x, y);
    }

    public int GenerateHashCode(AbstractGridNode node)
    {
        return GenerateHashCode(node.X, node.Y);
    }

    // Cantor pairing function, only for positive x, y
    public static int GenerateHashCode(int x, int y)
    {
        return (int)(0.5 * (x + y) * (x + y + 1) + y);
    }

    public bool IsInRect(Rect rect)
    {
        return !(X < rect.xMin
                 || Y < rect.yMin
                 || X > rect.xMax
                 || Y > rect.yMax);
    }
}

