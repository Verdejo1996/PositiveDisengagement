using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float damageReduction = 0f;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public bool IsDead => CurrentHealth <= 0;

    private Animator animator;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        int finalDamage = Mathf.RoundToInt(damage * (1f - damageReduction));

        CurrentHealth -= finalDamage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        Debug.Log(gameObject.name + " took damage: " + finalDamage + " | Current HP: " + CurrentHealth);

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

    public void SetDamageReduction(float value)
    {
        damageReduction = Mathf.Clamp01(value);
    }

    public void ResetHealth()
    {
        CurrentHealth = maxHealth;
    }

    private void Die()
    {
        if (animator != null)
            animator.SetTrigger("Die");
    }
}
