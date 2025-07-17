using System;
using UnityEngine;
using TMPro;
using System.Linq;
public class DailyQuest : MonoBehaviour
{
    #region Constants and Fields

    private const string QUEST1_COMPLETED_KEY = "Quest1Completed";
    private const string QUEST2_COMPLETED_KEY = "Quest2Completed";
    private const string QUEST3_COMPLETED_KEY = "Quest3Completed";
    private const string QUEST4_COMPLETED_KEY = "Quest4Completed";
    private const string QUEST5_COMPLETED_KEY = "Quest5Completed";
    private const string QUEST6_COMPLETED_KEY = "Quest6Completed";
    private const string QUEST7_COMPLETED_KEY = "Quest7Completed";
    private const string QUEST8_COMPLETED_KEY = "Quest8Completed";
    private const string QUEST9_COMPLETED_KEY = "Quest9Completed";
    private const string QUEST10_COMPLETED_KEY = "Quest10Completed";
    
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

    private const string ITEMS_UPGRADED_KEY = "BuildingUpgraded";
    private const int REQUIRED_ITEMS_UPGRADED = 1;
    private const int REWARD_COINS_QUEST4 = 100;

    private const string ADS_WATCHED_KEY = "AdsWatched";
    private const int REQUIRED_ADS_WATCHED = 1;
    private const int REWARD_COINS_QUEST5 = 100;

    private const string WHEEL_SPINS_KEY = "WheelSpins";
    private const int REQUIRED_WHEEL_SPINS = 1;
    private const int REWARD_COINS_QUEST6 = 100;

    private const string RESOURCES_TRADED_KEY = "ResourcesTraded";
    private const int REQUIRED_RESOURCES_TRADED = 1;
    private const int REWARD_COINS_QUEST7 = 120;

    private const string DECISIONS_MADE_KEY = "DecisionsMade";
    private const int REQUIRED_DECISIONS_MADE = 1;
    private const int REWARD_COINS_QUEST8 = 100;

    private const string MINIGAMES_PLAYED_KEY = "MinigamesPlayed";
    private const int REQUIRED_MINIGAMES_PLAYED = 1;
    private const int REWARD_COINS_QUEST9 = 100;

    private const string OPINION_POINTS_KEY = "OpinionPoints";
    private const int REQUIRED_OPINION_POINTS = 10;
    private const int REWARD_COINS_QUEST10 = 120;

    // New: Key to store selected quest indices
    private const string ACTIVE_QUESTS_KEY = "ActiveQuests";
    private const string DAILY_RESET_TIME_PREF_KEY = "DailyQuestResetTime";

    private static DateTime _nextDailyResetTime;

    public TextMeshProUGUI dailyQuestTimerText;

    #endregion

    #region Unity Lifecycle

