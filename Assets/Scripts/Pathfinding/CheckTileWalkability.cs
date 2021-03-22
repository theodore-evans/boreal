using UnityEngine;
using System.Collections;
using System;

public class CheckTileWalkability : MonoBehaviour, IWalkabilityChecker
{
    [SerializeField] float waterDepthModifier = 2f;
    [SerializeField] float gradientModifier = 2f;

    public float GetMovementPenalty(Tile tile)
    {
        float movementPenalty = 1f + gradientModifier * tile.Gradient;
        if (tile.Water) movementPenalty += waterDepthModifier * tile.WaterDepth;
        return movementPenalty;
    }

}
