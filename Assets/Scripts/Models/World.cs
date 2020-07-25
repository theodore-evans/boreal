using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    Tile[,] tiles;
    Dictionary<string, FurnitureObject> FurniturePrototypes;
    //TODO: extract furniture management to a new class

    Action<Tile> cbTileChanged;
    Action<FurnitureObject> cbFurnitureChanged;

    public World(int width, int height)
    {
        Width = width;
        Height = height;

        tiles = new Tile[width, height];

        CreateFurniturePrototypes();

        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                tiles[x, y] = new Tile(this, x, y);
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
        return tiles[x, y];
    }

    void CreateFurniturePrototypes()
    {
        FurniturePrototypes = new Dictionary<string, FurnitureObject>();

        FurniturePrototypes.Add("Tree",
          FurnitureObject.CreatePrototype(
            "Tree",
            0,
            1,
            1
          )
        );
    }

    public void PlaceFurnitureOnTile(string furnitureType, Tile t)
    {
        if (FurniturePrototypes.ContainsKey(furnitureType) == false) {
            Debug.LogError($"World.FurniturePrototypes does not contain element with key {furnitureType}");
            return;
        }

        FurnitureObject furn = FurnitureObject.CreateInstance(FurniturePrototypes[furnitureType], t);

        cbFurnitureChanged?.Invoke(furn);
    }

    public void RemoveFurnituresFromTile(Tile t)
    {
        FurnitureObject furn = t.Furniture;

        if (furn != null) {
            t.RemoveFurniture();
        }
        else {
            Debug.Log($"World.RemoveFurnituresFromTile: Tile [{t.X}, {t.Y}] has no Furniture");
        }

        cbFurnitureChanged?.Invoke(furn);

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
    
    public void RegisterFurnitureChangedCallback(Action<FurnitureObject> callback)
    {
        cbFurnitureChanged += callback;
    }

    public void UnregisterFurnitureChangedCallback(Action<FurnitureObject> callback)
    {
        cbFurnitureChanged -= callback;
    }
}
