﻿using UnityEngine;

public class PathfindingController : MonoBehaviour
{
    public bool drawGridGizmos;

    int gridWidth, gridHeight;
    Vector3 gridOrigin;

    private TileGameObjectController tileGameObjectController;
    private Grid grid;

    float nodeRadius;

    //
    // FIXME this class is a bit redundant
    //

    public void Initialise(WorldController worldController, TileGameObjectController tileGameObjectController)
    {
        this.tileGameObjectController = tileGameObjectController;

        gridOrigin = tileGameObjectController.transform.position;
        gridWidth = worldController.Width;
        gridHeight = worldController.Height;

        grid = GetComponent<Grid>();
        grid.Initialise(gridOrigin, gridWidth, gridHeight);

        nodeRadius = grid.nodeRadius;
        tileGameObjectController.RegisterGameObjectChangedCallback(grid.UpdateNodesOnGameObject);
    }

    public void UpdateGridNodesAtGameObject(GameObject go)
    {
        grid.UpdateNodesOnGameObject(go);
    }

    public void RegenerateGrid()
    {
        if (grid != null) grid.CreateGrid();
    }

    void OnDrawGizmosSelected()
    {
        if (grid != null && drawGridGizmos) {
            Node[,] nodes = grid.Nodes;

            //Node playernode = NodeAtWorldPoint(player.transform.position);
            //Node cursornode = grid.GetNodeAtWorldPoint(NavigationController.GetWorldPointUnderMouse());

            foreach (Node n in nodes) {
                Color color = (n.walkable) ? Color.white : Color.red;
                color.a = 0.25f;
                Gizmos.color = color;

                //if (n == cursornode) {
                //    Color color2 = (n.walkable) ? Color.cyan : Color.red;
                //    Gizmos.color = color2;
                //}

                Gizmos.DrawCube(n.worldPoint, Vector3.one * (nodeRadius));
            }
        }

    }
}
