using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeightMapGeneratorPerlin))]
public class PerlinHeightMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HeightMapGeneratorPerlin heightMapGenerator = (HeightMapGeneratorPerlin)target;
        TerrainGenerator terrainGen = heightMapGenerator.terrainGenerator;

        if (DrawDefaultInspector()) {
            if (terrainGen.autoUpdate) {
                terrainGen.Generate();
            }
        }
    }
}
