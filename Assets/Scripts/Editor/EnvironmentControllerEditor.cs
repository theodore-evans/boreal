using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnvironmentController))]
public class EnvironmentControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnvironmentController environmentController = (EnvironmentController)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Start rain")) {
            environmentController.StartRain();
        }

        if (GUILayout.Button("Stop rain")) {
            environmentController.StopRain();
        }

        if (GUILayout.Button("Remove all water")) {
            environmentController.RemoveAllWater();
        }

        if (GUILayout.Button("Grow grass")) {
            environmentController.StartGrowingGrass();
        }

        if (GUILayout.Button("Stop growing grass")) {
            environmentController.StopGrowingGrass();
        }

        if (GUILayout.Button("Remove all grass")) {
            environmentController.RemoveAllGrass();
        }
    }
}
