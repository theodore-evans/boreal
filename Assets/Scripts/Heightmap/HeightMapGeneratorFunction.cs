using UnityEngine;
using System.Collections;
using System;
using Extensions;


public class HeightMapGeneratorFunction : MonoBehaviour, IHeightMapGenerator
{
    private enum HeightmapFunction
    {
        Sine = 0,
        Gradient = 1
    }

    private Func<float, float, float> generatingFunction;

    [SerializeField] HeightmapFunction function = HeightmapFunction.Sine;
    [SerializeField] float xParameter = 1;
    [SerializeField] float yParameter = 1;
    [SerializeField] [Range(0,1)] float weight = 1;

    private int width;
    private int height;

    public float[,] GenerateHeightMap(int seed, int mapWidth, int mapHeight)
    {
        width = mapWidth;
        height = mapHeight;

        float[,] heightMap = new float[mapWidth, mapHeight];

        switch (function) {
            case HeightmapFunction.Sine:
                generatingFunction = Sine; break;
            case HeightmapFunction.Gradient:
                generatingFunction = Gradient; break;
            default:
                generatingFunction = (a, b) => 0; break;
        }

        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapWidth; y++) {

                float value = generatingFunction(x, y);

                heightMap[x, y] = value;

                minHeight = value < minHeight ? value : minHeight;
                maxHeight = value > maxHeight ? value : maxHeight;
            }

        }

        return heightMap.Normalize(0, weight, minHeight, maxHeight);
    }

    private float Sine(float x, float y)
    {
        return (float)(Math.Sin(Math.PI * x * xParameter / width) * Math.Sin(Math.PI * y * yParameter / height));
    }

    private float Gradient(float x, float y)
    {
        return (x * xParameter / width) + (y * yParameter / height);
    }
}
