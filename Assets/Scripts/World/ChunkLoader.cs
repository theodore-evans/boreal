﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour, ITileObserver, IChunkLoader
{
    Cache<Tile> tilesToUpdate = new Cache<Tile>();
    Cache<Tile> tilesToUpdateEarly = new Cache<Tile>();

    private void UpdateTiles(IEnumerable<Tile> tilesToUpdate)
    {
        cbWorldChanged?.Invoke(tilesToUpdate);
    }

    private Action<IEnumerable<Tile>> cbWorldChanged;

    System.Random rng = new System.Random();

    [SerializeField] Camera currentCamera = null;
    [SerializeField] [Range(0, 2)] float drawDistance = 1;
    [SerializeField] [Range(0, 1)] float onScreenUpdateRate = 0.1f;
    [SerializeField] [Range(0, 5000)] int minUpdatesPerFrame = 2500;

    private int numberToUpdate(int total) => Mathf.Max(minUpdatesPerFrame, Mathf.CeilToInt(total * onScreenUpdateRate));

    private void Start()
    {
        StartCoroutine(nameof(InvokeUpdatesCoroutine));
    }

    public void RegisterCallback(Action<IEnumerable<Tile>> callback)
    {
        cbWorldChanged += callback;
    }

    public void DeregisterCallback(Action<IEnumerable<Tile>> callback)
    {
        cbWorldChanged -= callback;
    }

    public void OnTileChanged(Tile t)
    {
        tilesToUpdate.Add(t);
    }

    private Rect GetOnScreenArea()
    {
        float widthMargin = Screen.width * drawDistance;
        float heightMargin = Screen.height * drawDistance;

        Vector2 bottomLeft = currentCamera.ScreenToWorldPoint(new Vector2(-widthMargin, -heightMargin));
        Vector2 topRight = currentCamera.ScreenToWorldPoint(new Vector2(Screen.width + widthMargin, Screen.height + heightMargin));

        return Rect.MinMaxRect(bottomLeft.x, bottomLeft.y, topRight.x, topRight.y);
    }

    private IEnumerator InvokeUpdatesCoroutine()
    {
        for (; ; ) {
            if (tilesToUpdateEarly.Count == 0 && tilesToUpdate.Count > 0) {
                tilesToUpdateEarly.Union(tilesToUpdate.PopAllWithinArea(GetOnScreenArea()));
                UpdateTiles(tilesToUpdate.Draw(numberToUpdate(tilesToUpdate.Count)));
            }

            if (tilesToUpdateEarly.Count > 0) {
                UpdateTiles(tilesToUpdateEarly.DrawRandom(numberToUpdate(tilesToUpdateEarly.Count)));
            }

            yield return null;
        }
    }
}