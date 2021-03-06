using System;
using UnityEngine;

public enum TypeId
{
    Blank = 0,
    Water = 1,
    Soil = 2,
    Grass = 3
}

public class Tile : AbstractNode, ISurface
{
    Action<Tile> cbTileChanged;

    private TypeId _type = global::TypeId.Blank;
    private float _waterDepth = 0.0f;
    private float _altitude = 0.5f;
    private Vector3 _normal = Vector3.zero;
    private float _waterThreshold = 0f;

    public TypeId TypeId
    {
        get {

            if (_waterDepth > _waterThreshold) return TypeId.Water;
            else return _type;
        }

        set {
            TypeId oldType = _type;
            _type = value;

            if (oldType != _type) InvokeTileChangedCallback();
        }
    }

    public float WaterDepth
    {
        get => _waterDepth;
        set {
            float oldDepth = _waterDepth;
            _waterDepth = value;

            if (oldDepth !=_waterDepth) InvokeTileChangedCallback();
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

            if (oldAltitude != _altitude) InvokeTileChangedCallback();
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

    public void InvokeTileChangedCallback()
    {
        cbTileChanged?.Invoke(this);
    }
}
