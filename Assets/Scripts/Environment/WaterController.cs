using UnityEngine;
using System.Collections.Generic;

public class WaterController : Controller
{
    [SerializeField] float waterThreshold = 0.05f;

    public override void UpdateTiles(IEnumerable<Tile> tilesToUpdate)
    {
        foreach (Tile tile in tilesToUpdate) {

            if (tile.Water.Depth > waterThreshold) tile.Water.Deep = true;
            else tile.Water.Deep = false;
        }
    }
}
