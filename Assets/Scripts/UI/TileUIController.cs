using UnityEngine;
using System;

public class TileUIController : MonoBehaviour
{
    [SerializeField] WorldController wc = null;

    GameObject[] UI_gos;

    ICursorProvider cursor;

    private Action<Tile> cbNewTileSelected;

    Tile oldTileUnderCursor = null;

    NodeGrid<Tile> _world;

    private void Awake()
    {
        wc.RegisterWorldCreatedCallback(RetrieveWorld);

        cursor = GetComponent<ICursorProvider>();

        ITileUIUpdateBehaviour[] updateBehaviours = GetComponents<ITileUIUpdateBehaviour>();

        foreach (ITileUIUpdateBehaviour updateBehaviour in updateBehaviours) {
            cbNewTileSelected += updateBehaviour.ActionWhenNewTileSelected;
        }

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
                cbNewTileSelected?.Invoke(t);
            }

            oldTileUnderCursor = t;
        }
        else foreach (GameObject go in UI_gos) go.SetActive(false);
    }
}