using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;

    [Header("Gameplay Scripts")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;

    [Header("Death")]
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject deathLootBagPrefab;
    [SerializeField] private Transform baseRespawnPoint;

    [Header("Enemies")]
    [SerializeField] private EnemySpawnZone[] enemyZonesToDeactivate;

    private bool isDead;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = GetComponent<Health>();

        if (playerInventory == null)
            playerInventory = GetComponent<PlayerInventory>();

        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();

        if (playerCombat == null)
            playerCombat = GetComponent<PlayerCombat>();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnDeath -= HandleDeath;
    }

    private void Start()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false);
    }

    private void HandleDeath()
    {
        if (isDead) return;

        isDead = true;

        DropInventoryBag();

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerCombat != null)
            playerCombat.enabled = false;

        if (deathPanel != null)
            deathPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void DropInventoryBag()
    {
        if (playerInventory == null) return;

        ResourceCost[] lostResources = playerInventory.GetAllResourcesAsCosts();

        if (lostResources.Length == 0)
            return;

        if (deathLootBagPrefab == null)
        {
            Debug.LogWarning("Death Loot Bag Prefab is missing.");
            return;
        }

        GameObject bagObject = Instantiate(
            deathLootBagPrefab,
            transform.position + Vector3.up * 0.3f,
            Quaternion.identity
        );

        
        if (bagObject.TryGetComponent<LootBag>(out var lootBag))
            lootBag.Initialize(lostResources);

        playerInventory.ClearAllResources();
    }

    public void RespawnAtBase()
    {
        if (baseRespawnPoint == null)
        {
            Debug.LogWarning("Base Respawn Point is missing.");
            return;
        }

        if (deathPanel != null)
            deathPanel.SetActive(false);

        DeactivateEnemies();

        if (characterController != null)
            characterController.enabled = false;

        transform.position = baseRespawnPoint.position;
        transform.rotation = baseRespawnPoint.rotation;

        if (characterController != null)
            characterController.enabled = true;

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }

        if (playerHealth != null)
            playerHealth.ResetHealth();

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (playerCombat != null)
            playerCombat.enabled = true;

        isDead = false;
    }

    private void DeactivateEnemies()
    {
        foreach (EnemySpawnZone zone in enemyZonesToDeactivate)
        {
            if (zone != null)
                zone.DeactivateAllEnemies();
        }
    }
}
