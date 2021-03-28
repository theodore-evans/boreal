using UnityEngine;
using System.Collections.Generic;

public class WaterController : Controller
{
    [SerializeField] float soilCarryingCapacity = 0.05f;

    public override void UpdateTiles(IEnumerable<Tile> tilesToUpdate)
    {
        foreach (Tile tile in tilesToUpdate) {
            if (tile.Water.Depth > (1 - tile.Water.Saturation) * soilCarryingCapacity) tile.Water.Surface = true;
            else tile.Water.Surface = false;
        }
    }
}
