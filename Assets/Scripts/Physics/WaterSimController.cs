using UnityEngine;
using System.Collections;

public class WaterSimController : MonoBehaviour
{
    [SerializeField] WorldController wc = null;

    internal SpaceGrid<Tile> world;
    SimulateDroplets simulateDroplets;
    SimulateWaterFlow simulateWaterFlow;

    void Start()
    {
        world = wc.world;
        simulateDroplets = GetComponent<SimulateDroplets>();
        simulateWaterFlow = GetComponent<SimulateWaterFlow>();
    }

    public void StartSimulation()
    {
        simulateDroplets.StartSimulation();
        simulateWaterFlow.StartSimulation();
    }

    public void StopSimulation()
    {
        simulateDroplets.StopSimulation();
        simulateWaterFlow.StopSimulation();
    }

    public void DropWater()
    {
        simulateDroplets.DropAllWater();
    }

    public void ResetWater()
    {
        for (int x = 0; x < world.GridSizeX; x++) {
            for (int y = 0; y < world.GridSizeY; y++) {
                world.GetNodeAt(x, y).WaterDepth = 0;
            }

        }
    }
}
