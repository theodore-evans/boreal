using System;
using System.Collections.Generic;
using UnityEngine;

public class PathGridController : MonoBehaviour
{
    public bool autoUpdate = true;

    internal NodeGrid<PathNode> grid;
    private NodeGrid<Tile> _world;
    private WorldController worldController;

    [SerializeField] [Range(0.1f, 1f)] float nodeRadius = 0.25f;
    [SerializeField] [Range(0f, 4f)] float heuristicWeight = 2f;

    private IWalkabilityChecker walkabilityChecker;

    private void Awake()
    {
        worldController = GetComponentInParent<WorldController>();
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
        Vector3 origin = _world.Origin;
        float width = _world.GridSizeX;
        float height = _world.GridSizeY;

        grid = new NodeGrid<PathNode>(origin, width, height, nodeRadius * 2);

        for (int x = 0; x < grid.GridSizeX; x++) {
            for (int y = 0; y < grid.GridSizeY; y++) {
                grid.AddNode(new PathNode(x, y, nodeRadius, heuristicWeight));
            }
        }

        UpdateWalkabilityForChangedTiles(_world.Nodes);
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
