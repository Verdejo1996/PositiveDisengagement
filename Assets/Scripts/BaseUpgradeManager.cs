using UnityEngine;

public class BaseUpgradeManager : MonoBehaviour
{
    [Header("Base Holder")]
    [SerializeField] private Transform baseHolder;

    private GameObject currentBase;

    public int CurrentBaseLevel { get; private set; } = 1;

    public void SetBase(GameObject basePrefab, int level)
    {
        if (basePrefab == null) return;

        if (currentBase != null)
            Destroy(currentBase);

        currentBase = Instantiate(
            basePrefab,
            baseHolder.position,
            baseHolder.rotation,
            baseHolder
        );

        currentBase.transform.localPosition = Vector3.zero;
        currentBase.transform.localRotation = Quaternion.identity;

        CurrentBaseLevel = level;

        Debug.Log("Base upgraded to level " + level);
    }
}
