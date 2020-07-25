using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (PathfindingController))]
public class PathfindingGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
		PathfindingController pathfinding= (PathfindingController)target;

		if (DrawDefaultInspector())
		{
			if (pathfinding.autoUpdate)
			{
				pathfinding.RegenerateGrid();
			}
		}

		if (GUILayout.Button("Regenerate Grid"))
		{
			pathfinding.RegenerateGrid();
		}
	}
}
