using System;
using UnityEngine;

public class DailyQuest
{
    #region QUEST1
    private const string LAST_COMPLETED_DATE_KEY = "LastCompletedDate";
    private const string LEVELS_COMPLETED_KEY = "LevelsCompleted";
    public const int REQUIRED_LEVELS = 10;
    private const int REWARD_COINS = 100;

    private static DateTime? _lastCompletedDateCache;

    // Method to check if the daily quest is available
    public static bool IsDailyQuestAvailable()
    {
        DateTime lastCompletedDate = GetLastCompletedDate();
        DateTime currentDate = DateTime.UtcNow.Date;

        return lastCompletedDate < currentDate;
    }

    // Method to mark a level as completed
    public static void CompleteLevel()
    {
        if (!IsDailyQuestAvailable()) return;

        int levelsCompleted = PlayerPrefs.GetInt(LEVELS_COMPLETED_KEY, 0);
        levelsCompleted++;
        PlayerPrefs.SetInt(LEVELS_COMPLETED_KEY, levelsCompleted);

        if (levelsCompleted >= REQUIRED_LEVELS)
        {
            RewardPlayer();
            ResetDailyQuest();
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    // Method to reward the player
    private static void RewardPlayer()
    {
        int currentCoins = Config.GetCoin();
        Config.SetCoin(currentCoins + REWARD_COINS);
        Debug.Log($"Player rewarded with {REWARD_COINS} coins.");
    }

    // Method to reset the daily quest
    private static void ResetDailyQuest()
    {
        PlayerPrefs.SetString(LAST_COMPLETED_DATE_KEY, DateTime.UtcNow.Date.ToString());
        PlayerPrefs.SetInt(LEVELS_COMPLETED_KEY, 0);
        PlayerPrefs.Save();

        // Reset cache
        _lastCompletedDateCache = DateTime.UtcNow.Date;
    }

    // Method to get the last completed date
    private static DateTime GetLastCompletedDate()
    {
        if (!_lastCompletedDateCache.HasValue)
        {
            string lastCompletedDateStr = PlayerPrefs.GetString(LAST_COMPLETED_DATE_KEY, DateTime.MinValue.ToString());
            DateTime.TryParse(lastCompletedDateStr, out DateTime lastCompletedDate);
            _lastCompletedDateCache = lastCompletedDate;
        }
        return _lastCompletedDateCache.Value;
    }

    // Method to get the number of completed levels today
    public static int GetCompletedLevelsToday()
    {
        return IsDailyQuestAvailable() ? PlayerPrefs.GetInt(LEVELS_COMPLETED_KEY, 0) : 0;
    }

    // Method to update levels completed using the current level from Config
    public static void UpdateLevelsCompleted()
    {
        if (!IsDailyQuestAvailable()) return;

        int currentLevel = Config.currLevel;
        int levelsCompleted = Mathf.Min(currentLevel, REQUIRED_LEVELS);
        PlayerPrefs.SetInt(LEVELS_COMPLETED_KEY, levelsCompleted);

        if (levelsCompleted >= REQUIRED_LEVELS)
        {
            RewardPlayer();
            ResetDailyQuest();
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    // New method to get the time remaining until the next quest reset
    public static TimeSpan GetTimeUntilNextReset()
    {
        DateTime currentDate = DateTime.UtcNow;
        DateTime nextResetDate = currentDate.Date.AddDays(1);
        return nextResetDate - currentDate;
    }
    #endregion

    #region QUEST2
   
    #endregion
}
