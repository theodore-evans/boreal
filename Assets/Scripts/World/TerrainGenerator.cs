using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class TerrainGenerator : MonoBehaviour, ITerrainGenerator
{  
    public bool autoUpdate = true;

    [SerializeField] int seed = 0;
    [SerializeField] [Range(0, 50)] float verticalScale = 1;
    [SerializeField] [Range(0, 1)] float seaLevel;

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

            foreach (IHeightMapGenerator generator in heightMapGenerators) {
                if (generator.Type == HeightMapType.Add) {
                    heightMap = heightMap.Add(generator.GenerateHeightMap(seed, width, height), generator.Weight);
                }
                else if (generator.Type == HeightMapType.Multiply) {
                    heightMap = heightMap.Multiply(generator.GenerateHeightMap(seed, width, height), generator.Weight);
                }
            }

            heightMap = heightMap.Normalize(-verticalScale * seaLevel, verticalScale * (1 - seaLevel));

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {

                    Tile t = _world.GetNodeAt(x, y);
                    t.Relief.Elevation = heightMap[x, y];

                    t.Water.Depth = Mathf.Max(0, -t.Relief.Elevation);
                    t.Water.Saturation = t.Relief.Elevation < 0 ? 1 : 0;
                }
            }
        }
    }
}

