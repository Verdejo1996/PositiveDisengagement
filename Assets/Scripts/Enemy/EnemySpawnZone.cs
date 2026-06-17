using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnZone : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private List<EnemyAI> enemies = new List<EnemyAI>();
    [SerializeField] private Transform[] spawnPoints;

    [Header("Zone Settings")]
    [SerializeField] private bool spawnOnlyOnce = false;

    private bool hasSpawned;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (spawnOnlyOnce && hasSpawned) return;

        ActivateEnemies(other.transform);
        hasSpawned = true;
    }

    private void ActivateEnemies(Transform player)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyAI enemy = enemies[i];

            if (enemy == null) continue;

            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];

            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;

            enemy.gameObject.SetActive(true);
            enemy.Initialize(player, spawnPoint.position, this);
        }
    }

    public void DeactivateEnemy(EnemyAI enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    public void DeactivateAllEnemies()
    {
        foreach (EnemyAI enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }
}
