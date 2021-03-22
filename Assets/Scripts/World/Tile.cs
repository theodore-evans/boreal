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
    ITileSubscriber _parent;

    private TileTypeId _type = TileTypeId.Blank;
    private float _waterDepth = 0.0f;
    private float _altitude = 0.5f;
    private Vector3 _normal = Vector3.zero;
    private float _waterThreshold = 0f;

    private Comparator comparator = new Comparator(threshold: 0.001f);
    private bool ValueHasChanged(ref float a, ref float b) => !comparator.ApproximatelyEqual(ref a, ref b);

    public void InvokeTileChangedCallback() => _parent.OnTileChanged(this);

    public Tile(int x, int y, float scale, ITileSubscriber parent) : base(x, y, scale) {
        _parent = parent;
    }

    public TileTypeId TypeId
    {
        get {
            if (_waterDepth > _waterThreshold) return TileTypeId.Water;
            else return _type;
        }

        set {
            TileTypeId oldType = _type;
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

            if (ValueHasChanged(ref oldDepth, ref _waterDepth)) InvokeTileChangedCallback();
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

            if (ValueHasChanged(ref oldAltitude, ref _altitude)) InvokeTileChangedCallback();
        }
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