    private void Start()
    {
        // Initialize the reset time
        //_nextDailyResetTime = DailyRewards.instance.GetResetTime();
        string activeQuest = PlayerPrefs.GetString("ActiveQuests");
        Debug.Log("DailyQuest"+activeQuest);
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(ACTIVE_QUESTS_KEY))||activeQuest.Length<5)
        {
            Debug.Log("random quest");
            SelectRandomQuests();
        }
    }

    private void Update()
    {
        //UpdateTimerUI();
        // if (DailyRewards.instance.isTimeToReset)
        // {
        //     ResetAllQuests();
        // }
    }

    #endregion
    #region Quest Selection Logic

    // New: Select 5 random quests and store their indices
    private static void SelectRandomQuests()
    {
        int[] allQuests = { 1, 2, 3, 4, 5, 6, 7, 8 ,9 ,10}; // Indices for all 8 quests
        System.Random random = new System.Random();
        int[] selectedQuests = allQuests.OrderBy(x => random.Next()).Take(5).ToArray();
        string questString = string.Join(",", selectedQuests);
        PlayerPrefs.SetString(ACTIVE_QUESTS_KEY, questString);
        PlayerPrefs.Save();
        Debug.Log($"Selected daily quests: {questString}");
    }

    // New: Check if a specific quest is active
    public static bool IsQuestActive(int questIndex)
    {
        string activeQuests = PlayerPrefs.GetString(ACTIVE_QUESTS_KEY, "");
        if (string.IsNullOrEmpty(activeQuests)) return false;
        string[] questIndices = activeQuests.Split(',');
        return questIndices.Contains(questIndex.ToString());
    }
    // New: Get the required amount for a quest based on its number
    public static int GetRequiredAmount(int questNumber)
    {
        return questNumber switch
        {
            1 => REQUIRED_LEVELS,
            2 => REQUIRED_TILES_MATCHED,
            3 => REQUIRED_SET_TILES_MATCHED,
            4 => REQUIRED_ITEMS_UPGRADED,
            5 => REQUIRED_ADS_WATCHED,
            6 => REQUIRED_WHEEL_SPINS,
            7 => REQUIRED_RESOURCES_TRADED,
            8 => REQUIRED_DECISIONS_MADE,
            9 => REQUIRED_MINIGAMES_PLAYED,
            10 => REQUIRED_OPINION_POINTS,
            _ => 0 // Return 0 for invalid quest numbers to prevent errors
        };
    }

    public static string GetQuestKey(int questNumber)
    {
        return questNumber switch
        {
            1 => LEVELS_COMPLETED_KEY,
            2 => TILES_MATCHED_KEY,
            3 => CLEAR_SET_TILE_KEY,
            4 => ITEMS_UPGRADED_KEY,
            5 => ADS_WATCHED_KEY,
            6 => WHEEL_SPINS_KEY,
            7 => RESOURCES_TRADED_KEY,
            8 => DECISIONS_MADE_KEY,
            9 => MINIGAMES_PLAYED_KEY,
            10 => OPINION_POINTS_KEY,
            _ => "unknown" // Return 0 for invalid quest numbers to prevent errors
        };
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
        int currentCoins = Config.Influence;
        Config.Influence = Config.Influence+REWARD_COINS_QUEST1;
        Debug.Log($"Quest 1 completed! Player rewarded with {REWARD_COINS_QUEST1} Influence.");
        PlayerPrefs.Save();
    }

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
        int currentCoins = Config.Influence;
        Config.Influence = Config.Influence+REWARD_COINS_QUEST2;
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
        int currentCoins = Config.Gold;
        Config.Gold = Config.Gold+REWARD_COINS_QUEST3;
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

    #region Quest 4 (Upgrade 1 Buliding)

    public static bool IsQuest4Completed()
    {
        return PlayerPrefs.GetInt(QUEST4_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateItemsUpgraded()
    {
        if (IsQuest4Completed() || !IsQuestActive(4)) return;

        int itemsUpgraded = PlayerPrefs.GetInt(ITEMS_UPGRADED_KEY, 0) + 1;
        PlayerPrefs.SetInt(ITEMS_UPGRADED_KEY, itemsUpgraded);

        if (itemsUpgraded >= REQUIRED_ITEMS_UPGRADED)
        {
            RewardQuest4();
            DailyRewards.instance.IncrementValue(50);
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest4()
    {
        PlayerPrefs.SetInt(QUEST4_COMPLETED_KEY, 1);
        int currentCoins = Config.Gold;
        Config.Gold = Config.Gold+REWARD_COINS_QUEST4;
        Debug.Log($"Quest 4 completed! Player rewarded with {REWARD_COINS_QUEST4} coins for upgrading an item.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest4()
    {
        PlayerPrefs.SetInt(ITEMS_UPGRADED_KEY, 0);
        PlayerPrefs.SetInt(QUEST4_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 5 (Watch an Ad for Bonus)

    public static bool IsQuest5Completed()
    {
        return PlayerPrefs.GetInt(QUEST5_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateAdsWatched()
    {
        if (IsQuest5Completed() || !IsQuestActive(5)) return;

        int adsWatched = PlayerPrefs.GetInt(ADS_WATCHED_KEY, 0) + 1;
        PlayerPrefs.SetInt(ADS_WATCHED_KEY, adsWatched);

        if (adsWatched >= REQUIRED_ADS_WATCHED)
        {
            RewardQuest5();
            DailyRewards.instance.IncrementValue(50);
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest5()
    {
        PlayerPrefs.SetInt(QUEST5_COMPLETED_KEY, 1);
        int currentCoins = Config.Magic;
        Config.Magic = Config.Magic+REWARD_COINS_QUEST5;
        Debug.Log($"Quest 5 completed! Player rewarded with {REWARD_COINS_QUEST5} coins for watching an ad.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest5()
    {
        PlayerPrefs.SetInt(ADS_WATCHED_KEY, 0);
        PlayerPrefs.SetInt(QUEST5_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 6 (Spin the Daily Wheel)

    public static bool IsQuest6Completed()
    {
        return PlayerPrefs.GetInt(QUEST6_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateWheelSpins()
    {
        if (IsQuest6Completed() || !IsQuestActive(6)) return;

        int wheelSpins = PlayerPrefs.GetInt(WHEEL_SPINS_KEY, 0) + 1;
        PlayerPrefs.SetInt(WHEEL_SPINS_KEY, wheelSpins);

        if (wheelSpins >= REQUIRED_WHEEL_SPINS)
        {
            RewardQuest6();
            DailyRewards.instance.IncrementValue(50);
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest6()
    {
        PlayerPrefs.SetInt(QUEST6_COMPLETED_KEY, 1);
        int currentCoins = Config.Gold;
        Config.Gold = Config.Gold+REWARD_COINS_QUEST6;
        Debug.Log($"Quest 6 completed! Player rewarded with {REWARD_COINS_QUEST6} coins for spinning the daily wheel.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest6()
    {
        PlayerPrefs.SetInt(WHEEL_SPINS_KEY, 0);
        PlayerPrefs.SetInt(QUEST6_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 7 (Trade Resources)

    public static bool IsQuest7Completed()
    {
        return PlayerPrefs.GetInt(QUEST7_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateResourcesTraded()
    {
        if (IsQuest7Completed() || !IsQuestActive(7)) return;

        int resourcesTraded = PlayerPrefs.GetInt(RESOURCES_TRADED_KEY, 0) + 1;
        PlayerPrefs.SetInt(RESOURCES_TRADED_KEY, resourcesTraded);

        if (resourcesTraded >= REQUIRED_RESOURCES_TRADED)
        {
            RewardQuest7();
            DailyRewards.instance.IncrementValue(50);
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest7()
    {
        PlayerPrefs.SetInt(QUEST7_COMPLETED_KEY, 1);
        int currentCoins = Config.Influence;
        Config.Influence = Config.Influence+REWARD_COINS_QUEST7;
        Debug.Log($"Quest 7 completed! Player rewarded with {REWARD_COINS_QUEST7} coins for trading resources.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest7()
    {
        PlayerPrefs.SetInt(RESOURCES_TRADED_KEY, 0);
        PlayerPrefs.SetInt(QUEST7_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 8 (Make a Decision)

    public static bool IsQuest8Completed()
    {
        return PlayerPrefs.GetInt(QUEST8_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateDecisionsMade()
    {
        if (IsQuest8Completed() || !IsQuestActive(8)) return;

        int decisionsMade = PlayerPrefs.GetInt(DECISIONS_MADE_KEY, 0) + 1;
        PlayerPrefs.SetInt(DECISIONS_MADE_KEY, decisionsMade);

        if (decisionsMade >= REQUIRED_DECISIONS_MADE)
        {
            RewardQuest8();
            DailyRewards.instance.IncrementValue(50);
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest8()
    {
        PlayerPrefs.SetInt(QUEST8_COMPLETED_KEY, 1);
        int currentCoins = Config.Gold;
        Config.Gold = Config.Gold+REWARD_COINS_QUEST8;
        Debug.Log($"Quest 8 completed! Player rewarded with {REWARD_COINS_QUEST8} coins for making a decision.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest8()
    {
        PlayerPrefs.SetInt(DECISIONS_MADE_KEY, 0);
        PlayerPrefs.SetInt(QUEST8_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 9 (Play a Minigame)

    public static bool IsQuest9Completed()
    {
        return PlayerPrefs.GetInt(QUEST9_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateMinigamesPlayed()
    {
        if (IsQuest9Completed() || !IsQuestActive(9)) return;

        int minigamesPlayed = PlayerPrefs.GetInt(MINIGAMES_PLAYED_KEY, 0) + 1;
        PlayerPrefs.SetInt(MINIGAMES_PLAYED_KEY, minigamesPlayed);

        if (minigamesPlayed >= REQUIRED_MINIGAMES_PLAYED)
        {
            RewardQuest9();
            DailyRewards.instance.IncrementValue(50);
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest9()
    {
        PlayerPrefs.SetInt(QUEST9_COMPLETED_KEY, 1);
        int currentCoins = Config.Gold;
        Config.Gold = Config.Gold+REWARD_COINS_QUEST9;
        Debug.Log($"Quest 9 completed! Player rewarded with {REWARD_COINS_QUEST9} coins for playing a minigame.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest9()
    {
        PlayerPrefs.SetInt(MINIGAMES_PLAYED_KEY, 0);
        PlayerPrefs.SetInt(QUEST9_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 10 (Earn 10 Opinion Points)

    public static bool IsQuest10Completed()
    {
        return PlayerPrefs.GetInt(QUEST10_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateOpinionPoints(int pointsEarned)
    {
        if (IsQuest10Completed() || !IsQuestActive(10)) return;

        int totalPoints = PlayerPrefs.GetInt(OPINION_POINTS_KEY, 0) + pointsEarned;
        PlayerPrefs.SetInt(OPINION_POINTS_KEY, totalPoints);

        if (totalPoints >= REQUIRED_OPINION_POINTS)
        {
            RewardQuest10();
            DailyRewards.instance.IncrementValue(50);
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest10()
    {
        PlayerPrefs.SetInt(QUEST10_COMPLETED_KEY, 1);
        int currentCoins = Config.Influence;
        Config.Influence = Config.Influence+REWARD_COINS_QUEST10;
        Debug.Log($"Quest 10 completed! Player rewarded with {REWARD_COINS_QUEST10} coins for earning opinion points.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest10()
    {
        PlayerPrefs.SetInt(OPINION_POINTS_KEY, 0);
        PlayerPrefs.SetInt(QUEST10_COMPLETED_KEY, 0);
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
        ResetQuest4();
        ResetQuest5();
        ResetQuest6();
        ResetQuest7();
        ResetQuest8();
        ResetQuest9();
        ResetQuest10();
        Debug.Log("All daily quests have been reset.");
    }

    // Get the timer string for UI
    public static string GetDailyQuestTimer()
    {
        TimeSpan timeRemaining = _nextDailyResetTime - DateTime.UtcNow;
        //Debug.Log($"Time Remaining: {timeRemaining}");

        if (timeRemaining.TotalSeconds > 0)
        {
            return $"{timeRemaining.Hours:D2}H {timeRemaining.Minutes:D2}M {timeRemaining.Seconds:D2}s";
        }
        else
        {
            Debug.Log("Resetting quests due to expired timer.");
            //ResetAllQuests();
            //_nextDailyResetTime = DailyRewards.instance.GetResetTime();
            //SaveDailyResetTime(_nextDailyResetTime);
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
