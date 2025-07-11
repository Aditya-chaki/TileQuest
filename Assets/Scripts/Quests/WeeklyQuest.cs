using System;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
public class WeeklyQuest : MonoBehaviour
{
    #region Constants and Fields

    private const string QUEST1_COMPLETED_KEY = "Quest1Completed";
    private const string QUEST2_COMPLETED_KEY = "Quest2Completed";
    private const string QUEST3_COMPLETED_KEY = "Quest3Completed";
    private const string QUEST4_COMPLETED_KEY = "Quest4Completed";
    private const string QUEST5_COMPLETED_KEY = "Quest5Completed";
    private const string QUEST6_COMPLETED_KEY = "Quest6Completed";
    private const string QUEST7_COMPLETED_KEY = "WeeklyQuest7Completed";
    private const string QUEST8_COMPLETED_KEY = "WeeklyQuest8Completed";
    private const string QUEST9_COMPLETED_KEY = "WeeklyQuest9Completed";
    private const string QUEST10_COMPLETED_KEY = "WeeklyQuest10Completed";
    

    private const string UPGRADE_BUILDING_KEY = "UpgradeBuilding";
    private const int REQUIRED_BUILDING_UPGRADED = 2;
    private const int REWARD_COINS_QUEST1 = 400;

    private const string UPGRADE_CASTLE_KEY = "UpgradeCastle";
    private const int REQUIRED_CASTLE_UPGRADED = 1;
    private const int REWARD_COINS_QUEST2 = 350;

    private const string EARN_GOLD_KEY = "EarnGold";
    private const int REQUIRED_GOLD_EARN = 2000;
    private const int REWARD_COINS_QUEST3 = 450;

    private const string INCREASE_MAGIC_KEY = "IncreaseMagic";
    private const int REQUIRED_MAGIC_INCREASE = 2000;
    private const int REWARD_COINS_QUEST4 = 400;

    private const string INCREASE_OPINION_KEY = "IncreaseOpinion";
    private const int REQUIRED_OPINION_INCREASE = 20;
    private const int REWARD_COINS_QUEST5 = 375;

    private const string COMPLETE_LEVEL_KEY = "CompleteLevel";
    private const int REQUIRED_LEVELS_COMPLETED = 10;
    private const int REWARD_COINS_QUEST6 = 425;

    private const string COLLECT_ITEM_KEY = "CollectItem";
    private const int REQUIRED_ITEMS_COLLECTED = 5;
    private const int REWARD_COINS_QUEST7 = 400;

    private const string PLAY_MINIGAMES_KEY = "PlayMiniGames";
    private const int REQUIRED_MINIGAMES_PLAYED = 10;
    private const int REWARD_COINS_QUEST8 = 450;

    private const string SPEND_MAGIC_KEY = "SpendMagic";
    private const int SPEND_MAGIC_SPENT = 2000;
    private const int REWARD_COINS_QUEST9 = 425;

    private const string SPEND_MORE_GOLD_KEY = "SpendMoreGold";
    private const int REQUIRED_MORE_GOLD_SPENT = 3000;
    private const int REWARD_COINS_QUEST10 = 450;

    private const string WEEKLY_RESET_TIME_PREF_KEY = "WeeklyQuestResetTime";
    private const string ACTIVE_QUESTS_KEY = "WeeklyActiveQuests";

