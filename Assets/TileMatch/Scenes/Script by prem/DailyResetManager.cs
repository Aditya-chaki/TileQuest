using System;
using TMPro;
using UnityEngine;

public class DailyResetManager : MonoBehaviour
{
    public static DailyResetManager instance;

    // Key to save the last reset time in PlayerPrefs
    private const string LAST_RESET_TIME_KEY = "LastDailyResetTime";

    // Stores the next time when reset should happen
    private DateTime _nextResetTime;

    // Property to check if current time has passed the next reset time
    public bool IsTimeToReset => DateTime.UtcNow >= _nextResetTime;

    [Header("UI Reference")]
    public TextMeshProUGUI resetTimerText; // 👈 Drag your TMP text object here in Inspector

    private void Awake()
    {
        // Singleton pattern to keep one instance across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object between scenes
            CheckAndHandleResetOnStart(); // Check reset when game launches
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Continually check if it's time to reset (during gameplay)
        if (IsTimeToReset)
        {
            PerformDailyReset();
        }

        // Update the countdown text
        if (resetTimerText != null)
        {
            resetTimerText.text = GetFormattedTimeRemaining();
        }
    }

    // This method is called once on game start to determine if reset is needed
    private void CheckAndHandleResetOnStart()
    {
        // Get the last reset time saved in PlayerPrefs
        string savedTime = PlayerPrefs.GetString(LAST_RESET_TIME_KEY, string.Empty);

        // Try to convert the saved string to a DateTime object
        if (DateTime.TryParse(savedTime, out DateTime lastResetTime))
        {
            // Set the next reset time to 24 hours after the last one
            _nextResetTime = lastResetTime.AddDays(1);

            // If current time is past the reset time, perform the reset
            if (DateTime.UtcNow >= _nextResetTime)
            {
                PerformDailyReset();
            }
        }
        else
        {
            // If it's the first time or time is invalid, perform initial reset
            PerformDailyReset();
        }
    }

    // Saves the current reset time in PlayerPrefs
    private void SaveResetTime(DateTime time)
    {
        PlayerPrefs.SetString(LAST_RESET_TIME_KEY, time.ToString());
        PlayerPrefs.Save();
    }

    // This method resets all daily content like quests
    private void PerformDailyReset()
    {
        Debug.Log("Daily Reset Triggered!");

        // Call the reset method from your DailyQuest system
        DailyQuest.ResetAllQuests();

        // Set the new reset time to 24 hours from now
        DateTime now = DateTime.UtcNow;
        _nextResetTime = now.AddDays(1);

        // Save the reset time
        SaveResetTime(now);
    }

    // Get how much time is left before the next reset
    public TimeSpan GetTimeRemaining()
    {
        return _nextResetTime - DateTime.UtcNow;
    }


    //We can show on ui remaining timing
    // Returns a string like "01H 23M 45s" for UI display
    public string GetFormattedTimeRemaining()
    {
        TimeSpan remaining = GetTimeRemaining();

        if (remaining.TotalSeconds > 0)
            return $"{remaining.Hours:D2}H : {remaining.Minutes:D2}M : {remaining.Seconds:D2}s";
        else
            return "Resetting...";
    }

}
