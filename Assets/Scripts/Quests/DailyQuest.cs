using System;
using UnityEngine;
using TMPro;

public class DailyQuest : MonoBehaviour
{
    #region Constants and Fields

    private const string QUEST1_COMPLETED_KEY = "Quest1Completed";
    private const string QUEST2_COMPLETED_KEY = "Quest2Completed";
    private const string QUEST3_COMPLETED_KEY = "Quest3Completed";

    private const string LEVELS_COMPLETED_KEY = "LevelsCompleted";
    public const int REQUIRED_LEVELS = 2;
    private const int REWARD_COINS_QUEST1 = 100;

    private const string TILES_MATCHED_KEY = "TilesMatched";
    private const int REQUIRED_TILES_MATCHED = 50;
    private const int REWARD_COINS_QUEST2 = 150;

    private const string CLEAR_SET_TILE_KEY = "ClearSetTile";
    private const int REQUIRED_SET_TILES_MATCHED = 15;
    private const int REWARD_COINS_QUEST3 = 150;
    private int setTile;

    private const string DAILY_RESET_TIME_PREF_KEY = "DailyQuestResetTime";

    private static DateTime _nextDailyResetTime;

    public TextMeshProUGUI dailyQuestTimerText;

    #endregion

    #region Unity Lifecycle

    private void Start()
    {
        // Initialize the reset time
        _nextDailyResetTime = DailyRewards.instance.GetResetTime();
        
    }

    private void Update()
    {
        UpdateTimerUI();
<<<<<<< HEAD
        if (DailyRewards.instance.isTimeToReset)
        {
            ResetAllQuests();
        }
=======
      /*  if (DailyRewards.instance.isTimeToReset)
        {
            ResetAllQuests();
        }*/
>>>>>>> b97c403d (Daily Quest reset update & Managed scenes)
    }

    #endregion

    #region Quest 1 (Levels Completed)

    // Check if quest 1 is completed
    public static bool IsQuest1Completed()
    {
        return PlayerPrefs.GetInt(QUEST1_COMPLETED_KEY, 0) == 1;
    }
    public static int GetCompletedLevelsToday()
    {
        int levelsCompleted = PlayerPrefs.GetInt(LEVELS_COMPLETED_KEY, 0);
        return levelsCompleted;
    }

