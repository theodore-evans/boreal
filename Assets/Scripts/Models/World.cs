using System;
using UnityEngine;
// TODO: eventually decouple from UnityEngine

public class World
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    Tile[,] tiles;
    NormalCalculator normalGenerator;

    Action<Tile> cbTileChanged;

    public World(int width, int height)
    {
        Width = width;
        Height = height;

        tiles = new Tile[width, height];
        normalGenerator = new NormalCalculator(this);

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
        if (x >= 0 && x < Width && y >= 0 && y < Height) {
            return tiles[x, y]; 
        }
        else return null;
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
