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

        if (GUILayout.Button("Remove all water")) {
            waterSim.RemoveAllWater();
        }

        if (GUILayout.Button("Start rain")) {
            waterSim.StartRain();
        }

        if (GUILayout.Button("Stop rain")) {
            waterSim.StopRain();
        }

        if (GUILayout.Button("Stop")) {
            waterSim.StopSimulation();
        }
    }
}
