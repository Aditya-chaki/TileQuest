// using System;
// using UnityEngine;

// public class MetricsDecay : MonoBehaviour
// {
//     // Singleton instance
//     public static MetricsDecay Instance { get; private set; }

//     // Decay rates (per second) in integers
//     public int foodDecayRate = 1; // Decay 1 unit per second
//     public int strengthDecayRate = 1; // Decay 1 unit per second
//     public int healthDecayRate = 1; // Decay 1 unit per second
//     public int goldDecayRate = 1; // Decay 1 unit per second

//     // Decay duration in seconds
//     public int decayDuration = 600; // Example: 10 minutes

//     private string lastSaveTimeKey = "LastSaveTime";
//     private int elapsedDecayTime = 0;

//     void Awake()
//     {
//         // Ensure only one instance exists
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject); // Optional: Makes the instance persistent across scenes
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     void Start()
//     {
//         ApplyOfflineDecay();
//         InvokeRepeating(nameof(ApplyDecay), 0, 1); // Call ApplyDecay every second
//     }

//     void ApplyDecay()
//     {
//         if (elapsedDecayTime >= decayDuration)
//         {
//             CancelInvoke(nameof(ApplyDecay)); // Stop decay when duration is reached
//             return;
//         }

//         elapsedDecayTime += 1; // Add 1 second per update

//         // Decay metrics
//         Config.Food = Mathf.Max(0, Config.Food - foodDecayRate);
//         Config.Strength = Mathf.Max(0, Config.Strength - strengthDecayRate);
//         Config.Health = Mathf.Max(0, Config.Health - healthDecayRate);
//         Config.Gold = Mathf.Max(0, Config.Gold - goldDecayRate);

//         // Save the last decay time
//         PlayerPrefs.SetString(lastSaveTimeKey, DateTime.UtcNow.ToString());
//         PlayerPrefs.Save();
//     }

//     void ApplyOfflineDecay()
//     {
//         if (PlayerPrefs.HasKey(lastSaveTimeKey))
//         {
//             DateTime lastSaveTime = DateTime.Parse(PlayerPrefs.GetString(lastSaveTimeKey));
//             TimeSpan timeAway = DateTime.UtcNow - lastSaveTime;

//             // Calculate time away in seconds as an integer
//             int timeAwaySeconds = Mathf.Min((int)timeAway.TotalSeconds, decayDuration - elapsedDecayTime);

//             if (timeAwaySeconds > 0)
//             {
//                 // Apply offline decay
//                 int offlineFoodDecay = foodDecayRate * timeAwaySeconds;
//                 int offlineStrengthDecay = strengthDecayRate * timeAwaySeconds;
//                 int offlineHealthDecay = healthDecayRate * timeAwaySeconds;
//                 int offlineGoldDecay = goldDecayRate * timeAwaySeconds;

//                 Config.Food = Mathf.Max(0, Config.Food - offlineFoodDecay);
//                 Config.Strength = Mathf.Max(0, Config.Strength - offlineStrengthDecay);
//                 Config.Health = Mathf.Max(0, Config.Health - offlineHealthDecay);
//                 Config.Gold = Mathf.Max(0, Config.Gold - offlineGoldDecay);

//                 elapsedDecayTime += timeAwaySeconds;
//             }
//         }

//         // Update last save time
//         PlayerPrefs.SetString(lastSaveTimeKey, DateTime.UtcNow.ToString());
//         PlayerPrefs.Save();
//     }

//     // Public access methods
//     public int GetFoodDecayRate() => foodDecayRate;
//     public int GetStrengthDecayRate() => strengthDecayRate;
//     public int GetHealthDecayRate() => healthDecayRate;
//     public int GetGoldDecayRate() => goldDecayRate;
//     public int GetDecayDuration() => decayDuration;
// }


using System;
using UnityEngine;

public class MetricsDecay : MonoBehaviour
{
    // Singleton instance
    public static MetricsDecay Instance { get; private set; }

    [Header("Decay Settings")]
    // Decay rates (per second) in integers
    public int foodDecayRate = 1; // Decay 1 unit per second
    public int strengthDecayRate = 1; // Decay 1 unit per second
    public int healthDecayRate = 1; // Decay 1 unit per second
    public int goldDecayRate = 1; // Decay 1 unit per second

    // Decay duration in seconds
    public int decayDuration = 600; // Example: 10 minutes

    [Header("Minimum Values")]
    public int minFood = 0;
    public int minStrength = 0;
    public int minHealth = 10; // Keep some health so game doesn't end
    public int minGold = 0;

    [Header("Pause Conditions")]
    public bool pauseDecayDuringDialogue = true;
    public bool pauseDecayWhenGamePaused = true;

    [Header("Debug")]
    public bool enableDebugLogs = false;

    private string lastSaveTimeKey = "LastSaveTime";
    private string elapsedDecayTimeKey = "ElapsedDecayTime"; // NEW: Save elapsed time
    private int elapsedDecayTime = 0;

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load elapsed decay time from save
        elapsedDecayTime = PlayerPrefs.GetInt(elapsedDecayTimeKey, 0);
        
