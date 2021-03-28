public interface IHeightMapGenerator
{
    float Weight { get; }
    HeightMapType Type { get; }
    float[,] GenerateHeightMap(int seed, int mapWidth, int mapHeight);
}

public enum HeightMapType
{
    Add,
    Multiply
}