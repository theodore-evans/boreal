public interface IHeightMapGenerator
{
    float[,] GenerateHeightMap(int seed, int mapWidth, int mapHeight);
}