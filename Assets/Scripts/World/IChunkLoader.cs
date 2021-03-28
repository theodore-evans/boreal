using System;
using System.Collections.Generic;

public interface IChunkLoader : ITileObserver
{
    void DeregisterCallback(Action<IEnumerable<Tile>> callback);
    void RegisterCallback(Action<IEnumerable<Tile>> callback);
}