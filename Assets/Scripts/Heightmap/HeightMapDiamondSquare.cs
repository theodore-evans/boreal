using System;
using UnityEngine;
using Extensions;

public class HeightMapDiamondSquare: MonoBehaviour, IHeightMapGenerator
{
    private int _terrainPoints;

    [SerializeField] [Range(-4,4)] float roughness = 1;
    [SerializeField] [Range(-4,4)] float cornerSeed = 1; // an initial seed value for the corners of the data
    [SerializeField] [Range(0, 1)] float weight = 1;

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
        //size of grid to generate, note this must be a
        //value 2^n+1
        int DATA_SIZE = _terrainPoints + 1;

        float[,] data = new float[DATA_SIZE, DATA_SIZE];
        data[0, 0] = data[0, DATA_SIZE - 1] = data[DATA_SIZE - 1, 0] =
          data[DATA_SIZE - 1, DATA_SIZE - 1] = cornerSeed;

        float h = roughness;
        System.Random r = new System.Random(_seed);

        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        for (int sideLength = DATA_SIZE - 1; sideLength >= 2; sideLength /= 2, h /= 2.0f) {
            int halfSide = sideLength / 2;

            //generate the new square values
            for (int x = 0; x < DATA_SIZE - 1; x += sideLength) {
                for (int y = 0; y < DATA_SIZE - 1; y += sideLength) {
                    //x, y is upper left corner of square
                    //calculate average of existing corners
                    float avg = data[x, y] + //top left
                        data[x + sideLength, y] +//top right
                        data[x, y + sideLength] + //lower left
                        data[x + sideLength, y + sideLength];//lower right
                        avg /= 4.0f;

                    //center is average plus random offset
                    float newSquareValue = avg + (float)(r.NextDouble() * 2 * h) - h;
                    data[x + halfSide, y + halfSide] = newSquareValue;
                    maxHeight = newSquareValue > maxHeight ? newSquareValue : maxHeight;
                    minHeight = newSquareValue < minHeight ? newSquareValue : minHeight;
                }
            }
            //generate the diamond values
            //since the diamonds are staggered we only move x
            //by half side
            //NOTE: if the data shouldn't wrap then x < DATA_SIZE
            //to generate the far edge values
            for (int x = 0; x < DATA_SIZE - 1; x += halfSide) {
                //and y is x offset by half a side, but moved by
                //the full side length
                //NOTE: if the data shouldn't wrap then y < DATA_SIZE
                //to generate the far edge values
                for (int y = (x + halfSide) % sideLength; y < DATA_SIZE - 1; y += sideLength) {
                    //x, y is center of diamond
                    //note we must use mod  and add DATA_SIZE for subtraction 
                    //so that we can wrap around the array to find the corners
                    float avg =
                      data[(x - halfSide + DATA_SIZE) % DATA_SIZE, y] + //left of center
                      data[(x + halfSide) % DATA_SIZE, y] + //right of center
                      data[x, (y + halfSide) % DATA_SIZE] + //below center
                      data[x, (y - halfSide + DATA_SIZE) % DATA_SIZE]; //above center
                    avg /= 4.0f;

                    //new value = average plus random offset
                    //We calculate random value in range of 2h
                    //and then subtract h so the end value is
                    //in the range (-h, +h)
                    float newDiamondValue = avg + (float)(r.NextDouble() * 2 * h) - h;
                    //update value for center of diamond
                    data[x, y] = newDiamondValue;
                    maxHeight = newDiamondValue > maxHeight ? newDiamondValue : maxHeight;
                    minHeight = newDiamondValue < minHeight ? newDiamondValue : minHeight;

                    //wrap values on the edges, remove
                    //this and adjust loop condition above
                    //for non-wrapping values.
                    if (x == 0) data[DATA_SIZE - 1, y] = avg;
                    if (y == 0) data[x, DATA_SIZE - 1] = avg;
                }
            }
        }

        return data.Normalize(0, weight, minHeight, maxHeight);
    }
}
