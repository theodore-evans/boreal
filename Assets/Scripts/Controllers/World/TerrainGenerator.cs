using UnityEngine;

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
}

public class TerrainGenerator : MonoBehaviour
{
    World world;
    int width, height;

    public int seed;
    public float scale;
    public int octaves;
    [Range(0, 1)]
    public float persistence;
    public float lacunarity;

    public TerrainType[] regions;

    public Vector2 offset = new Vector2(0, 0);

    public bool autoUpdate;

    public void SetWorld(World newWorld)
    {
        world = newWorld;
        width = world.Width;
        height = world.Height;
    }

    public void GenerateTerrain(World newWorld)
    {
        SetWorld(newWorld);
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        if (world == null) {
            return;
        }

        float[,] reliefMap = Noise.GenerateNoiseMap(width, height, seed, scale, octaves, persistence, lacunarity, offset);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Tile t = world.GetTileAt(x, y);
                t.Altitude = reliefMap[x, y];

                for (int i = 0; i < regions.Length; i++) {
                    if (t.Altitude <= regions[i].height) {
                        t.Type = regions[i].name;
                        break;
                    }
                }

            }
        }
    }

    public void RandomizeTerrain()
    {
        seed = new System.Random().Next(-100000, 100000);
        GenerateTerrain();
    }

    void OnValidate()
    {
        if (lacunarity < 1) {
            lacunarity = 1;
        }
        if (octaves < 0) {
            octaves = 0;
        }
    }

}

