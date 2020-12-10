using UnityEngine;

public interface ICursorProvider
{
    bool IsPointerOutOfFrame { get; }
    Vector3 GetPosition();
}