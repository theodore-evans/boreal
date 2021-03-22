using System;
using System.Collections.Generic;

public interface IChunkLoader
{
    void AddChangedTile(Tile t);
    void DeregisterCallback(Action<IEnumerable<Tile>> callback);
    void RegisterCallback(Action<IEnumerable<Tile>> callback);
}