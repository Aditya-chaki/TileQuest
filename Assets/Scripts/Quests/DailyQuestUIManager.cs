using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void Start()
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
    private void Update()
    {
        UpdateUI();
        if (DailyRewards.instance.isTimeToReset)
        {
            ResetUI();
        }
    }

    private void UpdateUI()
    {
        int currentValue = PlayerPrefs.GetInt($"{questKey}", 0);
        
        if (questNumber == 1)
        {
            bool isCompleted = false;
            isCompleted = DailyQuest.IsQuest1Completed();
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
        }
        else if (questNumber == 2)
        {
            bool isCompleted = false;
            isCompleted = DailyQuest.IsQuest2Completed();
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
        }
        else if (questNumber == 3)
        {
            bool isCompleted = false;
            isCompleted = DailyQuest.IsQuest3Completed();
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
        }


    }

    
}
