using System;
using System.Collections.Generic;

public class VisitationMap<T>: Cache<T> where T : AbstractNode
{
    private Dictionary<int, int> visits = new Dictionary<int, int>();
    private int GenerateHashCode(int x, int y) => AbstractNode.GenerateHashCode(x, y);

    public VisitationMap() : base() { }

    public new void Add(T item)
    {
        int hash = item.GetHashCode();
        bool alreadyContainsItem = base.Add(item);

        if (alreadyContainsItem) {
            visits.Add(hash, 1);
        }
        else {
            visits[hash]++;
        }
    }

    public new int this[int x, int y]
    {
        get {
            int hash = GenerateHashCode(x, y);
            if (visits.ContainsKey(hash)) return visits[hash];
            else return 0;
        }
    }

    public int this[T node]
    {
        get {
            int hash = node.GetHashCode();
            if (visits.ContainsKey(hash)) return visits[hash];
            else return 0;
        }
    }

}
