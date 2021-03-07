using UnityEngine;
using UnityEditor;
// TODO implement scriptable object to catch updates
[CustomEditor(typeof(HeightMapGeneratorFunction))]
public class HeightMapGeneratorFunctionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HeightMapGeneratorFunction heightMapGenerator = (HeightMapGeneratorFunction)target;
        TerrainGenerator terrainGen = heightMapGenerator.terrainGenerator;

        if (DrawDefaultInspector()) {
            if (terrainGen != null && terrainGen.autoUpdate) {
                terrainGen.Generate();
            }
        }
    }
}
