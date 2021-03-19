public interface ITerrainGenerator
{
    float VerticalScale { get; }

    void Generate(NodeGrid<Tile> world);
    void Generate();
    void RandomizeSeed();
}