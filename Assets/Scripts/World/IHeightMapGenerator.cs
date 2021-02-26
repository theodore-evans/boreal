public interface IHeightMapGenerator
{
    float VerticalScale { get; }

    TerrainGenerator terrainGenerator { get; }

    float[,] GenerateHeightMap(int seed, int mapWidth, int mapHeight, float eps = 0.0001F);
}