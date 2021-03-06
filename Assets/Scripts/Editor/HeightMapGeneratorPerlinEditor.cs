using UnityEngine;
using UnityEditor;
// TODO implement scriptable object to catch updates
[CustomEditor(typeof(HeightMapGeneratorPerlin))]
public class HeightMapGeneratorPerlinEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HeightMapGeneratorPerlin heightMapGenerator = (HeightMapGeneratorPerlin)target;
        TerrainGenerator terrainGen = heightMapGenerator.terrainGenerator;

        if (DrawDefaultInspector()) {
            if (terrainGen != null && terrainGen.autoUpdate) {
                terrainGen.Generate();
            }
        }
    }
}
