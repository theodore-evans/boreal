using UnityEngine;
using TMPro;
using System.Collections;

public class TileInfoUpdater : MonoBehaviour, ITileUIUpdateBehaviour
{
    [SerializeField] GameObject tileInfo_go = null;
    [SerializeField] float tileInfoUpdatePeriod = 1f;
    [SerializeField] bool verbose = true;

    private TextMeshProUGUI tileInfo;
    private Tile selectedTile;

    System.Text.StringBuilder sb = new System.Text.StringBuilder();

    void Start()
    {
        tileInfo = tileInfo_go.GetComponent<TextMeshProUGUI>();
        StartCoroutine(nameof(DisplayTileInfoCoroutine));
    }

    public void ActionWhenNewTileSelected(Tile newlySelectedTile)
    {
        selectedTile = newlySelectedTile;
        DisplayTileInfo();
    }

    private IEnumerator DisplayTileInfoCoroutine()
    {
        for(; ; ) {
            if (selectedTile != null) {
                DisplayTileInfo();
            }
            yield return new WaitForSeconds(tileInfoUpdatePeriod);
        }
    }

    private void DisplayTileInfo()
    {
        sb.Clear();
        sb.Append(selectedTile.X.ToString("F0")).Append(", ").Append(selectedTile.Y.ToString("F0"));
        sb.AppendLine();
        sb.Append("Elevation: ").Append(selectedTile.Relief.Elevation.ToString("F2")).Append(" m");
        sb.AppendLine();

        if (verbose) {
            sb.Append("Normal: ").Append(selectedTile.Relief.Normal.ToString("F1"));
            sb.AppendLine();
            sb.Append("Gradient: ").Append(selectedTile.Relief.Gradient.ToString("F2")).Append("\x00B0");
            sb.AppendLine();
        }

        if (selectedTile.Water.Surface) {
            sb.Append("Water");
            sb.AppendLine();
            sb.Append("     Depth: ").Append(selectedTile.Water.Depth.ToString("F3"));
            sb.AppendLine();
            sb.Append("     Level: ").Append(selectedTile.Water.Level.ToString("F3"));
            sb.AppendLine();
        }

        sb.Append("Ground saturation: ").Append(selectedTile.Water.Saturation.ToString("F3"));
        sb.AppendLine();
        sb.Append("Grass cover: ").Append(selectedTile.Cover.Grass.ToString("P"));

        tileInfo.text = sb.ToString();
    }
}
