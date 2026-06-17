using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [Header("Resource")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int amount = 1;

    [Header("Interaction")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("State")]
    [SerializeField] private bool destroyAfterCollect = false;
    [SerializeField] private GameObject visualObject;

    private Transform player;
    private bool collected;

    private void Start()
    {
        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");

        if (foundPlayer != null)
            player = foundPlayer.transform;

        if (visualObject == null)
            visualObject = gameObject;
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

    private void Collect()
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (inventory == null)
        {
            Debug.LogWarning("PlayerInventory not found on Player.");
            return;
        }

        inventory.AddResource(resourceType, amount);

        collected = true;

        if (destroyAfterCollect)
        {
            Destroy(gameObject);
        }
        else
        {
            visualObject.SetActive(false);
        }
    }
}
