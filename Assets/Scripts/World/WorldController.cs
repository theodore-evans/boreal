using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour, ITileSubscriber
{
    public NodeGrid<Tile> world { get; protected set; }

    public float WorldVerticalScale { get => terrainGenerator.VerticalScale; }

    [SerializeField] private Vector3 origin = new Vector3(0, 0, 0);
    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;
    private float nodeSpacing = 1f;

    private Action<NodeGrid<Tile>> cbWorldCreated;

    ITerrainGenerator terrainGenerator;
    IChunkLoader chunkLoader;

    [SerializeField] bool verbose = true;

    private void Start()
    {
        terrainGenerator = GetComponent<TerrainGenerator>();
        chunkLoader = GetComponent<ChunkLoader>();

        world = CreateWorldGrid(origin, width, height, nodeSpacing);
        Debug.Log($"{this} created world {width} x {height}");

        cbWorldCreated?.Invoke(world);
        terrainGenerator.Generate(world);
    }

    private NodeGrid<Tile> CreateWorldGrid(Vector3 origin, int width, int height, float nodeSpacing)
    {
        world = new NodeGrid<Tile>(origin, width, height, nodeSpacing);
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                world.AddNode(new Tile(x, y, nodeSpacing, this));
            }
        }
        return world;
    }

    public void OnTileChanged(Tile tile)
    {
        chunkLoader.AddChangedTile(tile);
    }

    public void RegisterWorldCreatedCallback(Action<NodeGrid<Tile>> callback)
    {
        //TODO: extract logging to its own class
        if (verbose) Debug.Log($"{callback.Method.DeclaringType.Name} registered world created callback: {callback.Method.Name}");
        cbWorldCreated += callback;
    }

    public void RegisterWorldChangedCallback(Action<IEnumerable<Tile>> callback)
    {
        if (verbose) Debug.Log($"{callback.Method.DeclaringType.Name} registered world changed callback: {callback.Method.Name}");
        chunkLoader.RegisterCallback(callback);
    }

    public void DeregisterWorldChangedCallback(Action<IEnumerable<Tile>> callback)
    {
        if (verbose) Debug.Log($"{callback.Method.DeclaringType.Name} deregistered world changed callback: {callback.Method.Name}");
        chunkLoader.DeregisterCallback(callback);
    }

    private void OnValidate()
    {
        width = width < 1 ? 1 : width;
        height = height < 1 ? 1 : height;
    }
}
