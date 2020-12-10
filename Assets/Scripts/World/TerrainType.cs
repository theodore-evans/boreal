using UnityEngine;

[System.Serializable]
public struct TerrainType
{
    public string name;
    [Range(0, 1)] public float height;
}

