using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Movement : IComponentData
{
    public float speed;
    public float3 direction;
}
