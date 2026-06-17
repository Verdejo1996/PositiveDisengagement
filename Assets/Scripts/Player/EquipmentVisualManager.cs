using UnityEngine;

public class EquipmentVisualManager : MonoBehaviour
{
    [Header("Weapon Holder")]
    [SerializeField] private Transform weaponHolder;
    private GameObject currentWeapon;

    [Header("Shield Holder")]
    [SerializeField] private Transform shieldHolder;
    private GameObject currentShield;

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (weaponPrefab == null) return;

        if (currentWeapon != null)
            Destroy(currentWeapon);

        currentWeapon = Instantiate(
            weaponPrefab,
            weaponHolder.position,
            weaponHolder.rotation,
            weaponHolder
        );

        currentWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void EquipShield(GameObject shieldPrefab)
    {
        if (shieldPrefab == null) return;

        if (currentShield != null)
            Destroy(currentShield);

        currentShield = Instantiate(
            shieldPrefab,
            shieldHolder.position,
            shieldHolder.rotation,
            shieldHolder
        );

        currentShield.transform.localPosition = Vector3.zero;
        currentShield.transform.localRotation = Quaternion.identity;
    }
}
