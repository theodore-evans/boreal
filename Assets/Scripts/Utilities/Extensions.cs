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

        public static List<T> Shuffle<T>(this List<T> list)
        {
            System.Random rng = new System.Random();

            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }

    public static class RectExtensions
    {
        public static Rect Expand(this Rect rect, float amount)
        {
            return Rect.MinMaxRect(rect.xMin - amount, rect.yMin - amount, rect.xMax + amount, rect.yMax + amount);
        }
    }

    public static class ArrayExtensions1D
    {
        public static float[] Normalize(this float[] data, float min, float max, float dataMin, float dataMax)
        {
            if (dataMax == max && dataMin == min) return data;

            float range = dataMax - dataMin;
            if (range == 0) return data;

            return data
                .Select(d => (d - dataMin) / range)
                .Select(n => (1 - n) * min + n * max)
                .ToArray();
        }

        public static float[] Normalize(this float[] data, float min, float max)
        {
            float dataMax = data.Max();
            float dataMin = data.Min();

            return data.Normalize(min, max, dataMin, dataMax);

        }

        public static T[,] Make2D<T>(this T[] input, int width, int height)
        {
            T[,] output = new T[height, width];

            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    output[i, j] = input[i * width + j];
                }
            }
            return output;
        }
    }

    public static class ArrayExtensions2D
    {
        public static float[,] Normalize(this float[,] data, float min, float max)
        {
            float dataMin = float.MaxValue;
            float dataMax = float.MinValue;

            for (int x = 0; x <= data.GetUpperBound(0); x++) {
                for (int y = 0; y <= data.GetUpperBound(1); y++) {
                    dataMin = data[x, y] < dataMin ? data[x, y] : dataMin;
                    dataMax = data[x, y] > dataMax ? data[x, y] : dataMax;
                }
            }

            return data.Normalize(min, max, dataMin, dataMax);
        }

        public static float[,] Normalize(this float[,] data, float min, float max, float dataMin, float dataMax) 
        {
            if (dataMin == min && dataMax == max) return data;

            float range = dataMax - dataMin;
            if (range == 0) return data;

            for (int x = 0; x <= data.GetUpperBound(0); x++) {
                for (int y = 0; y <= data.GetUpperBound(1); y++) {
                    float n = (data[x, y] - dataMin) / range;
                    data[x, y] = (1 - n) * min + n * max;
                }
            }
            return data;
        }

        public static float[,] Elementwise(this float[,] a, float[,] b, Func<float, float, float> operation)
        {
            for (int i = 0; i < 2; i++) {
                if (a.GetUpperBound(i) != b.GetUpperBound(i)) {
                    throw new Exception($"shape of a and b must match (axis {i}, {a.GetUpperBound(i)} and {b.GetUpperBound(i)})");
                }
            }

            float[,] result = new float[a.GetUpperBound(0)+1, a.GetUpperBound(1)+1];

            for (int x = 0; x <= result.GetUpperBound(0); x++) {
                for (int y = 0; y <= result.GetUpperBound(1); y++) {
                    result[x, y] = operation(a[x, y], b[x, y]);
                }
            }

            return result;
        }

        public static float[,] Add(this float[,] array, float[,] other)
        {
            return Elementwise(array, other, (a, b) => a + b);
        }

        public static float[,] Multiply(this float[,] array, float[,] other)
        {
            return Elementwise(array, other, (a, b) => a * b);
        }

        public static float[,] MultiplyByScalar(this float[,] data, float scalar)
        {
            float[,] result = new float[data.GetUpperBound(0) + 1, data.GetUpperBound(1) + 1];

            for (int x = 0; x <= data.GetUpperBound(0); x++) {
                for (int y = 0; y <= data.GetUpperBound(1); y++) {
                    result[x, y] = data[x,y] * scalar;
                }
            }

            return data;
        }

        public static float[,] Crop(this float[,] data, int newXSize, int newYSize)
        {
            float[,] result = new float[newXSize, newYSize];

            for (int x = 0; x < newXSize; x++) {
                for (int y = 0; y < newYSize; y++) {
                    result[x,y] = data[x, y];
                }
            }

            return result;
        }

        public static float[] Flatten(this float[,] data)
        {
            int size = data.Length;
            float[] result = new float[size];

            int write = 0;
            for (int x = 0; x <= data.GetUpperBound(0); x++) {
                for (int y = 0; y <= data.GetUpperBound(1); y++) {
                    result[write++] = data[x, y];
                }
            }

            return result;
        }
    }
}