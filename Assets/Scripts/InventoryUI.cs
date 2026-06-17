using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInventory inventory;

    [Header("Texts")]
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text rareStoneText;
    [SerializeField] private TMP_Text herbText;

    private void Start()
    {
        if (inventory == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                inventory = player.GetComponent<PlayerInventory>();
        }

        if (inventory != null)
        {
            inventory.OnInventoryChanged += UpdateUI;
            UpdateUI();
        }
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        woodText.text = "Wood: " + inventory.GetResourceAmount(ResourceType.Wood);
        stoneText.text = "Stone: " + inventory.GetResourceAmount(ResourceType.Stone);
        goldText.text = "Gold: " + inventory.GetResourceAmount(ResourceType.Gold);
        rareStoneText.text = "Rare Stone: " + inventory.GetResourceAmount(ResourceType.RareStone);
        herbText.text = "Herb: " + inventory.GetResourceAmount(ResourceType.Herb);
    }
}
