using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TileUIController : MonoBehaviour
{
    [SerializeField] GameObject worldController_go = null;
    
    GameObject[] UI_gos;

    ICursorProvider cursor;

    ITileUIUpdateBehaviour[] updateBehaviours;

    Tile oldTileUnderCursor = null;

    SpaceGrid<Tile> world;

    private void Start()
    {
        cursor = GetComponent<ICursorProvider>();
        updateBehaviours = GetComponents<ITileUIUpdateBehaviour>();

        world = worldController_go.GetComponent<WorldController>().world;

        UI_gos = GameObject.FindGameObjectsWithTag("TileUI");
    }

    void Update()
    {
        //FIXME this check is not working, cursor is always being considered to be in frame
        if (!cursor.IsPointerOutOfFrame) {
            Tile t = world.GetNodeAt(cursor.GetPosition());

            if (t != null && t != oldTileUnderCursor) {
                foreach (ITileUIUpdateBehaviour updateBehaviour in updateBehaviours) {
                    updateBehaviour.UpdateTileUI(cursor, t);
                }
            }

            oldTileUnderCursor = t;
        }
        else foreach (GameObject go in UI_gos) go.SetActive(false);
    }
}
