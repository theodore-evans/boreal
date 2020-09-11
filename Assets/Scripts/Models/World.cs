using System;
using UnityEngine;

public class World
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    Tile[,] tiles;

    Action<Tile> cbTileChanged;

    public World(int width, int height)
    {
        Width = width;
        Height = height;

        tiles = new Tile[width, height];

        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                tiles[x, y] = new Tile(x, y);
                tiles[x, y].RegisterTileChangedCallback(OnTileChanged);
            }
        }

        Debug.Log("World created with " + (Width * Height) + " tiles.");
    }

    public Tile GetTileAt(int x, int y)
    {
        if (x > Width || x < 0 || y > Height || y < 0) {
            return null;
        }

        Tile currTile = tiles[x,y];

        if (currTile == null) {
            Debug.LogError($"{this}: No tile at {x}, {y})");
            return null;
        }

        return tiles[x, y];
    }

    public Tile GetTileAt(Vector3 worldPoint)
    {
        int x = Mathf.FloorToInt(worldPoint.x);
        int y = Mathf.FloorToInt(worldPoint.y);

        return GetTileAt(x, y);
    }

    void OnTileChanged(Tile t)
    {
        cbTileChanged?.Invoke(t);
    }

    public void RegisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged += callback;
    }

    public void UnregisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged -= callback;
    }
    
}
