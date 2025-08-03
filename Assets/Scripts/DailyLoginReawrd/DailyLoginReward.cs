using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DailyLoginReward : MonoBehaviour
{
    [System.Serializable]
    public struct Reward
    {
        public int day;
        public string rewardName;
        public int rewardAmount;
        public Sprite rewardIcon;
    }

    [SerializeField] private Reward[] rewards; // Array of rewards for each day
    [SerializeField] private GameObject[] completeIMG; 
    [SerializeField] private GameObject[] rewardDayContainer; 
    [SerializeField] private TMP_Text statusText; // Text to show claim status
    [SerializeField] private Color activeColor = Color.white; // Color for current day
    [SerializeField] private Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Color for inactive days
    [SerializeField] private GameObject loginRewardPanel;
    [SerializeField] private Button claimButton; // Button to claim reward
    private Image[] rewardDayImages; 
    private const string LAST_LOGIN_KEY = "LastLoginTime";
    private const string STREAK_KEY = "LoginStreak";
    private int currentStreak;
    private DateTime lastLoginTime;

  void Start()
    {
        
        // Load saved data
        LoadPlayerData();
        // Initialize UI
        InitializeRewardUI();
        // Check and update reward status
        CheckDailyReward();
    }

    void LoadPlayerData()
    {
        // Load last login time (stored as ticks)
        string lastLoginString = PlayerPrefs.GetString(LAST_LOGIN_KEY, DateTime.MinValue.Ticks.ToString());
        lastLoginTime = new DateTime(long.Parse(lastLoginString));

        // Load login streak
        currentStreak = PlayerPrefs.GetInt(STREAK_KEY, 0);
       
    }

    void InitializeRewardUI()
    {
        for(int i=0;i<rewards.Length;i++)
        {
            Debug.Log(rewards[i].rewardIcon==null);
            rewardDayContainer[i].transform.Find("RewardIcon").GetComponent<Image>().sprite = rewards[i].rewardIcon;
            rewardDayContainer[i].transform.Find("RewardTxt").GetComponent<TMP_Text>().text ="x"+rewards[i].rewardAmount.ToString();
        }

        for(int i=0;i<currentStreak;i++)
        {
            completeIMG[i].SetActive(true);
        }
        for(int i=currentStreak;i<completeIMG.Length;i++)
        {
            completeIMG[i].SetActive(false);
            rewardDayContainer[i].GetComponent<Image>().color = inactiveColor;
        }
    }

    void CheckDailyReward()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan timeSinceLastLogin = currentTime - lastLoginTime;
        // Update UI for all days
        UpdateRewardUI();

        // Check if it's a new day (24 hours have passed)
        if (timeSinceLastLogin.TotalHours >= 24)
        {
            Debug.Log("Time to collect");
            // Check if the streak should reset (more than 48 hours since last login)
            if (timeSinceLastLogin.TotalHours >= 48)
            {
                currentStreak = 0; // Reset streak
            }

            // Increment streak
            currentStreak = Mathf.Min(currentStreak + 1, rewards.Length); // Cap streak at max rewards
            UpdateRewardUI();
            claimButton.interactable = true; // Enable claim button
            //statusText.text = $"Claim your Day {currentStreak} reward!";
        }
        else
        {
            // Reward already claimed today
            //statusText.text = "Come back tomorrow for your next reward!";
            Debug.Log("login reward time:"+timeSinceLastLogin.TotalHours);
            claimButton.interactable = false;
        }
    }

    void UpdateRewardUI()
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            // Highlight current day, darken others
            rewardDayContainer[i].transform.Find("RewardIcon").GetComponent<Image>().color = (i + 1 == currentStreak) ? activeColor : inactiveColor;
        }
    }

    public void ClaimReward()
    {
        if (claimButton.interactable)
        {
            int rewardIndex = currentStreak - 1; // Array is 0-based, streak is 1-based
            if (rewardIndex >= 0 && rewardIndex < rewards.Length)
            {
                Reward reward = rewards[rewardIndex];
                // Grant the reward (e.g., add to player's inventory)
                Debug.Log($"Granted {reward.rewardName} x{reward.rewardAmount}");
                if(reward.rewardName=="Gold")
                {
                    Config.Gold = Config.Gold+reward.rewardAmount;
                }
                else if(reward.rewardName=="Magic")
                {
                     Config.Magic = Config.Magic+reward.rewardAmount;
                }
                else if(reward.rewardName=="Influence")
                {
                     Config.Influence = Config.Influence+reward.rewardAmount;
                }

                // Update last login time and streak
                lastLoginTime = DateTime.Now;
                PlayerPrefs.SetString(LAST_LOGIN_KEY, lastLoginTime.Ticks.ToString());
                PlayerPrefs.SetInt(STREAK_KEY, currentStreak);
                PlayerPrefs.Save();

                // Update UI and disable claim button
                claimButton.interactable = false;
                //statusText.text = "Come back tomorrow for your next reward!";
                UpdateRewardUI();
                completeIMG[currentStreak-1].SetActive(true);
            }
        }
    }

    public void CloseButton()
    {
        loginRewardPanel.SetActive(false);
    }

    public void ShowLoginPanel()
    {
        loginRewardPanel.SetActive(true);
    }
}