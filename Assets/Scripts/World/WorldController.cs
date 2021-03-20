using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public NodeGrid<Tile> world { get; protected set; }

    public float WorldVerticalScale { get => terrainGenerator.VerticalScale; }

    [SerializeField] private Vector3 origin = new Vector3(0, 0, 0);
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
                Tile newTile = new Tile(x, y, nodeSpacing);
                newTile.RegisterTileChangedCallback(tileUpdater.AddChangedTile);
                world.SetNodeAt(x, y, newTile);
            }
        }
        return world;
    }

    public void RegisterWorldCreatedCallback(Action<NodeGrid<Tile>> callback)
    {
        //TODO: extract logging to its own class
        Debug.Log($"{callback.Method.DeclaringType.Name} registered world created callback: {callback.Method.Name}");
        cbWorldCreated += callback;
    }

    public void RegisterWorldChangedCallback(Action<IEnumerable<Tile>> callback)
    {
        Debug.Log($"{callback.Method.DeclaringType.Name} registered world changed callback: {callback.Method.Name}");
        tileUpdater.RegisterCallback(callback);
    }

    public void DeregisterWorldChangedCallback(Action<IEnumerable<Tile>> callback)
    {
        Debug.Log($"{callback.Method.DeclaringType.Name} deregistered world changed callback: {callback.Method.Name}");
        tileUpdater.DeregisterCallback(callback);
    }

    private void OnValidate()
    {
        width = width < 1 ? 1 : width;
        height = height < 1 ? 1 : height;
    }
}
