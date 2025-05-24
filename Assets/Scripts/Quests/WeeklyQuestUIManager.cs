using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class WeeklyQuestUIManager : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Weekly quest UI");
        questKey = WeeklyQuest.GetQuestKey(questIndex);
        requiredValue = WeeklyQuest.GetRequiredAmount(questNumber);
        discriptionTxt.text = WeeklyQuest.GetQuestName(questNumber);
        int currentValue = PlayerPrefs.GetInt($"{questKey}", 0);
        progressText.text = $"{currentValue}/{requiredValue}";
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
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
    private bool IsQuestCompleted()
    {
        switch (questIndex)
        {
            case 1: return WeeklyQuest.IsQuest1Completed();
            case 2: return WeeklyQuest.IsQuest2Completed();
            case 3: return WeeklyQuest.IsQuest3Completed();
            default:
                Debug.LogWarning($"Invalid quest number: {questIndex}");
                return false;
        }
    }

    private void UpdateUI()
    {
        if (!DailyQuest.IsQuestActive(questIndex))
        {
           // UpdateQuestVisibility();
            return;
        }
        discriptionTxt.text = WeeklyQuest.GetQuestName(questNumber);
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
}
