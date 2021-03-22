using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour, ITileSubscriber, IChunkLoader
{
    Cache<Tile> tilesToUpdate = new Cache<Tile>();
    Cache<Tile> tilesToUpdateEarly = new Cache<Tile>();
    private Action<IEnumerable<Tile>> cbWorldChanged;

    Rect areaToLoad;

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

    private Rect GetOnScreenArea(float drawDistance = 1)
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

            if (tilesToUpdateEarly.Count > 0) {
                int updatesThisIteration = numberToUpdate(tilesToUpdateEarly.Count);
                cbWorldChanged?.Invoke(tilesToUpdateEarly.DrawRandom(updatesThisIteration));
                yield return 0;
            }

            if (tilesToUpdate.Count > 0) {
                Rect areaToLoad = GetOnScreenArea(drawDistance);
                tilesToUpdateEarly.Union(tilesToUpdate.PopAllWithinArea(areaToLoad));

                if (tilesToUpdateEarly.Count == 0) {
                    int updatesThisIteration = numberToUpdate(tilesToUpdate.Count);
                    cbWorldChanged?.Invoke(tilesToUpdate.Draw(minUpdatesPerFrame));
                }
            }

            yield return 0;
        }
    }
}