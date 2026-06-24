using System;
using UnityEngine;

public enum QuestObjectiveType
{
    CollectResource,
    KillEnemy,
    UpgradeWeapon,
    UpgradeShield,
    UpgradeBase,
    ReturnToBase
}

[Serializable]
public class QuestObjective
{
    [Header("Objective Info")]
    public string description;
    public QuestObjectiveType objectiveType;

    [Header("Progress")]
    public int currentAmount;
    public int requiredAmount = 1;

    [Header("Optional Filters")]
    public ResourceType requiredResource;
    public string requiredEnemyId;
    public int requiredLevel = 1;

    public bool IsCompleted => currentAmount >= requiredAmount;

    public void AddProgress(int amount)
    {
        if (IsCompleted) return;

        currentAmount += amount;

        if (currentAmount > requiredAmount)
            currentAmount = requiredAmount;
    }

    public string GetProgressText()
    {
        if (objectiveType == QuestObjectiveType.UpgradeWeapon ||
            objectiveType == QuestObjectiveType.UpgradeShield ||
            objectiveType == QuestObjectiveType.UpgradeBase ||
            objectiveType == QuestObjectiveType.ReturnToBase)
        {
            return IsCompleted ? "[X] " + description : "[ ] " + description;
        }

        return IsCompleted
            ? "[X] " + description + " [" + currentAmount + " / " + requiredAmount + "]"
            : "[ ] " + description + " [" + currentAmount + " / " + requiredAmount + "]";
    }

    public void ResetProgress()
    {
        currentAmount = 0;
    }
}
