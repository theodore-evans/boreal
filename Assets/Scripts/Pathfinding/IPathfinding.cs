using UnityEngine;

public interface IPathfinding
{
    float MaxMovementCost { get; }

    void StartFindPath(Vector3 startPos, Vector3 targetPos);
    bool IsWalkable(PathNode node);
}