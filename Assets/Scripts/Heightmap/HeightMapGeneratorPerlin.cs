using UnityEngine;
using Extensions;

[System.Serializable]
public class NoiseParameters
{
    [Range(1, 100)] public float scale = 19;
    [Range(1, 10)] public int octaves = 4;
    [Range(0, 1)] public float persistence = 0.5f;
    [Range(1, 5)] public float lacunarity = 2.5f;
    [Range(0, 5)] public float power = 1;

    public Vector2 offset = new Vector2(0, 0);

    public void ValidateValues()
    {
        scale = Mathf.Max(scale, 0.01f);
        octaves = Mathf.Max(octaves, 1);
        lacunarity = Mathf.Max(lacunarity, 1);
        persistence = Mathf.Clamp01(persistence);
    }
}

public class HeightMapGeneratorPerlin : MonoBehaviour, IHeightMapGenerator
{
    [SerializeField] NoiseParameters parameters;
    [SerializeField] [Range(-1, 1f)] float weight;
    [SerializeField] HeightMapType type = HeightMapType.Add;

    public float Weight { get => weight; }
    public HeightMapType Type { get => type; }

    public float[,] GenerateHeightMap(int seed, int mapWidth, int mapHeight)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[parameters.octaves];
        for (int i = 0; i < parameters.octaves; i++) {
            float offsetX = prng.Next(-100000, 100000) + parameters.offset.x;
            float offsetY = prng.Next(-100000, 100000) + parameters.offset.y;
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

                for (int i = 0; i < parameters.octaves; i++) {
                    float sampleX = (x - halfWidth) / parameters.scale * frequency  + (frequency * octaveOffsets[i].x);
                    float sampleY = (y - halfHeight) / parameters.scale * frequency + (frequency * octaveOffsets[i].y);

                    float noiseValue = Mathf.PerlinNoise(sampleX, sampleY);

                    height += noiseValue * amplitude;
                    amplitude *= parameters.persistence;
                    frequency *= parameters.lacunarity;
                }

                maxHeight = height > maxHeight ? height : maxHeight;
                minHeight = height < minHeight ? height : minHeight;

                noiseMap[x, y] = parameters.power == 1 ?  height : Mathf.Pow(height, parameters.power);
            }
        }

        return noiseMap.Normalize(0, 1, minHeight, maxHeight);
    }
}