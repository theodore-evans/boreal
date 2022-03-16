using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalGenerator : MonoBehaviour
{

    WorldController wc;
    private NodeGrid<Tile> _world;

    private void Awake()
    {
        wc = GetComponent<WorldController>();
        wc.RegisterWorldCreatedCallback(Initialise);
    }

    public void Initialise(NodeGrid<Tile> world)
    {
        wc.RegisterWorldChangedCallback(UpdateNormals);
        _world = world;
    }

    public void UpdateNormals(IEnumerable<Tile> changedTiles)
    {
        HashSet<Tile> changedTilesHashSet = new HashSet<Tile>(changedTiles);

        foreach (Tile tile in changedTilesHashSet) {
            tile.Relief.Normal = CalculateNormal(tile);
            foreach (Tile neighbour in tile.Neighbours) {
                if (!changedTilesHashSet.Contains(neighbour)) {
                    neighbour.Relief.Normal = CalculateNormal(neighbour);
                }
            }
        }
    }

    private Vector3 CalculateNormal(Tile tile)
    {
        int x = tile.X;
        int y = tile.Y;

        float E = GetTileAltitudeAt(x + 1, y);
        float W = GetTileAltitudeAt(x - 1, y);
        float N = GetTileAltitudeAt(x, y + 1);
        float S = GetTileAltitudeAt(x, y - 1);

        Vector3 normal = new Vector3(E - W, N - S, -2);
        return Vector3.Normalize(-normal);
    }

    private float GetTileAltitudeAt(int x, int y)
    {
        Tile t = _world.GetNodeAt(x, y);

        if (t != null) {
            return GetTileAltiude(t);
        }

        else {
            int count = 0;
            float sum = 0f;
            IEnumerable<Tile> neighbours = _world.GetNeighbours(x, y);

            foreach (Tile neighbour in neighbours) {
                sum += GetTileAltiude(neighbour);
                count++;
            }

            return sum / count;
        }
    }

    private float GetTileAltiude(Tile t)
    {
        return t.Water.Level;
    }
}
