using UnityEngine;

// TODO add this to the PathfindingController GO
public class CheckSphereWalkability : MonoBehaviour, IWalkabilityChecker
{
    [SerializeField] LayerMask unwalkableMask = 0; // TODO change this in the editor to the correct value

    public bool IsWalkable(Vector3 worldPoint, float radius) //FIXME
    {
        return !Physics.CheckSphere(worldPoint, radius - 0.01f, unwalkableMask);
    }
}
