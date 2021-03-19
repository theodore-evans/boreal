using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TileUIController : MonoBehaviour
{
    [SerializeField] WorldController wc = null;
    
    GameObject[] UI_gos;

    ICursorProvider cursor;

    ITileUIUpdateBehaviour[] updateBehaviours;

    Tile oldTileUnderCursor = null;

    NodeGrid<Tile> _world;

    private void Awake()
    {
        wc.RegisterWorldCreatedCallback(RetrieveWorld);

        cursor = GetComponent<ICursorProvider>();
        updateBehaviours = GetComponents<ITileUIUpdateBehaviour>();
        UI_gos = GameObject.FindGameObjectsWithTag("TileUI");
    }

    public void RetrieveWorld(NodeGrid<Tile> world)
    {
        _world = world;
    }

    void Update()
    {
        if (!cursor.IsPointerOutOfFrame) {
            Tile t = _world.GetNodeAt(cursor.GetPosition());

            if (t != null && t != oldTileUnderCursor) {
                foreach (ITileUIUpdateBehaviour updateBehaviour in updateBehaviours) {
                    updateBehaviour.UpdateTileUI(ref cursor, t);
                }
            }

            oldTileUnderCursor = t;
        }
        else foreach (GameObject go in UI_gos) go.SetActive(false);
    }
}
