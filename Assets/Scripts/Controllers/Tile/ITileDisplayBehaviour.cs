using UnityEngine;

public interface ITileUpdateBehaviour
{
    void OnTileChanged(GameObject tile_go, Tile tile_data);
}