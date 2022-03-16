using System;
using System.Collections.Generic;

public class Tile : AbstractGridNode
{
    private readonly Action<Tile> cbTileChanged;

    public Water Water { get; protected set; }
    public Relief Relief { get; protected set; }
    public Cover Cover { get; protected set; }

    public void OnObservableChanged() 
    {
        cbTileChanged?.Invoke(this);
    }

    public Tile(int x, int y, float scale, IEnumerable<Tile> neighbours, Action<Tile> cbTileChanged) : base(x, y, scale) {
        this.cbTileChanged = cbTileChanged;
        Water = new Water(this);
        Relief = new Relief(this);
        Cover = new Cover(this);
        _neighbours = neighbours;
    }
}

