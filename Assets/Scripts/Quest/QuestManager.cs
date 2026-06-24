using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public event Action OnQuestUpdated;
    public event Action OnQuestCompleted;
    public event Action OnAllQuestsCompleted;

    [Header("Quest List")]
    [SerializeField] private QuestData[] quests;

    [Header("Current Quest")]
    [SerializeField] private int currentQuestIndex = 0;

    [Header("Progression")]
    [SerializeField] private bool autoStartNextQuest = true;
    [SerializeField] private float nextQuestDelay = 2f;

    private bool questCompletedNotified;

    public QuestData CurrentQuest
    {
        get
        {
            if (quests == null || quests.Length == 0)
                return null;

            if (currentQuestIndex < 0 || currentQuestIndex >= quests.Length)
                return null;

            return quests[currentQuestIndex];
        }
    }

    public string QuestTitle => CurrentQuest != null ? CurrentQuest.questTitle : "";
    public string QuestDescription => CurrentQuest != null ? CurrentQuest.questDescription : "";
    public QuestObjective[] Objectives => CurrentQuest != null ? CurrentQuest.objectives : null;

    public bool IsQuestCompleted
    {
        get
        {
            if (CurrentQuest == null || CurrentQuest.objectives == null)
                return false;

            foreach (QuestObjective objective in CurrentQuest.objectives)
            {
                if (!objective.IsCompleted)
                    return false;
            }

            return true;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        ResetCurrentQuestProgress();
        OnQuestUpdated?.Invoke();
    }

    public void RegisterResourceCollected(ResourceType resourceType, int amount)
    {
        QuestObjective[] objectives = Objectives;
        if (objectives == null) return;

        foreach (QuestObjective objective in objectives)
        {
            if (objective.objectiveType != QuestObjectiveType.CollectResource)
                continue;

            if (objective.requiredResource != resourceType)
                continue;

            objective.AddProgress(amount);
        }

        NotifyUpdate();
    }

    public void RegisterEnemyKilled(string enemyId)
    {
        QuestObjective[] objectives = Objectives;
        if (objectives == null) return;

        foreach (QuestObjective objective in objectives)
        {
            if (objective.objectiveType != QuestObjectiveType.KillEnemy)
                continue;

            if (!string.IsNullOrEmpty(objective.requiredEnemyId) &&
                objective.requiredEnemyId != enemyId)
                continue;

            objective.AddProgress(1);
        }

        NotifyUpdate();
    }

    public void RegisterWeaponUpgraded(int weaponLevel)
    {
        RegisterUpgradeObjective(QuestObjectiveType.UpgradeWeapon, weaponLevel);
    }

    public void RegisterShieldUpgraded(int shieldLevel)
    {
        RegisterUpgradeObjective(QuestObjectiveType.UpgradeShield, shieldLevel);
    }

    public void RegisterBaseUpgraded(int baseLevel)
    {
        RegisterUpgradeObjective(QuestObjectiveType.UpgradeBase, baseLevel);
    }

    private void RegisterUpgradeObjective(QuestObjectiveType type, int level)
    {
        QuestObjective[] objectives = Objectives;
        if (objectives == null) return;

        foreach (QuestObjective objective in objectives)
        {
            if (objective.objectiveType != type)
                continue;

            if (level >= objective.requiredLevel)
                objective.AddProgress(objective.requiredAmount);
        }

        NotifyUpdate();
    }

    public void RegisterReturnToBase()
    {
        QuestObjective[] objectives = Objectives;
        if (objectives == null) return;

        foreach (QuestObjective objective in objectives)
        {
            if (objective.objectiveType != QuestObjectiveType.ReturnToBase)
                continue;

            objective.AddProgress(objective.requiredAmount);
        }

        NotifyUpdate();
    }

    private void NotifyUpdate()
    {
        OnQuestUpdated?.Invoke();

        if (IsQuestCompleted && !questCompletedNotified)
        {
            questCompletedNotified = true;

            Debug.Log("Quest completed: " + QuestTitle);

            OnQuestCompleted?.Invoke();

            if (autoStartNextQuest)
                Invoke(nameof(StartNextQuest), nextQuestDelay);
        }
    }

    public void StartNextQuest()
    {
        if (currentQuestIndex + 1 >= quests.Length)
        {
            Debug.Log("All quests completed.");
            OnAllQuestsCompleted?.Invoke();
            return;
        }

        currentQuestIndex++;
        questCompletedNotified = false;

        ResetCurrentQuestProgress();

        Debug.Log("New quest started: " + QuestTitle);

        OnQuestUpdated?.Invoke();
    }

    private void ResetCurrentQuestProgress()
    {
        QuestObjective[] objectives = Objectives;
        if (objectives == null) return;

        foreach (QuestObjective objective in objectives)
        {
            objective.ResetProgress();
        }
    }
}