    private const string WEEKLY_QUEST_COMPLETED = "WeeklyQuestComplete";
    private const string QUEST_REWARD_CLAIMED_KEY = "WeeklyQuestRewardClaimed_"; // Suffix with milestone (1, 2, 3)
    private static int numOfQuest = 10;
    private static int completedQuest;
    private static readonly int[] REWARD_MILESTONES = { 3, 6, 10 }; // Quests completed for rewards
    private static readonly int[] MILESTONE_REWARDS = { 200, 400, 600 }; // Coins for each milestone
    public Slider questProgressSlider;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        completedQuest = PlayerPrefs.GetInt(WEEKLY_QUEST_COMPLETED,0);
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(ACTIVE_QUESTS_KEY)))
        {
            SelectRandomQuests();
        }
        UpdateProgressSlider();
    }

    // Update is called once per frame
    void Update()
    {
     questProgressSlider.value = completedQuest/numOfQuest;   
    }

    #region Quest Selection Logic

    private static void SelectRandomQuests()
    {
        int[] allQuests = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        System.Random random = new System.Random();
        int[] selectedQuests = allQuests.OrderBy(x => random.Next()).Take(numOfQuest).ToArray();
        string questString = string.Join(",", selectedQuests);
        PlayerPrefs.SetString(ACTIVE_QUESTS_KEY, questString);
        PlayerPrefs.Save();
        Debug.Log($"Selected weekly quests: {questString}");
    }

    public static bool IsQuestActive(int questIndex)
    {
        string activeQuests = PlayerPrefs.GetString(ACTIVE_QUESTS_KEY, "");
        if (string.IsNullOrEmpty(activeQuests)) return false;
        string[] questIndices = activeQuests.Split(',');
        return questIndices.Contains(questIndex.ToString());
    }
    public static string GetQuestName(int questNumber)
    {
        return questNumber switch
        {
            1 => "Upgrade 2 Buildings",
            2 => "Upgrade Castle by 1 Level",
            3 => "Earn 3000 Gold",
            4 => "Increase Magic by 2000",
            5 => "Increase Opinion by 20",
            6 => "Complete 10 Levels",
            7 => "Collect 5 Items",
            8 => "Play 10 MiniGames",
            9 => "Spend 2000 Magic",
            10 => "Spend 3000 Gold",
            _ => "Unknown Quest"
        };
    }

    public static int GetRequiredAmount(int questNumber)
    {
        return questNumber switch
        {
            1 => REQUIRED_BUILDING_UPGRADED,
            2 => REQUIRED_CASTLE_UPGRADED,
            3 => REQUIRED_GOLD_EARN,
            4 => REQUIRED_MAGIC_INCREASE,
            5 => REQUIRED_OPINION_INCREASE,
            6 => REQUIRED_LEVELS_COMPLETED,
            7 => REQUIRED_ITEMS_COLLECTED,
            8 => REQUIRED_MINIGAMES_PLAYED,
            9 => SPEND_MAGIC_SPENT,
            10 => REQUIRED_MORE_GOLD_SPENT,
            _ => 0
        };
    }
    public static string GetQuestKey(int questNumber)
    {
        return questNumber switch
        {
            1 => UPGRADE_BUILDING_KEY,
            2 => UPGRADE_CASTLE_KEY,
            3 => EARN_GOLD_KEY,
            4 => INCREASE_MAGIC_KEY,
            5 => INCREASE_OPINION_KEY,
            6 => COMPLETE_LEVEL_KEY,
            7 => COLLECT_ITEM_KEY,
            8 => PLAY_MINIGAMES_KEY,
            9 => SPEND_MAGIC_KEY,
            10 => SPEND_MORE_GOLD_KEY,
            _ => "Unknown", // Return 0 for invalid quest numbers to prevent errors
        };
    }
    #endregion

    #region Quest 1(Upgrade 2 Buildings)
