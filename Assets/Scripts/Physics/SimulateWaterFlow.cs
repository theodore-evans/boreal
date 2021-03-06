using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SimulateWaterFlow : MonoBehaviour
{
    SpaceGrid<Tile> _world;
    internal Cache<Tile> openSet = new Cache<Tile>();

    bool globalEquilibrium;

    [SerializeField] [Range(0, 1000)] int raindropsPerUpdate = 100;
    [SerializeField] [Range(0.01f, 1f)] float waterPerRaindrop = 0.1f;
    [SerializeField] [Range(0.01f, 1)] float flowRate = 0.5f;
    [SerializeField] [Range(-0.1f, 0)] float minHead = -0.01f;

    public void SetWorld(SpaceGrid<Tile> world)
    {
        _world = world;
    }

    public void StartSimulation()
    {
        StartCoroutine(nameof(SimulateWaterCoroutine));
    }

    public void StopSimulation()
    {
        StopCoroutine(nameof(SimulateWaterCoroutine));
    }

    private IEnumerator SimulateWaterCoroutine()
    {
        for (; ; ) {
            globalEquilibrium = true;

            foreach (Tile tile in openSet.ToList()) Flow(tile);

            if (globalEquilibrium) {
                openSet.Clear();
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    void Flow(Tile tile)
    {
        bool equilibrated = true;

        if (tile.WaterDepth > 0) {

            List<Tile> neighbours = _world.GetNeighbours(tile.X, tile.Y).OrderBy(o => o.WaterLevel).ToList();

            foreach (Tile neighbour in neighbours) {

                if (neighbour.WaterLevel - tile.WaterLevel < minHead) {
                    equilibrated = false;

                    float waterFlow = Mathf.Clamp(Mathf.Lerp(0, tile.WaterLevel - neighbour.WaterLevel, flowRate), 0, tile.WaterDepth);

                    tile.WaterDepth -= waterFlow;
                    neighbour.WaterDepth += waterFlow;
                    openSet.Add(neighbour);
                }
            }
        }
        else openSet.Remove(tile);

        if (!equilibrated) globalEquilibrium = false;
    }

    private IEnumerator RainCoroutine()
    {
        System.Random rng = new System.Random();

        for (; ; ) {
            for (int i = 0; i < raindropsPerUpdate; i++) {
                Tile newWetTile = _world.Nodes[rng.Next(_world.MaxSize)];
                newWetTile.WaterDepth += waterPerRaindrop;
                openSet.Add(newWetTile);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
