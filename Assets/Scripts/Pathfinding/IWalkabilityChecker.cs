using UnityEngine;

public interface IWalkabilityChecker
{
    float GetMovementPenalty(Tile tile);
}