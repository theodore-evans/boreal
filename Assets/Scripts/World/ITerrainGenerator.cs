public interface ITerrainGenerator
{
    void Initialise(WorldController wc);

    void Generate();
    void RandomizeSeed();
}