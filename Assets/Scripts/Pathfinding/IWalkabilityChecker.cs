using UnityEngine;

public interface IWalkabilityChecker
{
    float MaxWalkableMovementCost { get; }

    float GetMovementCost(Tile tile);
    bool IsWalkable(Tile tile);
}