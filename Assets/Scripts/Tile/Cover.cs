using UnityEngine;

public class Cover: AbstractTileObservable
{
    internal Cover(Tile parent) : base(parent) { }

    private float _grass = 0f;

    public float Grass { get => _grass; set => SetObservableProperty(ref _grass, Mathf.Clamp01(value)); }
    public float Total { get => Mathf.Max(Grass); } // TODO add other cover types, type of aggregation TBD

}