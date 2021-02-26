﻿using System;
using UnityEngine;

public abstract class AbstractNode
{
    public int X { get; }
    public int Y { get; }
    private int _hashCode;

    public override int GetHashCode()
    {
        return _hashCode;
    }

    public override bool Equals(object obj)
    {
        return 0 == this.CompareTo((AbstractNode)obj);
    }

    private int CompareTo(AbstractNode node)
    {
        return Math.Abs(X - node.X) + Math.Abs(Y - node.Y);
    }

    protected AbstractNode(int x, int y)
    {
        X = x;
        Y = y;
        _hashCode = GenerateHashCode(x, y);
    }

    private int GenerateHashCode(int x, int y)
    {
        return 1572869 * x + 786433 * y;
    }
}