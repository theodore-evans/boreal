using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;

public class ShuffledCache<T> : Cache<T> where T : AbstractNode
{
    private int _batchSize;
    private List<T> shuffledItems;

    private System.Random rng = new System.Random();

    public ShuffledCache(int batchSize = 100) : base()
    {
        _batchSize = batchSize;
    }

    public new IEnumerator<IEnumerable<T>> GetEnumerator()
    {
        List<T> items = new List<T>();
        int itemCount = Count;
        int batchSize = Math.Min(_batchSize, itemCount);
        for (int i = 0; i < batchSize; i++) {
            T item = base.inner.Values.ToList()[rng.Next(0, Count)];
            items.Add(item);
            Remove(item);
            itemCount -= _batchSize;
        }
        yield return items;
    }
}
