using System;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public SpaceGrid<Tile> world { get; protected set; }
    public int Width { get => width; private set => width = Mathf.Max(1, value); }
    public int Height { get => height; private set => height = Mathf.Max(1, value); }

    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;
    [SerializeField] GameObject tileGOController_go = null;
    //[SerializeField] GameObject weatherController_go = null;

    private ITerrainGenerator terrainGen;
    private INormalCalculator normalCalculator;
    private TileGOController tileGOController;

    private Action<Tile> cbWorldChanged;

    private void Awake() // TODO check whether the order of these initialisations matters
    {
        CreateWorldGrid();

        normalCalculator = GetComponent<INormalCalculator>();
        terrainGen = GetComponent<ITerrainGenerator>();
        tileGOController = tileGOController_go.GetComponent<TileGOController>();
        
        //TODO some of this is completely unnecessary
        tileGOController.Initialise(this);
        normalCalculator.Initialise(this);
        terrainGen.Initialise(this);
       
        //tileGOController.CreateTileGameObjects();
    }

    private void Start()
    {
        terrainGen.Generate();
    }

    private void CreateWorldGrid()
    {
        world = new SpaceGrid<Tile>(tileGOController_go.transform.position, width, height, 1f);
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Tile newTile = new Tile(x, y);
                newTile.RegisterTileChangedCallback(OnTileChanged);
                world.SetNodeAt(x, y, newTile);
            }
        }
    }

    void OnTileChanged(Tile t) // TODO implement update-based callback caching
    {
        //cbWorldChanged?.Invoke(t);
    }

    public void RegisterWorldChangedCallback(Action<Tile> callback)
    {
        cbWorldChanged += callback;
    }

    public void UnregisterWorldChangedCallback(Action<Tile> callback)
    {
        cbWorldChanged -= callback;
    }

    private void OnValidate()
    {
        Width = width;
        Height = height;
    }
}
