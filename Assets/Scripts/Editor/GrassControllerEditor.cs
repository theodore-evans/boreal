using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GrassController))]
public class GrassControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GrassController grassController = (GrassController)target;

        if (DrawDefaultInspector()) {
            grassController.UpdateAllGrass();
        }
        
    }
}
