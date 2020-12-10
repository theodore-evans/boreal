using System.Collections.Generic;
using UnityEngine;

public class PerlinTerrainGen : MonoBehaviour, ITerrainGenerator
{
    private SpaceGrid<Tile> _world;

    //hack
    private Color[] tileColours = new Color[] {
        Color.white,  //Blank
        Color.blue,   //Water
        Color.red,   //Soil
        Color.green   //Grass
    };

    public bool autoUpdate;
    //TODO pack up in NoiseData scriptable object - https://www.youtube.com/watch?v=2IZ-99ueB4A
    [SerializeField] int seed = 0;
    [SerializeField] float verticalScale = 1;
    [SerializeField] float scale = 19;
    [SerializeField] int octaves = 4;
    [SerializeField] [Range(0, 1)] float persistence = 0.5f;
    [SerializeField] float lacunarity = 2.5f;
    [SerializeField] Vector2 offset = new Vector2(0,0);
    [SerializeField] TerrainType[] regions = null;

    public MapDisplay display;
    Mesh terrainMesh;

    public void RandomizeSeed()
    {
        seed = Random.Range(-100000, 100000);
    }

    public void Initialise(WorldController wc)
    {
        _world = wc.world;
        terrainMesh = MeshGenerator.GenerateTerrainMesh(_world.GridSizeX + 1, _world.GridSizeY + 1);
    }

    public void Generate()
    {
        if (_world != null) {

            int width = _world.GridSizeX;
            int height = _world.GridSizeY;

            float[,] reliefMap = Noise.GenerateNoiseMap(width, height, seed, scale, octaves, persistence, lacunarity, offset);

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {

                    Tile t = _world.GetNodeAt(x, y);
                    t.Altitude = reliefMap[x, y] * verticalScale;

                    // TODO extract tile type logic to a new class
                    for (int i = 0; i < regions.Length; i++) {

                        if (t.Altitude <= regions[i].height * verticalScale) {
                            t.Type = regions[i].name;
                            break;
                        }
                    }
                }
            }

            display.DrawMesh(terrainMesh, TextureGenerator.TextureFromWorldData(_world, tileColours));
        }
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

