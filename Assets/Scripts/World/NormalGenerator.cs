using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalGenerator : MonoBehaviour
{

    WorldController wc;
    private SpaceGrid<Tile> _world;

    private void Awake()
    {
        wc = GetComponent<WorldController>();
        wc.RegisterWorldCreatedCallback(Initialise);
    }

    public void Initialise(SpaceGrid<Tile> world)
    {
        wc.RegisterWorldChangedCallback(UpdateNormals);
        _world = world;
    }

    public void UpdateNormals(IEnumerable<Tile> changedTiles)
    {
        HashSet<Tile> changedTilesHS = new HashSet<Tile>(changedTiles);

        foreach (Tile tile in changedTilesHS) {
            tile.Normal = CalculateNormal(tile);
            foreach (Tile neighbour in _world.GetNeighbours(tile.X, tile.Y)) {
                if (!changedTilesHS.Contains(neighbour)) {
                    neighbour.Normal = CalculateNormal(neighbour);
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

        Vector3 normal = new Vector3(W - E, S - N, 2);
        return Vector3.Normalize(normal);
    }

    private float GetTileAltitudeAt(int x, int y)
    {
        Tile t = _world.GetNodeAt(x, y);

        if (t != null) {
            return GetTileAltiude(t);
        }

        else {
            float sum = 0f;
            List<Tile> neighbours = _world.GetNeighbours(x, y);

            foreach (Tile neighbour in neighbours) {
                sum += GetTileAltiude(neighbour);
            }

            return sum / neighbours.Count;
        }
    }

    private float GetTileAltiude(Tile t)
    {
        return t.WaterLevel;
    }
}
