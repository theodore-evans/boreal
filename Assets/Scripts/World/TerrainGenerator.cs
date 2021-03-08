using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class TerrainGenerator : MonoBehaviour
{
    private SpaceGrid<Tile> _world;
    public bool autoUpdate = true;

    [SerializeField] [Range(0,1)] float waterLevel = 0;
    [SerializeField] TerrainType[] regions = null;
    [SerializeField] [Range(0, 50)] float verticalScale = 1;
    [SerializeField] int seed = 0;

    IHeightMapGenerator[] heightMapGenerators;

    public float VerticalScale { get => verticalScale; }

    private void Awake()
    {
        heightMapGenerators = GetComponents<IHeightMapGenerator>();
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

            float[,] reliefMap = new float[width, height];

            foreach (IHeightMapGenerator heightMapGenerator in heightMapGenerators) {
                reliefMap = reliefMap.Add(heightMapGenerator.GenerateHeightMap(seed, width, height));
            }

            reliefMap = reliefMap.Normalize(0,1);

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {

                    Tile t = _world.GetNodeAt(x, y);
                    t.Altitude = reliefMap[x, y] * VerticalScale;

                    for (int i = 0; i < regions.Length; i++) {
                        if (t.Altitude <= regions[i].height * VerticalScale) {
                            t.TypeId = regions[i].typeId;
                            break;
                        }
                    }

                    t.WaterDepth = Mathf.Max(0, waterLevel * VerticalScale - t.Altitude);

                }
            }
        }
    }
}

