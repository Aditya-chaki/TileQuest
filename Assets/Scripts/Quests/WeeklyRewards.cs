using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WeeklyRewards : MonoBehaviour
{
    public static WeeklyRewards instance;
    [Header("UI Elements")]
    [SerializeField] private Slider progressSlider;  // Slider to represent progress
    [SerializeField] private TextMeshProUGUI progressText;  // Text to show progress
    [SerializeField] private TextMeshProUGUI timerText;  // Text to show the timer

    private int currentValue;  // The variable to track progress
    private int maxValue = 100;  // The maximum value for the slider
    private const string PROGRESS_PREF_KEY = "ProgressValue";  // PlayerPrefs key for saving progress
    private const string RESET_TIME_PREF_KEY = "WeeklyResetTime";  // PlayerPrefs key for reset time

    private DateTime nextResetTime;  // The next reset time

    // Rewards at specific thresholds
    private int[] rewardThresholds = { 10, 30, 60, 100 };
    private int[] coinRewards = { 100, 200, 400, 600 };  // Corresponding coin rewards

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    
    private void Start()
    {
        // Load progress and reset time from PlayerPrefs
        currentValue = PlayerPrefs.GetInt(PROGRESS_PREF_KEY, 0);
        nextResetTime = LoadResetTime();

        // Initialize slider and UI
        progressSlider.maxValue = maxValue;
        UpdateUI();
    }

    private void Update()
    {
        // Update the timer every frame
        UpdateTimer();
    }

    // Method to increment the value from other scripts
    public void IncrementValue(int amount)
    {
        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);  // Ensure the value doesn't exceed the max
        CheckForRewards();
        SaveProgress();  // Save progress when the value changes
        UpdateUI();
    }

    // Check if the current value reaches a reward threshold
    private void CheckForRewards()
    {
        for (int i = 0; i < rewardThresholds.Length; i++)
        {
            if (currentValue == rewardThresholds[i])
            {
                GiveReward(coinRewards[i]);
                break;
            }
        }
    }

    // Method to give a reward based on the threshold index using Config.cs coin system
    private void GiveReward(int coinAmount)
    {
        int currentCoins = Config.GetCoin();  // Get current coin count from Config
        Config.SetCoin(currentCoins + coinAmount);  // Add the reward to the current coins
        Debug.Log($"Player rewarded with {coinAmount} coins. Total coins: {Config.GetCoin()}");
    }

    // Update the slider and progress text
    private void UpdateUI()
    {
        progressSlider.value = currentValue;
        progressText.text = $"{currentValue}/{maxValue}";
    }

    // Save the current progress to PlayerPrefs
    private void SaveProgress()
    {
        PlayerPrefs.SetInt(PROGRESS_PREF_KEY, currentValue);
        PlayerPrefs.Save();
        Debug.Log($"Progress saved: {currentValue}");
    }

    // Method to load the reset time from PlayerPrefs
    private DateTime LoadResetTime()
    {
        string resetTimeString = PlayerPrefs.GetString(RESET_TIME_PREF_KEY, string.Empty);
        if (!string.IsNullOrEmpty(resetTimeString))
        {
            return DateTime.Parse(resetTimeString);
        }

        // If reset time doesn't exist, set the next reset to be 7 days from now
        DateTime newResetTime = DateTime.UtcNow.AddDays(7);
        SaveResetTime(newResetTime);
        return newResetTime;
    }

    // Method to save the reset time to PlayerPrefs
    private void SaveResetTime(DateTime resetTime)
    {
        PlayerPrefs.SetString(RESET_TIME_PREF_KEY, resetTime.ToString());
        PlayerPrefs.Save();
    }

    // Update the timer text
    private void UpdateTimer()
    {
        TimeSpan timeRemaining = nextResetTime - DateTime.UtcNow;

        if (timeRemaining.TotalSeconds > 0)
        {
            timerText.text = $"{timeRemaining.Days:D2}D {timeRemaining.Hours:D2}H {timeRemaining.Minutes:D2}M {timeRemaining.Seconds:D2}s";
        }
        else
        {
            timerText.text = "Resetting...";
            ResetProgress();  // Reset progress and set new reset time
        }
    }

    // Reset progress and set the next reset time
    private void ResetProgress()
    {
        currentValue = 0;
        SaveProgress();

        // Set the next reset to be 7 days from now
        nextResetTime = DateTime.UtcNow.AddDays(7);
        SaveResetTime(nextResetTime);

        UpdateUI();  // Update the UI after resetting progress
    }
}
