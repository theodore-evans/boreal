using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class GrassGrowthParameters
{
    [Range(0f, 1f)] public float baseGrass = 0.01f;
    public AnimationCurve growthOverGradient;
    public AnimationCurve growthOverSaturation;
}

public class GrassController : WorldSubscriber
{
    [SerializeField] GrassGrowthParameters parameters;
    float availableWater;

    public override void UpdateTiles(IEnumerable<Tile> updatedTiles)
    {
        foreach (Tile tile in updatedTiles) {
            GrowGrass(tile);
        }
    }

    private void GrowGrass(Tile tile)
    {
        if (tile.Relief.Elevation > 0) {
            availableWater = tile.Water.Saturation;
            foreach (Tile neighbour in tile.Neighbours) {
                availableWater += neighbour.Water.Saturation;
            }
            tile.Cover.Grass = parameters.baseGrass *
                parameters.growthOverGradient.Evaluate(tile.Relief.Gradient / 90f) *
                parameters.growthOverSaturation.Evaluate(availableWater / 9);
        }
    }

    public void UpdateAllGrass()
    {
        foreach (Tile tile in world.Nodes) {
            GrowGrass(tile);
        }
    }
}