        ApplyOfflineDecay();
        InvokeRepeating(nameof(ApplyDecay), 0, 1); // Call ApplyDecay every second
    }

    void ApplyDecay()
    {
        // Check if decay should be paused
        if (ShouldPauseDecay())
        {
            return;
        }

        if (elapsedDecayTime >= decayDuration)
        {
            CancelInvoke(nameof(ApplyDecay)); // Stop decay when duration is reached
            if (enableDebugLogs)
            {
                Debug.Log("Decay duration reached. Stopping decay.");
            }
            return;
        }

        elapsedDecayTime += 1; // Add 1 second per update

        // Decay metrics with minimum value protection
        int oldFood = Config.Food;
        int oldStrength = Config.Strength;
        int oldHealth = Config.Health;
        int oldGold = Config.Gold;

        Config.Food = Mathf.Max(minFood, Config.Food - foodDecayRate);
        Config.Strength = Mathf.Max(minStrength, Config.Strength - strengthDecayRate);
        Config.Health = Mathf.Max(minHealth, Config.Health - healthDecayRate);
        Config.Gold = Mathf.Max(minGold, Config.Gold - goldDecayRate);

        // Debug logging
        if (enableDebugLogs)
        {
            Debug.Log($"Decay applied. Food: {oldFood}->{Config.Food}, Strength: {oldStrength}->{Config.Strength}, Health: {oldHealth}->{Config.Health}, Gold: {oldGold}->{Config.Gold}");
        }

        // Save both timestamps
        PlayerPrefs.SetString(lastSaveTimeKey, DateTime.UtcNow.ToString());
        PlayerPrefs.SetInt(elapsedDecayTimeKey, elapsedDecayTime); // NEW: Save elapsed time
        PlayerPrefs.Save();
    }

    void ApplyOfflineDecay()
    {
        try
        {
            if (PlayerPrefs.HasKey(lastSaveTimeKey))
            {
                DateTime lastSaveTime = DateTime.Parse(PlayerPrefs.GetString(lastSaveTimeKey));
                TimeSpan timeAway = DateTime.UtcNow - lastSaveTime;

                // Calculate time away in seconds as an integer
                int timeAwaySeconds = Mathf.Min((int)timeAway.TotalSeconds, decayDuration - elapsedDecayTime);

                if (timeAwaySeconds > 0)
                {
                    if (enableDebugLogs)
                    {
                        Debug.Log($"Player was away for {timeAwaySeconds} seconds. Applying offline decay.");
                    }

                    // Apply offline decay
                    int offlineFoodDecay = foodDecayRate * timeAwaySeconds;
                    int offlineStrengthDecay = strengthDecayRate * timeAwaySeconds;
                    int offlineHealthDecay = healthDecayRate * timeAwaySeconds;
                    int offlineGoldDecay = goldDecayRate * timeAwaySeconds;

                    Config.Food = Mathf.Max(minFood, Config.Food - offlineFoodDecay);
                    Config.Strength = Mathf.Max(minStrength, Config.Strength - offlineStrengthDecay);
                    Config.Health = Mathf.Max(minHealth, Config.Health - offlineHealthDecay);
                    Config.Gold = Mathf.Max(minGold, Config.Gold - offlineGoldDecay);

                    elapsedDecayTime += timeAwaySeconds;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error applying offline decay: {e.Message}. Resetting decay timer.");
            // Reset if there's an error
            elapsedDecayTime = 0;
        }

        // Update last save time
        PlayerPrefs.SetString(lastSaveTimeKey, DateTime.UtcNow.ToString());
        PlayerPrefs.SetInt(elapsedDecayTimeKey, elapsedDecayTime);
        PlayerPrefs.Save();
    }

    bool ShouldPauseDecay()
    {
        // Pause during dialogue if enabled
        if (pauseDecayDuringDialogue)
        {
            // Check if dialogue is active
            GameObject dialoguePanel = GameObject.Find("Dialogue Box");
            if (dialoguePanel != null && dialoguePanel.activeSelf)
            {
                return true;
            }
        }

        // Pause when game is paused
        if (pauseDecayWhenGamePaused && Time.timeScale == 0f)
        {
            return true;
        }

        return false;
    }

    // Public access methods
    public int GetFoodDecayRate() => foodDecayRate;
    public int GetStrengthDecayRate() => strengthDecayRate;
    public int GetHealthDecayRate() => healthDecayRate;
    public int GetGoldDecayRate() => goldDecayRate;
    public int GetDecayDuration() => decayDuration;
    public int GetElapsedDecayTime() => elapsedDecayTime;
    public int GetRemainingDecayTime() => Mathf.Max(0, decayDuration - elapsedDecayTime);
    
    // NEW: Get formatted time remaining
    public string GetFormattedRemainingTime()
    {
        int remainingSeconds = GetRemainingDecayTime();
        int minutes = remainingSeconds / 60;
        int seconds = remainingSeconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }

    // NEW: Manual control methods
    public void PauseDecay()
    {
        CancelInvoke(nameof(ApplyDecay));
        if (enableDebugLogs)
        {
            Debug.Log("Decay manually paused");
        }
    }

    public void ResumeDecay()
    {
        CancelInvoke(nameof(ApplyDecay));
        InvokeRepeating(nameof(ApplyDecay), 0, 1);
        if (enableDebugLogs)
        {
            Debug.Log("Decay manually resumed");
        }
    }

    public void ResetDecayTimer()
    {
        elapsedDecayTime = 0;
        PlayerPrefs.SetInt(elapsedDecayTimeKey, 0);
        PlayerPrefs.Save();
        if (enableDebugLogs)
        {
            Debug.Log("Decay timer reset");
        }
    }
}