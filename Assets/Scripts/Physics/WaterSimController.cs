using UnityEngine;
using System.Collections;

public class WaterSimController : Controller
{
    SimulateWaterFlow simulateWaterFlow;

    private void Awake()
    {
        simulateWaterFlow = GetComponent<SimulateWaterFlow>();
    }

    public void StartRain() // extract to new class
    {
        simulateWaterFlow.SetWorld(world);
        simulateWaterFlow.StartRain();
    }

    public void StopRain()
    {
        simulateWaterFlow.StopRain();
    }

    public void RemoveAllWater()
    {
        for (int x = 0; x < world.GridSizeX; x++) {
            for (int y = 0; y < world.GridSizeY; y++) {
                Tile t = world.GetNodeAt(x, y);
                if (t.WaterLevel > 0) t.WaterDepth = 0;
            }

        }
    }

}
