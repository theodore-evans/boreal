using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class TerrainGenerator : MonoBehaviour
{
    private NodeGrid<Tile> _world;
    public bool autoUpdate = true;

    [SerializeField] int seed = 0;
    [SerializeField] [Range(0, 50)] float verticalScale = 1;
    [SerializeField] [Range(0, 1)] float seaLevel = 0;
    [SerializeField] [Range(0, 1)] float waterLevelAdjustment = 0;
    [SerializeField] TerrainType[] regions = null;
    
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

            reliefMap = reliefMap.Normalize(-verticalScale * seaLevel, verticalScale * (1 - seaLevel));

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {

                    Tile t = _world.GetNodeAt(x, y);
                    t.Altitude = reliefMap[x, y];

                    for (int i = 0; i < regions.Length; i++) {
                        if (t.Altitude <= regions[i].height * verticalScale) {
                            t.TypeId = regions[i].typeId;
                            break;
                        }
                    }

                    t.WaterDepth = Mathf.Max(0, -t.Altitude + waterLevelAdjustment * verticalScale);

                }
            }
        }
    }
}

