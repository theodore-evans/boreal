using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MapRenderController : MonoBehaviour
{
    [SerializeField] MeshFilter meshFilter = null;
	[SerializeField] MeshRenderer meshRenderer = null;

    [SerializeField] Color32[] tileColours = new Color32[] {
        Color.white,  //Blank
        Color.blue,   //Water
        Color.red,   //Soil
        Color.green   //Grass
    };

    WorldController worldController;

    MeshGenerator meshGenerator;
    TextureGenerator textureGenerator;

    private int width;
    private int height;
    private float verticalScale;
    private Mesh mapMesh;
    private MapTexture mapTexture;

    private struct MapTexture
    {
        public Texture2D control;
        public Texture2D normal;

        public MapTexture(Texture2D controlTexture, Texture2D normalTexture)
        {
            control = controlTexture;
            normal = normalTexture;
        }
    }

    private void Awake()
    {
        worldController = GetComponentInParent<WorldController>();
        worldController.RegisterWorldCreatedCallback(Initialize);

        meshGenerator = GetComponent<MeshGenerator>();
        textureGenerator = GetComponent<TextureGenerator>();
    }

    private void Initialize(NodeGrid<Tile> world)
    {
        width = world.GridSizeX;
        height = world.GridSizeY;
        verticalScale = worldController.WorldVerticalScale;

        Vector2 bottomLeftCorner = world.Origin;

        mapMesh = meshGenerator.CreateMesh(bottomLeftCorner, width, height);
        meshFilter.sharedMesh = mapMesh;
        mapTexture = CreateMapTexture(width, height);

        meshRenderer.material.SetTexture("_Control", mapTexture.control);
        meshRenderer.material.SetTexture("_NormalMap", mapTexture.normal);

        worldController.RegisterWorldChangedCallback(RenderMap);
    }

    private void RenderMap(IEnumerable<Tile> changedTiles)
    {
        foreach (Tile t in changedTiles) {
            float grassCover = t.Cover.Grass;
            float waterDepth = t.Water.Depth;

            Color tileControl = new Color(1 - grassCover, grassCover, 0);

            //TODO implement better water rendering
            if (t.Water.Surface) {
                tileControl = Color.blue;
                tileControl.a = waterDepth / verticalScale;
            }

            mapTexture.control.SetPixel(t.X, t.Y, tileControl);

            Vector3 tileNormal = new Vector3(t.Relief.Normal.x, t.Relief.Normal.y, t.Relief.Normal.z) * 0.5f + 0.5f * Vector3.one;
            Color tileNormalColor = new Color(tileNormal.x, tileNormal.y, tileNormal.z);

            mapTexture.normal.SetPixel(t.X, t.Y, tileNormalColor);
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
