using System;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public event Action OnExperienceChanged;
    public event Action OnLevelUp;

    [Header("Level")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int maxLevel = 10;

    [Header("Experience")]
    [SerializeField] private int currentExperience = 0;
    [SerializeField] private int baseExperienceToNextLevel = 100;
    [SerializeField] private int experienceIncreasePerLevel = 50;

    [Header("Optional Level Rewards")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private int maxHealthIncreasePerLevel = 10;

    public int CurrentLevel => currentLevel;
    public int CurrentExperience => currentExperience;
    public int ExperienceToNextLevel => baseExperienceToNextLevel + ((currentLevel - 1) * experienceIncreasePerLevel);
    public int MaxLevel => maxLevel;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = GetComponent<Health>();
    }

    public void AddExperience(int amount)
    {
        if (amount <= 0) return;
        if (currentLevel >= maxLevel) return;

        currentExperience += amount;

        Debug.Log("Player gained XP: " + amount);

        while (currentExperience >= ExperienceToNextLevel && currentLevel < maxLevel)
        {
            currentExperience -= ExperienceToNextLevel;
            LevelUp();
        }

        if (currentLevel >= maxLevel)
            currentExperience = 0;

        OnExperienceChanged?.Invoke();
    }

    private void LevelUp()
    {
        currentLevel++;

        Debug.Log("Level Up! New level: " + currentLevel);

        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(maxHealthIncreasePerLevel, true);
        }

        OnLevelUp?.Invoke();
    }
}
