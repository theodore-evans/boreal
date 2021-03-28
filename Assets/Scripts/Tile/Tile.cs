using System;
using UnityEngine;
using Extensions;

public class Tile : AbstractNode
{
    private readonly Action<Tile> cbTileChanged;

    public Water Water { get; protected set; }
    public Relief Relief { get; protected set; }
    public Cover Cover { get; protected set; }

    public void OnObservableChanged() //TODO inject into observables
    {
        cbTileChanged?.Invoke(this);
    }

    public Tile(int x, int y, float scale, Action<Tile> cbTileChanged) : base(x, y, scale) {
        this.cbTileChanged = cbTileChanged;
        Water = new Water(this);
        Relief = new Relief(this);
        Cover = new Cover(this);
    }
}

