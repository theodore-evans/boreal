using System;
using System.Collections.Generic;
using UnityEngine;

public class PathGridController : MonoBehaviour
{
    public bool autoUpdate = true;

    public NodeGrid<PathNode> grid { get; protected set; }
    private NodeGrid<Tile> _world;

    [SerializeField] WorldController worldController;
    [SerializeField] bool drawGridGizmos = false;
    [SerializeField] [Range(0.1f, 1f)] float nodeRadius = 0.25f;
    [SerializeField] [Range(0f, 4f)] float heuristicWeight = 2f;

    private IPathfinding pathfinding;

    private IWalkabilityChecker walkabilityChecker;

    private void Awake()
    {
        walkabilityChecker = GetComponent<IWalkabilityChecker>();
        worldController.RegisterWorldCreatedCallback(Initialize);
        pathfinding = GetComponent<IPathfinding>();
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

    // TODO: implement smarter data structure?
    public void CreateGrid(NodeGrid<Tile> world)
    {
        Vector3 origin = world.Origin;
        float width = world.GridSizeX;
        float height = world.GridSizeY;

        grid = new NodeGrid<PathNode>(origin, width, height, nodeRadius * 2);

        for (int x = 0; x < grid.GridSizeX; x++) {
            for (int y = 0; y < grid.GridSizeY; y++) {
                grid.SetNodeAt(x, y, new PathNode(x, y, nodeRadius, heuristicWeight));
            }
        }

        UpdateWalkabilityForChangedTiles(world.Nodes);
    }

    public void UpdateWalkabilityForChangedTiles(IEnumerable<Tile> changedTiles)
    {
        foreach (Tile tile in changedTiles) {
            foreach (PathNode node in grid.GetNodesOnOtherGridsNode(tile)) {
                node.movementCost = walkabilityChecker.GetMovementCost(tile);
                node.Altitude = tile.Altitude;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (grid != null && drawGridGizmos) {
            foreach (PathNode n in grid.Nodes) {
                if (pathfinding.IsWalkable(n)) {
                    Color color = Color.Lerp(Color.white, Color.red, Mathf.Clamp01(n.movementCost / pathfinding.MaxMovementCost));
                    color.a = 0.25f;
                    Gizmos.color = color;
                    Gizmos.DrawCube(grid.GetNodePosition(n) + n.Radius * new Vector3(1, 1, -2), Vector3.one * n.Radius * 2 * 0.9f);
                }

            }
        }
    }
    
}
