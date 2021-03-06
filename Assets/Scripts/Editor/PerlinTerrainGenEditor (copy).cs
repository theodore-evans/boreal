using UnityEngine;
using UnityEditor;
// TODO implement scriptable object to catch updates
[CustomEditor(typeof(HeightMapDiamondSquare))]
public class HeightMapGeneratorDiamondEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HeightMapDiamondSquare heightMapGenerator = (HeightMapDiamondSquare)target;
        TerrainGenerator terrainGen = heightMapGenerator.terrainGenerator;

        if (DrawDefaultInspector()) {
            if (terrainGen.autoUpdate) {
                terrainGen.Generate();
            }
        }
    }
}
