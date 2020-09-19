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
        tileInfo.text = $"[{t.X}, {t.Y}]\n{t.Type}\nElevation: " + t.Altitude.ToString("F2");
        tileInfo_go.SetActive(true);
    }
}
