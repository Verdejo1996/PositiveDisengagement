using TMPro;
using UnityEngine;

public class InventoryWindowUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject inventoryWindow;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerEquipment playerEquipment;

    [Header("Resource Texts")]
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text rareStoneText;
    [SerializeField] private TMP_Text herbText;

    [Header("Equipment Texts")]
    [SerializeField] private TMP_Text swordText;
    [SerializeField] private TMP_Text shieldText;

    [Header("Input")]
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;

    private bool isOpen;

    private void Start()
    {
        if (inventoryWindow != null)
            inventoryWindow.SetActive(false);

        if (playerInventory == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                playerInventory = player.GetComponent<PlayerInventory>();
        }

        if (playerEquipment == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                playerEquipment = player.GetComponent<PlayerEquipment>();
        }

        if (playerInventory != null)
            playerInventory.OnInventoryChanged += UpdateUI;

        if (playerEquipment != null)
            playerEquipment.OnEquipmentChanged += UpdateUI;

        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
            playerInventory.OnInventoryChanged -= UpdateUI;

        if (playerEquipment != null)
            playerEquipment.OnEquipmentChanged -= UpdateUI;
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;

        if (inventoryWindow != null)
            inventoryWindow.SetActive(isOpen);

        if (isOpen)
            UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateResourcesUI();
        UpdateEquipmentUI();
    }

    private void UpdateResourcesUI()
    {
        if (playerInventory == null) return;

        if (woodText != null)
            woodText.text = "Wood: " + playerInventory.GetResourceAmount(ResourceType.Wood);

        if (stoneText != null)
            stoneText.text = "Stone: " + playerInventory.GetResourceAmount(ResourceType.Stone);

        if (goldText != null)
            goldText.text = "Gold: " + playerInventory.GetResourceAmount(ResourceType.Gold);

        if (rareStoneText != null)
            rareStoneText.text = "Rare Stone: " + playerInventory.GetResourceAmount(ResourceType.RareStone);

        if (herbText != null)
            herbText.text = "Herb: " + playerInventory.GetResourceAmount(ResourceType.Herb);
    }

    private void UpdateEquipmentUI()
    {
        if (playerEquipment == null) return;

        if (swordText != null)
        {
            swordText.text =
                "Sword: " +
                playerEquipment.EquippedSwordName +
                " | Lv. " +
                playerEquipment.SwordLevel;
        }

        if (shieldText != null)
        {
            shieldText.text =
                "Shield: " +
                playerEquipment.EquippedShieldName +
                " | Lv. " +
                playerEquipment.ShieldLevel;
        }
    }
}
