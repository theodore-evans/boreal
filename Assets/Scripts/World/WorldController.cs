using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
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

    Cache<Tile> tilesToUpdate = new Cache<Tile>();
    Cache<Tile> tilesToUpdateThisLoop = new Cache<Tile>();

    private Action<NodeGrid<Tile>> cbWorldCreated;
    private Action<IEnumerable<Tile>> cbWorldChanged;

    TerrainGenerator terrainGenerator;
    [SerializeField] Camera currentCamera = null;
    [SerializeField] [Range(0, 2)] float drawDistance = 1;
    [SerializeField] [Range(0,1)] float onScreenUpdateRate = 0.5f;
    [SerializeField] [Range(0, 1)] float offScreenUpdateRate = 0.5f;
    [SerializeField] [Range(0, 5000)] int minUpdatesPerFrame = 1000;

    System.Random rng = new System.Random();

    public bool TileIsOnScreen(Tile tile)
    {
        Vector3 tilePositionOnScreen = currentCamera.WorldToScreenPoint(new Vector3(tile.X, tile.Y));

        return !(tilePositionOnScreen.x < -drawDistance
                || tilePositionOnScreen.y < -drawDistance
                || tilePositionOnScreen.x > Screen.width + drawDistance
                || tilePositionOnScreen.y > Screen.height + drawDistance);
    }

    private Rect GetAreaToLoad(float drawDistance = 1)
    {
        float widthMargin = Screen.width * drawDistance;
        float heightMargin = Screen.height * drawDistance;

        Vector2 bottomLeft = currentCamera.ScreenToWorldPoint(new Vector2(-widthMargin, -heightMargin));
        Vector2 topRight = currentCamera.ScreenToWorldPoint(new Vector2(Screen.width + widthMargin, Screen.height + heightMargin));

        return Rect.MinMaxRect(bottomLeft.x, bottomLeft.y, topRight.x, topRight.y);
    }

    private void Start()
    {
        terrainGenerator = GetComponent<TerrainGenerator>();

        CreateWorld();

        StartCoroutine(nameof(InvokeTileUpdatesCoroutine));
        StartCoroutine(nameof(CacheTileUpdatesCoroutine));
    }

    public void CreateWorld()
    {
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
                newTile.RegisterTileChangedCallback(OnTileChanged);
                world.SetNodeAt(x, y, newTile);
            }
        }
        return world;
    }


    private IEnumerator CacheTileUpdatesCoroutine()
    {
       
        for (; ; ) {
            if (tilesToUpdate.Count > 0) {
                Rect areaToLoad = GetAreaToLoad(drawDistance);
                tilesToUpdateThisLoop.Union(tilesToUpdate.PopAllWithinArea(areaToLoad));
            }

            yield return 0;
        }
    }

    private IEnumerator InvokeTileUpdatesCoroutine()
    {
        for (; ; ) {
            if (tilesToUpdateThisLoop.Count > 0) {
                int tilesToUpdateThisFrame = Mathf.Max(Mathf.CeilToInt(onScreenUpdateRate * tilesToUpdateThisLoop.Count), minUpdatesPerFrame);
                cbWorldChanged?.Invoke(tilesToUpdateThisLoop.DrawRandom(tilesToUpdateThisFrame));
            }
            else if (tilesToUpdate.Count > 0) {
                int tilestoUpdateThisFrame = Mathf.Max(Mathf.CeilToInt(offScreenUpdateRate * tilesToUpdate.Count), minUpdatesPerFrame);
                cbWorldChanged?.Invoke(tilesToUpdate.Draw(tilestoUpdateThisFrame));

            }
            yield return new WaitForEndOfFrame();
        }
    }

    void OnTileChanged(Tile t)
    {
        tilesToUpdate.Add(t);
    }

    public void RegisterWorldCreatedCallback(Action<NodeGrid<Tile>> callback)
    {
        cbWorldCreated += callback;
    }

    public void RegisterWorldChangedCallback(Action<IEnumerable<Tile>> callback)
    {
        cbWorldChanged += callback;
    }

    public void UnregisterWorldChangedCallback(Action<IEnumerable<Tile>> callback)
    {
        cbWorldChanged -= callback;
    }

    private void OnValidate()
    {
        WorldWidth = width;
        WorldHeight = height;
    }
}