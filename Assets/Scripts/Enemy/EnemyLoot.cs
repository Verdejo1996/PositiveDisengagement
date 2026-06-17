using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [Header("Random Loot Table")]
    [SerializeField] private ResourceDropRange[] possibleDrops;

    [Header("Loot Bag")]
    [SerializeField] private GameObject lootBagPrefab;

    [Header("Experience")]
    [SerializeField] private int experienceReward = 10;

    public void DropLoot(Vector3 position)
    {
        if (lootBagPrefab == null)
        {
            Debug.LogWarning("LootBag prefab is missing on " + gameObject.name);
            return;
        }

        ResourceCost[] generatedLoot = GenerateLoot();

        if (generatedLoot.Length == 0)
        {
            Debug.Log(gameObject.name + " dropped nothing.");
            return;
        }

        GameObject lootBagObject = Instantiate(
            lootBagPrefab,
            position + Vector3.up * 0.3f,
            Quaternion.identity
        );

        LootBag lootBag = lootBagObject.GetComponent<LootBag>();

        if (lootBag != null)
            lootBag.Initialize(generatedLoot);

        Debug.Log(gameObject.name + " dropped random loot bag.");
    }

    private ResourceCost[] GenerateLoot()
    {
        List<ResourceCost> finalLoot = new List<ResourceCost>();

        foreach (ResourceDropRange drop in possibleDrops)
        {
            if (drop == null) continue;

            float randomChance = Random.value;

            if (randomChance > drop.dropChance)
                continue;

            int amount = Random.Range(drop.minAmount, drop.maxAmount + 1);

            if (amount <= 0)
                continue;

            ResourceCost cost = new ResourceCost
            {
                resourceType = drop.resourceType,
                amount = amount
            };

            finalLoot.Add(cost);
        }

        return finalLoot.ToArray();
    }

    public int GetExperienceReward()
    {
        return experienceReward;
    }
}
