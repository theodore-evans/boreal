using System;
using System.Collections.Generic;
using UnityEngine;

public class PathGridController : MonoBehaviour
{
    public bool autoUpdate = true;

    internal NodeGrid<PathNode> grid;
    private NodeGrid<Tile> _world;

    [SerializeField] WorldController worldController;
    [SerializeField] [Range(0.1f, 1f)] float nodeRadius = 0.25f;
    [SerializeField] [Range(0f, 4f)] float heuristicWeight = 2f;

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
                node.movementPenalty = walkabilityChecker.GetMovementPenalty(tile);
                node.Altitude = tile.Altitude;
            }
        }
    }
}
