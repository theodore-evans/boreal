using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureGenerator
{
    FilterMode _filterMode;
    TextureWrapMode _wrapMode;

    public TextureGenerator(FilterMode filterMode = FilterMode.Bilinear,
                            TextureWrapMode wrapMode = TextureWrapMode.Mirror)
    {
        _filterMode = filterMode;
        _wrapMode = wrapMode;
    }

    public Texture2D CreateTextureFromColourMap(Color32[] colourMap, int width, int height)
	{
        Texture2D texture = new Texture2D(width, height) {
            filterMode = _filterMode,
            wrapMode = _wrapMode

        };
        texture.SetPixels32(colourMap);
		texture.Apply();
		return texture;
	}
}


