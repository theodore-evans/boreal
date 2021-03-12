using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CheckTileWalkability))]
public class WalkabilityCheckerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		CheckTileWalkability walkabilityChecker = (CheckTileWalkability)target;
		PathGridController gridController = walkabilityChecker.GetComponent<PathGridController>();

		if (DrawDefaultInspector()) {
			if (gridController.autoUpdate) {
				gridController.CreateGrid();
			}
		}
	}
}
