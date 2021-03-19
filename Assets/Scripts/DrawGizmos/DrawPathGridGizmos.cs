using UnityEngine;

public class DrawPathGridGizmos : MonoBehaviour
{
    private PathGridController pathGridController;
    private IPathfinding pathfinding;

    [SerializeField] bool drawGridGizmos = false;

    private ref NodeGrid<PathNode> grid => ref pathGridController.grid;

    private void Start()
    {
        pathGridController = GetComponent<PathGridController>();
        pathfinding = GetComponent<IPathfinding>();
    }

    private void OnDrawGizmos()
    {
        if (grid != null && drawGridGizmos) {
            foreach (PathNode n in grid.Nodes) {
                if (pathfinding.IsWalkable(n)) {
                    Color color = Color.Lerp(Color.white, Color.red, Mathf.Clamp01(n.movementPenalty / pathfinding.MaxMovementCost));
                    color.a = 0.25f;
                    Gizmos.color = color;
                    Gizmos.DrawCube(grid.GetNodePosition(n) + n.Radius * new Vector3(1, 1, -2), Vector3.one * n.Radius * 2 * 0.9f);
                }

            }
        }
    }

}
