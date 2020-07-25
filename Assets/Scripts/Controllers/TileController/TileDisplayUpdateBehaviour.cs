using System;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteUpdateBehaviour : ITileUpdateBehaviour
{

    System.Random rng;

    Dictionary<string, List<Sprite>> tileSpritesMap;

    // TheWorld rendering parameters
    float tileShaderNoiseSigma;
    float tileShaderDepth;

    public TileSpriteUpdateBehaviour(float tileShaderNoiseSigma, float tileShaderDepth)
    {
        this.tileShaderNoiseSigma = tileShaderNoiseSigma;
        this.tileShaderDepth = tileShaderDepth;

        rng = new System.Random();
        tileSpritesMap = SpritesMap.Load("Tile");
    }

    public void OnTileChanged(GameObject tile_go, Tile tile_data)
    {
        if (tile_go == null) {
            Debug.Log("No tile GameObject provided");
            return;
        }
        if (tile_data == null) {
            Debug.Log("No tile data provided");
            return;
        }

        ApplySpriteToTile(tile_go, tile_data);
        ShadeSpriteTile(tile_go, tile_data);

    }

    void ApplySpriteToTile(GameObject tile_go, Tile tile_data)
    {
        string tileType = tile_data.Type;

        if (tileSpritesMap.ContainsKey(tileType) == false)
        {
            Debug.Log("SpriteController.ApplyRandomSpriteToTile -- no sprites found for tile type " + tileType);
            return;
        }

        if (tile_go.GetComponent<SpriteRenderer>() == null)
        {
            tile_go.AddComponent<SpriteRenderer>();
        }

        List<Sprite> sprites = tileSpritesMap[tileType];
        Sprite randomSprite = sprites[rng.Next(sprites.Count)];
        tile_go.GetComponent<SpriteRenderer>().sprite = randomSprite;

    }

    void ShadeSpriteTile(GameObject tile_go, Tile tile_data)
    {
        // adjust renderer brightness according to tile altitude
        float intensity = 1 + tileShaderDepth * (tile_data.Altitude - 1);
        intensity += Noise.NextGaussian(rng, 0, tileShaderNoiseSigma);

        Color shaderColor = new Color(intensity, intensity, intensity, 1.0f);

        tile_go.GetComponent<SpriteRenderer>().material.SetColor("_Color", shaderColor);
    }

}
