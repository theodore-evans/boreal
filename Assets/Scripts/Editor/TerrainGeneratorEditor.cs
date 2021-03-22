using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainGenerator terrainGen = (TerrainGenerator)target;

        if (DrawDefaultInspector()) {
            if (terrainGen.autoUpdate) {
                if (!terrainGen.IsGenerating) terrainGen.Generate();
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
