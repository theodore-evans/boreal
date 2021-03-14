using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//TODO: optimise, better manage contiguous water bodies?
public class SimulateWaterFlow : MonoBehaviour
{
    NodeGrid<Tile> _world;
    internal Cache<Tile> openSet = new Cache<Tile>();
    private VisitationMap<Tile> visitedSet = new VisitationMap<Tile>();

    bool globalEquilibrium;

    [SerializeField] [Range(0, 100)] int raindropsPerUpdate = 100;
    [SerializeField] [Range(0.01f, 1f)] float waterPerRaindrop = 0.1f;
    [SerializeField] [Range(0.01f, 1)] float flowRate = 0.5f;
    [SerializeField] [Range(0f, 0.1f)] float minHead = 0.05f;
    [SerializeField] [Range(0f, 1f)] float erosionCoeff = 0.5f;

    [SerializeField] [Range(0, 1000)] int showFlowMin = 50;
    [SerializeField] [Range(0, 1000)] int showFlowMax = 1000;

    private float seaLevel = 0f; // TODO actually find out from world, if not zero

    public void SetWorld(NodeGrid<Tile> world)
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
                if (tile.WaterLevel - neighbour.WaterLevel > minHead) {
                    equilibrated = false;                    

                    float waterFlow = Mathf.Clamp(Mathf.Lerp(0, tile.WaterLevel - neighbour.WaterLevel, flowRate), 0, tile.WaterDepth);
                    tile.WaterDepth -= waterFlow;
                    tile.Altitude -= waterFlow * erosionCoeff * (tile.Altitude - neighbour.Altitude);

                    if (neighbour.WaterLevel > seaLevel) {
                        neighbour.WaterDepth += waterFlow;
                        openSet.Add(neighbour);
                        visitedSet.Add(neighbour);
                    }
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
                if (newWetTile.WaterLevel > minHead) {
                    newWetTile.WaterDepth += waterPerRaindrop;
                    openSet.Add(newWetTile);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (visitedSet != null) {
            foreach (Tile t in visitedSet) {
                if (visitedSet[t] > showFlowMin) {
                    Color color = Color.blue;
                    color.a = Mathf.Lerp(0, 1, Mathf.Clamp01((visitedSet[t] - showFlowMin) / (float)showFlowMax));
                    Gizmos.color = color;
                    Gizmos.DrawCube(new Vector3(t.X + t.Scale / 2f, t.Y + t.Scale / 2f, -2f), Vector3.one * 0.9f);
                }
            }
        }
    }
}
