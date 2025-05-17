using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class DailyQuestUI : MonoBehaviour
{
    [Header("Quest Configuration")]
    [SerializeField] private string questKey;  // Unique identifier for the quest
    [SerializeField] private int requiredValue;  // Value needed to complete the quest
    [SerializeField] private int questNumber;

    [Header("UI Elements")]
    [SerializeField] private Slider progressSlider;  // Slider for quest progress
    [SerializeField] private TextMeshProUGUI progressText;  // Text to show progress
    [SerializeField] private Image completedIndicator;  // Indicator for quest completion
    [SerializeField] private TextMeshProUGUI discriptionTxt; //Text to show quest discription 
    private int questIndex;
    
    private void Start()
    {
        string activeQuest = PlayerPrefs.GetString("ActiveQuests");
        int n = 5;
        int[] activeQuestArray = activeQuest.Split(',').Select(s => int.TryParse(s, out n) ? n : 0).ToArray(); 
        Debug.Log(activeQuestArray.Length+" "+activeQuestArray[0]+" "+activeQuest);
        questIndex = activeQuestArray[questNumber-1];
        questKey = DailyQuest.GetQuestKey(questIndex);
        requiredValue = DailyQuest.GetRequiredAmount(questIndex);
        UpdateUI();
    }

    public void ResetUI()
    {
        int currentValue = PlayerPrefs.GetInt($"{questKey}", 0);
        progressSlider.value = 0;
        //progressSlider.value = currentValue;
        progressText.text = $"{currentValue}/{requiredValue}";
        completedIndicator.enabled = false;
    }
    private void Update()
    {
        UpdateUI();
        // if (DailyRewards.instance.isTimeToReset)
        // {
        //     ResetUI();
        // }
    }

    private void UpdateUI()
    {
        if (!DailyQuest.IsQuestActive(questIndex))
        {
           // UpdateQuestVisibility();
            return;
        }

        int currentValue = PlayerPrefs.GetInt($"{questKey}", 0);
        bool isCompleted = false;
        isCompleted = IsQuestCompleted();
        
        if (isCompleted)
        {
            progressSlider.value = requiredValue;
            progressText.text = "Completed!";
            completedIndicator.enabled = true;
        }
        else
        {
            progressSlider.maxValue = requiredValue;
            progressSlider.value = currentValue;
            progressText.text = $"{currentValue}/{requiredValue}";
            completedIndicator.enabled = false;
        }

        //UpdateQuestVisibility();
    }

    // New: Check completion status for any quest number
    private bool IsQuestCompleted()
    {
        switch (questIndex)
        {
            case 1: return DailyQuest.IsQuest1Completed();
            case 2: return DailyQuest.IsQuest2Completed();
            case 3: return DailyQuest.IsQuest3Completed();
            case 4: return DailyQuest.IsQuest4Completed();
            case 5: return DailyQuest.IsQuest5Completed();
            case 6: return DailyQuest.IsQuest6Completed();
            case 7: return DailyQuest.IsQuest7Completed();
            case 8: return DailyQuest.IsQuest8Completed();
            case 9: return DailyQuest.IsQuest9Completed();
            case 10: return DailyQuest.IsQuest10Completed();
            default:
                Debug.LogWarning($"Invalid quest number: {questIndex}");
                return false;
        }
    }

    //Enable or disable the quest UI based on whether the quest is active
    private void UpdateQuestVisibility()
    {
        bool isActive = DailyQuest.IsQuestActive(questIndex);            
        gameObject.SetActive(isActive);
    }

    private void SetQuestTitle()
    {
    discriptionTxt.text = questIndex switch
    {
        1 => "Complete 2 Levels",
        2 => "Match 50 Tiles",
        3 => "Clear 15 Set Tiles",
        4 => "Upgrade 1 Bulding",
        5 => "Watch an Ad",
        6 => "Spin the Daily Wheel",
        7 => "Trade Resources",
        8 => "Make a Decision",
        9 => "Play a Minigame",
        10 => "Earn 10 Opinion Points",
        _ => "Unknown Quest"
    };
    }
    
}
