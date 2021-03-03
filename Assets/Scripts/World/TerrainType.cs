using UnityEngine;

[System.Serializable]
public struct TerrainType
{
    public TypeId typeId; 
    [Range(0, 1)] public float height;
}

