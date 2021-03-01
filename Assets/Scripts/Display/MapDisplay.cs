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
    private Mesh mapMesh;
    private MapTexture mapTexture;

    bool redrawMap;

    bool TileIsWithinMeshBounds(Tile tile, Mesh mesh)
    {
        return true;
    }

    private struct MapTexture
    {
        public Texture2D control;
        public Texture2D normal;

        public MapTexture(Texture2D controlTexture, Texture2D normalTexture)
        {
            this.control = controlTexture;
            this.normal = normalTexture;
        }
    }

    private void Awake()
    {
        wc.RegisterWorldCreatedCallback(Initialize);

        meshGenerator = GetComponent<MeshGenerator>();
        textureGenerator = GetComponent<TextureGenerator>();
    }

    private void Initialize(SpaceGrid<Tile> world)
    {
        _world = world;

        width = wc.WorldWidth;
        height = wc.WorldHeight;
        verticalScale = wc.WorldVerticalScale;

        Vector2 bottomLeftCorner = wc.Origin;

        mapMesh = meshGenerator.CreateMesh(bottomLeftCorner, width, height);
        meshFilter.sharedMesh = mapMesh;
        mapTexture = CreateMapTexture(width, height);

        meshRenderer.material.SetTexture("_Control", mapTexture.control);
        meshRenderer.material.SetTexture("_BumpMap", mapTexture.normal);

        wc.RegisterWorldChangedCallback(UpdateMap);
    }

    private void UpdateMap(HashSet<Tile> changedTiles)
    {
        foreach (Tile t in changedTiles) {
            Color tileControl = tileColours[(int)t.TypeId];
            tileControl.a = t.WaterDepth / verticalScale;
            mapTexture.control.SetPixel(t.X, t.Y, tileControl);

            Color tileNormal = new Color(t.Normal.x, t.Normal.y, t.Normal.z);
            mapTexture.normal.SetPixel(t.X, t.Y, tileNormal);
        }
        mapTexture.normal.Apply();
        mapTexture.control.Apply();
    }

    private MapTexture CreateMapTexture(int width, int height)
    {
        Texture2D controlTexture = textureGenerator.CreateTextureFromColourMap(width, height);
        Texture2D normalTexture = textureGenerator.CreateTextureFromColourMap(width, height);

        return new MapTexture(controlTexture, normalTexture);
    }

}
