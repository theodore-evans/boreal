using System;
using UnityEngine;

public enum typeId
{
    Blank = 0,
    Water = 1,
    Soil = 2,
    Grass = 3
}

public class Tile : AbstractNode, ISurface
{
    Action<Tile> cbTileChanged;

    private string _type = "Blank";
    private float _waterDepth = 0.0f;
    private float _altitude = 0.5f;
    private Vector3 _normal = Vector3.zero;

    public typeId TypeId
    {
        get {
            if (_waterDepth > 0.1) return typeId.Water;
            else if (_type == "Soil") return typeId.Soil;
            else if (_type == "Grass") return typeId.Grass;
            else return typeId.Blank;
        }
    }



    public string Type
    {
        get => _type;

        set {
            string oldType = _type;
            _type = value;

            if (oldType != _type) cbTileChanged?.Invoke(this);
        }
    }

    public float WaterDepth
    {
        get => _waterDepth;
        set {
            float oldDepth = _waterDepth;
            _waterDepth = value;

            if (oldDepth !=_waterDepth) cbTileChanged?.Invoke(this);
        }
    }

    public float WaterLevel
    {
        get => _altitude + _waterDepth;
    }

    public float Altitude
    {
        get => _altitude;

        set {
            float oldAltitude = _altitude;
            _altitude = value;

            if (oldAltitude != _altitude) cbTileChanged?.Invoke(this);
        }
    }

    public Vector3 Normal
    {
        get => _normal;

        set {
            _normal = value;
            Gradient = Mathf.Tan(Mathf.Deg2Rad * Vector3.Angle(Vector3.back, _normal));
            Downhill = Vector3.SqrMagnitude(Normal) * Vector3.forward - Vector3.Dot(Normal, Vector3.forward) * Normal;
        }
    }

    public Vector3 Downhill
    {
        get; protected set;
    }

    public float Gradient
    {
        get; protected set;
    }

    public Tile(int x, int y, float scale) : base(x, y, scale) { }

    public void RegisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged += callback;
    }

    public void UnregisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged -= callback;
    }

}
