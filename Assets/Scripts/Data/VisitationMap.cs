using System;
using System.Collections.Generic;

public class VisitationMap<T>: Cache<T> where T : AbstractNode
{
    private float maxValue = 0;
    private Dictionary<int, float> visits = new Dictionary<int, float>();
    private int GenerateHashCode(int x, int y) => AbstractNode.GenerateHashCode(x, y);
    public float MaxValue { get => maxValue; }

    public VisitationMap() : base() { }

    public new void Add(T item)
    {
        Add(item, 1f);
    }

    public float Add(T item, float value)
    {
        int hash = item.GetHashCode();
        bool isNewItem = base.Add(item);

        if (isNewItem) {
            visits.Add(hash, value);
        }
        else {
            visits[hash] += value;
        }

        float newValue = visits[hash];
        maxValue = (newValue > maxValue) ? newValue : maxValue;

        return newValue / maxValue;
    }

    public new float this[int x, int y]
    {
        get {
            int hash = GenerateHashCode(x, y);
            if (visits.ContainsKey(hash)) return visits[hash] / maxValue;
            else return 0;
        }
    }

    public float this[T node]
    {
        get {
            int hash = node.GetHashCode();
            if (visits.ContainsKey(hash)) return visits[hash] / maxValue;
            else return 0;
        }
    }

    public new void Clear()
    {
        base.Clear();
        visits.Clear();
    }

}
