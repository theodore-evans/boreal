using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PerlinTerrainGen))]
public class PerlinTerrainGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PerlinTerrainGen terrainGen = (PerlinTerrainGen)target;

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
