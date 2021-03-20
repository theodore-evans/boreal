using UnityEngine;
using System;

public class TileUIController : MonoBehaviour
{
    [SerializeField] WorldController wc = null;
    [SerializeField] GameObject[] UIElementGameObjects;

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
    }

    public void RetrieveWorld(NodeGrid<Tile> world)
    {
        _world = world;
    }

    void Update()
    {
        if (!cursor.IsPointerOutOfFrame) {

            foreach (GameObject UIElement in UIElementGameObjects) UIElement.SetActive(true);

            Tile t = _world.GetNodeAt(cursor.GetPosition());

            if (t != null && t != oldTileUnderCursor) {
                cbNewTileSelected?.Invoke(t);
            }

            oldTileUnderCursor = t;
        }
        else foreach (GameObject go in UIElementGameObjects) go.SetActive(false);
    }
}