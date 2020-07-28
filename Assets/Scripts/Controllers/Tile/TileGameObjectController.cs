using System;
using System.Collections.Generic;
using UnityEngine;

public class TileGameObjectController : MonoBehaviour
{
    // TODO read these in from a config file
    public float tileShaderNoiseSigma = 0.01f;
    public float tileShaderDepth = 0.8f;

    private int width, height;
    private WorldController worldController;

    ITileUpdateBehaviour displayUpdater, propertyUpdater;

    Action<GameObject> cbGameObjectChanged;

    public Dictionary<Tile, GameObject> TileGameObjectMap { get; protected set; }

    public void Initialise(WorldController worldController)
    {
        this.worldController = worldController;

        this.width = worldController.Width;
        this.height = worldController.Height;
    }

    private void Start()
    {
        displayUpdater = new TileSpriteUpdateBehaviour(tileShaderNoiseSigma, tileShaderDepth);
        propertyUpdater = new TilePropertyUpdateBehaviour();

        //Create GameObjects to display for each tile
        TileGameObjectMap = new Dictionary<Tile, GameObject>();

        List<GameObject> tile_gos = new List<GameObject>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                // creates a new go and adds it to the scene
                GameObject tile_go = new GameObject();

                // add box collider for pathfinding, make sure collider is centered on tile
                tile_go.AddComponent<BoxCollider>();
                tile_go.GetComponent<BoxCollider>().center = new Vector3(0.5f, 0.5f, 0);

                // TODO: remove coupling between tile controller and world??
                Tile tile_data = worldController.GetTileAt(new Vector3 (x,y));

                // add mapping of game object -> tile data to the dictionary
                TileGameObjectMap.Add(tile_data, tile_go);

                // sets the tile_go position
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y);
                tile_go.transform.SetParent(this.gameObject.transform, true);
            }
        }

        worldController.RegisterTileChangedCallback(OnTileChanged);
    }

    public GameObject GetTileGameObject(Tile tile_data)
    {
        if (TileGameObjectMap.ContainsKey(tile_data) == false)
        {
            Debug.LogError($"GetTileGameObject: No key found in tileGameObjectMap for tile at [{tile_data.X}, {tile_data.Y}] ");
            return null;
        }

        GameObject tile_go = TileGameObjectMap[tile_data];

        if (tile_go == null)
        {
            Debug.LogError($"GetTileGameObject: No GameObject found for tile at [{tile_data.X}, {tile_data.Y}] ");
            return null;
        }

        return tile_go;
    }

    public void OnTileChanged(Tile tile_data)
    {
        GameObject tile_go = GetTileGameObject(tile_data);

        displayUpdater.OnTileChanged(tile_go, tile_data);
        propertyUpdater.OnTileChanged(tile_go, tile_data);

        cbGameObjectChanged?.Invoke(tile_go);
    }

    public void RegisterGameObjectChangedCallback(Action<GameObject> callback)
    {
        cbGameObjectChanged += callback;
    }

    public void UnregisterGameObjectChangedCallback(Action<GameObject> callback)
    {
        cbGameObjectChanged -= callback;
    }
}
