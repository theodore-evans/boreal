using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
		TerrainGenerator terrainGen = (TerrainGenerator)target;

		if (DrawDefaultInspector())
		{
			if (terrainGen.autoUpdate)
			{
				terrainGen.GenerateTerrain();
			}
		}

		if (GUILayout.Button("Generate"))
		{
			terrainGen.GenerateTerrain();
		}

		if (GUILayout.Button("Randomize"))
		{
			terrainGen.RandomizeTerrain();
		}
	}
}
