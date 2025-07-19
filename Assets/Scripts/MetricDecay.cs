using System;
using UnityEngine;

public class MetricsDecay : MonoBehaviour
{
    public static MetricsDecay Instance { get; private set; }

    [Header("Decay Settings")]
    public int influenceDecayRate = 1;
    public int magicDecayRate = 1;
    public int goldDecayRate = 1;
    public int decayDuration = 600; // 10 minutes

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
        InvokeRepeating(nameof(ApplyDecay), 0, 1);
        
        if (enableDebugLogs)
            Debug.Log("MetricsDecay started. Initial elapsed time: " + elapsedDecayTime);
    }

    void ApplyDecay()
    {
        if (enableDebugLogs)
            Debug.Log($"ApplyDecay called. Time.timeScale: {Time.timeScale}");

        if (ShouldPauseDecay())
        {
            if (enableDebugLogs)
                Debug.Log("Decay paused by ShouldPauseDecay()");
            return;
        }

        // Check if decay duration has been reached
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
            Debug.Log($"Elapsed time: {elapsedDecayTime}/{decayDuration} seconds");
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
            {
                if (enableDebugLogs)
                    Debug.Log("Decay paused: Dialogue Box is active");
                return true;
            }
        }
        
        if (pauseDecayWhenGamePaused && Time.timeScale == 0f)
        {
            if (enableDebugLogs)
                Debug.Log("Decay paused: Game is paused (timeScale = 0)");
            return true;
        }
        
        if (enableDebugLogs)
            Debug.Log("Decay not paused, continuing...");
        return false;
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