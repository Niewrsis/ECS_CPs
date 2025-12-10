using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct OrbitCenter : IComponentData
{
    public float3 Value;
}
