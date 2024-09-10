using UnityEngine;
using UnityEngine.UI;  // For using the Slider component
using TMPro;

public class Talk_Quest : MonoBehaviour
{
    public TextMeshProUGUI progressText;  // Progress text for displaying talk quest progress
    public Slider progressSlider;  // New slider for showing progress visually
    public Image Completed;

    private void Start()
    {
        // Initialize the UI, hide any unnecessary text initially
        UpdateProgress();
    }

    private void Update()
    {
        // Optionally, if progress is dynamic, you could track it every frame
        UpdateProgress();
    }

    // Method to update the progress on the UI
    public void UpdateProgress()
    {
        int currentTalkCount = Config.GetTalkVariable();
        int requiredTalkCount = Config.TALK_REQUIRED_COUNT;

        // Update progress slider
        progressSlider.maxValue = requiredTalkCount;
        progressSlider.value = currentTalkCount;

        // Update progress text
        progressText.text = $"Daily Talk Quest: {currentTalkCount}/{requiredTalkCount}";

        // Check if the quest is completed
        if (currentTalkCount >= requiredTalkCount)
        {
            ShowCompletionMessage();
            Completed.enabled = true;
        }
        else
        {
            Completed.enabled = false;

        }
    }

    // Method to show the completion message
    public void ShowCompletionMessage()
    {
        progressText.text = "Quest Completed!";
    }


}
