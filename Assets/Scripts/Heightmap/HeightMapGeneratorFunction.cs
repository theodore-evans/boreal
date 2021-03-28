using UnityEngine;
using System.Collections;
using System;
using Extensions;


public class HeightMapGeneratorFunction : MonoBehaviour, IHeightMapGenerator
{
    private enum HeightmapFunction
    {
        Gradient = 0,
        SineProduct = 1,
        SineSum = 2

    }

    private Func<float, float, float> generatingFunction;

    [SerializeField] HeightmapFunction function = HeightmapFunction.SineProduct;
    [SerializeField] float xParameter = 1;
    [SerializeField] float yParameter = 1;
    [SerializeField] [Range(-1, 1f)] float weight;
    [SerializeField] HeightMapType type = HeightMapType.Add;

    public float Weight { get => weight; }
    public HeightMapType Type { get => type; }

    private int width;
    private int height;

    public float[,] GenerateHeightMap(int seed, int mapWidth, int mapHeight)
    {
        width = mapWidth;
        height = mapHeight;

        float[,] heightMap = new float[mapWidth, mapHeight];

        switch (function) { //TODO implement this with inheritance
            case HeightmapFunction.SineProduct:
                generatingFunction = SineProduct; break;
            case HeightmapFunction.SineSum:
                generatingFunction = SineSum; break;
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

        return heightMap.Normalize(0, 1, minHeight, maxHeight);
    }

    private float SineProduct(float x, float y)
    {
        return (float)(Math.Sin(Math.PI * x * xParameter / width) * Math.Sin(Math.PI * y * yParameter / height));
    }

    private float SineSum(float x, float y)
    {
        return (float)(Math.Sin(Math.PI * (x * xParameter + y * yParameter) / width));
    }

    private float Gradient(float x, float y)
    {
        return (x * xParameter / width) + (y * yParameter / height);
    }
}
