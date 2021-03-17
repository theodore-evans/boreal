using System;
using System.Collections.Generic;

public class VisitationMap<T>: Cache<T> where T : AbstractNode
{
    private Dictionary<int, float> visits = new Dictionary<int, float>();
    private int GenerateHashCode(int x, int y) => AbstractNode.GenerateHashCode(x, y);

    public VisitationMap() : base() { }

    public new void Add(T item)
    {
        Add(item, 1f);
    }

    public void Add(T item, float value)
    {
        int hash = item.GetHashCode();
        bool alreadyContainsItem = base.Add(item);

        if (alreadyContainsItem) {
            visits.Add(hash, value);
        }
        else {
            visits[hash]+= value;
        }
    }

    public new float this[int x, int y]
    {
        get {
            int hash = GenerateHashCode(x, y);
            if (visits.ContainsKey(hash)) return visits[hash];
            else return 0;
        }
    }

    public float this[T node]
    {
        get {
            int hash = node.GetHashCode();
            if (visits.ContainsKey(hash)) return visits[hash];
            else return 0;
        }
    }

    public new void Clear()
    {
        base.Clear();
        visits.Clear();
    }

}