// Check if quest 1 is completed
    public static bool IsQuest1Completed()
    {
        return PlayerPrefs.GetInt(QUEST1_COMPLETED_KEY, 0) == 1;
    }
    public static int GetCompletedLevelsToday()
    {
        int levelsCompleted = PlayerPrefs.GetInt(UPGRADE_BUILDING_KEY, 0);
        return levelsCompleted;
    }

    // Update levels completed progress
    public static void UpdateBuildingUpgrade()
    {
        if (IsQuest1Completed()) return;

        int buildingsUpgrade = PlayerPrefs.GetInt(UPGRADE_BUILDING_KEY, 0) + 1;
        PlayerPrefs.SetInt(UPGRADE_BUILDING_KEY, buildingsUpgrade);

        if (buildingsUpgrade >= REQUIRED_BUILDING_UPGRADED)
        {
            WeeklyRewards.instance.IncrementValue(50);
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
        int currentCoins = Config.Gold;
        Config.Gold = Config.Gold+REWARD_COINS_QUEST1;
        Debug.Log($"Quest 1 completed! Player rewarded with {REWARD_COINS_QUEST1} coins.");
        UpdateCompletedQuests();
        PlayerPrefs.Save();
    }

    // Reset quest 1
    private static void ResetQuest1()
    {
        PlayerPrefs.SetInt(UPGRADE_BUILDING_KEY,0);
        
        PlayerPrefs.SetInt(QUEST1_COMPLETED_KEY, 0); // Reset completion status
        PlayerPrefs.Save();
    }



    #endregion

    #region  Quest 2(Upgrade Castle by 1 level)
    // Check if quest 2 is completed
    public static bool IsQuest2Completed()
    {
        return PlayerPrefs.GetInt(QUEST2_COMPLETED_KEY, 0) == 1;
    }

    // Update tiles matched progress
    public static void UpdateCastleUpgrade()
    {
        if (IsQuest2Completed()) return;

        int CastleUpgrade = PlayerPrefs.GetInt(UPGRADE_CASTLE_KEY, 0) + 1;
        PlayerPrefs.SetInt(UPGRADE_CASTLE_KEY, CastleUpgrade);

        if (CastleUpgrade >= REQUIRED_CASTLE_UPGRADED)
        {
            WeeklyRewards.instance.IncrementValue(50);
            RewardQuest2();
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
        int currentCoins = Config.Food;
        Config.Food = Config.Food+REWARD_COINS_QUEST2;
        Debug.Log($"Quest 2 completed! Player rewarded with {REWARD_COINS_QUEST2} coins.");
        UpdateCompletedQuests();
        PlayerPrefs.Save();
    }

    // Reset quest 2
    private static void ResetQuest2()
    {
        PlayerPrefs.SetInt(UPGRADE_CASTLE_KEY, 0);
        PlayerPrefs.SetInt(QUEST2_COMPLETED_KEY, 0); // Reset completion status
        PlayerPrefs.Save();
    }

    #endregion

    #region  Quest 3(Earn 3000 gold)
    public static bool IsQuest3Completed()
    {
        return PlayerPrefs.GetInt(QUEST3_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateGoldEarned(int goldEarned)
    {
        if (IsQuest3Completed()) return;

        int totalGold = PlayerPrefs.GetInt(EARN_GOLD_KEY, 0) + goldEarned;
        PlayerPrefs.SetInt(EARN_GOLD_KEY, totalGold);

        if (totalGold >= REQUIRED_GOLD_EARN)
        {
            WeeklyRewards.instance.IncrementValue(50);
            RewardQuest3();
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest3()
    {
        PlayerPrefs.SetInt(QUEST3_COMPLETED_KEY, 1);
        int currentCoins = Config.Food;
        Config.Food = Config.Food+REWARD_COINS_QUEST3;
        Debug.Log($"Quest 3 completed! Player rewarded with {REWARD_COINS_QUEST3} coins.");
        UpdateCompletedQuests();
        PlayerPrefs.Save();
    }

    private static void ResetQuest3()
    {
        PlayerPrefs.SetInt(EARN_GOLD_KEY, 0);
        PlayerPrefs.SetInt(QUEST3_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }
    #endregion


    #region Quest 4 (Increase Magic by 2000)

    public static bool IsQuest4Completed()
    {
        return PlayerPrefs.GetInt(QUEST4_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateMagicIncreased(int magicIncreased)
    {
        if (IsQuest4Completed() || !IsQuestActive(4)) return;

        int totalMagic = PlayerPrefs.GetInt(INCREASE_MAGIC_KEY, 0) + magicIncreased;
        PlayerPrefs.SetInt(INCREASE_MAGIC_KEY, totalMagic);

        if (totalMagic >= REQUIRED_MAGIC_INCREASE)
        {
            WeeklyRewards.instance.IncrementValue(50);
            RewardQuest4();
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest4()
    {
        PlayerPrefs.SetInt(QUEST4_COMPLETED_KEY, 1);
        int currentCoins = Config.Food;
        Config.Food = Config.Food+REWARD_COINS_QUEST4;
        UpdateCompletedQuests();
        Debug.Log($"Quest 4 completed! Player rewarded with {REWARD_COINS_QUEST4} coins.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest4()
    {
        PlayerPrefs.SetInt(INCREASE_MAGIC_KEY, 0);
        PlayerPrefs.SetInt(QUEST4_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 5 (Increase Opinion by 20)

    public static bool IsQuest5Completed()
    {
        return PlayerPrefs.GetInt(QUEST5_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateOpinionIncreased(int opinionIncreased)
    {
        if (IsQuest5Completed() || !IsQuestActive(5)) return;

        int totalOpinion = PlayerPrefs.GetInt(INCREASE_OPINION_KEY, 0) + opinionIncreased;
        PlayerPrefs.SetInt(INCREASE_OPINION_KEY, totalOpinion);

        if (totalOpinion >= REQUIRED_OPINION_INCREASE)
        {
            WeeklyRewards.instance.IncrementValue(50);
            RewardQuest5();
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest5()
    {
        PlayerPrefs.SetInt(QUEST5_COMPLETED_KEY, 1);
        int currentCoins = Config.Gold;
        Config.Gold = Config.Gold+REWARD_COINS_QUEST5;
        UpdateCompletedQuests();
        Debug.Log($"Quest 5 completed! Player rewarded with {REWARD_COINS_QUEST5} coins.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest5()
    {
        PlayerPrefs.SetInt(INCREASE_OPINION_KEY, 0);
        PlayerPrefs.SetInt(QUEST5_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 6 (Complete 10 Levels)

    public static bool IsQuest6Completed()
    {
        return PlayerPrefs.GetInt(QUEST6_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateLevelsCompleted()
    {
        if (IsQuest6Completed() || !IsQuestActive(6)) return;

        int levelsCompleted = PlayerPrefs.GetInt(COMPLETE_LEVEL_KEY, 0) + 1;
        PlayerPrefs.SetInt(COMPLETE_LEVEL_KEY, levelsCompleted);

        if (levelsCompleted >= REQUIRED_LEVELS_COMPLETED)
        {
            WeeklyRewards.instance.IncrementValue(50);
            RewardQuest6();
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
        UpdateCompletedQuests();
        Debug.Log($"Quest 6 completed! Player rewarded with {REWARD_COINS_QUEST6} coins.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest6()
    {
        PlayerPrefs.SetInt(COMPLETE_LEVEL_KEY, 0);
        PlayerPrefs.SetInt(QUEST6_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 7 (Collect 5 Items)

    public static bool IsQuest7Completed()
    {
        return PlayerPrefs.GetInt(QUEST7_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateItemsCollected()
    {
        if (IsQuest7Completed() || !IsQuestActive(7)) return;

        int itemsCollected = PlayerPrefs.GetInt(COLLECT_ITEM_KEY, 0) + 1;
        PlayerPrefs.SetInt(COLLECT_ITEM_KEY, itemsCollected);

        if (itemsCollected >= REQUIRED_ITEMS_COLLECTED)
        {
            WeeklyRewards.instance.IncrementValue(50);
            RewardQuest7();
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest7()
    {
        PlayerPrefs.SetInt(QUEST7_COMPLETED_KEY, 1);
        int currentCoins = Config.Food;
        Config.Food = Config.Food+REWARD_COINS_QUEST7;
        UpdateCompletedQuests();
        Debug.Log($"Quest 7 completed! Player rewarded with {REWARD_COINS_QUEST7} coins.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest7()
    {
        PlayerPrefs.SetInt(COLLECT_ITEM_KEY, 0);
        PlayerPrefs.SetInt(QUEST7_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 8 (Play 10 MiniGames)

    public static bool IsQuest8Completed()
    {
        return PlayerPrefs.GetInt(QUEST8_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateMiniGamesPlayed()
    {
        if (IsQuest8Completed() || !IsQuestActive(8)) return;

        int miniGamesPlayed = PlayerPrefs.GetInt(PLAY_MINIGAMES_KEY, 0) + 1;
        PlayerPrefs.SetInt(PLAY_MINIGAMES_KEY, miniGamesPlayed);

        if (miniGamesPlayed >= REQUIRED_MINIGAMES_PLAYED)
        {
            WeeklyRewards.instance.IncrementValue(50);
            RewardQuest8();
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest8()
    {
        PlayerPrefs.SetInt(QUEST8_COMPLETED_KEY, 1);
        int currentCoins = Config.Food;
        Config.Food = Config.Food+REWARD_COINS_QUEST8;
        UpdateCompletedQuests();
        Debug.Log($"Quest 8 completed! Player rewarded with {REWARD_COINS_QUEST8} coins.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest8()
    {
        PlayerPrefs.SetInt(PLAY_MINIGAMES_KEY, 0);
        PlayerPrefs.SetInt(QUEST8_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 9 (Spend 2000 Magic)

    public static bool IsQuest9Completed()
    {
        return PlayerPrefs.GetInt(QUEST9_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateMagicSpent(int goldSpent)
    {
        if (IsQuest9Completed() || !IsQuestActive(9)) return;

        int totalGoldSpent = PlayerPrefs.GetInt(SPEND_MAGIC_KEY, 0) + goldSpent;
        PlayerPrefs.SetInt(SPEND_MAGIC_KEY, totalGoldSpent);

        if (totalGoldSpent >= SPEND_MAGIC_SPENT)
        {
            WeeklyRewards.instance.IncrementValue(50);
            RewardQuest9();
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
        Config.Gold = Config.Food+REWARD_COINS_QUEST9;
        UpdateCompletedQuests();
        Debug.Log($"Quest 9 completed! Player rewarded with {REWARD_COINS_QUEST9} coins.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest9()
    {
        PlayerPrefs.SetInt(SPEND_MAGIC_KEY, 0);
        PlayerPrefs.SetInt(QUEST9_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region Quest 10 (Spend 3000 Gold)

    public static bool IsQuest10Completed()
    {
        return PlayerPrefs.GetInt(QUEST10_COMPLETED_KEY, 0) == 1;
    }

    public static void UpdateMoreGoldSpent(int goldSpent)
    {
        if (IsQuest10Completed() || !IsQuestActive(10)) return;

        int totalGoldSpent = PlayerPrefs.GetInt(SPEND_MORE_GOLD_KEY, 0) + goldSpent;
        PlayerPrefs.SetInt(SPEND_MORE_GOLD_KEY, totalGoldSpent);

        if (totalGoldSpent >= REQUIRED_MORE_GOLD_SPENT)
        {
            WeeklyRewards.instance.IncrementValue(50);
            RewardQuest10();
        }
        else
        {
            PlayerPrefs.Save();
        }
    }

    private static void RewardQuest10()
    {
        PlayerPrefs.SetInt(QUEST10_COMPLETED_KEY, 1);
        int currentCoins = Config.Food;
        Config.Food = Config.Food+REWARD_COINS_QUEST10;
        UpdateCompletedQuests();
        Debug.Log($"Quest 10 completed! Player rewarded with {REWARD_COINS_QUEST10} coins.");
        PlayerPrefs.Save();
    }

    private static void ResetQuest10()
    {
        PlayerPrefs.SetInt(SPEND_MORE_GOLD_KEY, 0);
        PlayerPrefs.SetInt(QUEST10_COMPLETED_KEY, 0);
        PlayerPrefs.Save();
    }
    #endregion

    #region Timer and Reset Management


    private static void SaveWeeklyResetTime(DateTime resetTime)
    {
        PlayerPrefs.SetString(WEEKLY_RESET_TIME_PREF_KEY, resetTime.ToString());
        PlayerPrefs.Save();
    }

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
        PlayerPrefs.SetInt(WEEKLY_QUEST_COMPLETED, 0);
        completedQuest = 0;
        foreach (int milestone in REWARD_MILESTONES)
        {
            PlayerPrefs.DeleteKey(QUEST_REWARD_CLAIMED_KEY + milestone);
        }
        SelectRandomQuests();
        Debug.Log("All weekly quests have been reset and new quests selected.");
    }
    #endregion

    

    #region Quest Milestone Completion Rewards

    private static void UpdateCompletedQuests()
    {
        int completedCount = PlayerPrefs.GetInt(WEEKLY_QUEST_COMPLETED, 0);
        completedCount++;
        completedQuest = completedCount;
        PlayerPrefs.SetInt(WEEKLY_QUEST_COMPLETED, completedCount);
        PlayerPrefs.Save();
        CheckMilestoneRewards(completedCount);
    }

    private static void CheckMilestoneRewards(int completedCount)
    {
        for (int i = 0; i < REWARD_MILESTONES.Length; i++)
        {
            if (completedCount == REWARD_MILESTONES[i] && !PlayerPrefs.HasKey(QUEST_REWARD_CLAIMED_KEY + REWARD_MILESTONES[i]))
            {
                GrantMilestoneReward(REWARD_MILESTONES[i], MILESTONE_REWARDS[i]);
            }
        }
    }

    private static void GrantMilestoneReward(int milestone, int coins)
    {
        PlayerPrefs.SetInt(QUEST_REWARD_CLAIMED_KEY + milestone, 1);
        int currentCoins = Config.GetCoin();
        Config.SetCoin(currentCoins + coins);
        Debug.Log($"Milestone reward granted for completing {milestone} weekly quest(s): {coins} coins.");
        PlayerPrefs.Save();
    }

    public static (int current, int total) GetQuestCompletionProgress()
    {
        int completedCount = PlayerPrefs.GetInt(WEEKLY_QUEST_COMPLETED, 0);
        return (Mathf.Min(completedCount, numOfQuest), numOfQuest);
    }

    private void UpdateProgressSlider()
    {
        if (questProgressSlider != null)
        {
            var (current, total) = GetQuestCompletionProgress();
            questProgressSlider.maxValue = total;
            questProgressSlider.value = current;
        }
    }

    #endregion
}
