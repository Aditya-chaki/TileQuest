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
        PlayerPrefs.SetInt("Event_CurrentLevel",currentLevelCompleted);
    }
    
   
    public override void UpdateProgress() 
    { 
        currentLevelCompleted = PlayerPrefs.GetInt("Event_CurrentLevel");
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
        }
    }
}
