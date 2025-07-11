using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGamesEvent : BaseEvent
{
    public int requiredLevelToCompleteWaterSort;
    private int currentLevelCompletedWaterSort = 0;

    public int requiredLevelToCompleteOnet;
    private int currentLevelCompletedOnet = 0;

    public override void Initialize()
    {
        PlayerPrefs.SetInt("Event_WaterSortLevelRequired",requiredLevelToCompleteWaterSort);
        currentLevelCompletedWaterSort=PlayerPrefs.GetInt("Event_WaterSortCurrentLevel",0);
        PlayerPrefs.SetInt("Event_OnetLevelRequired",requiredLevelToCompleteOnet);
        currentLevelCompletedOnet=PlayerPrefs.GetInt("Event_OnetCurrentLevel",0);
        PlayerPrefs.Save();
    }
    
   
    public override void UpdateProgress() 
    { 
        currentLevelCompletedWaterSort = PlayerPrefs.GetInt("Event_WaterSortCurrentLevel");
        currentLevelCompletedOnet=PlayerPrefs.GetInt("Event_OnetCurrentLevel",0);
        PlayerPrefs.Save();
    }

    public override bool IsCompleted()
    {
        return requiredLevelToCompleteWaterSort == currentLevelCompletedOnet && requiredLevelToCompleteOnet == currentLevelCompletedWaterSort;
    }

    public override void ClaimReward()
    {
        if (IsCompleted())
        {
            Debug.Log("Custom Level Event Completed!");
            PlayerPrefs.SetInt("Event_WaterSortCurrentLevel",0);
            PlayerPrefs.SetInt("Event_OnetCurrentLevel",0);
        }
    }
}
