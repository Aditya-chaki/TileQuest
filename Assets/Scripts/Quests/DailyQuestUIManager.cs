using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyQuestUI : MonoBehaviour
{
    [SerializeField] private Slider levelCompletionSlider;  // Slider for level completion
    [SerializeField] private TextMeshProUGUI progressText;  // New text to show progress (e.g., "3/10 Levels Completed")
    [SerializeField] private Image Completed;  // New text to show progress (e.g., "3/10 Levels Completed")

    private int lastTrackedLevel;

    private void Start()
    {
        lastTrackedLevel = Config.currLevel;
        UpdateUI();

        // Update the UI whenever the scene starts
        UpdateLevelsBasedOnCurrentLevel();
    }

    private void Update()
    {
        // Continuously track the current level and update UI if level changes
        if (Config.currLevel > lastTrackedLevel)
        {
            UpdateLevelsBasedOnCurrentLevel();
            lastTrackedLevel = Config.currLevel;
            UpdateUI();
        }
    }

    private void UpdateLevelsBasedOnCurrentLevel()
    {
        Config.UpdateLevelsCompleted();
    }

    private void UpdateUI()
    {
        int completedLevels = Config.GetCompletedLevelsToday();
        int requiredLevels = Config.REQUIRED_LEVELS;

        if (Config.IsDailyQuestAvailable())
        {
            levelCompletionSlider.maxValue = requiredLevels;  // Set the max value based on required levels
            levelCompletionSlider.value = completedLevels;    // Set the current value as the completed levels

            progressText.text = $"{completedLevels}/{requiredLevels} ";  // Display progress
             Completed.enabled = false;
        }
        else
        {
            levelCompletionSlider.value = levelCompletionSlider.maxValue;  // Set the slider to full when the quest is completed
            progressText.text = "Daily Quest Completed!";
            Completed.enabled = true;
            
        }
    }
}
