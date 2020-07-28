using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Grid))]
public class PathfindingGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
		Grid grid = (Grid)target;

		if (DrawDefaultInspector())
		{
			if (grid.autoUpdate)
			{
				grid.CreateGrid();
			}
		}

		if (GUILayout.Button("Regenerate Grid"))
		{
			grid.CreateGrid();
		}
	}
}
