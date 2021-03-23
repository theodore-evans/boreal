using System;
using UnityEngine;
using Extensions;

public enum TileTypeId
{
    Blank = 0,
    Water = 1,
    Soil = 2,
    Grass = 3
}

public class Tile : AbstractNode
{
    private ITileSubscriber _subscriber;
    private float callbackTriggerThreshold = 0.0001f;

    private TileTypeId _type = TileTypeId.Blank;
    private float _altitude = 0.5f;
    private Vector3 _normal = Vector3.zero;

    public Water Water { get; protected set; }

    internal void SetObservableProperty(ref float observedProperty, float newValue)
    {
        if (observedProperty.Similar(newValue, callbackTriggerThreshold) == false) {
            observedProperty = newValue;
            InvokeTileChangedCallback();
        }
    }

    internal void SetObservableProperty<T>(ref T observedProperty, T newValue)
    {
        if (observedProperty.Equals(newValue) == false) {
            observedProperty = newValue;
            InvokeTileChangedCallback();
        }
    }

    private void InvokeTileChangedCallback() => _subscriber.OnTileChanged(this);

    public Tile(int x, int y, float scale, ITileSubscriber subscriber) : base(x, y, scale) {
        _subscriber = subscriber;
        Water = new Water(this);
    }

    public TileTypeId TypeId
    {
        get => _type;
        set => SetObservableProperty(ref _type, value);
    }

    public float Altitude
    {
        get => _altitude;
        set => SetObservableProperty(ref _altitude, value);
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