    // Update levels completed progress
    public static void UpdateLevelsCompleted()
    {
        if (IsQuest1Completed()) return;

        int levelsCompleted = PlayerPrefs.GetInt(LEVELS_COMPLETED_KEY, 0) + 1;
        PlayerPrefs.SetInt(LEVELS_COMPLETED_KEY, levelsCompleted);

        if (levelsCompleted >= REQUIRED_LEVELS)
        {
            DailyRewards.instance.IncrementValue(50);
            RewardQuest1();
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    // Reward for completing quest 1
    private static void RewardQuest1()
    {
        PlayerPrefs.SetInt(QUEST1_COMPLETED_KEY, 1); // Mark quest 1 as completed
        int currentCoins = Config.GetCoin();
        Config.SetCoin(currentCoins + REWARD_COINS_QUEST1);
        Debug.Log($"Quest 1 completed! Player rewarded with {REWARD_COINS_QUEST1} coins.");
        PlayerPrefs.Save();
    }

<<<<<<< HEAD
=======
    public void Reset_Quest_1_Call()
    {
        ResetQuest1();
    }


>>>>>>> b97c403d (Daily Quest reset update & Managed scenes)
    // Reset quest 1
    private static void ResetQuest1()
    {
        PlayerPrefs.SetInt(LEVELS_COMPLETED_KEY,0);
        
        PlayerPrefs.SetInt(QUEST1_COMPLETED_KEY, 0); // Reset completion status
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 2 (Tiles Matched)

    // Check if quest 2 is completed
    public static bool IsQuest2Completed()
    {
        return PlayerPrefs.GetInt(QUEST2_COMPLETED_KEY, 0) == 1;
    }

    // Update tiles matched progress
    public static void UpdateTilesMatched(int tilesMatched)
    {
        if (IsQuest2Completed()) return;

        int totalTilesMatched = PlayerPrefs.GetInt(TILES_MATCHED_KEY, 0) + tilesMatched;
        PlayerPrefs.SetInt(TILES_MATCHED_KEY, totalTilesMatched);

        if (totalTilesMatched >= REQUIRED_TILES_MATCHED)
        {
            RewardQuest2();
            DailyRewards.instance.IncrementValue(50);
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    // Reward for completing quest 2
    private static void RewardQuest2()
    {
        PlayerPrefs.SetInt(QUEST2_COMPLETED_KEY, 1); // Mark quest 2 as completed
        int currentCoins = Config.GetCoin();
        Config.SetCoin(currentCoins + REWARD_COINS_QUEST2);
        Debug.Log($"Quest 2 completed! Player rewarded with {REWARD_COINS_QUEST2} coins.");
        PlayerPrefs.Save();
    }

    // Reset quest 2
    private static void ResetQuest2()
    {
        PlayerPrefs.SetInt(TILES_MATCHED_KEY, 0);
        PlayerPrefs.SetInt(QUEST2_COMPLETED_KEY, 0); // Reset completion status
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 3(Clear same set of tiles)
    public static bool IsQuest3Completed()
    {
        return PlayerPrefs.GetInt(QUEST3_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateSetTiles(int tilesMatched)
    {
        if (IsQuest3Completed()) return;

        int totalTilesMatched = PlayerPrefs.GetInt(CLEAR_SET_TILE_KEY, 0) + tilesMatched;
        PlayerPrefs.SetInt(CLEAR_SET_TILE_KEY, totalTilesMatched);

        if (totalTilesMatched >= REQUIRED_SET_TILES_MATCHED)
        {
            RewardQuest3();
            DailyRewards.instance.IncrementValue(50);
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest3()
    {
        PlayerPrefs.SetInt(QUEST3_COMPLETED_KEY, 1); // Mark quest 2 as completed
        int currentCoins = Config.GetCoin();
        Config.SetCoin(currentCoins + REWARD_COINS_QUEST3);
        Debug.Log($"Quest 3 completed! Player rewarded with {REWARD_COINS_QUEST3} coins.");
        PlayerPrefs.Save();
    }

    // Reset quest 3
    private static void ResetQuest3()
    {
        PlayerPrefs.SetInt(CLEAR_SET_TILE_KEY, 0);
        PlayerPrefs.SetInt(QUEST3_COMPLETED_KEY, 0); // Reset completion status
        PlayerPrefs.Save();
    }

    #endregion


    #region Timer and Reset Management

    // Load the daily reset time
    public static DateTime GetDailyQuestDate()
    {
        return _nextDailyResetTime;
    }
    


    // Save the daily reset time
    private static void SaveDailyResetTime(DateTime resetTime)
    {
        PlayerPrefs.SetString(DAILY_RESET_TIME_PREF_KEY, resetTime.ToString());
        PlayerPrefs.Save();
    }

    // Reset all quests
    public static void ResetAllQuests()
    {
        ResetQuest1();
        ResetQuest2();
        ResetQuest3();
        Debug.Log("All daily quests have been reset.");
    }

    // Get the timer string for UI
    public static string GetDailyQuestTimer()
    {
        TimeSpan timeRemaining = _nextDailyResetTime - DateTime.UtcNow;
        Debug.Log($"Time Remaining: {timeRemaining}");

        if (timeRemaining.TotalSeconds > 0)
        {
            return $"{timeRemaining.Hours:D2}H {timeRemaining.Minutes:D2}M {timeRemaining.Seconds:D2}s";
        }
        else
        {
            Debug.Log("Resetting quests due to expired timer.");
            ResetAllQuests();
            _nextDailyResetTime = DailyRewards.instance.GetResetTime();
            SaveDailyResetTime(_nextDailyResetTime);
            return "Resetting...";
        }
    }


    // Update the timer UI
    private void UpdateTimerUI()
    {
        if (dailyQuestTimerText != null)
        {
            dailyQuestTimerText.text = GetDailyQuestTimer();
        }
    }

    #endregion
}
