

public class WaterFlowController : WorldSubscriber
{
    SimulateWaterFlow simulateWaterFlow;

    internal ref VisitationMap<Tile> moistureMap => ref simulateWaterFlow.moistureMap;

    private void Awake()
    {
        simulateWaterFlow = GetComponent<SimulateWaterFlow>();
    }

    internal override void OnInitialize()
    {
        simulateWaterFlow.Setup(world);
    }

    public void StartRain() 
    {
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
                if (t.Water.Level > 0) t.Water.Depth = 0;
            }

        }
        simulateWaterFlow.openSet.Clear();
        simulateWaterFlow.moistureMap.Clear();
    }
}