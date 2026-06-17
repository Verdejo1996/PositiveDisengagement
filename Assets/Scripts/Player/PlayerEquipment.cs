using System;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public event Action OnEquipmentChanged;

    [Header("Weapon")]
    [SerializeField] private string equippedSwordName = "Basic Sword";
    [SerializeField] private int swordLevel = 1;

    [Header("Shield")]
    [SerializeField] private string equippedShieldName = "Basic Shield";
    [SerializeField] private int shieldLevel = 1;

    public string EquippedSwordName => equippedSwordName;
    public int SwordLevel => swordLevel;

    public string EquippedShieldName => equippedShieldName;
    public int ShieldLevel => shieldLevel;

    public void SetSword(string swordName, int level)
    {
        equippedSwordName = swordName;
        swordLevel = level;

        OnEquipmentChanged?.Invoke();
    }

    public void SetShield(string shieldName, int level)
    {
        equippedShieldName = shieldName;
        shieldLevel = level;

        OnEquipmentChanged?.Invoke();
    }
}
