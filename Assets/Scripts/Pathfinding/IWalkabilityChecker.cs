using UnityEngine;

public interface IWalkabilityChecker
{
    bool IsWalkable(Vector3 worldPoint, float nodeRadius);
}