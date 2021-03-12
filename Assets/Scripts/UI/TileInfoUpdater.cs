using UnityEngine;
using TMPro;

public class TileInfoUpdater : MonoBehaviour, ITileUIUpdateBehaviour
{
    [SerializeField] GameObject tileInfo_go = null;

    private TextMeshProUGUI tileInfo;

    void Start()
    {
        tileInfo = tileInfo_go.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateTileUI(ICursorProvider cursor, Tile t)
    {
        tileInfo.text = $"[{t.X}, {t.Y}]"
            + $"\n{t.TypeId}"
            + "\nElevation: " + t.Altitude.ToString("F3")
            + "\nNormal: " + t.Normal.ToString("F2")
            + "\nGradient: " + t.Gradient.ToString("F2");
        if (t.TypeId == TypeId.Water) {
            tileInfo.text += "\nWater Depth: " + t.WaterDepth.ToString("F3")
                          + "\nWater Level: " + t.WaterLevel.ToString("F3");
        }
        tileInfo_go.SetActive(true);
    }
}
