using UnityEngine;

public class DrawUnitPathGizmos : MonoBehaviour
{
    private Unit unit = null;
    private ref Vector3[] path => ref unit.path;
    private ref int targetIndex => ref unit.targetIndex;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    public void OnDrawGizmos()
    {
        if (unit != null) {
            if (path != null) {
                for (int i = targetIndex; i < path.Length; i++) {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex) {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
    }
}
