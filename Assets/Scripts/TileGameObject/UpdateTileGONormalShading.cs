using UnityEngine;
using System.Collections;

public class UpdateTileGONormalShading : MonoBehaviour, ITileGOUpdateBehaviour
{
    [SerializeField] float intensityMultipler = 0.5f;
    [SerializeField] [Range(0, 2 * Mathf.PI)] float polar = 0;
    [SerializeField] [Range(0, 2 * Mathf.PI)] float azimuth = 0;

    Vector3 lightDirection;

    void Start()
    {
        UpdateLightDirection();
    }

    public void UpdateTile(GameObject tile_go, Tile tile_data)
    {
        float intensity = Mathf.Cos(Vector3.Angle(tile_go.transform.position + lightDirection, tile_data.Normal) * Mathf.PI / 180f) * intensityMultipler;

        Color shaderColor = new Color(intensity, intensity, intensity, 1.0f);

        tile_go.GetComponent<SpriteRenderer>().material.SetColor("_Color", shaderColor);
    }

    public void UpdateLightDirection()
    {
        float a = Mathf.Sin(azimuth);
        lightDirection = new Vector3(a * Mathf.Cos(polar), a * Mathf.Sin(polar), Mathf.Cos(azimuth));
        Debug.Log(lightDirection);
    }
}
