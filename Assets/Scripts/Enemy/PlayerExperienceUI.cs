using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperienceUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerExperience playerExperience;

    [Header("UI")]
    [SerializeField] private Slider experienceSlider;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text experienceText;

    private void Start()
    {
        if (playerExperience == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                playerExperience = player.GetComponent<PlayerExperience>();
        }

        if (playerExperience != null)
        {
            playerExperience.OnExperienceChanged += UpdateUI;
            playerExperience.OnLevelUp += UpdateUI;
        }

        UpdateUI();
    }

    private void OnDestroy()
    {
        if (playerExperience != null)
        {
            playerExperience.OnExperienceChanged -= UpdateUI;
            playerExperience.OnLevelUp -= UpdateUI;
        }
    }

    private void UpdateUI()
    {
        if (playerExperience == null) return;

        if (levelText != null)
            levelText.text = "Level " + playerExperience.CurrentLevel;

        if (experienceSlider != null)
        {
            experienceSlider.maxValue = playerExperience.ExperienceToNextLevel;
            experienceSlider.value = playerExperience.CurrentExperience;
        }

        if (experienceText != null)
        {
            experienceText.text =
                playerExperience.CurrentExperience +
                " / " +
                playerExperience.ExperienceToNextLevel +
                " XP";
        }
    }
}
