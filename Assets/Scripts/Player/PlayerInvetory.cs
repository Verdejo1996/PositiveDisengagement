using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public event Action OnInventoryChanged;

    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    private void Awake()
    {
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            resources[type] = 0;
        }
    }

    public void AddResource(ResourceType type, int amount)
    {
        resources[type] += amount;

        Debug.Log($"Added {amount} {type}. Total: {resources[type]}");

        OnInventoryChanged?.Invoke();
    }

    public bool HasResource(ResourceType type, int amount)
    {
        return resources[type] >= amount;
    }

    public bool HasResources(ResourceCost[] costs)
    {
        foreach (ResourceCost cost in costs)
        {
            if (!HasResource(cost.resourceType, cost.amount))
                return false;
        }

        return true;
    }

    public void SpendResources(ResourceCost[] costs)
    {
        foreach (ResourceCost cost in costs)
        {
            resources[cost.resourceType] -= cost.amount;
        }

        OnInventoryChanged?.Invoke();
    }

    public int GetResourceAmount(ResourceType type)
    {
        return resources[type];
    }
}
