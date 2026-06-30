using UnityEngine;
using System;

public class CraftingManager : MonoBehaviour
{
    public event Action OnCraftingChanged;

    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerEquipment playerEquipment;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private Health playerHealth;
    [SerializeField] private EquipmentVisualManager equipmentVisualManager;
    [SerializeField] private BaseUpgradeManager baseUpgradeManager;

    [Header("Base Upgrades")]
    [SerializeField] private UpgradeData[] baseUpgrades;

    [Header("Weapon Upgrades")]
    [SerializeField] private UpgradeData[] weaponUpgrades;
    [SerializeField] private int[] weaponDamages;

    [Header("Shield Upgrades")]
    [SerializeField] private UpgradeData[] shieldUpgrades;
    [SerializeField] private float[] shieldDamageReductions;

    private int currentBaseIndex = 0;
    private int currentWeaponIndex = 0;
    private int currentShieldIndex = 0;

    private void Start()
    {
        FindReferencesIfNeeded();

        ApplyInitialEquipment();
    }

    private void FindReferencesIfNeeded()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (playerInventory == null)
                playerInventory = player.GetComponent<PlayerInventory>();

            if (playerEquipment == null)
                playerEquipment = player.GetComponent<PlayerEquipment>();

            if (playerCombat == null)
                playerCombat = player.GetComponent<PlayerCombat>();

            if (playerHealth == null)
                playerHealth = player.GetComponent<Health>();

            if (equipmentVisualManager == null)
                equipmentVisualManager = player.GetComponent<EquipmentVisualManager>();
        }
    }

    private void ApplyInitialEquipment()
    {
        if (baseUpgrades.Length > 0 && baseUpgradeManager != null)
            baseUpgradeManager.SetBase(baseUpgrades[0].prefab, baseUpgrades[0].level);

        if (weaponUpgrades.Length > 0)
            ApplyWeaponUpgrade(0);

        if (shieldUpgrades.Length > 0)
            ApplyShieldUpgrade(0);
    }

    public void UpgradeBase()
    {
        int nextIndex = currentBaseIndex + 1;

        if (nextIndex >= baseUpgrades.Length)
        {
            Debug.Log("Base is already max level.");
            return;
        }

        UpgradeData nextUpgrade = baseUpgrades[nextIndex];

        if (!CanPay(nextUpgrade.costs))
            return;

        playerInventory.SpendResources(nextUpgrade.costs);

        currentBaseIndex = nextIndex;

        if (baseUpgradeManager != null)
            baseUpgradeManager.SetBase(nextUpgrade.prefab, nextUpgrade.level);

        QuestManager.Instance?.RegisterBaseUpgraded(nextUpgrade.level);

        NotifyCraftingChanged();

        Debug.Log("Base upgraded: " + nextUpgrade.upgradeName);
    }

    public void UpgradeWeapon()
    {
        int nextIndex = currentWeaponIndex + 1;

        if (nextIndex >= weaponUpgrades.Length)
        {
            Debug.Log("Weapon is already max level.");
            return;
        }

        UpgradeData nextUpgrade = weaponUpgrades[nextIndex];

        if (!CanPay(nextUpgrade.costs))
            return;

        playerInventory.SpendResources(nextUpgrade.costs);

        currentWeaponIndex = nextIndex;

        ApplyWeaponUpgrade(currentWeaponIndex);

        QuestManager.Instance?.RegisterWeaponUpgraded(nextUpgrade.level);

        NotifyCraftingChanged();

        Debug.Log("Weapon upgraded: " + nextUpgrade.upgradeName);
    }

    public void UpgradeShield()
    {
        int nextIndex = currentShieldIndex + 1;

        if (nextIndex >= shieldUpgrades.Length)
        {
            Debug.Log("Shield is already max level.");
            return;
        }

        UpgradeData nextUpgrade = shieldUpgrades[nextIndex];

        if (!CanPay(nextUpgrade.costs))
            return;

        playerInventory.SpendResources(nextUpgrade.costs);

        currentShieldIndex = nextIndex;

        ApplyShieldUpgrade(currentShieldIndex);

        QuestManager.Instance?.RegisterShieldUpgraded(nextUpgrade.level);

        NotifyCraftingChanged();

        Debug.Log("Shield upgraded: " + nextUpgrade.upgradeName);
    }

    private bool CanPay(ResourceCost[] costs)
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("PlayerInventory missing.");
            return false;
        }

        if (!playerInventory.HasResources(costs))
        {
            Debug.Log("Not enough resources.");
            return false;
        }

        return true;
    }

    private void ApplyWeaponUpgrade(int index)
    {
        UpgradeData weaponUpgrade = weaponUpgrades[index];

        if (equipmentVisualManager != null)
            equipmentVisualManager.EquipWeapon(weaponUpgrade.prefab);

        if (playerEquipment != null)
            playerEquipment.SetSword(weaponUpgrade.upgradeName, weaponUpgrade.level);

        if (playerCombat != null && index < weaponDamages.Length)
            playerCombat.SetDamage(weaponDamages[index]);

        QuestManager.Instance?.RegisterWeaponUpgraded(weaponUpgrade.level);
    }

    private void ApplyShieldUpgrade(int index)
    {
        UpgradeData shieldUpgrade = shieldUpgrades[index];

        if (equipmentVisualManager != null)
            equipmentVisualManager.EquipShield(shieldUpgrade.prefab);

        if (playerEquipment != null)
            playerEquipment.SetShield(shieldUpgrade.upgradeName, shieldUpgrade.level);

        if (playerHealth != null && index < shieldDamageReductions.Length)
            playerHealth.SetDamageReduction(shieldDamageReductions[index]);


    }
    public bool CanUpgradeBase()
    {
        return CanUpgrade(currentBaseIndex, baseUpgrades);
    }

    public bool CanUpgradeWeapon()
    {
        return CanUpgrade(currentWeaponIndex, weaponUpgrades);
    }

    public bool CanUpgradeShield()
    {
        return CanUpgrade(currentShieldIndex, shieldUpgrades);
    }

    private bool CanUpgrade(int currentIndex, UpgradeData[] upgrades)
    {
        if (playerInventory == null)
            FindReferencesIfNeeded();

        if (playerInventory == null)
            return false;

        int nextIndex = currentIndex + 1;

        if (upgrades == null || nextIndex >= upgrades.Length)
            return false;

        UpgradeData nextUpgrade = upgrades[nextIndex];

        return playerInventory.HasResources(nextUpgrade.costs);
    }

    private void NotifyCraftingChanged()
    {
        OnCraftingChanged?.Invoke();
    }
}
