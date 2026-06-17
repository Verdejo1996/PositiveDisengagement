using UnityEngine;

public class LootBag : MonoBehaviour
{
    [Header("Loot")]
    [SerializeField] private ResourceCost[] loot;

    [Header("Interaction")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Optional")]
    [SerializeField] private float lifeTime = 60f;

    private Transform player;
    private bool collected;

    private void Start()
    {
        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");

        if (foundPlayer != null)
            player = foundPlayer.transform;

        if (lifeTime > 0)
            Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (collected) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactDistance && Input.GetKeyDown(interactKey))
        {
            Collect();
        }
    }

    public void Initialize(ResourceCost[] newLoot)
    {
        loot = newLoot;
    }

    private void Collect()
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (inventory == null)
        {
            Debug.LogWarning("PlayerInventory not found.");
            return;
        }

        foreach (ResourceCost item in loot)
        {
            inventory.AddResource(item.resourceType, item.amount);
        }

        collected = true;
        Destroy(gameObject);
    }
}
