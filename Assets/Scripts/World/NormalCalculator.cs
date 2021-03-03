using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalCalculatorMoore : MonoBehaviour, INormalCalculator
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
        foreach (Tile tile in changedTiles) {
            tile.Normal = CalculateNormal(tile);
            foreach (Tile neighbour in _world.GetNeighbours(tile.X, tile.Y)) {
                neighbour.Normal = CalculateNormal(neighbour);
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

        float NE = GetTileAltitudeAt(x + 1, y + 1);
        float NW = GetTileAltitudeAt(x - 1, y + 1);
        float SE = GetTileAltitudeAt(x + 1, y - 1);
        float SW = GetTileAltitudeAt(x - 1, y - 1);

        float WeightedAvg(float a, float b, float c) => (a + 1.4f * b + c) / 3.4f;

        Vector3 normal = new Vector3(WeightedAvg(NW, W, SW) - WeightedAvg(NE, E, SE), WeightedAvg(SW, S, SE) - WeightedAvg(NW, N, NE), 2);
        return Vector3.Normalize(normal);
    }

    private float GetTileAltitudeAt(int x, int y)
    {
        Tile t = _world.GetNodeAt(x, y);

        if (t != null) {
            return t.WaterLevel;
        }

        else {
            float sum = 0f;
            List<Tile> neighbours = _world.GetNeighbours(x, y);

            foreach (Tile neighbour in neighbours) {
                sum += neighbour.WaterLevel;
            }

            return sum / neighbours.Count;
        }
    }
}
