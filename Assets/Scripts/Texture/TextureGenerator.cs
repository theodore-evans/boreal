using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureGenerator: MonoBehaviour
{
    [SerializeField] FilterMode _filterMode;
    [SerializeField] TextureWrapMode _wrapMode;

    public TextureGenerator(FilterMode filterMode = FilterMode.Bilinear,
                            TextureWrapMode wrapMode = TextureWrapMode.Mirror)
    {
        _filterMode = filterMode;
        _wrapMode = wrapMode;
    }

    public Texture2D CreateTextureFromColourMap(int width, int height)
	{
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, true, true) {
            filterMode = _filterMode,
            wrapMode = _wrapMode,

        };
        return texture;
	}

    public void UpdateTexture(Texture2D texture, Color32[] colourMap)
    {
        texture.SetPixels32(colourMap);
        texture.Apply();
    }
}


