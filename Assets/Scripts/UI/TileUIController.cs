using UnityEngine;
using System;

public class TileUIController : MonoBehaviour
{
    [SerializeField] GameObject[] UIElementGameObjects;
    [SerializeField] Camera currentCamera;

    ICursorProvider cursor;

    private Action<Tile> cbNewTileSelected;
    Tile oldTileUnderCursor = null;

    WorldController worldController;
    NodeGrid<Tile> _world;

    private void Awake()
    {
        worldController = GetComponentInParent<WorldController>();
        worldController.RegisterWorldCreatedCallback(Initialize);

        cursor = GetComponent<ICursorProvider>();
        cursor.SetCamera(ref currentCamera);

        ITileUIUpdateBehaviour[] updateBehaviours = GetComponents<ITileUIUpdateBehaviour>();

        foreach (ITileUIUpdateBehaviour updateBehaviour in updateBehaviours) {
            cbNewTileSelected += updateBehaviour.ActionWhenNewTileSelected;
        }
    }

    public void Initialize(NodeGrid<Tile> world)
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