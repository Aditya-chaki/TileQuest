using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DailyRewards : MonoBehaviour
{
    public static DailyRewards instance;
    [Header("UI Elements")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI timerText;

    private int currentValue;
    private int maxValue = 100;
    private const string PROGRESS_PREF_KEY = "ProgressValue";
    private const string DAILY_RESET_TIME_PREF_KEY = "DailyQuestResetTime";

    private DateTime nextResetTime;
    public bool isTimeToReset = false;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        currentValue = PlayerPrefs.GetInt(PROGRESS_PREF_KEY, 0);
        nextResetTime = DateTime.UtcNow.AddSeconds(30);
        SaveResetTime(nextResetTime); // Ensure reset time is saved properly
        progressSlider.maxValue = maxValue;
        UpdateUI();
    }

    private void Update()
    {
        UpdateTimer();
    }
    public void IncrementValue(int amount)
    {
        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);  // Ensure the value doesn't exceed the max
        SaveProgress();  // Save progress when the value changes
        UpdateUI();
    }

    public DateTime GetResetTime()
    {
        return nextResetTime;
    }

    private DateTime LoadResetTime()
    {
        string resetTimeString = PlayerPrefs.GetString(DAILY_RESET_TIME_PREF_KEY, string.Empty);
        Debug.Log($"[DailyRewards] Loaded reset time: {resetTimeString}");

        if (!string.IsNullOrEmpty(resetTimeString) && DateTime.TryParse(resetTimeString, out DateTime savedResetTime))
        {
            return savedResetTime;
        }

        // Default to the next day if the key is empty or invalid
        DateTime newResetTime = DateTime.UtcNow.Date.AddDays(1);
        SaveResetTime(newResetTime);
        return newResetTime;
    }

    private void SaveResetTime(DateTime resetTime)
    {
        PlayerPrefs.SetString(DAILY_RESET_TIME_PREF_KEY, resetTime.ToString());
        PlayerPrefs.Save();
        Debug.Log($"[DailyRewards] Saved reset time: {resetTime}");
    }

    private void UpdateTimer()
    {
        TimeSpan timeRemaining = nextResetTime - DateTime.UtcNow;

        if (timeRemaining.TotalSeconds > 0)
        {
            timerText.text = $"{timeRemaining.Hours:D2}H {timeRemaining.Minutes:D2}M {timeRemaining.Seconds:D2}s";
            isTimeToReset = false;
        }
        else
        {
            DailyQuest.ResetAllQuests();
            timerText.text = "Resetting...";
            isTimeToReset = true;
            ResetProgress();
        }
    }

    private void ResetProgress()
    {
        currentValue = 0;
        SaveProgress();

        nextResetTime = DateTime.UtcNow.Date.AddDays(1);
        SaveResetTime(nextResetTime);
        UpdateUI();
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(PROGRESS_PREF_KEY, currentValue);
        PlayerPrefs.Save();
        Debug.Log($"[DailyRewards] Progress saved: {currentValue}");
    }

    private void UpdateUI()
    {
        progressSlider.value = currentValue;
        progressText.text = $"{currentValue}/{maxValue}";
    }
}
