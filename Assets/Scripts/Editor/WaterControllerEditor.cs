using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaterFlowController))]
public class WaterControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WaterFlowController environmentController = (WaterFlowController)target;

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
    }
}
