using UnityEngine;
using TMPro;
using UnityEngine.UI;  // For using the Slider component

public class TileJoin_Quest : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI tileJoinProgressText;  // Text to show progress
    public Slider tileJoinProgressSlider;  // New slider for visual progress
    public Image Completed;

    void Start()
    {
        // Initialize the UI based on the current quest state
        UpdateUI();
    }

    void Update()
    {
        // Optionally, if progress is dynamic, you could track it every frame
        UpdateUI();
    }

    void UpdateUI()
    {
        // Update the progress
        int completedJoins = PlayerPrefs.GetInt(Config.TILE_JOIN_VARIABLE_KEY, 0);
        int requiredJoins = Config.TILE_JOIN_REQUIRED_COUNT;

        // Update the progress slider
        tileJoinProgressSlider.maxValue = requiredJoins;
        tileJoinProgressSlider.value = completedJoins;

        // Update the progress text
        tileJoinProgressText.text = $"Tile Joins: {completedJoins}/{requiredJoins}";

        // Automatically claim reward if quest is complete
        CheckAndClaimReward();
    }

    void CheckAndClaimReward()
    {
        // Automatically claim the reward if the quest is complete
        int completedJoins = PlayerPrefs.GetInt(Config.TILE_JOIN_VARIABLE_KEY, 0);
        if (completedJoins >= Config.TILE_JOIN_REQUIRED_COUNT)
        {
            Config.RewardPlayerForTileJoin();
            Config.ResetTileJoinDailyQuest();
            UpdateUI(); // Update the UI after claiming the reward
            Completed.enabled = true;
        }
        else{
             Completed.enabled = false;
        }
    }

    // Removed the timer text and associated logic
}
