using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] MeshFilter meshFilter = null;
	[SerializeField] MeshRenderer meshRenderer = null;

    [SerializeField] Color32[] tileColours = new Color32[] {
        Color.white,  //Blank
        Color.blue,   //Water
        Color.red,   //Soil
        Color.green   //Grass
    };

    [SerializeField] WorldController wc = null;
    SpaceGrid<Tile> _world;

    MeshGenerator meshGenerator;
    TextureGenerator textureGenerator;

    private int width;
    private int height;
    private MapMesh mapMesh;

    private struct MapMesh
    {
        public Mesh mesh;
        public Bounds bounds;

        public MapMesh(Mesh mesh, Bounds bounds)
        {
            this.mesh = mesh;
            this.bounds = bounds;
        }
    }

    private void Awake()
    {
        wc.RegisterWorldCreatedCallback(Initialize);
    }

    private void Initialize(SpaceGrid<Tile> world)
    {
        _world = world;

        meshGenerator = new MeshGenerator();
        textureGenerator = new TextureGenerator(filterMode: FilterMode.Bilinear);

        wc.RegisterWorldChangedCallback(UpdateMesh);

        width = wc.WorldWidth;
        height = wc.WorldHeight;
        
        mapMesh = CreateMapMesh(0, 0, width, height);
        meshFilter.sharedMesh = mapMesh.mesh;
    }

    bool TileIsWithinMeshBounds(Tile tile, MapMesh mesh)
    {
        return true;
    }

    private void UpdateMesh(HashSet<Tile> changedTiles)
    {
        foreach (Tile changedTile in changedTiles) {
            if (TileIsWithinMeshBounds(changedTile, mapMesh)) {
                UpdateMeshTexture(CreateMapTexture());
                break;
            }
        }   
    }

    public void UpdateMeshTexture(Texture2D texture)
	{
		meshRenderer.material.SetTexture("_Control", texture); //TODO mesh rendering for individual meshes
	}

    private MapMesh CreateMapMesh(int bottomLeftX, int bottomLeftY, int width, int height)
    {

        Mesh mesh = meshGenerator.CreateMesh(bottomLeftX, bottomLeftY, width, height);
        Vector3 center = new Vector3((bottomLeftX + width) / 2, (bottomLeftY + height) / 2, 0);
        Vector3 size = new Vector3(width, height, 0);
        Bounds bounds = new Bounds(center, size);

        return new MapMesh(mesh, bounds);
    }

    public Texture2D CreateMapTexture()
    {
        Color32[] colourMap = new Color32[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                Color tileColour = tileColours[(int)_world.GetNodeAt(x, y).TypeId];
                colourMap[y * width + x] = tileColour;
            }
        }

        return textureGenerator.CreateTextureFromColourMap(colourMap, width, height);
    }
}
