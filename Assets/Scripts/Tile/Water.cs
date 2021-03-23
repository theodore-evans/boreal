using System;

public class Water
{
    Tile _tile;

    void SetPropertyWithNotification<T>(ref T originalValue, T newValue) =>
        _tile.SetObservableProperty(ref originalValue, newValue);

    bool _deep;
    float _depth;

    public Water(Tile tile)
    {
        _tile = tile;
        _deep = false;
        _depth = 0f;
    }

    public bool Deep { get => _deep; set => _deep = value; }
    public float Level => _tile.Altitude + _depth;

    public float Depth { get => _depth; set => SetPropertyWithNotification(ref _depth, value); }

}
