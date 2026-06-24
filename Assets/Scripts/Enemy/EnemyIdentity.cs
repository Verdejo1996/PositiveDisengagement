using UnityEngine;

public class EnemyIdentity : MonoBehaviour
{
    [SerializeField] private string enemyId = "Zombie";

    public string EnemyId => enemyId;
}
