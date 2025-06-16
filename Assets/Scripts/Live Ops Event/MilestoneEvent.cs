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
        currentFood = PlayerPrefs.GetInt("Event_requiredFood");
        currentGold = PlayerPrefs.GetInt("Event_currentGold");
        currentMagic = PlayerPrefs.GetInt("Event_requiredMagic");
    }

    public override void UpdateProgress()
    {
        currentFood = PlayerPrefs.GetInt("Event_requiredFood");
        currentGold = PlayerPrefs.GetInt("Event_currentGold");
        currentMagic = PlayerPrefs.GetInt("Event_requiredMagic");
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
        }
    }
}
