using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaterSimController))]
public class WaterSimEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WaterSimController waterSim = (WaterSimController)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Start")) {
            waterSim.StartSimulation();
        }

        if (GUILayout.Button("Drop Water")) {
            waterSim.DropWater();
        }

        if (GUILayout.Button("Reset Water")) {
            waterSim.ResetWater();
        }

        if (GUILayout.Button("Stop")) {
            waterSim.StopSimulation();
        }
    }
}
