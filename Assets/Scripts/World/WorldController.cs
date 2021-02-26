using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public SpaceGrid<Tile> world { get; protected set; }
    public int WorldWidth { get => width; private set => width = Mathf.Max(1, value); }
    public int WorldHeight { get => height; private set => height = Mathf.Max(1, value); }
    public float WorldVerticalScale { get => terrainGenerator.VerticalScale; }

    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;
    private float nodeSpacing = 1f;

    HashSet<Tile> changedTiles;
    HashSet<Tile> changedTilesThisFrame;

    [SerializeField] int maxCallbacksPerFrame = 100;

    private Action<SpaceGrid<Tile>> cbWorldCreated;
    private Action<HashSet<Tile>> cbWorldChanged;

    TerrainGenerator terrainGenerator;

    public bool TileIsInFrame(Tile tile)
    {
        Camera cam = Camera.main;
        Vector3 tilePositionOnScreen = cam.WorldToScreenPoint(new Vector3(tile.X, tile.Y));
        return (tilePositionOnScreen.x > -1
                || tilePositionOnScreen.x < Screen.width + 1
                || tilePositionOnScreen.y > -1
                || tilePositionOnScreen.y < Screen.height + 1);
    }

    private void Start()
    {
        world = CreateWorldGrid();
        changedTiles = new HashSet<Tile>();
        changedTilesThisFrame = new HashSet<Tile>();

        terrainGenerator = GetComponent<TerrainGenerator>();
        terrainGenerator.Generate(world);
        cbWorldCreated?.Invoke(world);
    }

    private SpaceGrid<Tile> CreateWorldGrid()
    {
        world = new SpaceGrid<Tile>(gameObject.transform.position, width, height, nodeSpacing);
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Tile newTile = new Tile(x, y, nodeSpacing);
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
                if (TileIsInFrame(changedTile)) {
                    changedTilesThisFrame.Add(changedTile);
                    count++;
                }
                
                if (count >= maxCallbacksPerFrame) break;
            }

            foreach (Tile changedTileThisFrame in changedTilesThisFrame) {
                changedTiles.Remove(changedTileThisFrame);
            }

            cbWorldChanged?.Invoke(changedTilesThisFrame);
            changedTilesThisFrame.Clear();
        }
    }

    void OnTileChanged(Tile t)
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
