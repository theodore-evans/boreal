using UnityEngine;
using System.Collections;

public class WaterSimController : MonoBehaviour
{
    WorldController worldController;

    internal NodeGrid<Tile> _world;
    SimulateWaterFlow simulateWaterFlow;

    private void Awake()
    {
        worldController = GetComponentInParent<WorldController>();
        simulateWaterFlow = GetComponent<SimulateWaterFlow>();
        worldController.RegisterWorldCreatedCallback(Initialize);
    }

    public void Initialize(NodeGrid<Tile> world)
    {
        _world = world;
    }

    public void StartRain() // extract to new class
    {
        simulateWaterFlow.SetWorld(_world);
        simulateWaterFlow.StartRain();
    }

    public void StopRain()
    {
        simulateWaterFlow.StopRain();
    }

    public void RemoveAllWater()
    {
        for (int x = 0; x < _world.GridSizeX; x++) {
            for (int y = 0; y < _world.GridSizeY; y++) {
                Tile t = _world.GetNodeAt(x, y);
                if (t.WaterLevel > 0) t.WaterDepth = 0;
            }

        }
    }

}
