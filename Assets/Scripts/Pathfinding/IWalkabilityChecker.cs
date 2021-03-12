using UnityEngine;

public interface IWalkabilityChecker
{
    float GetMovementCost(Tile tile);
}