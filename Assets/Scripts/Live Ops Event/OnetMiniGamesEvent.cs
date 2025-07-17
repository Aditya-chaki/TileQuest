using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnetMiniGamesEvent : BaseEvent
{
    
    public int requiredLevelToCompleteOnet;
    private int currentLevelCompletedOnet = 0;

    public override void Initialize()
    {       
        PlayerPrefs.SetInt("Event_OnetLevelRequired",requiredLevelToCompleteOnet);
        currentLevelCompletedOnet=PlayerPrefs.GetInt("Event_OnetCurrentLevel",0);
        PlayerPrefs.Save();
    }
    
   
    public override void UpdateProgress() 
    { 
        currentLevelCompletedOnet=PlayerPrefs.GetInt("Event_OnetCurrentLevel",0);
        PlayerPrefs.Save();
    }

    public override bool IsCompleted()
    {
        return requiredLevelToCompleteOnet == currentLevelCompletedOnet;
    }

    public override void ClaimReward()
    {
        if (IsCompleted())
        {
            Debug.Log("Custom onet minigame Completed!");
            PlayerPrefs.SetInt("Event_OnetCurrentLevel",0);
        }
    }
}
