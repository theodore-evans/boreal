using UnityEngine;
using System.Collections.Generic;

public class DrawNormalsGizmos : MonoBehaviour
{
    private NodeGrid<Tile> _world;
    private void Awake()
    {
        GetComponent<WorldController>().RegisterWorldCreatedCallback(RetrieveWorld);
    }

    private void RetrieveWorld(NodeGrid<Tile> world)
    {
        _world = world;
    }

    void OnDrawGizmosSelected()
    {
        if (_world != null) {
            foreach (Tile tile in _world.Nodes) {

                Vector3 tileCenter = new Vector3(tile.X + 0.5f * tile.Scale, tile.Y + 0.5f * tile.Scale, 0f);
                float gradient = tile.Gradient;

                Color color = Color.Lerp(Color.red, Color.black, 1 - (gradient * 5f));

                Gizmos.color = color;

                Gizmos.DrawLine(tileCenter, tileCenter + tile.Normal);
            }
        }
    }
}
