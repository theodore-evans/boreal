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
				grid.UpdateGrid();
			}
		}

		if (GUILayout.Button("Regenerate Grid"))
		{
			grid.UpdateGrid();
		}
	}
}
