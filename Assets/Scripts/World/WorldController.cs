using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public SpaceGrid<Tile> world { get; protected set; }
    public int WorldWidth { get => width; private set => width = Mathf.Max(1, value); }
    public int WorldHeight { get => height; private set => height = Mathf.Max(1, value); }

    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;

    HashSet<Tile> changedTiles;
    HashSet<Tile> changedTilesThisFrame;

    [SerializeField] int maxCallbacksPerFrame = 100;

    private Action<SpaceGrid<Tile>> cbWorldCreated;
    private Action<HashSet<Tile>> cbWorldChanged;

    private void Start()
    {
        world = CreateWorldGrid();
        changedTiles = new HashSet<Tile>();
        changedTilesThisFrame = new HashSet<Tile>();

        TerrainGenerator terrainGenerator = GetComponent<TerrainGenerator>();
        terrainGenerator.Generate(world);
        cbWorldCreated?.Invoke(world);
    }

    private SpaceGrid<Tile> CreateWorldGrid()
    {
        world = new SpaceGrid<Tile>(gameObject.transform.position, width, height, 1f);
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Tile newTile = new Tile(x, y);
                newTile.RegisterTileChangedCallback(OnTileChanged);
                world.SetNodeAt(x, y, newTile);
            }
        }
        return world;
    }

    private void Update()
    {
        if (changedTiles.Count > 0) {
            int count = 0;

            foreach (Tile changedTile in changedTiles) {
                changedTilesThisFrame.Add(changedTile);
                count++;
                if (count >= maxCallbacksPerFrame) break;
            }

            foreach (Tile changedTileThisFrame in changedTilesThisFrame) {
                changedTiles.Remove(changedTileThisFrame);
            }

            cbWorldChanged?.Invoke(changedTilesThisFrame);
            changedTilesThisFrame.Clear();
        }
    }

    void OnTileChanged(Tile t) // TODO implement update-based callback caching
    {
        changedTiles.Add(t);
    }

    public void RegisterWorldCreatedCallback(Action<SpaceGrid<Tile>> callback)
    {
        cbWorldCreated += callback;
    }

    public void RegisterWorldChangedCallback(Action<HashSet<Tile>> callback)
    {
        cbWorldChanged += callback;
    }

    public void UnregisterWorldChangedCallback(Action<HashSet<Tile>> callback)
    {
        cbWorldChanged -= callback;
    }

    private void OnValidate()
    {
        WorldWidth = width;
        WorldHeight = height;
    }
}
