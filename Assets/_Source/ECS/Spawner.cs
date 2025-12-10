using System;
using Unity.Entities;

[Serializable]
public struct Spawner : IComponentData
{
    public int GridSizeX;
    public int GridSizeZ;
    public float Spacing;
    public bool HasSpawned;
}