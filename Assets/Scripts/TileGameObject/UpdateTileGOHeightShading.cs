using UnityEngine;

public class UpdateTileGOHeightShading : MonoBehaviour, ITileGOUpdateBehaviour
{
    [SerializeField] float intensityMultipler = 0.5f;
    [SerializeField] float tileShaderNoiseSigma = 0.01f;
    [SerializeField] float tileShaderDepth = 0.6f;

    [SerializeField] float maxAltitude = 10; //FIXME: this whole height shading is a hack, needs reworking

    System.Random prng;

    void Awake()
    {
        prng = new System.Random();
    }

    public void UpdateTile(GameObject tile_go, Tile tile_data)
    {
        // adjust renderer brightness according to tile altitude
        float intensity = intensityMultipler * Mathf.Lerp(tileShaderDepth, 1, (tile_data.WaterLevel - tile_data.WaterDepth / 2) / maxAltitude);
        intensity += Noise.NextGaussian(prng, 0, tileShaderNoiseSigma);

        Color shaderColor = new Color(intensity, intensity, intensity, 1.0f);

        tile_go.GetComponent<SpriteRenderer>().material.SetColor("_Color", shaderColor);
    }
}