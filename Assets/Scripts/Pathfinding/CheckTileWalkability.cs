using UnityEngine;
using System.Collections;
using System;

public class CheckTileWalkability : MonoBehaviour, IWalkabilityChecker
{
    [SerializeField] float waterDepthModifier = 2f;
    [SerializeField] float gradientModifier = 2f;
    [SerializeField] float maxWalkableWaterDepth = 0.5f;
    [SerializeField] float maxWalkableMovementCost = 5f;

    public float MaxWalkableMovementCost { get => maxWalkableMovementCost; }

    public float GetMovementCost(Tile tile)
    {
        return 1f + gradientModifier * -tile.Gradient + waterDepthModifier * tile.WaterDepth;
    }

    public bool IsWalkable(Tile tile)
    {
        return tile.WaterDepth < maxWalkableWaterDepth && GetMovementCost(tile) < maxWalkableMovementCost;
    }
}
