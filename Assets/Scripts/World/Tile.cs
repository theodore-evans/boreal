using System;
using UnityEngine;

public enum TileTypeId
{
    Blank = 0,
    Water = 1,
    Soil = 2,
    Grass = 3
}

public class Tile : AbstractNode
{
    ITileSubscriber _subscriber;

    private TileTypeId _type = TileTypeId.Blank;
    private float _waterDepth = 0.0f;
    private float _altitude = 0.5f;
    private Vector3 _normal = Vector3.zero;
    private bool _water = false;

    private void UpdateField(ref float field, float newValue, float invokeThreshold)
    {
        float oldValue = field;
        field = newValue;

        if (Compare.ApproximatelyEqual(oldValue, newValue, invokeThreshold) == false) InvokeTileChangedCallback();
    }

    public void InvokeTileChangedCallback() => _subscriber.OnTileChanged(this);

    public Tile(int x, int y, float scale, ITileSubscriber subscriber) : base(x, y, scale) {
        _subscriber = subscriber;
    }

    public TileTypeId TypeId
    {
        get => _type;
        set {
            TileTypeId oldType = _type;
            _type = value;

            if (oldType != _type) InvokeTileChangedCallback();
        }
    }

    public float WaterDepth
    {
        get => _waterDepth;
        set => UpdateField(ref _waterDepth, value, 0.001f);
    }

    public float WaterLevel
    {
        get => _altitude + _waterDepth;
    }

    public bool Water
    {
        get => _water;
        set => _water = value;
    }

    public float Altitude
    {
        get => _altitude;
        set => UpdateField(ref _altitude, value, 0.001f);
    }

    public Vector3 Normal
    {
        get => _normal;

        set {
            _normal = value;
            Gradient = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(Normal, Vector3.forward));
        }
    }

    public float Gradient
    {
        get; protected set;
    }
}
