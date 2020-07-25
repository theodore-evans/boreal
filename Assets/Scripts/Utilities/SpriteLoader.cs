using System.Collections.Generic;
using UnityEngine;

public class SpritesMap
{
    public static Dictionary<string, List<Sprite>> Load(string spriteType)
    {
        Dictionary<string, List<Sprite>> spritesMap = new Dictionary<string, List<Sprite>>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/" + spriteType);
        foreach (Sprite sprite in sprites)
        {
            string tileType = sprite.name.Split('_')[0];
            if (spritesMap.ContainsKey(tileType) == false)
            {
                //Debug.Log("Creating new tile sprite type:" + spriteType);
                List<Sprite> specificTileSprites = new List<Sprite> { sprite };
                spritesMap.Add(tileType, specificTileSprites);
            }
            //Debug.Log("Adding Sprite" + sprite.name + " of type " + spriteType );
            spritesMap[tileType].Add(sprite);

        }
        return spritesMap;
    }
}
