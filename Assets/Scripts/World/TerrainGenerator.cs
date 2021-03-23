using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class TerrainGenerator : MonoBehaviour, ITerrainGenerator
{  
    public bool autoUpdate = true;

    [SerializeField] int seed = 0;
    [SerializeField] [Range(0, 50)] float verticalScale = 1;
    [SerializeField] [Range(0, 1)] float seaLevel;
    [SerializeField] TerrainType[] regions = null;

    private NodeGrid<Tile> _world;
    private int width;
    private int height;
    private float[,] heightMap;

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

    public void Generate(NodeGrid<Tile> world)
    {
        _world = world;
        width = _world.GridSizeX;
        height = _world.GridSizeY;

        Generate();
    }

    public void Generate()
    {
        if (_world != null) {
            
            heightMap = new float[width, height];

            foreach (IHeightMapGenerator heightMapGenerator in heightMapGenerators) {
                heightMap = heightMap.Add(heightMapGenerator.GenerateHeightMap(seed, width, height));
            }

            heightMap = heightMap.Normalize(-verticalScale * seaLevel, verticalScale * (1 - seaLevel));

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {

                    Tile t = _world.GetNodeAt(x, y);
                    t.Altitude = heightMap[x, y];

                    for (int i = 0; i < regions.Length; i++) {
                        if (t.Altitude <= regions[i].height * verticalScale) {
                            t.TypeId = regions[i].typeId;
                            break;
                        }
                    }

                    t.Water.Depth = Mathf.Max(0, -t.Altitude);
                }
            }
        }
    }
}

