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
    public static MetricsDecay Instance { get; private set; }

    [Header("Decay Settings")]
    // Decay rates (per second) in integers
    public int influenceDecayRate = 1; // Decay 1 unit per second
    public int magicDecayRate = 1;     // Decay 1 unit per second
    public int goldDecayRate = 1;      // Decay 1 unit per second

    // Decay duration in seconds
    public int decayDuration = 600; // Example: 10 minutes

    [Header("Minimum Values")]
    public int minInfluence = 0;
    public int minMagic = 0;
    public int minGold = 0;

    [Header("Pause Conditions")]
    public bool pauseDecayDuringDialogue = true;
    public bool pauseDecayWhenGamePaused = true;

    [Header("Debug")]
    public bool enableDebugLogs = false;

    private string lastSaveTimeKey = "LastSaveTime";
    private string elapsedDecayTimeKey = "ElapsedDecayTime";
    private int elapsedDecayTime = 0;

    void Awake()
    {
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
        elapsedDecayTime = PlayerPrefs.GetInt(elapsedDecayTimeKey, 0);
        ApplyOfflineDecay();
        InvokeRepeating(nameof(ApplyDecay), 0, 1); // Call ApplyDecay every second
    }

    void ApplyDecay()
    {
        if (ShouldPauseDecay())
            return;

        if (elapsedDecayTime >= decayDuration)
        {
            CancelInvoke(nameof(ApplyDecay));
            if (enableDebugLogs)
                Debug.Log("Decay duration reached. Stopping decay.");
            return;
        }

        elapsedDecayTime += 1;

        int oldInfluence = Config.Influence;
        int oldMagic = Config.Magic;
        int oldGold = Config.Gold;

        Config.Influence = Mathf.Max(minInfluence, Config.Influence - influenceDecayRate);
        Config.Magic = Mathf.Max(minMagic, Config.Magic - magicDecayRate);
        Config.Gold = Mathf.Max(minGold, Config.Gold - goldDecayRate);

        if (enableDebugLogs)
        {
            Debug.Log($"Decay applied. Influence: {oldInfluence}->{Config.Influence}, Magic: {oldMagic}->{Config.Magic}, Gold: {oldGold}->{Config.Gold}");
        }

        PlayerPrefs.SetString(lastSaveTimeKey, DateTime.UtcNow.ToString());
        PlayerPrefs.SetInt(elapsedDecayTimeKey, elapsedDecayTime);
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

                int timeAwaySeconds = Mathf.Min((int)timeAway.TotalSeconds, decayDuration - elapsedDecayTime);

                if (timeAwaySeconds > 0)
                {
                    if (enableDebugLogs)
                        Debug.Log($"Player was away for {timeAwaySeconds} seconds. Applying offline decay.");

                    int offlineInfluenceDecay = influenceDecayRate * timeAwaySeconds;
                    int offlineMagicDecay = magicDecayRate * timeAwaySeconds;
                    int offlineGoldDecay = goldDecayRate * timeAwaySeconds;

                    Config.Influence = Mathf.Max(minInfluence, Config.Influence - offlineInfluenceDecay);
                    Config.Magic = Mathf.Max(minMagic, Config.Magic - offlineMagicDecay);
                    Config.Gold = Mathf.Max(minGold, Config.Gold - offlineGoldDecay);

                    elapsedDecayTime += timeAwaySeconds;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error applying offline decay: {e.Message}. Resetting decay timer.");
            elapsedDecayTime = 0;
        }

        PlayerPrefs.SetString(lastSaveTimeKey, DateTime.UtcNow.ToString());
        PlayerPrefs.SetInt(elapsedDecayTimeKey, elapsedDecayTime);
        PlayerPrefs.Save();
    }

    bool ShouldPauseDecay()
    {
        if (pauseDecayDuringDialogue)
        {
            GameObject dialoguePanel = GameObject.Find("Dialogue Box");
            if (dialoguePanel != null && dialoguePanel.activeSelf)
                return true;
        }
        if (pauseDecayWhenGamePaused && Time.timeScale == 0f)
            return true;
        return false;
    }

    // Public access methods
    public int GetInfluenceDecayRate() => influenceDecayRate;
    public int GetMagicDecayRate() => magicDecayRate;
    public int GetGoldDecayRate() => goldDecayRate;
    public int GetDecayDuration() => decayDuration;
    public int GetElapsedDecayTime() => elapsedDecayTime;
    public int GetRemainingDecayTime() => Mathf.Max(0, decayDuration - elapsedDecayTime);

    public string GetFormattedRemainingTime()
    {
        int remainingSeconds = GetRemainingDecayTime();
        int minutes = remainingSeconds / 60;
        int seconds = remainingSeconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }

    public void PauseDecay()
    {
        CancelInvoke(nameof(ApplyDecay));
        if (enableDebugLogs)
            Debug.Log("Decay manually paused");
    }

    public void ResumeDecay()
    {
        CancelInvoke(nameof(ApplyDecay));
        InvokeRepeating(nameof(ApplyDecay), 0, 1);
        if (enableDebugLogs)
            Debug.Log("Decay manually resumed");
    }

    public void ResetDecayTimer()
    {
        elapsedDecayTime = 0;
        PlayerPrefs.SetInt(elapsedDecayTimeKey, 0);
        PlayerPrefs.Save();
        if (enableDebugLogs)
            Debug.Log("Decay timer reset");
    }
}