using UnityEngine;
public class MilestoneEvent : BaseEvent
{
    public int requiredFood;
    public int requiredGold;
    public int requiredMagic;
    
    private int currentFood;
    private int currentGold;
    private int currentMagic;

    public override void Initialize()
    {
        currentFood = PlayerPrefs.GetInt("Event_currentFood",0);
        currentGold = PlayerPrefs.GetInt("Event_currentGold",0);
        currentMagic = PlayerPrefs.GetInt("Event_currentMagic",0);
        PlayerPrefs.SetInt("Event_requiredFood",requiredFood);
        PlayerPrefs.SetInt("Event_requiredGold",requiredGold);
        PlayerPrefs.SetInt("Event_requiredMagic",requiredMagic);
        PlayerPrefs.Save();
    }

    public override void UpdateProgress()
    {
        currentFood = PlayerPrefs.GetInt("Event_currentFood");
        currentGold = PlayerPrefs.GetInt("Event_currentGold");
        currentMagic = PlayerPrefs.GetInt("Event_currentMagic");
        PlayerPrefs.Save();
    }

    public override bool IsCompleted()
    {
        
        if(currentFood<requiredFood)
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

            PlayerPrefs.SetInt("Event_currentFood",0);
            PlayerPrefs.SetInt("Event_currentGold",0);
            PlayerPrefs.SetInt("Event_currentMagic",0);
        }
    }
}
