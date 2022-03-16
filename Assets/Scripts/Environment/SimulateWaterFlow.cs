using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//TODO make a POCO, with parameters injected.

[Serializable]
public class WaterFlowParameters
{
    [Range(0, 100)] public int raindropsPerUpdate = 100;
    [Range(0.01f, 1f)] public float waterPerRaindrop = 0.1f;
    [Range(0.01f, 1)] public float flowRate = 0.5f;
    [Range(0f, 1f)] public float erosionCoefficient = 0.2f;
    [Range(0f, 1f)] public float channelWideningCoefficient = 0.1f;
}

public class SimulateWaterFlow : MonoBehaviour
{
    NodeGrid<Tile> _world;
    internal Cache<Tile> openSet = new Cache<Tile>();
    internal VisitationMap<Tile> moistureMap = new VisitationMap<Tile>(); // TODO extract map to scriptable data object

    bool globalEquilibrium;

    [SerializeField] WaterFlowParameters parameters;
    [SerializeField] [Range(0f, 1f)] float simulationSpeed = 0.75f;

    private Coroutine simulateWaterCoroutine;
    private Coroutine rainCoroutine;

    private WaitForSeconds waitBetweenIterations;

    private float seaLevel = 0f; // TODO actually find out from world, if not zero

    public float totalRainVolume = 0f;

    private bool are4Connected(Tile a, Tile b) => (b.X - a.X) * (b.Y - a.Y) == 0;

    private float normalizedWaterLevel(Tile tileA, Tile tileB)
    {
        if (are4Connected(tileA, tileB))
            return tileB.Water.Level;
        else return Mathf.Lerp(tileA.Water.Level, tileB.Water.Level, 0.707f);
    }

    public void Setup(NodeGrid<Tile> world)
    {
        _world = world;
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

            yield return waitBetweenIterations;
        }
    }

    void Flow(Tile tile)
    {
        bool equilibrated = true;

        if (tile.Water.Depth > 0 && tile.Water.Level > seaLevel) {
            IEnumerable<Tile> neighbours = tile.Neighbours.OrderBy(o => normalizedWaterLevel(tile, o));

            float erosionAmount = 0;

            foreach (Tile neighbour in neighbours) {
                if (tile.Water.Level - neighbour.Water.Level > 0) {
                    equilibrated = false;

                    float head = tile.Water.Level - neighbour.Water.Level;
                    float waterFlow = Mathf.Clamp(Mathf.Lerp(0, head / 2, parameters.flowRate), 0, tile.Water.Depth);

                    erosionAmount += parameters.erosionCoefficient * waterFlow * (tile.Relief.Elevation - neighbour.Relief.Elevation);

                    tile.Water.Depth -= waterFlow;
                    tile.Water.Saturation = moistureMap.Add(tile, waterFlow) * totalRainVolume / 100f;

                    if (neighbour.Water.Level > seaLevel) {
                        neighbour.Water.Depth += waterFlow;
                        openSet.Add(neighbour);
                    }
                }
                else neighbour.Relief.Elevation -= erosionAmount * parameters.channelWideningCoefficient;
            }
            tile.Relief.Elevation -= erosionAmount;

        }
        else openSet.Remove(tile);

        if (!equilibrated) globalEquilibrium = false;
    }

    // TODO rain will be ultimately encapsulated to its own class, just here for testing
    System.Random rng = new System.Random();

    public void StartRain()
    {
        if (simulateWaterCoroutine != null) {
            StopCoroutine(simulateWaterCoroutine);
            simulateWaterCoroutine = null;
        }

        if (rainCoroutine == null) {
            rainCoroutine = StartCoroutine(nameof(RainCoroutine));
        }

        simulateWaterCoroutine = StartCoroutine(nameof(SimulateWaterCoroutine));
        waitBetweenIterations = new WaitForSeconds(Mathf.Lerp(0.1f, 0f, simulationSpeed));
    }

    public void StopRain()
    {
        if (rainCoroutine != null) {
            StopCoroutine(rainCoroutine);
            rainCoroutine = null;
        }
    }

    private IEnumerator RainCoroutine()
    {
        for (; ; ) {
            for (int i = 0; i < parameters.raindropsPerUpdate; i++) {
                Tile newWetTile = _world.Nodes[rng.Next(_world.MaxSize)];
                if (newWetTile.Water.Level > 0) {
                    newWetTile.Water.Depth += parameters.waterPerRaindrop;
                    totalRainVolume += parameters.waterPerRaindrop;
                    openSet.Add(newWetTile);
                }
            }

            yield return waitBetweenIterations;
        }
    }
}
