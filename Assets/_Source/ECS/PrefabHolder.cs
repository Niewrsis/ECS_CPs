using System;
using Unity.Entities;

[Serializable]
public struct PrefabHolder : IComponentData
{
    public Entity Value;
}
