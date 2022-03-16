using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    private NodeGrid<Tile> _world;

    private void Awake()
    {
        WorldController worldController = GetComponentInParent<WorldController>();
        worldController.RegisterWorldCreatedCallback(Initialise);
    }

    public void Initialise(NodeGrid<Tile> world)
    {
        _world = world;
    }

    public void Rain()
    {

    }
}
