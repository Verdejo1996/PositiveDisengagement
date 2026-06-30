using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private CraftingManager craftingManager;
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Buttons")]
    [SerializeField] private Button upgradeBaseButton;
    [SerializeField] private Button upgradeWeaponButton;
    [SerializeField] private Button upgradeShieldButton;

    [Header("Input")]
    [SerializeField] private KeyCode toggleKey = KeyCode.U;

    private bool isOpen;

    private void Start()
    {
        FindReferencesIfNeeded();

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        if (upgradeBaseButton != null)
            upgradeBaseButton.onClick.AddListener(UpgradeBase);

        if (upgradeWeaponButton != null)
            upgradeWeaponButton.onClick.AddListener(UpgradeWeapon);

        if (upgradeShieldButton != null)
            upgradeShieldButton.onClick.AddListener(UpgradeShield);

        if (playerInventory != null)
            playerInventory.OnInventoryChanged += UpdateButtons;

        if (craftingManager != null)
            craftingManager.OnCraftingChanged += UpdateButtons;

        UpdateButtons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            TogglePanel();
        }
    }

    private void OnDestroy()
    {
        if (upgradeBaseButton != null)
            upgradeBaseButton.onClick.RemoveListener(UpgradeBase);

        if (upgradeWeaponButton != null)
            upgradeWeaponButton.onClick.RemoveListener(UpgradeWeapon);

        if (upgradeShieldButton != null)
            upgradeShieldButton.onClick.RemoveListener(UpgradeShield);

        if (playerInventory != null)
            playerInventory.OnInventoryChanged -= UpdateButtons;

        if (craftingManager != null)
            craftingManager.OnCraftingChanged -= UpdateButtons;
    }

    private void FindReferencesIfNeeded()
    {
        if (craftingManager == null)
            craftingManager = FindObjectOfType<CraftingManager>();

        if (playerInventory == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                playerInventory = player.GetComponent<PlayerInventory>();
        }
    }

    public void TogglePanel()
    {
        isOpen = !isOpen;

        if (upgradePanel != null)
            upgradePanel.SetActive(isOpen);

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (craftingManager == null) return;

        if (upgradeBaseButton != null)
            upgradeBaseButton.interactable = craftingManager.CanUpgradeBase();

        if (upgradeWeaponButton != null)
            upgradeWeaponButton.interactable = craftingManager.CanUpgradeWeapon();

        if (upgradeShieldButton != null)
            upgradeShieldButton.interactable = craftingManager.CanUpgradeShield();
    }

    private void UpgradeBase()
    {
        if (craftingManager == null) return;

        craftingManager.UpgradeBase();
        UpdateButtons();
    }

    private void UpgradeWeapon()
    {
        if (craftingManager == null) return;

        craftingManager.UpgradeWeapon();
        UpdateButtons();
    }

    private void UpgradeShield()
    {
        if (craftingManager == null) return;

        craftingManager.UpgradeShield();
        UpdateButtons();
    }
}
