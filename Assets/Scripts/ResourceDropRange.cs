using System;
using UnityEngine;

[Serializable]
public class ResourceDropRange
{
    public ResourceType resourceType;

    [Min(0)] public int minAmount = 1;
    [Min(0)] public int maxAmount = 3;

    [Range(0f, 1f)]
    public float dropChance = 1f;
}
