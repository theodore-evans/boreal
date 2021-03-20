using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Cache<T> : IEnumerable<T> where T : AbstractNode
{
    private Dictionary<int, T> inner = new Dictionary<int, T>();
    private int GenerateHashCode(int x, int y) => AbstractNode.GenerateHashCode(x, y); // TODO: is there a way to get T.GenerateHashCode in case it is overriden?

    public Cache() : base() { }

    public Cache(IEnumerable<T> source) : base()
    {
        foreach (T item in source) Add(item);
    }

    public int Count => inner.Count;

    public T this[int x, int y]
    {
        get {
            int hashCode = GenerateHashCode(x, y);
            if (inner.ContainsKey(hashCode)) return inner[hashCode];
            else return default;
        }
    }

    public bool Add(T item)
    {
        if (!Contains(item)) {
            inner.Add(item.GetHashCode(), item);
            return true;
        }
        return false;
    }

    internal void Union(Cache<T> cache)
    {
        foreach (T item in cache) {
            Add(item);
        }
    }

    public bool Remove(T item)
    {
        return inner.Remove(item.GetHashCode());
    }

    public bool Contains(T item)
    {
        return inner.ContainsKey(item.GetHashCode());
    }

    public bool Contains(int x, int y)
    {
        return inner.ContainsKey(GenerateHashCode(x, y));
    }

    public void ExceptWith(ICollection<T> other)
    {
        foreach (T item in other) {
            Remove(item);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return inner.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public Cache<T> PopWithinArea(int xMin, int xMax, int yMin, int yMax, int numberToPop)
    {
        Cache<T> items = new Cache<T>();
        int numberPopped = 0;

        for (int x = xMin; x < xMax; x++) {
            for (int y = yMin; y < yMax; y++) {
                if (numberPopped > numberToPop) break;
                int key = GenerateHashCode(x, y);
                if (inner.ContainsKey(key)) {
                    items.Add(inner[key]);
                    inner.Remove(key);
                }
            }
        }

        return items;
    }

    public Cache<T> PopAllWithinArea(Rect rect)
    {
        return PopWithinArea(rect, int.MaxValue);
    }

    public Cache<T> PopWithinArea(Rect rect, int numberToPop)
    {
        int xMin = Mathf.FloorToInt(rect.xMin);
        int yMin = Mathf.FloorToInt(rect.yMin);
        int xMax = Mathf.CeilToInt(rect.xMax);
        int yMax = Mathf.CeilToInt(rect.yMax);

        return PopWithinArea(xMin, xMax, yMin, yMax, numberToPop);
    }

    public IEnumerable<T> DrawRandom(int numberToDraw)
    {
        List<T> shuffledValues = inner.Values.ToList().Shuffle();
        return SelectFrom(shuffledValues, numberToDraw, false);
    }

    public IEnumerable<T> Draw(int numberToDraw)
    {
        List<T> values = inner.Values.ToList();
        return SelectFrom(values, numberToDraw, false);
    }

    public IEnumerable<T> PickRandom(int numberToDraw)
    {
        List<T> shuffledValues = inner.Values.ToList().Shuffle();
        return SelectFrom(shuffledValues, numberToDraw, true);
    }

    public IEnumerable<T> Pick(int numberToDraw)
    {
        List<T> values = inner.Values.ToList();
        return SelectFrom(values, numberToDraw, true);
    }

    private IEnumerable<T> SelectFrom(List<T> values, int numberToDraw, bool replace) {

        List<T> selectedItems = new List<T>();

        numberToDraw = Math.Min(numberToDraw, inner.Count);

        for (int i = 0; i < numberToDraw; i++) {
            T selectedItem = values[i];
            selectedItems.Add(selectedItem);
            if (!replace) Remove(selectedItem);
        }

        return selectedItems;
    }

    internal void Clear()
    {
        inner.Clear();
    }
}
