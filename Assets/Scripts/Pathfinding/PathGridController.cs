using System;
using System.Collections.Generic;
using UnityEngine;

public class PathGridController : MonoBehaviour
{
    public bool autoUpdate = true;
    [SerializeField] bool drawGridGizmos = true;

    [SerializeField] [Range (0.1f, 1f)] float nodeRadius = 0.25f;

    public NodeGrid<PathNode> pathGrid { get; protected set; }
    private NodeGrid<Tile> _world;

    [SerializeField] WorldController worldController;

    private IWalkabilityChecker walkabilityChecker;

    private void Awake()
    {
        walkabilityChecker = GetComponent<IWalkabilityChecker>();
        worldController.RegisterWorldCreatedCallback(Initialize);
    }

    private void Initialize(NodeGrid<Tile> world)
    {
        _world = world;
        CreateGrid();
        worldController.RegisterWorldChangedCallback(UpdateWalkabilityForChangedTiles);
    }
    
    public void CreateGrid()
    {
        CreateGrid(_world);
    }

    // TODO: implement smarter data structure
    public void CreateGrid(NodeGrid<Tile> world)
    {
        Vector3 origin = world.Origin;
        float width = world.GridSizeX;
        float height = world.GridSizeY;

        pathGrid = new NodeGrid<PathNode>(origin, width, height, nodeRadius * 2);

        for (int x = 0; x < pathGrid.GridSizeX; x++) {
            for (int y = 0; y < pathGrid.GridSizeY; y++) {
                pathGrid.SetNodeAt(x, y, new PathNode(x, y, nodeRadius));
            }
        }

        UpdateWalkabilityForChangedTiles(world.Nodes);
    }

    public void UpdateWalkabilityForChangedTiles(IEnumerable<Tile> changedTiles)
    {
        foreach (Tile tile in changedTiles) {
            foreach (PathNode node in pathGrid.GetNodesOnOtherGridsNode(tile)) {
                node.movementCost = walkabilityChecker.GetMovementCost(tile);
                node.Walkable = walkabilityChecker.IsWalkable(tile);
                node.Altitude = tile.Altitude;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (pathGrid != null && drawGridGizmos) {
 
            foreach (PathNode n in pathGrid.Nodes) {
                if (n.Walkable) {
                    Color color = Color.Lerp(Color.white, Color.red, Mathf.Clamp01(n.movementCost / walkabilityChecker.MaxWalkableMovementCost));
                    color.a = 0.25f;
                    Gizmos.color = color;
                    Gizmos.DrawCube(pathGrid.GetNodePosition(n) + nodeRadius * new Vector3(1, 1, -2), Vector3.one * nodeRadius * 2 * 0.9f);
                }
                
            }
        }

    }
}
