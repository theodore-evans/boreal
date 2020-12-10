using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpdateTileGONormalShading))]
public class UpdateNormalShadingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UpdateTileGONormalShading normalShading = (UpdateTileGONormalShading)target;

        if (DrawDefaultInspector()) {
            normalShading.UpdateLightDirection();
        }
    }
}