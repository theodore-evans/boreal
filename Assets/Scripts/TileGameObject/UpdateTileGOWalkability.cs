using System;
using UnityEngine;


public class UpdateTileGOWalkability : MonoBehaviour, ITileGOUpdateBehaviour
{
    [SerializeField]
    enum playerLayers
    {
        Walkable = 0,
        Unwalkable = 8
    }

    public void UpdateTile(GameObject tile_go, Tile tile_data)
    {
        if (tile_data.Type == "Water") {
            tile_go.layer = (int)playerLayers.Unwalkable;
        }
        else tile_go.layer = (int)playerLayers.Walkable;
    }
}
