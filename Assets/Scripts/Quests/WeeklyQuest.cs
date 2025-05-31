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

    private const string UPGRADE_BUILDING_KEY = "UpgradeBuilding";
    private const int REQUIRED_BUILDING_UPGRADED = 2;
    private const int REWARD_COINS_QUEST1 = 400;

    private const string UPGRADE_CASTLE_KEY = "UpgradeCastle";
    private const int REQUIRED_CASTLE_UPGRADED = 1;
    private const int REWARD_COINS_QUEST2 = 350;

    private const string EARN_GOLD_KEY = "EarnGold";
    private const int REQUIRED_GOLD_EARN = 2;
    private const int REWARD_COINS_QUEST3 = 450;

    private const string WEEKLY_RESET_TIME_PREF_KEY = "WeeklyQuestResetTime";
    private const string ACTIVE_QUESTS_KEY = "WeeklyActiveQuests";

    private const string WEEKLY_QUETS_COMPLETED = "WeeklyQuestComplete";
    private const string QUEST_REWARD_CLAIMED_KEY = "WeeklyQuestRewardClaimed_"; // Suffix with milestone (1, 2, 3)
    private static int numOfQuest = 3;
    private static int completedQuest;
    private static readonly int[] REWARD_MILESTONES = { 1, 2, 3 }; // Quests completed for rewards
    private static readonly int[] MILESTONE_REWARDS = { 200, 400, 600 }; // Coins for each milestone
    public Slider questProgressSlider;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        completedQuest = PlayerPrefs.GetInt(WEEKLY_QUETS_COMPLETED,0);
    }

    // Update is called once per frame
    void Update()
    {
     questProgressSlider.value = completedQuest/numOfQuest;   
    }

    public static string GetQuestName(int questNumber)
    {
        return questNumber switch
        {
            1 => "Upgrade 2 Buildings",
            2 => "Upgrade Castle by 1 Level",
            3 => "Earn 3000 Gold",
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
            _ => "unknown" // Return 0 for invalid quest numbers to prevent errors
        };
    }

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
        int currentCoins = Config.GetCoin();
        Config.SetCoin(currentCoins + REWARD_COINS_QUEST1);
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
        int currentCoins = Config.GetCoin();
        Config.SetCoin(currentCoins + REWARD_COINS_QUEST2);
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
        int currentCoins = Config.GetCoin();
        Config.SetCoin(currentCoins + REWARD_COINS_QUEST3);
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
        Debug.Log("All weekly quests have been reset and new quests selected.");
    }
    #endregion

    

    #region Quest Milestone Completion Rewards

    private static void UpdateCompletedQuests()
    {
        int completedCount = PlayerPrefs.GetInt(WEEKLY_QUETS_COMPLETED, 0);
        completedCount++;
        completedQuest = completedCount;
        PlayerPrefs.SetInt(WEEKLY_QUETS_COMPLETED, completedCount);
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
        int completedCount = PlayerPrefs.GetInt(WEEKLY_QUETS_COMPLETED, 0);
        return (Mathf.Min(completedCount, numOfQuest), numOfQuest);
    }

    #endregion
}
