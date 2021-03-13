using UnityEngine;
using System.Collections;
using System;

public class CheckTileWalkability : MonoBehaviour, IWalkabilityChecker
{
    [SerializeField] float waterDepthModifier = 2f;
    [SerializeField] float gradientModifier = 2f;

    public float GetMovementPenalty(Tile tile)
    {
        return 1f + gradientModifier * -tile.Gradient + waterDepthModifier * tile.WaterDepth;
    }

}
