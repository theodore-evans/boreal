using System;
using UnityEngine;
using Extensions;

[Serializable]
public class DiamondSquareParameters
{
    [Range(-2,2)] public float roughness = 1;
    public CornerSeed cornerSeed;

    [Serializable]
    public struct CornerSeed {
        [Range(-4, 4)] public float bottomLeft;
        [Range(-4, 4)] public float bottomRight;
        [Range(-4, 4)] public float topLeft;
        [Range(-4, 4)] public float topRight;
    }
}

public class HeightMapDiamondSquare: MonoBehaviour, IHeightMapGenerator
{
    private int _terrainPoints;

    [SerializeField] DiamondSquareParameters parameters;
    [SerializeField] [Range(-1, 1)] float weight = 1;
    [SerializeField] HeightMapType type = HeightMapType.Add;

    public float Weight { get => weight; }
    public HeightMapType Type { get => type;  }

    private int _seed;

    public float[,] GenerateHeightMap(int seed, int mapWidth, int mapHeight)
    {
        _terrainPoints = mapWidth; // map must be square with side power of two
        _seed = seed;
        float [,] data = DiamondSquareAlgorithm().Crop(mapWidth, mapHeight);
        return data;
    }

    // adapted from https://gist.github.com/awilki01/83b65ad852a0ab30192af07cda3d7c0b
    private float[,] DiamondSquareAlgorithm()
    {
        int DATA_SIZE = _terrainPoints + 1;

        float[,] data = new float[DATA_SIZE, DATA_SIZE];
        data[0, 0] = parameters.cornerSeed.bottomLeft;
        data[0, DATA_SIZE - 1] = parameters.cornerSeed.bottomRight;
        data[DATA_SIZE - 1, 0] = parameters.cornerSeed.topLeft;
        data[DATA_SIZE - 1, DATA_SIZE - 1] = parameters.cornerSeed.topRight;

        float h = parameters.roughness;
        System.Random r = new System.Random(_seed);

        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        for (int sideLength = DATA_SIZE - 1; sideLength >= 2; sideLength /= 2, h /= 2.0f) {
            int halfSide = sideLength / 2;

            for (int x = 0; x < DATA_SIZE - 1; x += sideLength) {
                for (int y = 0; y < DATA_SIZE - 1; y += sideLength) {
                    float avg = data[x, y] + 
                        data[x + sideLength, y] +
                        data[x, y + sideLength] + 
                        data[x + sideLength, y + sideLength];
                        avg /= 4.0f;

                    float newSquareValue = avg + (float)(r.NextDouble() * 2 * h) - h;
                    data[x + halfSide, y + halfSide] = newSquareValue;
                    maxHeight = newSquareValue > maxHeight ? newSquareValue : maxHeight;
                    minHeight = newSquareValue < minHeight ? newSquareValue : minHeight;
                }
            }

            for (int x = 0; x < DATA_SIZE - 1; x += halfSide) {

                for (int y = (x + halfSide) % sideLength; y < DATA_SIZE - 1; y += sideLength) {
                    float avg =
                      data[(x - halfSide + DATA_SIZE) % DATA_SIZE, y] + //left of center
                      data[(x + halfSide) % DATA_SIZE, y] + //right of center
                      data[x, (y + halfSide) % DATA_SIZE] + //below center
                      data[x, (y - halfSide + DATA_SIZE) % DATA_SIZE]; //above center
                    avg /= 4.0f;

                    float newDiamondValue = avg + (float)(r.NextDouble() * 2 * h) - h;

                    data[x, y] = newDiamondValue;
                    maxHeight = newDiamondValue > maxHeight ? newDiamondValue : maxHeight;
                    minHeight = newDiamondValue < minHeight ? newDiamondValue : minHeight;

                    if (x == 0) data[DATA_SIZE - 1, y] = avg;
                    if (y == 0) data[x, DATA_SIZE - 1] = avg;
                }
            }
        }

        return data.Normalize(0, 1, minHeight, maxHeight);
    }
}
