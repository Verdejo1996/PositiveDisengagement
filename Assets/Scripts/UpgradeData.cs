using System;
using UnityEngine;

[Serializable]
public class UpgradeData
{
    public string upgradeName;
    public int level;
    public ResourceCost[] costs;
    public GameObject prefab;
}
