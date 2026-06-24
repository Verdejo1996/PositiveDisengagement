using UnityEngine;

public class BaseSafeZone : MonoBehaviour
{
    [SerializeField] private EnemySpawnZone[] enemyZones;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        QuestManager.Instance?.RegisterReturnToBase();

        foreach (EnemySpawnZone zone in enemyZones)
        {
            if (zone != null)
            {
                zone.DeactivateAllEnemies();
            }
        }
    }
}
