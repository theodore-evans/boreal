using UnityEngine;

[System.Serializable]
public struct TerrainType
{
    public TileTypeId typeId;
    [Range(0, 1)] public float height;
}

