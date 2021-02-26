using UnityEngine;

public class PerlinHeightMapGenerator : MonoBehaviour, IHeightMapGenerator
{
    public float VerticalScale { get; protected set; } = 1;

    [SerializeField] float scale = 19;
    [SerializeField] int octaves = 4;
    [SerializeField] [Range(0, 1)] float persistence = 0.5f;
    [SerializeField] float lacunarity = 2.5f;
    [SerializeField] Vector2 offset = new Vector2(0, 0);

    public TerrainGenerator terrainGenerator { get; protected set; }

    private void Start()
    {
        terrainGenerator = GetComponent<TerrainGenerator>();
    }

    public float[,] GenerateHeightMap(int seed, int mapWidth, int mapHeight, float eps = (float)1e-4)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        float minNoiseHeight = float.MaxValue;
        float maxNoiseHeight = float.MinValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapHeight; y++) {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (x - halfWidth) / (scale + eps) * frequency  + (frequency * octaveOffsets[i].x);
                    float sampleY = (y - halfHeight) / (scale + eps) * frequency + (frequency * octaveOffsets[i].y);

                    float noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    noiseHeight += noiseValue * amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                noiseMap[x, y] = VerticalScale * Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
        return noiseMap;

    }
}