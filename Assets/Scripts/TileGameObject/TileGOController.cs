using System;
using System.Collections.Generic;
using UnityEngine;

//TODO remove this whole thing! Generate a mesh instead!!
public class TileGOController : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab = null;

    private NodeGrid<Tile> _world;

    ITileGOUpdateBehaviour[] tileUpdateBehaviours;

    Action<GameObject> cbTileGameObjectChanged;

    public Dictionary<Tile, GameObject> TileGameObjectMap { get; protected set; }

    private void Awake()
    {
        tileUpdateBehaviours = GetComponents<ITileGOUpdateBehaviour>();
    }

    public void Initialise(WorldController wc)
    {
        _world = wc.world;
        wc.RegisterWorldChangedCallback(OnTileChanged);
    }

    public void OnTileChanged(IEnumerable<Tile> changedTiles)
    {
        foreach (Tile tile in changedTiles) {
            GameObject tile_go = GetTileGameObject(tile);

            foreach (ITileGOUpdateBehaviour tileUpdater in tileUpdateBehaviours) {
                tileUpdater.UpdateTile(tile_go, tile);
            }

            cbTileGameObjectChanged?.Invoke(tile_go);
        }
    }

    public void CreateTileGameObjects()
    {
        TileGameObjectMap = new Dictionary<Tile, GameObject>();

        foreach (Tile t in _world.Nodes) {
            GameObject newTileGameObject = Instantiate(tilePrefab, _world.GetNodePosition(t), Quaternion.identity, transform);

            newTileGameObject.name = $"Tile_{t.X}_{t.Y}";
            TileGameObjectMap.Add(t, newTileGameObject);
        }
    }

    public GameObject GetTileGameObject(Tile tile_data)
    {
        if (TileGameObjectMap.ContainsKey(tile_data)) {

            GameObject tile_go = TileGameObjectMap[tile_data];

            if (tile_go != null) return tile_go;
            else Debug.LogError($"GetTileGameObject: No GameObject found for tile at [{tile_data.X}, {tile_data.Y}] ");
        }
        else Debug.LogError($"GetTileGameObject: No key found in tileGameObjectMap for tile at [{tile_data.X}, {tile_data.Y}] ");
        return null;
    }

    public void RegisterTileGameObjectChangedCallback(Action<GameObject> callback)
    {
        cbTileGameObjectChanged += callback;
    }

    public void UnregisterTileGameObjectChangedCallback(Action<GameObject> callback)
    {
        cbTileGameObjectChanged -= callback;
    }
}
