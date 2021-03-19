using UnityEngine;

class DrawAStarGizmos : MonoBehaviour
{
    private AStar aStar;

    private ref Heap<PathNode> openSet => ref aStar.openSet;
    private ref NodeGrid<PathNode> grid => ref aStar.grid;

    private void Start()
    {
        aStar = GetComponent<AStar>();
    }

    void OnDrawGizmos()
    {
        if (openSet != null) {
            foreach (PathNode n in openSet) {
                Color color = Color.blue;
                color.a = 0.5f;
                Gizmos.color = color;
                Gizmos.DrawCube(grid.GetNodePosition(n) + n.Radius * new Vector3(1, 1, -2), Vector3.one * n.Radius * 2 * 0.9f);
            }
        }
    }
}
