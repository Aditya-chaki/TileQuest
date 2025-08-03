using UnityEngine;
public class MilestoneEvent : BaseEvent
{
    public int requiredInfluence;
    public int requiredGold;
    public int requiredMagic;
    
    private int currentInfluence;
    private int currentGold;
    private int currentMagic;

    public override void Initialize()
    {
        currentInfluence = PlayerPrefs.GetInt("Event_currentInfluence",0);
        currentGold = PlayerPrefs.GetInt("Event_currentGold",0);
        currentMagic = PlayerPrefs.GetInt("Event_currentMagic",0);
        PlayerPrefs.SetInt("Event_requiredInfluence",requiredInfluence);
        PlayerPrefs.SetInt("Event_requiredGold",requiredGold);
        PlayerPrefs.SetInt("Event_requiredMagic",requiredMagic);
        PlayerPrefs.Save();
    }

    public override void UpdateProgress()
    {
        currentInfluence = PlayerPrefs.GetInt("Event_currentInfluence");
        currentGold = PlayerPrefs.GetInt("Event_currentGold");
        currentMagic = PlayerPrefs.GetInt("Event_currentMagic");
        PlayerPrefs.Save();
    }

    public override bool IsCompleted()
    {
        
        if(currentInfluence<requiredInfluence)
            return false;
        else if(currentGold<requiredGold)
            return false;
        else if(currentMagic<requiredMagic)
            return false;        

        return true;
    }

    public override void ClaimReward()
    {
        if (IsCompleted())
        {
            // Give rewards
            Debug.Log("Milestone Reward Claimed!");

            PlayerPrefs.SetInt("Event_currentInfluence",0);
            PlayerPrefs.SetInt("Event_currentGold",0);
            PlayerPrefs.SetInt("Event_currentMagic",0);
            Config.Gold = Config.Gold+1000;
        }
    }
}
