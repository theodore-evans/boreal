
public class EnvironmentController : Controller
{
    GrowGrass growGrass;
    SimulateWaterFlow simulateWaterFlow;

    internal ref VisitationMap<Tile> moistureMap => ref simulateWaterFlow.moistureMap;

    private void Awake()
    {
        growGrass = GetComponent<GrowGrass>();
        simulateWaterFlow = GetComponent<SimulateWaterFlow>();
        
    }

    internal override void OnInitialize()
    {
        growGrass.Setup(world);
        simulateWaterFlow.Setup(world);
    }

    public void StartGrowingGrass() // extract to new class
    {
        growGrass.StartGrowingGrass();
    }

    public void StopGrowingGrass()
    {
        growGrass.StopGrowingGrass();
    }

    public void RemoveAllGrass()
    {
        for (int x = 0; x < world.GridSizeX; x++) {
            for (int y = 0; y < world.GridSizeY; y++) {
                world.GetNodeAt(x, y).Cover.Grass = 0;
            }

        }
        growGrass.openSet.Clear();
    }

    public void StartRain() // extract to new class
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