﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    private NodeGrid<Tile> _world;

    public void Initialise(WorldController wc)
    {
        _world = wc.world;
    }

    public void Rain()
    {

    }
}
