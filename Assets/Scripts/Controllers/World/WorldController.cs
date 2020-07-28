using System;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public int Width;
    public int Height;

    public GameObject tileController_go, pathfindingController_go, navigationController_go;

    public World World { get; protected set; }

    bool isFirstFrame = true;

    // Start is called before the first frame update
    void Awake()
    {
        World = new World(Width, Height);

        // TODO: handle gameobject initialisation in a separate class
        TileGameObjectController tileGameObjectController = tileController_go.GetComponent<TileGameObjectController>();
        PathfindingController pathfindingController = pathfindingController_go.GetComponent<PathfindingController>();
        NavigationController navigationController = navigationController_go.GetComponent<NavigationController>();

        tileGameObjectController.Initialise(this);
        pathfindingController.Initialise(this, tileGameObjectController);
        navigationController.Initialise(this);

        //TODO: add gameobjects from prefab if they don't exist
    }

    private void Update()
    {
        if (isFirstFrame) {
            TerrainGenerator terrainGenerator = GetComponent<TerrainGenerator>();
            if (terrainGenerator == null) {
                terrainGenerator = gameObject.AddComponent<TerrainGenerator>();
            }

            terrainGenerator.GenerateTerrain(World);
            isFirstFrame = false;
        }
    }

    public Tile GetTileAt(Vector3 worldPoint)
    {
        int x = Mathf.FloorToInt(worldPoint.x);
        int y = Mathf.FloorToInt(worldPoint.y);

        Tile tile = World.GetTileAt(x, y);
        if (tile == null) {
            Debug.LogError("No tile at point " + worldPoint);
            return null;
        }
        else return tile;

    }

    // hack this can be done better?
    public void RegisterTileChangedCallback(Action<Tile> onTileChanged)
    {
        World.RegisterTileChangedCallback(onTileChanged);
    }

    public void UnregisterTileChangedCallback(Action<Tile> onTileChanged)
    {
        World.UnregisterTileChangedCallback(onTileChanged);
    }

    private void OnValidate()
    {
        if (Width <= 0) {
            Width = 1;
        }

        if (Height <= 0) {
            Height = 1;
        }
    }
}
