using System;
using UnityEngine;

public class PathGridController : MonoBehaviour
{
    public bool autoUpdate = true;
    [SerializeField] bool drawGridGizmos = true;

    [SerializeField] GameObject worldController_go = null;
    [SerializeField] GameObject tileGOController_go = null;
    [SerializeField] [Range (0.1f, 0.5f)] float nodeRadius = 0.25f;

    public SpaceGrid<PathNode> pathGrid { get; protected set; }

    TileGOController tileGOController;
    WorldController worldController;

    private IWalkabilityChecker walkabilityChecker;

    private void Awake()
    {
        tileGOController = tileGOController_go.GetComponent<TileGOController>();
        worldController = worldController_go.GetComponent<WorldController>();

        walkabilityChecker = GetComponent<IWalkabilityChecker>();

        tileGOController.RegisterTileGameObjectChangedCallback(UpdateNodesForGameObject);

        CreateGrid();
    }

    public void CreateGrid()
    {
        float margin = 0.5f - nodeRadius;
        Vector3 origin = tileGOController.transform.position + new Vector3(0.5f, 0.5f, 0);
        float width = worldController.WorldWidth - margin;
        float height = worldController.WorldHeight - margin;

        pathGrid = new SpaceGrid<PathNode>(origin, width, height, nodeRadius * 2);

        for (int x = 0; x < pathGrid.GridSizeX; x++) {
            for (int y = 0; y < pathGrid.GridSizeY; y++) {
                pathGrid.SetNodeAt(x, y, new PathNode(x, y, nodeRadius));
            }
        }
    }

    public void UpdateNodesForGameObject(GameObject go)
    {
        Bounds bounds = go.GetComponent<Collider>().bounds;

        foreach (PathNode node in pathGrid.GetNodesInsideBounds(bounds)) {
            Vector3 worldPoint = pathGrid.GetNodePosition(node);
            node.Walkable = walkabilityChecker.IsWalkable(worldPoint, node.Radius);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (pathGrid != null && drawGridGizmos) {
 
            foreach (PathNode n in pathGrid.Nodes) {
                Color color = (n.Walkable) ? Color.white : Color.red;
                color.a = 0.25f;
                Gizmos.color = color;

                Gizmos.DrawCube(pathGrid.GetNodePosition(n), Vector3.one * nodeRadius * 2 * 0.9f);
            }
        }

    }
}
