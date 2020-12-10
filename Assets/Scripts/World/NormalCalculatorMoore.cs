using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalCalculatorMoore : MonoBehaviour, INormalCalculator
{
    private SpaceGrid<Tile> world;

    public void Initialise(WorldController wc)
    {
        wc.RegisterWorldChangedCallback(UpdateNormals);
        world = wc.world;
    }

    public void UpdateNormals(Tile tile)
    {
        foreach (Tile neighbour in world.GetNeighbours(tile.X, tile.Y)) {
            CalculateNormal(neighbour);
        }
    }

    private void CalculateNormal(Tile tile)
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

        Vector3 normal = new Vector3(WeightedAvg(NW, W, SW) - WeightedAvg(NE, E, SE), WeightedAvg(SW, S, SE) - WeightedAvg(NW, N, NE), -2);
        tile.Normal = Vector3.Normalize(normal);
    }

    //private void CalculateNormal(Tile tile)
    //{
    //    int x = tile.X;
    //    int y = tile.Y;

    //    float E = GetTileAltitudeAt(x + 1, y);
    //    float W = GetTileAltitudeAt(x - 1, y);
    //    float N = GetTileAltitudeAt(x, y + 1);
    //    float S = GetTileAltitudeAt(x, y - 1);

    //    Vector3 normal = new Vector3(W - E, S - N, -2);
    //    tile.Normal = Vector3.Normalize(normal);
    //}

    private float GetTileAltitudeAt(int x, int y)
    {
        Tile t = world.GetNodeAt(x, y);

        if (t != null) {
            return t.Altitude;
        }

        // for tiles 'off the edge' of the map, the average height of on-map 8-neighbours is used
        // still leads to plateauing at the edges, mitigating this and other edge effects by adding a 1-tile margin to the world

        else {
            float sum = 0f;
            List<Tile> neighbours = world.GetNeighbours(x, y);

            foreach (Tile neighbour in neighbours) {
                sum += neighbour.Altitude;
            }

            return sum / neighbours.Count;
        }
    }
}
