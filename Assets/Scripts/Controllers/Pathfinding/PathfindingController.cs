using UnityEngine;
using System.Collections;

public class PathfindingController : MonoBehaviour
{
    private WorldController worldController;
    private TileController tileController;
    private Grid grid;

    public float nodeRadius;
    public LayerMask unwalkableMask;

    public bool autoUpdate;

    // Use this for initialization
    void Start()
    {
        GameObject worldController_go = GameObject.Find("WorldController");
        worldController = worldController_go.GetComponent<WorldController>();
        tileController = worldController_go.GetComponentInChildren<TileController>();

        Vector3 worldOrigin = worldController_go.transform.position;
        int worldWidth = worldController.World.Width;
        int worldHeight = worldController.World.Height;

        grid = new Grid(worldOrigin, worldWidth, worldHeight, nodeRadius, unwalkableMask);

        tileController.RegisterGameObjectChangedCallback(grid.UpdateNodesAtGameObject);
    }

    public void UpdateGridNodesAtGameObject(GameObject go)
    {
        grid.UpdateNodesAtGameObject(go);
    }

    public void RegenerateGrid()
    {
        if (grid != null) grid.GenerateGrid(nodeRadius, unwalkableMask);
    }

    void OnDrawGizmos()
    {
        if (grid != null) {
            Node[,] nodes = grid.Nodes;
     
            //Node playernode = NodeAtWorldPoint(player.transform.position);
            Node cursornode = grid.NodeAtWorldPoint(NavigationController.GetWorldPointUnderMouse());

            foreach (Node n in nodes) {
                Color color = (n.walkable) ? Color.white : Color.red;
                color.a = 0.25f;
                Gizmos.color = color;

                if (n == cursornode) {
                    Gizmos.color = Color.cyan;
                }

                Gizmos.DrawCube(n.worldPoint, Vector3.one * (nodeRadius));
            }
        }
    }

    private void OnValidate()
    {
        if (nodeRadius < 0.01) { nodeRadius = 0.01f; }
    }
}
