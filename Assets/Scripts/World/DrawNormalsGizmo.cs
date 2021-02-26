using UnityEngine;
using System.Collections.Generic;

public class DrawNormalsGizmo : MonoBehaviour
{
    private SpaceGrid<Tile> _world;
    //FIXME: doesn't work, don't know why
    private void Awake()
    {
        GetComponent<WorldController>().RegisterWorldCreatedCallback(Initialize);
    }

    private void Initialize(SpaceGrid<Tile> world)
    {
        _world = world;
    }

    void OnDrawGizmos()
    {
        if (_world != null) {
            foreach (Tile tile in _world) {

                Vector3 tileCenter = new Vector3(tile.X + 0.5f * tile.Scale, tile.Y + 0.5f * tile.Scale, 10f);
                float gradient = tile.Gradient;

                Color color = Color.Lerp(Color.red, Color.green, 1 - (gradient * 5f));

                Gizmos.color = color;

                Gizmos.DrawLine(tileCenter, tileCenter + tile.Downhill);
            }
        }
    }
}
