using System.Collections.Generic;
using UnityEngine;

public class HarvestableResource : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 40;

    [Header("Random Drops Per Hit")]
    [SerializeField] private ResourceDropRange[] dropsPerHit;

    [Header("Loot Bag")]
    [SerializeField] private GameObject lootBagPrefab;

    [Header("Floating Text")]
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Vector3 floatingTextOffset = new Vector3(0f, 2.2f, 0f);

    [Header("Visual")]
    [SerializeField] private GameObject visualObject;

    [Header("Respawn")]
    [SerializeField] private bool respawn = true;
    [SerializeField] private float respawnTime = 20f;

    private int currentHealth;
    private bool destroyed;
    private Transform player;

    private Dictionary<ResourceType, int> accumulatedLoot = new Dictionary<ResourceType, int>();

    private void Awake()
    {
        currentHealth = maxHealth;

        if (visualObject == null)
            visualObject = gameObject;
    }

    private void Start()
    {
        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");

        if (foundPlayer != null)
            player = foundPlayer.transform;
    }

    public void TakeDamage(int damage)
    {
        if (destroyed) return;

        currentHealth -= damage;

        GenerateRandomLootPerHit();

        if (currentHealth <= 0)
        {
            BreakResource();
        }
    }

    private void GenerateRandomLootPerHit()
    {
        foreach (ResourceDropRange drop in dropsPerHit)
        {
            if (drop == null) continue;

            float randomChance = Random.value;

            if (randomChance > drop.dropChance)
                continue;

            int amount = Random.Range(drop.minAmount, drop.maxAmount + 1);

            if (amount <= 0)
                continue;

            if (!accumulatedLoot.ContainsKey(drop.resourceType))
                accumulatedLoot[drop.resourceType] = 0;

            accumulatedLoot[drop.resourceType] += amount;

            ShowFloatingText("+" + amount + " " + drop.resourceType);
        }
    }

    private void ShowFloatingText(string message)
    {
        if (floatingTextPrefab == null) return;
        if (player == null) return;

        Vector3 spawnPosition = player.position + floatingTextOffset;

        GameObject textObject = Instantiate(
            floatingTextPrefab,
            spawnPosition,
            Quaternion.identity
        );

        FloatingResourceText floatingText = textObject.GetComponent<FloatingResourceText>();

        if (floatingText != null)
            floatingText.Initialize(message);
    }

    private void BreakResource()
    {
        destroyed = true;

        DropAccumulatedLootBag();

        if (visualObject != null)
            visualObject.SetActive(false);

        Collider col = GetComponent<Collider>();

        if (col != null)
            col.enabled = false;

        if (respawn)
            Invoke(nameof(Respawn), respawnTime);
    }

    private void DropAccumulatedLootBag()
    {
        if (lootBagPrefab == null)
        {
            Debug.LogWarning("LootBag prefab missing on " + gameObject.name);
            return;
        }

        if (accumulatedLoot.Count == 0)
            return;

        List<ResourceCost> finalLoot = new List<ResourceCost>();

        foreach (KeyValuePair<ResourceType, int> item in accumulatedLoot)
        {
            ResourceCost cost = new ResourceCost
            {
                resourceType = item.Key,
                amount = item.Value
            };

            finalLoot.Add(cost);
        }

        GameObject lootBagObject = Instantiate(
            lootBagPrefab,
            transform.position + Vector3.up * 0.3f,
            Quaternion.identity
        );

        LootBag lootBag = lootBagObject.GetComponent<LootBag>();

        if (lootBag != null)
            lootBag.Initialize(finalLoot.ToArray());
    }

    private void Respawn()
    {
        destroyed = false;
        currentHealth = maxHealth;
        accumulatedLoot.Clear();

        if (visualObject != null)
            visualObject.SetActive(true);

        Collider col = GetComponent<Collider>();

        if (col != null)
            col.enabled = true;
    }
}