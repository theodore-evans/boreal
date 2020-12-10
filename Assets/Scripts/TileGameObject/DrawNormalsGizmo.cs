using UnityEngine;
using System.Collections.Generic;

public class DrawNormalsGizmo : MonoBehaviour
{
    Dictionary<Tile, GameObject> tileGameObjectMap;

    private void Start()
    {
        tileGameObjectMap = GetComponent<TileGOController>().TileGameObjectMap;
    }

    void OnDrawGizmosSelected()
    {
        if (tileGameObjectMap != null) {

            foreach (KeyValuePair<Tile, GameObject> entry in tileGameObjectMap) {

                Tile t = entry.Key;
                GameObject t_go = entry.Value;

                Vector3 tileCenter = t_go.transform.GetComponent<Collider>().bounds.center;
                float gradient = t.Gradient;

                Color color = Color.Lerp(Color.red, Color.green, 1 - (gradient * 5f));

                Gizmos.color = color;

                Gizmos.DrawLine(tileCenter, tileCenter + t.Downhill);
            }
        }
    }
}
