using UnityEngine;
using System.Collections.Generic;

public abstract class Controller : MonoBehaviour
{
    internal WorldController worldController;
    internal NodeGrid<Tile> world;

    private void OnEnable()
    {
        worldController = GetComponentInParent<WorldController>();
        worldController.RegisterWorldCreatedCallback(Initialize);
    }

    public virtual void Initialize(NodeGrid<Tile> world)
    {
        this.world = world;
        OnInitialize();
        worldController.RegisterWorldChangedCallback(UpdateTiles);
    }

    public virtual void UpdateTiles(IEnumerable<Tile> tile) { }
    internal virtual void OnInitialize() { }
}