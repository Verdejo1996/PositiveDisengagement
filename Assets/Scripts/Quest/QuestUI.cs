using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuestManager questManager;

    [Header("UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text objectivesText;
    [SerializeField] private TMP_Text completedText;

    private void Start()
    {
        if (questManager == null)
            questManager = QuestManager.Instance;

        if (questManager != null)
        {
            questManager.OnQuestUpdated += UpdateUI;
            questManager.OnQuestCompleted += ShowCompleted;
        }

        if (completedText != null)
            completedText.gameObject.SetActive(false);

        UpdateUI();
    }

    private void OnDestroy()
    {
        if (questManager != null)
        {
            questManager.OnQuestUpdated -= UpdateUI;
            questManager.OnQuestCompleted -= ShowCompleted;
        }
    }

    private void UpdateUI()
    {
        if (questManager == null) return;

        if (titleText != null)
            titleText.text = questManager.QuestTitle;

        if (descriptionText != null)
            descriptionText.text = questManager.QuestDescription;

        if (objectivesText != null)
        {
            objectivesText.text = "";

            if (questManager.Objectives == null)
                return;

            foreach (QuestObjective objective in questManager.Objectives)
            {
                objectivesText.text += objective.GetProgressText() + "\n";
            }
        }
    }

    private void ShowCompleted()
    {
        if (completedText != null)
        {
            completedText.gameObject.SetActive(true);
            completedText.text = "Quest Complete";
        }
    }
}
