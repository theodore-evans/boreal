using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public SpaceGrid<Tile> world { get; protected set; }
    public int WorldWidth { get => width; private set => width = Mathf.Max(1, value); }
    public int WorldHeight { get => height; private set => height = Mathf.Max(1, value); }
    public float WorldVerticalScale { get => terrainGenerator.VerticalScale; }

    public Vector3 Origin { get; protected set; } = new Vector3(0, 0, 0);
    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;
    private float nodeSpacing = 1f;

    HashSet<Tile> tilesToUpdate;
    List<Tile> tilesToUpdateThisLoop;

    private Action<SpaceGrid<Tile>> cbWorldCreated;
    private Action<IEnumerable<Tile>> cbWorldChanged;

    TerrainGenerator terrainGenerator;
    [SerializeField] Camera currentCamera = null;
    [SerializeField] int drawDist = 256;
    [SerializeField] int maxInFrameUpdates = 2000;
    [SerializeField] int maxOutOfFrameUpdates = 10000;

    System.Random rng;

    public bool TileIsOnScreen(Tile tile)
    {
        Vector3 tilePositionOnScreen = currentCamera.WorldToScreenPoint(new Vector3(tile.X, tile.Y));

        return !(tilePositionOnScreen.x < -drawDist
                || tilePositionOnScreen.y < -drawDist
                || tilePositionOnScreen.x > Screen.width + drawDist
                || tilePositionOnScreen.y > Screen.height + drawDist);
    }

    private Rect GetAreaToLoad(int margin = 0)
    {
        Vector2 bottomLeft = currentCamera.ScreenToWorldPoint(new Vector2(-margin, -margin));
        Vector2 topRight = currentCamera.ScreenToWorldPoint(new Vector2(Screen.width + margin, Screen.height + margin));

        return Rect.MinMaxRect(bottomLeft.x, bottomLeft.y, topRight.x, topRight.y);
    }

    private void Start()
    {
        world = CreateWorldGrid();
        tilesToUpdate = new HashSet<Tile>();
        rng = new System.Random();
        tilesToUpdateThisLoop = new List<Tile>();
        terrainGenerator = GetComponent<TerrainGenerator>();
        terrainGenerator.Generate(world);
        cbWorldCreated?.Invoke(world);

        StartCoroutine(nameof(InvokeTileUpdatesCoroutine));
        StartCoroutine(nameof(CacheTileUpdatesCoroutine));
    }

    private SpaceGrid<Tile> CreateWorldGrid()
    {
        world = new SpaceGrid<Tile>(Origin, width, height, nodeSpacing);
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
                HashSet<Tile> tilesToUpdateCopy = new HashSet<Tile>(tilesToUpdate);
                Rect areaToLoad = GetAreaToLoad(drawDist);

                foreach (Tile tileToUpdate in tilesToUpdateCopy) {
                    if (tileToUpdate.IsInRect(areaToLoad)) { // use better accessor for this > HashSet & Array
                        tilesToUpdateThisLoop.Add(tileToUpdate);
                        tilesToUpdate.Remove(tileToUpdate);

                    }
                }
            }

            yield return 0;
        }
    }

    private IEnumerator InvokeTileUpdatesCoroutine()
    {
        for (; ; ) {
            if (tilesToUpdateThisLoop.Count > 0) {
                tilesToUpdateThisLoop.Shuffle();
                cbWorldChanged?.Invoke(tilesToUpdateThisLoop.PopLastRange(maxInFrameUpdates));
                
            }
            else if (tilesToUpdate.Count > 0){
                tilesToUpdateThisLoop = tilesToUpdate.Take(maxOutOfFrameUpdates).ToList();
                cbWorldChanged?.Invoke(tilesToUpdateThisLoop);
                tilesToUpdate.ExceptWith(tilesToUpdateThisLoop);
                tilesToUpdateThisLoop.Clear();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void OnTileChanged(Tile t)
    {
        tilesToUpdate.Add(t);
    }

    public void RegisterWorldCreatedCallback(Action<SpaceGrid<Tile>> callback)
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
