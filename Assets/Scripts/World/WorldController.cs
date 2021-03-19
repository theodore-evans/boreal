using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public NodeGrid<Tile> world { get; protected set; }
    public int WorldWidth { get => width; private set => width = Mathf.Max(1, value); }
    public int WorldHeight { get => height; private set => height = Mathf.Max(1, value); }
    public float WorldVerticalScale { get => terrainGenerator.VerticalScale; }

    public Vector3 Origin { get; protected set; } = new Vector3(0, 0, 0);
    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;
    private float nodeSpacing = 1f;

    private Action<NodeGrid<Tile>> cbWorldCreated;

    ITerrainGenerator terrainGenerator;
    ITileUpdater tileUpdater;

    private void Start()
    {
        terrainGenerator = GetComponent<TerrainGenerator>();
        tileUpdater = GetComponent<TileUpdater>();

        world = CreateWorldGrid(Origin, width, height, nodeSpacing);

        terrainGenerator.Generate(world);
        cbWorldCreated?.Invoke(world);
    }

    private NodeGrid<Tile> CreateWorldGrid(Vector3 origin, int width, int height, float nodeSpacing)
    {
        world = new NodeGrid<Tile>(origin, width, height, nodeSpacing);
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Tile newTile = new Tile(x, y, nodeSpacing);
                newTile.RegisterTileChangedCallback(tileUpdater.AddChangedTile);
                world.SetNodeAt(x, y, newTile);
            }
        }
        return world;
    }

    public void RegisterWorldCreatedCallback(Action<NodeGrid<Tile>> callback)
    {
        cbWorldCreated += callback;
    }

    // TODO: seems like duplication here, better solution maybe required
    public void RegisterWorldChangedCallback(Action<IEnumerable<Tile>> callback)
    {
        tileUpdater.RegisterCallback(callback);
    }

    public void DeregisterWorldChangedCallback(Action<IEnumerable<Tile>> callback)
    {
        tileUpdater.DeregisterCallback(callback);
    }

    private void OnValidate()
    {
        WorldWidth = width;
        WorldHeight = height;
    }
}
