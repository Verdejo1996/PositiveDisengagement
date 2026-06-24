using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Attack Settings")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private float attackCooldown = 0.7f;

    [Header("Layers")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask resourceLayer;

    private float nextAttackTime;
    private int attackIndex = 0;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            attackIndex++;

            if (attackIndex > 2)
                attackIndex = 1;

            if (attackIndex == 1)
                animator.SetTrigger("Attack01");
            else
                animator.SetTrigger("Attack02");

            DealDamage();
        }
    }

    private void DealDamage()
    {
        Vector3 offset = new (0, 1, 0);
        Vector3 attackCenter = transform.position + transform.forward * attackRange + offset;

        HitEnemies(attackCenter);
        HitResources(attackCenter);
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private void HitEnemies(Vector3 attackCenter)
    {
        Collider[] hits = Physics.OverlapSphere(
            attackCenter,
            attackRadius,
            enemyLayer
        );

        foreach (Collider hit in hits)
        {
            Health enemyHealth = hit.GetComponentInParent<Health>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    private void HitResources(Vector3 attackCenter)
    {
        Collider[] hits = Physics.OverlapSphere(
            attackCenter,
            attackRadius,
            resourceLayer
        );

        foreach (Collider hit in hits)
        {
            HarvestableResource resource = hit.GetComponentInParent<HarvestableResource>();

            if (resource != null)
            {
                resource.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 offset = new(0, 1, 0);
        Vector3 attackCenter = transform.position + transform.forward * attackRange + offset;

        Gizmos.DrawWireSphere(attackCenter, attackRadius);
    }
}
