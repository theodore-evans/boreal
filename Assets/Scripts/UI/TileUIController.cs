using UnityEngine;
using System;

public class TileUIController : Controller
{
    [SerializeField] GameObject[] UIElementGameObjects;
    [SerializeField] Camera currentCamera;

    ICursorProvider cursor;

    private Action<Tile> cbNewTileSelected;
    Tile oldTileUnderCursor = null;

    private void Awake()
    {
        cursor = GetComponent<ICursorProvider>();
        cursor.SetCamera(ref currentCamera);

        ITileUIUpdateBehaviour[] updateBehaviours = GetComponents<ITileUIUpdateBehaviour>();

        foreach (ITileUIUpdateBehaviour updateBehaviour in updateBehaviours) {
            cbNewTileSelected += updateBehaviour.ActionWhenNewTileSelected;
        }
    }

    void Update()
    {
        if (!cursor.IsPointerOutOfFrame) {

            foreach (GameObject UIElement in UIElementGameObjects) UIElement.SetActive(true);

            Tile t = world.GetNodeAt(cursor.GetPosition());

            if (t != null && t != oldTileUnderCursor) {
                cbNewTileSelected?.Invoke(t);
            }

            oldTileUnderCursor = t;
        }
        else foreach (GameObject go in UIElementGameObjects) go.SetActive(false);
    }
}