using System;
using UnityEngine;

public class Tile
{

    Action<Tile> cbTileChanged;

    public FurnitureObject Furniture { get; protected set; }

    World world;

    private string _type = "Blank";

    public string Type
    {
        get
        {
            return _type;
        }
        set
        {
            string oldType = _type;
            _type = value;

            // call callback, notify of change
            if (cbTileChanged != null && oldType != _type)
                cbTileChanged(this);
        }
    }

    private float _altitude = 0.5f;

    public float Altitude
    {
        get
        {
            return _altitude;
        }
        set
        {
            float oldAltitude = _altitude;
            _altitude = value;

            //call callback, notify of change
            if (cbTileChanged != null && oldAltitude != _altitude)
                cbTileChanged(this);
        }
    }

    int x;
    public int X
    {
        get { return x; }
    }

    int y;
    public int Y
    {
        get { return y; }
    }

    public Tile(World world, int x, int y)
    {
        this.world = world;
        this.x = x;
        this.y = y;
    }

    public bool AddFurniture(FurnitureObject furnInstance)
    {
        if (furnInstance == null)
        {
            RemoveFurniture();
            return true;
        }

        if (Furniture != null)
        {
            Debug.Log("Tile.AddFurniture -- could not add furniture to tile already containing furniture!");
            return false;
        }

        Furniture = furnInstance;
        return true;

    }

    public bool RemoveFurniture()
    {

        if (Furniture != null)
        {
            Furniture = null;
            return true;
        }

        else
        {
            Debug.Log("Tile.RemoveFurniture -- no furniture to remove!");
            return false;
        }
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
