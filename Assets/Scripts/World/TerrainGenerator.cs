using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private SpaceGrid<Tile> _world;
    public bool autoUpdate = true;

    [SerializeField] int seed = 0;
    [SerializeField] [Range(0,1)] float waterLevel = 0;
    [SerializeField] TerrainType[] regions = null;

    IHeightMapGenerator heightmapGenerator;

    public float VerticalScale { get; private set; }

    private void Awake()
    {
        heightmapGenerator = GetComponent<IHeightMapGenerator>();
    }

    public void RandomizeSeed()
    {
        seed = Random.Range(-100000, 100000);
    }

    public void Generate(SpaceGrid<Tile> world)
    {
        _world = world;
        Generate();
    }

    public void Generate()
    {
        if (_world != null) {
            int width = _world.GridSizeX;
            int height = _world.GridSizeY;

            float[,] reliefMap = heightmapGenerator.GenerateHeightMap(seed, width, height);

            VerticalScale = heightmapGenerator.VerticalScale;

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {

                    Tile t = _world.GetNodeAt(x,y);
                    t.Altitude = reliefMap[x, y];

                    for (int i = 0; i < regions.Length; i++) {
                        if (t.Altitude <= regions[i].height * VerticalScale) {
                            t.Type = regions[i].name;
                            break;
                        }
                    }

                    t.WaterDepth = Mathf.Max(0, waterLevel * VerticalScale - t.Altitude);
                }
            }
        }
    }
}

