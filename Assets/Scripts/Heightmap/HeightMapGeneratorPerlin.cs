using UnityEngine;
using Extensions;

public class HeightMapGeneratorPerlin : MonoBehaviour, IHeightMapGenerator
{
    [SerializeField] [Range(1, 100)] float scale = 19;
    [SerializeField] [Range(1,10)] int octaves = 4;
    [SerializeField] [Range(0, 1)] float persistence = 0.5f;
    [SerializeField] [Range(1, 5)] float lacunarity = 2.5f;
    [SerializeField] Vector2 offset = new Vector2(0, 0);
    [SerializeField] [Range(0, 5)] private float power = 1;
    [SerializeField] [Range(0,1)] float weight = 1;
    
    public float[,] GenerateHeightMap(int seed, int mapWidth, int mapHeight)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapHeight; y++) {
                float amplitude = 1;
                float frequency = 1;
                float height = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (x - halfWidth) / scale * frequency  + (frequency * octaveOffsets[i].x);
                    float sampleY = (y - halfHeight) / scale * frequency + (frequency * octaveOffsets[i].y);

                    float noiseValue = Mathf.PerlinNoise(sampleX, sampleY);

                    height += noiseValue * amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                maxHeight = height > maxHeight ? height : maxHeight;
                minHeight = height < minHeight ? height : minHeight;

                noiseMap[x, y] = power != 1 ? Mathf.Pow(height, power) : height;
            }
        }

        return noiseMap.Normalize(0, weight, minHeight, maxHeight);
    }
}