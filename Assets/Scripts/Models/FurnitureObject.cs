using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// installed objects are immovable objects like walls, doors, trees

public class FurnitureObject
{
    public Tile Tile
    {
        get; protected set;
    }

    // this "objectType" will be queried by the visual system to determine sprite to render
    public string FurnitureType
    {
        get; protected set;
    }

    float movementCost; // a multiplier, a value of 2 means you move at half speed

    int width;
    int height;

    protected FurnitureObject()
    {
    }

    public static FurnitureObject CreatePrototype(string furnitureType, float movementCost = 1f, int width = 1, int height = 1)
    {
        FurnitureObject furn = new FurnitureObject();

        furn.FurnitureType = furnitureType;
        furn.movementCost = movementCost;
        furn.width = width;
        furn.height = height;

        return furn;
    }

    public static FurnitureObject CreateInstance(FurnitureObject proto, Tile tile)
    {
        FurnitureObject furn = new FurnitureObject();

        furn.FurnitureType = proto.FurnitureType;
        furn.movementCost = proto.movementCost;
        furn.width = proto.width;
        furn.height = proto.height;

        furn.Tile = tile;

        if (tile.AddFurniture(furn) == false)
        {
            return null;
        }

        return furn;
    }
}
