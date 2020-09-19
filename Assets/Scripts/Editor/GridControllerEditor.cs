using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (PathGridController))]
public class GridControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
		PathGridController gridController = (PathGridController)target;

		if (DrawDefaultInspector())
		{
			if (gridController.autoUpdate)
			{
				gridController.CreateGrid();
			}
		}

		if (GUILayout.Button("Regenerate Grid"))
		{
			gridController.CreateGrid();
		}
	}
}
