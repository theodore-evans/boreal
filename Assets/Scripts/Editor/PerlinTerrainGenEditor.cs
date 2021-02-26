using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PerlinHeightMapGenerator))]
public class PerlinHeightMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PerlinHeightMapGenerator heightMapGenerator = (PerlinHeightMapGenerator)target;
        TerrainGenerator terrainGen = heightMapGenerator.terrainGenerator;

        if (DrawDefaultInspector()) {
            if (terrainGen.autoUpdate) {
                terrainGen.Generate();
            }
        }

        if (GUILayout.Button("Generate")) {
            terrainGen.Generate();
        }

        if (GUILayout.Button("Randomize")) {
            terrainGen.RandomizeSeed();
            terrainGen.Generate();
        }
    }
}
