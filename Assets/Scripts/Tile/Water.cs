using System;
using UnityEngine;

public class Water : AbstractTileObservable
{
    bool _surface = false;
    float _depth = 0f;
    float _saturation = 0f;

    internal Water(Tile parent) : base(parent) { }

    public bool Surface { get => _surface; set => _surface = value; }
    public float Level => parent.Relief.Elevation + _depth;
    public float Saturation { get => _saturation; set => _saturation = Mathf.Clamp01(value); }

    public float Depth { get => _depth; set => SetObservableProperty(ref _depth, value); }
}
