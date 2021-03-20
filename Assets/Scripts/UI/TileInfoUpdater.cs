﻿using UnityEngine;
using TMPro;
using System.Collections;

public class TileInfoUpdater : MonoBehaviour, ITileUIUpdateBehaviour
{
    [SerializeField] GameObject tileInfo_go = null;

    private TextMeshProUGUI tileInfo;
    private Tile selectedTile;

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
            yield return new WaitForSeconds(1f);
        }
    }

    private void DisplayTileInfo()
    {
        tileInfo.text = $"[{selectedTile.X}, {selectedTile.Y}]"
            + $"\n{selectedTile.TypeId}"
            + "\nElevation: " + selectedTile.Altitude.ToString("F3") + "m"
            + "\nNormal: " + selectedTile.Normal.ToString("F1")
            + "\nGradient: " + selectedTile.AngleFromNormal.ToString("F2") + "\x00B0";
        if (selectedTile.TypeId == TypeId.Water) {
            tileInfo.text += "\nWater Depth: " + selectedTile.WaterDepth.ToString("F3")
                          + "\nWater Level: " + selectedTile.WaterLevel.ToString("F3");
        }
        tileInfo_go.SetActive(true);
    }
}
