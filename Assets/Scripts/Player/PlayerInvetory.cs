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

        QuestManager.Instance?.RegisterResourceCollected(type, amount);

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

    public ResourceCost[] GetAllResourcesAsCosts()
    {
        List<ResourceCost> result = new();

        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            int amount = GetResourceAmount(type);

            if (amount <= 0)
                continue;

            ResourceCost cost = new()
            {
                resourceType = type,
                amount = amount
            };

            result.Add(cost);
        }

        return result.ToArray();
    }

    public void ClearAllResources()
    {
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            resources[type] = 0;
        }

        OnInventoryChanged?.Invoke();
    }
}
