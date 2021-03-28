using UnityEngine;
using System;
using System.Collections;
using System.Linq;

[Serializable]
public class GrassGrowthParameters
{
    [Range(0, 100)] public int numberOfStartingSeeds = 10;
    [Range(0f, 1f)] public float growthRate = 0.01f;
    [Range(0,1)] public float minSaturation = 0.1f;
    [Range(0, 1)] public float maxSaturation = 0.9f;
    [Range(0, 90)] public float maxGradient = 45f;
    public AnimationCurve growthRateOverGradient;
    public AnimationCurve growthRateOverSaturation;
}


public class GrowGrass : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] float simulationSpeed = 0.75f;
    [SerializeField] GrassGrowthParameters parameters;

    internal Cache<Tile> openSet = new Cache<Tile>();
    private Coroutine sowGrassCoroutine;
    private Coroutine growGrassCoroutine;
    private WaitForSeconds waitBetweenIterations;

    private NodeGrid<Tile> _world;

    System.Random rng = new System.Random();

    private bool CanNotGrow(Tile tile, float availableWater) =>
        availableWater < parameters.minSaturation
        || availableWater >= parameters.maxSaturation
        || tile.Relief.Gradient >= parameters.maxGradient
        || tile.Relief.Elevation <= 0;

    public void Setup(NodeGrid<Tile> world)
    {
        _world = world;
    }

    public void StartGrowingGrass()
    {
        waitBetweenIterations = new WaitForSeconds(Mathf.Lerp(1f, 0f, simulationSpeed));
        StopGrowingGrass();

        SowGrass();
        growGrassCoroutine = StartCoroutine(nameof(GrowGrassCoroutine));
    }

    public void StopGrowingGrass()
    {
        StopAllCoroutines();
    }

    private void SowGrass()
    {
        int i = 0;
        while (i < parameters.numberOfStartingSeeds) {
            Tile newGrassTile = _world.Nodes[rng.Next(_world.MaxSize)];
            if (CanNotGrow(newGrassTile, newGrassTile.Water.Saturation)) continue;
            else { 
                newGrassTile.Cover.Grass += parameters.growthRate;
                openSet.Add(newGrassTile);
                i++;
            }
        }
    }

    private IEnumerator GrowGrassCoroutine()
    {
        for (; ; ) {
            foreach (Tile tile in openSet.ToList()) {
                Grow(tile);
            }

            yield return waitBetweenIterations;
        }
    }

    private void Grow(Tile tile)
    {
        IEnumerable neighbours = _world.GetNeighbours(tile);

        if (tile.Relief.Elevation > 0) {
            float availableWater = 0;
            foreach (Tile neighbour in neighbours) {
                availableWater += neighbour.Water.Saturation - neighbour.Cover.Grass;

                if (CanNotGrow(neighbour, neighbour.Water.Saturation) == false) {
                    openSet.Add(neighbour);
                }
            }
            availableWater += tile.Water.Saturation - tile.Cover.Grass;
            availableWater = Mathf.Clamp01(availableWater / 9);

            float newGrowth = parameters.growthRate;
            newGrowth *= parameters.growthRateOverGradient.Evaluate(tile.Relief.Gradient / 90f) *
                parameters.growthRateOverSaturation.Evaluate(availableWater);

            foreach (Tile neighbour in neighbours) {
                neighbour.Cover.Grass += newGrowth / 2f;
            }
            tile.Cover.Grass += newGrowth;
        }

        if (tile.Cover.Grass <= 0) openSet.Remove(tile);
    }

}
