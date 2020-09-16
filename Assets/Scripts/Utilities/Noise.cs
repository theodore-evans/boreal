using UnityEngine;

public class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
    {

        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale == 0)
        {
            scale = 0.0001f;
        }

        float minNoiseHeight = float.MaxValue;
        float maxNoiseHeight = float.MinValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    // calculate sample indices based on the coordinates and the scale
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    // generate value from PerlinNoise
                    float noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += noiseValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }

        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
        return noiseMap;

    }

    public static float NextGaussian(System.Random prng, float mu = 0, float sigma = 1)
    {
        float u1 = (float)prng.NextDouble();
        float u2 = (float)prng.NextDouble();

        float rand_std_normal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                            Mathf.Sin(2.0f * Mathf.PI * u2);

        float rand_normal = mu + sigma * rand_std_normal;

        return rand_normal;
    }

}
