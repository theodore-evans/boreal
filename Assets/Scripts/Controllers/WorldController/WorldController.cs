using UnityEngine;

public class WorldController : MonoBehaviour
{
    public int width;
    public int height;

    public static WorldController Instance { get; protected set; }

    public World World { get; protected set; }

    // Start is called before the first frame update
    void Awake()
    {
        // initialise random number generator
        if (Instance != null)
        {
            Debug.Log("There should never be two instances of WorldController");
        }

        Instance = this;
        World = new World(width, height);

        Vector3 worldCenter = new Vector3((float)width / 2f, (float)height / 2f, Camera.main.transform.position.z);

        Camera.main.transform.position = worldCenter;

        TerrainGenerator terrainGenerator = GetComponent<TerrainGenerator>();
        if (terrainGenerator == null) {
            terrainGenerator = gameObject.AddComponent<TerrainGenerator>();
        }
        terrainGenerator.GenerateTerrain(World);
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

}
