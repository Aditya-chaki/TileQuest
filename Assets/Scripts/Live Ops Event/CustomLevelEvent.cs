using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CustomLevelEvent : BaseEvent
{
    public int requiredLevelToComplete;
    private int currentLevelCompleted = 0;

    
    public override void Initialize()
    {
        PlayerPrefs.SetInt("Event_LevelRequired",requiredLevelToComplete);
        currentLevelCompleted=PlayerPrefs.GetInt("Event_CurrentLevel",0);
        PlayerPrefs.Save();
    }
    
   
    public override void UpdateProgress() 
    { 
        currentLevelCompleted = PlayerPrefs.GetInt("Event_CurrentLevel");
        PlayerPrefs.Save();
    }

    public override bool IsCompleted()
    {
        return requiredLevelToComplete == currentLevelCompleted;
    }

    public override void ClaimReward()
    {
        if (IsCompleted())
        {
            Debug.Log("Custom Level Event Completed!");
            PlayerPrefs.SetInt("Event_CurrentLevel",0);
            PlayerPrefs.Save();
        }
    }
}
