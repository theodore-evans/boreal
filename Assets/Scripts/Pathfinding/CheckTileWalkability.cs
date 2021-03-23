using UnityEngine;
using System.Collections;
using System;

public class CheckTileWalkability : MonoBehaviour, IWalkabilityChecker
{
    [SerializeField] float WaterDepthModifier = 2f;
    [SerializeField] float gradientModifier = 2f;

    public float GetMovementPenalty(Tile tile)
    {
        float movementPenalty = 1f + gradientModifier * tile.Gradient;
        if (tile.Water.Deep) movementPenalty += WaterDepthModifier * tile.Water.Depth;
        return movementPenalty;
    }

}
