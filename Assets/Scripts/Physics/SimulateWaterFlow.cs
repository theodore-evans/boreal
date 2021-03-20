using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//TODO: optimise, better manage contiguous water bodies?
public class SimulateWaterFlow : MonoBehaviour
{
    NodeGrid<Tile> _world;
    internal Cache<Tile> openSet = new Cache<Tile>();
    internal VisitationMap<Tile> visitedSet = new VisitationMap<Tile>();

    bool globalEquilibrium;

    [SerializeField] [Range(0, 100)] int raindropsPerUpdate = 100;
    [SerializeField] [Range(0.01f, 1f)] float waterPerRaindrop = 0.1f;
    [SerializeField] [Range(0.01f, 1)] float flowRate = 0.5f;
    [SerializeField] [Range(0f, 0.1f)] float minHead = 0.05f;
    [SerializeField] [Range(0f, 1f)] float erosionCoefficient = 0.2f;
    [SerializeField] [Range(0f, 1f)] float channelWideningCoefficient = 0.1f;

    private float seaLevel = 0f; // TODO actually find out from world, if not zero

    private bool are4Connected(Tile a, Tile b) => (b.X - a.X) * (b.Y - a.Y) == 0;

    private float normalizedWaterLevel(Tile tileA, Tile tileB)
    {
        if (are4Connected(tileA, tileB))
            return tileB.WaterLevel;
        else return Mathf.Lerp(tileA.WaterLevel, tileB.WaterLevel, 0.707f);
    }

    public void SetWorld(NodeGrid<Tile> world)
    {
        _world = world;
    }

    public void StartRain()
    {
        StartCoroutine(nameof(RainCoroutine));
        StartCoroutine(nameof(SimulateWaterCoroutine));
    }

    public void StopRain()
    {
        StopCoroutine(nameof(RainCoroutine));
    }

    private IEnumerator SimulateWaterCoroutine()
    {
        visitedSet.Clear();

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

    void Flow(Tile tile)
    {
        bool equilibrated = true;

        if (tile.WaterDepth > minHead) {

            List<Tile> neighbours = _world.GetNeighbours(tile.X, tile.Y).OrderBy(o => normalizedWaterLevel(tile, o)).ToList();

            float erosionAmount = 0;
            foreach (Tile neighbour in neighbours) {
                if (tile.WaterDepth > minHead && tile.WaterLevel - neighbour.WaterLevel > minHead) {
                    equilibrated = false;

                    float waterFlow = Mathf.Clamp(Mathf.Lerp(0, tile.WaterLevel - neighbour.WaterLevel, flowRate), 0, tile.WaterDepth);
                    tile.WaterDepth -= waterFlow;
                    visitedSet.Add(neighbour, waterFlow);
                    erosionAmount += erosionCoefficient * waterFlow * (tile.Altitude - neighbour.Altitude);

                    if (neighbour.WaterLevel > seaLevel) {
                        neighbour.WaterDepth += waterFlow;
                        openSet.Add(neighbour);
                    }
                }
                else neighbour.Altitude -= erosionAmount * channelWideningCoefficient;
            }
            tile.Altitude -= erosionAmount;

        }
        else openSet.Remove(tile);

        if (!equilibrated) globalEquilibrium = false;
    }
}
