using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TextureGenerator
{

	public static Texture2D TextureFromColourMap(Color32[] colourMap, int width, int height)
	{
        Texture2D texture = new Texture2D(width, height) {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Mirror

        };
        texture.SetPixels32(colourMap);
		texture.Apply();
		return texture;
	}

	public static Texture2D TextureFromHeightMap(float[,] heightMap)
	{
		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);

		Color32[] colourMap = new Color32[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colourMap[y * width + x] = Color32.Lerp(Color.black, Color.white, heightMap[x, y]);
			}
		}

		return TextureFromColourMap(colourMap, width, height);
	}

	public static Texture2D TextureFromWorldData(SpaceGrid<Tile> world, Color32[] tileColours)
	{
		int width = world.GridSizeX;
		int height = world.GridSizeY;

		Color32[] colourMap = new Color32[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				Color tileColour = tileColours[(int)world.GetNodeAt(x, y).TypeId];
				colourMap[y * width + x] = tileColour;
			}
		}

		return TextureFromColourMap(colourMap, width, height);
	}
}


