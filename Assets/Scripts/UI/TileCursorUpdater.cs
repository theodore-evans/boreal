﻿using UnityEngine;

public class TileCursorUpdater : MonoBehaviour, ITileUIUpdateBehaviour
{
    [SerializeField] GameObject tileCursor_prefab = null;

    private GameObject tileCursor_go;

    private void Start()
    {
        tileCursor_go = Instantiate(tileCursor_prefab, transform);
    }

    public void ActionWhenNewTileSelected(Tile t)
    {
        tileCursor_go.transform.position = new Vector3(t.X, t.Y, -10);
    }
}
