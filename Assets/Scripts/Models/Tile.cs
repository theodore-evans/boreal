using System;
using UnityEngine;

public class Tile
{
    Action<Tile> cbTileChanged;

    public int X { get; }
    public int Y { get; }

    private string _type = "Blank";
    private float _altitude = 0.5f;
    private Vector3 _normal = Vector3.zero;

    public string Type
    {
        get => _type;

        set {
            string oldType = _type;
            _type = value;

            if (cbTileChanged != null && oldType != _type)
                cbTileChanged(this);
        }
    }

    public float Altitude
    {
        get => _altitude;
        
        set {
            float oldAltitude = _altitude;
            _altitude = value;

            if (cbTileChanged != null && oldAltitude != _altitude)
                cbTileChanged(this);
        }
    }

    public Vector3 Normal
    {
        get => _normal;

        set {
            _normal = value;
        }
    }

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
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
