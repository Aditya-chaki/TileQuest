using UnityEngine;
using System;

public class BuildingData:MonoBehaviour
{
    public string name;
    public int level = 1;
    public int baseCost = 100;
    public float costMultiplier = 1.5f;
    public bool isMain = false;

    public int GetUpgradeCost()
    {
        return (int)(baseCost * Mathf.Pow(costMultiplier, level - 1));
    }
}
