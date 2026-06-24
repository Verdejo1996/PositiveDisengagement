using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;

    [Header("Colors")]
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color midHealthColor = Color.yellow;
    [SerializeField] private Color lowHealthColor = Color.red;

    [Header("Enemy Bar Settings")]
    [SerializeField] private bool hideWhenFull = false;
    [SerializeField] private GameObject barRoot;

    private void Awake()
    {
        if (healthSlider == null)
            healthSlider = GetComponentInChildren<Slider>();

        if (barRoot == null)
            barRoot = gameObject;
    }

    private void Start()
    {
        if (health != null)
        {
            health.OnHealthChanged += UpdateBar;
            UpdateBar(health.CurrentHealth, health.MaxHealth);
        }
    }

    private void OnDestroy()
    {
        if (health != null)
            health.OnHealthChanged -= UpdateBar;
    }

    private void UpdateBar(int currentHealth, int maxHealth)
    {
        if (healthSlider == null) return;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        UpdateColor(currentHealth, maxHealth);

        if (hideWhenFull && barRoot != null)
        {
            bool shouldShow = currentHealth < maxHealth && currentHealth > 0;
            barRoot.SetActive(shouldShow);
        }
    }

    private void UpdateColor(int currentHealth, int maxHealth)
    {
        if (fillImage == null) return;

        float healthPercent = (float)currentHealth / maxHealth;

        if (healthPercent > 0.5f)
        {
            float t = (healthPercent - 0.5f) / 0.5f;
            fillImage.color = Color.Lerp(midHealthColor, fullHealthColor, t);
        }
        else
        {
            float t = healthPercent / 0.5f;
            fillImage.color = Color.Lerp(lowHealthColor, midHealthColor, t);
        }
    }

    public void SetHealth(Health newHealth)
    {
        if (health != null)
            health.OnHealthChanged -= UpdateBar;

        health = newHealth;

        if (health != null)
        {
            health.OnHealthChanged += UpdateBar;
            UpdateBar(health.CurrentHealth, health.MaxHealth);
        }
    }
}