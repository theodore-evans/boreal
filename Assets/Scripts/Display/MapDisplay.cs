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
    float verticalScale;

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

    private struct MapTexture
    {
        public Texture controlTexture;
        public Texture normalTexture;

        public MapTexture(Texture controlTexture, Texture normalTexture)
        {
            this.controlTexture = controlTexture;
            this.normalTexture = normalTexture;
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
        verticalScale = wc.WorldVerticalScale;
        mapMesh = CreateMapMesh(0, 0, width, height);
        meshFilter.sharedMesh = mapMesh.mesh;
        //meshRenderer.material.EnableKeyword("_NORMALMAP");
    }

    bool TileIsWithinMeshBounds(Tile tile, MapMesh mesh)
    {
        return true;
    }

    private void UpdateMesh(HashSet<Tile> changedTiles)
    {
        foreach (Tile changedTile in changedTiles) {
            if (TileIsWithinMeshBounds(changedTile, mapMesh)) {
                UpdateMeshTexture();
                break;
            }
        }   
    }

    public void UpdateMeshTexture()
	{
        MapTexture mapTexture = CreateMapTexture();

        meshRenderer.material.SetTexture("_Control", mapTexture.controlTexture); //TODO mesh rendering for individual meshes
        meshRenderer.material.SetTexture("_BumpMap", mapTexture.normalTexture);
	}

    private MapMesh CreateMapMesh(int bottomLeftX, int bottomLeftY, int width, int height)
    {

        Mesh mesh = meshGenerator.CreateMesh(bottomLeftX, bottomLeftY, width, height);
        Vector3 center = new Vector3((bottomLeftX + width) / 2, (bottomLeftY + height) / 2, 0);
        Vector3 size = new Vector3(width, height, 0);
        Bounds bounds = new Bounds(center, size);

        return new MapMesh(mesh, bounds);
    }

    private MapTexture CreateMapTexture()
    {
        Color32[] colourMap = new Color32[width * height];
        Color32[] normalMap = new Color32[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                Tile t = _world.GetNodeAt(x, y);
                Color tileColour = tileColours[(int)t.TypeId];
                tileColour.a = t.Altitude;
                colourMap[y * width + x] = tileColour;

                Vector3 tangentNormal = Quaternion.Euler(0, 0, 0) * t.Normal;
                Color tileNormal = new Color(tangentNormal.x, tangentNormal.y, tangentNormal.z);
                normalMap[y * width + x] = tileNormal;
            }
        }

        Texture controlTexture = textureGenerator.CreateTextureFromColourMap(colourMap, width, height);
        Texture normalTexture = textureGenerator.CreateTextureFromColourMap(normalMap, width, height);

        return new MapTexture(controlTexture, normalTexture);
    }
}
