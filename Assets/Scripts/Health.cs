using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float damageReduction = 0f;

    [Header("Regeneration")]
    [SerializeField] private bool canRegenerate = true;
    [SerializeField] private float timeBeforeRegen = 5f;
    [SerializeField] private int regenAmountPerTick = 2;
    [SerializeField] private float regenTickRate = 1f;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public bool IsDead => CurrentHealth <= 0;

    private Animator animator;

    private float lastDamageTime;
    private float regenTimer;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    private void Update()
    {
        HandleRegeneration();
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        int finalDamage = Mathf.RoundToInt(damage * (1f - damageReduction));
        finalDamage = Mathf.Max(finalDamage, 1);

        CurrentHealth -= finalDamage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        lastDamageTime = Time.time;
        regenTimer = 0f;

        Debug.Log(gameObject.name + " took damage: " + finalDamage + " | Current HP: " + CurrentHealth);

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("Hit");
        }
    }

    private void HandleRegeneration()
    {
        if (!canRegenerate) return;
        if (IsDead) return;
        if (CurrentHealth >= maxHealth) return;

        bool recentlyDamaged = Time.time < lastDamageTime + timeBeforeRegen;

        if (recentlyDamaged)
            return;

        regenTimer += Time.deltaTime;

        if (regenTimer >= regenTickRate)
        {
            regenTimer = 0f;
            Heal(regenAmountPerTick);
        }
    }

    public void Heal(int amount)
    {
        if (IsDead) return;

        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void SetDamageReduction(float value)
    {
        damageReduction = Mathf.Clamp01(value);
    }

    public void ResetHealth()
    {
        CurrentHealth = maxHealth;
        lastDamageTime = 0f;
        regenTimer = 0f;

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    private void Die()
    {
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        OnDeath?.Invoke();

        if (animator != null)
            animator.SetTrigger("Die");
    }

    public void IncreaseMaxHealth(int amount, bool healToFull)
    {
        maxHealth += amount;

        if (healToFull)
            CurrentHealth = maxHealth;
        else
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }
}