using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTileGOSprite : MonoBehaviour, ITileGOUpdateBehaviour
{

    System.Random rng;

    Dictionary<string, List<Sprite>> tileSpritesMap;

    void Awake()
    {
        rng = new System.Random();
        tileSpritesMap = SpritesMap.Load("Tile");
    }

    public void UpdateTile(GameObject tile_go, Tile tile_data)
    {
        if (tile_go == null) {
            Debug.Log("Null tile GameObject provided");
            return;
        }

        if (tile_data == null) {
            Debug.Log("Null tile data provided");
            return;
        }

        string tileType = tile_data.Type;

        if (tileSpritesMap.ContainsKey(tileType)) {

            if (tile_go.GetComponent<SpriteRenderer>() == null) tile_go.AddComponent<SpriteRenderer>();

            List<Sprite> sprites = tileSpritesMap[tileType];
            Sprite randomSprite = sprites[rng.Next(sprites.Count)];
            tile_go.GetComponent<SpriteRenderer>().sprite = randomSprite;
        }
        else Debug.Log("SpriteController.ApplyRandomSpriteToTile -- no sprites found for tile type " + tileType);

    }
}
