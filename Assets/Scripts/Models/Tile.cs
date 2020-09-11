using System;

public class Tile
{
    Action<Tile> cbTileChanged;

    public int X { get; }
    public int Y { get; }

    private string _type = "Blank";

    public string Type
    {
        get
        {
            return _type;
        }
        set
        {
            string oldType = _type;
            _type = value;

            if (cbTileChanged != null && oldType != _type)
                cbTileChanged(this);
        }
    }

    private float _altitude = 0.5f;

    public float Altitude
    {
        get
        {
            return _altitude;
        }
        set
        {
            float oldAltitude = _altitude;
            _altitude = value;

            if (cbTileChanged != null && oldAltitude != _altitude)
                cbTileChanged(this);
        }
    }

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void RegisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged += callback;
    }

    public void UnregisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged -= callback;
    }

}
