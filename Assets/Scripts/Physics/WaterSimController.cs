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

        wc.RegisterWorldCreatedCallback(Initialize);
    }

    public void Initialize(SpaceGrid<Tile> world)
    {
        _world = world;
        
    }

    public void StartSimulation()
    {
        simulateDroplets.StartSimulation(_world);
        simulateWaterFlow.StartSimulation(_world);
    }

    public void StopSimulation()
    {
        simulateDroplets.StopSimulation();
        simulateWaterFlow.StopSimulation();
    }

    public void DropWater()
    {
        simulateDroplets.DropAllWater();
        simulateWaterFlow.StartSimulation(_world);
    }

    public void ResetWater()
    {
        for (int x = 0; x < _world.GridSizeX; x++) {
            for (int y = 0; y < _world.GridSizeY; y++) {
                _world.GetNodeAt(x, y).WaterDepth = 0;
            }

        }
    }
}
