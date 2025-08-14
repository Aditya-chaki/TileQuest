using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingManager : MonoBehaviour
{
    public BuildingData mainBuilding;
    public BuildingData[] otherBuildings;

    private void Start()
    {
        LoadData();
    }


    public void UpgradeMainBuilding()
    {
        if (CanAfford(mainBuilding.GetUpgradeCost()))
        {
            Config.Gold = Config.Gold-(int)mainBuilding.GetUpgradeCost();
            mainBuilding.level++;
            Debug.Log("Main Building upgraded to Level " + mainBuilding.level);
            SaveData();
        }
    }

    public void UpgradeOtherBuilding(int index)
    {
        if (index < 0 || index >= otherBuildings.Length) return;

        var b = otherBuildings[index];

        if (b.level < mainBuilding.level) // Level cap check
        {
            if (CanAfford(b.GetUpgradeCost()))
            {
                Config.Gold = Config.Gold-b.GetUpgradeCost();
                b.level++;
                Debug.Log(b.name + " upgraded to Level " + b.level);
                SaveData();
            }
        }
        else
        {
            Debug.Log(b.name + " is capped at Main Building level " + mainBuilding.level);
        }
    }

    private bool CanAfford(float cost)
    {
        return Config.Gold>=cost;
    }
    
    private void SaveData()
    {
        PlayerPrefs.SetInt("MainLevel", mainBuilding.level);
        for (int i = 0; i < otherBuildings.Length; i++)
        {
            PlayerPrefs.SetInt($"Building_{i}_Level", otherBuildings[i].level);
        }
    }

    private void LoadData()
    {
        mainBuilding.level = PlayerPrefs.GetInt("MainLevel");
        for (int i = 0; i < otherBuildings.Length; i++)
        {
            otherBuildings[i].level = PlayerPrefs.GetInt($"Building_{i}_Level");
        }
    }
}
