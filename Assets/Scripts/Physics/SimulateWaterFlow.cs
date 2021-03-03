using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SimulateWaterFlow : MonoBehaviour
{
    SpaceGrid<Tile> _world;
    internal HashSet<Tile> openSet;

    bool globalEquilibrium;

    [SerializeField] [Range(0.01f, 1)] float flowRate = 0.5f;
    [SerializeField] [Range(-0.02f, 0)] float minHead = -0.01f;

    public void Start()
    {
        openSet = new HashSet<Tile>();
    }

    public void StartSimulation(SpaceGrid<Tile> world)
    {
        _world = world;
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

            yield return new WaitForFixedUpdate();
        }
    }

    void Flow(Tile tile) //TODO implement as cellular automata
    {
        bool equilibrated = true;

        if (tile.WaterDepth > 0) {

            List<Tile> neighbours = _world.GetNeighbours(tile.X, tile.Y).OrderBy(o => o.WaterLevel).ToList();

            foreach (Tile neighbour in neighbours) {

                if (neighbour.WaterLevel - tile.WaterLevel < minHead) {
                    equilibrated = false;

                    float waterFlow = Mathf.Clamp(Mathf.Lerp(0, tile.WaterLevel - neighbour.WaterLevel, flowRate * Time.deltaTime), 0, tile.WaterDepth);

                    tile.WaterDepth -= waterFlow;
                    neighbour.WaterDepth += waterFlow;
                    openSet.Add(neighbour);
                }
            }
        }
        else openSet.Remove(tile);

        if (!equilibrated) globalEquilibrium = false;
    }
}
