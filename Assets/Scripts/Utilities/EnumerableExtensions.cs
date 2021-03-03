using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class EnumerableExtensions
    {
        public static List<T> PopLastRange<T>(this List<T> source, int count)
        {
            int startIndex = Math.Max(0, source.Count - count);
            int numItems = Math.Min(count, source.Count);
            List<T> cached = source.GetRange(startIndex, numItems);
            source.RemoveRange(startIndex, numItems);
            return cached;
        }

        public static List<T> PopFraction<T>(this List<T> source, float fraction)
        {
            int count = (int)Math.Ceiling(source.Count * fraction);
            return source.PopLastRange(count);
        }
    }

    public static class RectExtensions
    {
        public static Rect Expand(this Rect rect, float amount)
        {
            return Rect.MinMaxRect(rect.xMin - amount, rect.yMin - amount, rect.xMax + amount, rect.yMax + amount);
        }
    }

}