using UnityEngine;
using System.Collections;

public class WaterSimController : MonoBehaviour
{
    [SerializeField] WorldController wc = null;

    internal SpaceGrid<Tile> _world;
    SimulateDroplets simulateDroplets;
    SimulateWaterFlow simulateWaterFlow;

    private void Awake()
    {
        simulateDroplets = GetComponent<SimulateDroplets>();
        simulateWaterFlow = GetComponent<SimulateWaterFlow>();

        wc.RegisterWorldCreatedCallback(RetrieveWorld);
    }

    public void RetrieveWorld(SpaceGrid<Tile> world)
    {
        _world = world;
    }

    public void StartSimulation()
    {
        simulateWaterFlow.SetWorld(_world);
        simulateDroplets.StartSimulation(_world); //FIXME
        simulateWaterFlow.StartSimulation();
    }

    public void StopSimulation()
    {
        simulateDroplets.StopSimulation();
        simulateWaterFlow.StopSimulation();
    }

    public void DropWater()
    {
        simulateWaterFlow.SetWorld(_world);
        simulateDroplets.DropAllWater();
        simulateWaterFlow.StartSimulation();
    }

    public void RemoveAllWater()
    {
        for (int x = 0; x < _world.GridSizeX; x++) {
            for (int y = 0; y < _world.GridSizeY; y++) {
                _world.GetNodeAt(x, y).WaterDepth = 0;
            }

        }
    }

    public void StartRain() // extract to new class
    {
        simulateWaterFlow.SetWorld(_world);
        simulateWaterFlow.StartCoroutine("RainCoroutine");
        simulateWaterFlow.StartCoroutine("SimulateWaterCoroutine");
    }

    public void StopRain()
    {
        simulateWaterFlow.StopCoroutine("RainCoroutine");
    }
}
