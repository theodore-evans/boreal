using System;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    World world;

    public float tileShaderNoiseSigma = 0.01f;
    public float tileShaderDepth = 0.8f;

    ITileUpdateBehaviour displayUpdater, propertyUpdater;

    Action<GameObject> cbGameObjectChanged;

    public Dictionary<Tile, GameObject> TileGameObjectMap { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        //TODO add tilecontroller as child of worldcontroller
        world = GetComponentInParent<WorldController>().World;

        displayUpdater = new TileSpriteUpdateBehaviour(tileShaderNoiseSigma, tileShaderDepth);
        propertyUpdater = new TilePropertyUpdateBehaviour();

        //Create GameObjects to display for each tile
        TileGameObjectMap = new Dictionary<Tile, GameObject>();

        for (int x = 0; x < world.Width; x++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                // creates a new go and adds it to the scene
                GameObject tile_go = new GameObject();

                // add box collider for pathfinding, make sure collider is centered on tile
                tile_go.AddComponent<BoxCollider>();
                tile_go.GetComponent<BoxCollider>().center = new Vector3(0.5f, 0.5f, 0);

                // gets the Tile for this point in the TheWorld
                Tile tile_data = world.GetTileAt(x, y);

                // add mapping of game object -> tile data to the dictionary
                TileGameObjectMap.Add(tile_data, tile_go);

                // sets the tile_go position
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y);
                tile_go.transform.SetParent(this.gameObject.transform, true);

                OnTileChanged(tile_data);
            }
        }

        world.RegisterTileChangedCallback(OnTileChanged);

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

    void OnTileChanged(Tile tile_data)
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
