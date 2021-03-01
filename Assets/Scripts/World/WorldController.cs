using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
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
    HashSet<Tile> tilesToUpdateThisLoop;
    HashSet<Tile> tilesToUpdateAfterThisLoop;

    [SerializeField] int maxTilesToUpdatePerLoop = 8192;
    [SerializeField] int batchSize = 1024;

    private Action<SpaceGrid<Tile>> cbWorldCreated;
    private Action<HashSet<Tile>> cbWorldChanged;

    TerrainGenerator terrainGenerator;
    [SerializeField] Camera currentCamera = null;
    [SerializeField] float baseMargin = 256;
    float margin;

    private System.Random rng = new System.Random();

    public bool TileIsOnScreen(Tile tile)
    {
        Vector3 tilePositionOnScreen = currentCamera.WorldToScreenPoint(new Vector3(tile.X, tile.Y));

        return !(tilePositionOnScreen.x < -margin
                || tilePositionOnScreen.x > Screen.width + margin
                || tilePositionOnScreen.y < -margin
                || tilePositionOnScreen.y > Screen.height + margin);
    }

    private void Start()
    {
        world = CreateWorldGrid();
        tilesToUpdate = new HashSet<Tile>();
        tilesToUpdateThisLoop = new HashSet<Tile>();
        tilesToUpdateAfterThisLoop = new HashSet<Tile>();

        terrainGenerator = GetComponent<TerrainGenerator>();
        terrainGenerator.Generate(world);
        cbWorldCreated?.Invoke(world);

        margin = baseMargin * 10000 / maxTilesToUpdatePerLoop;

        StartCoroutine("TileUpdateCoroutine");
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

    IEnumerator TileUpdateCoroutine()
    {
        for (; ;){
            if (tilesToUpdate.Count > 0) {
                int tilesUpdated = 0;

                List<Tile> shuffledTilesToUpdate = RandomUtils.Shuffle(tilesToUpdate.ToList(), rng);
                foreach (Tile tileToUpdate in shuffledTilesToUpdate) {
                    if (tilesUpdated > maxTilesToUpdatePerLoop) break;

                    if (TileIsOnScreen(tileToUpdate)) {
                        tilesToUpdateThisLoop.Add(tileToUpdate);
                        tilesToUpdate.Remove(tileToUpdate);
                        tilesUpdated++;
                    }                    
                }

                cbWorldChanged?.Invoke(tilesToUpdateThisLoop);
                tilesToUpdateThisLoop.Clear();
            }
            yield return 0;
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
