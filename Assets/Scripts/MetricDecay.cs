using System;
using UnityEngine;

public class MetricsDecay : MonoBehaviour
{
    // Decay rates (per second) in integers
    public int foodDecayRate = 1; // Decay 1 unit per second
    public int strengthDecayRate = 1; // Decay 1 unit per second
    public int healthDecayRate = 1; // Decay 1 unit per second
    public int goldDecayRate = 1; // Decay 1 unit per second

    // Decay duration in seconds
    public int decayDuration = 600; // Example: 10 minutes

    private string lastSaveTimeKey = "LastSaveTime";
    private int elapsedDecayTime = 0;

    void Start()
    {
        ApplyOfflineDecay();
        InvokeRepeating(nameof(ApplyDecay), 0, 1); // Call ApplyDecay every second
    }

    void ApplyDecay()
    {
        if (elapsedDecayTime >= decayDuration)
        {
            CancelInvoke(nameof(ApplyDecay)); // Stop decay when duration is reached
            return;
        }

        elapsedDecayTime += 1; // Add 1 second per update

        // Decay metrics
        Config.Food = Mathf.Max(0, Config.Food - foodDecayRate);
        Config.Strength = Mathf.Max(0, Config.Strength - strengthDecayRate);
        Config.Health = Mathf.Max(0, Config.Health - healthDecayRate);
        Config.Gold = Mathf.Max(0, Config.Gold - goldDecayRate);

        // Save the last decay time
        PlayerPrefs.SetString(lastSaveTimeKey, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();
    }

    void ApplyOfflineDecay()
    {
        if (PlayerPrefs.HasKey(lastSaveTimeKey))
        {
            DateTime lastSaveTime = DateTime.Parse(PlayerPrefs.GetString(lastSaveTimeKey));
            TimeSpan timeAway = DateTime.UtcNow - lastSaveTime;

            // Calculate time away in seconds as an integer
            int timeAwaySeconds = Mathf.Min((int)timeAway.TotalSeconds, decayDuration - elapsedDecayTime);

            if (timeAwaySeconds > 0)
            {
                // Apply offline decay
                int offlineFoodDecay = foodDecayRate * timeAwaySeconds;
                int offlineStrengthDecay = strengthDecayRate * timeAwaySeconds;
                int offlineHealthDecay = healthDecayRate * timeAwaySeconds;
                int offlineGoldDecay = goldDecayRate * timeAwaySeconds;

                Config.Food = Mathf.Max(0, Config.Food - offlineFoodDecay);
                Config.Strength = Mathf.Max(0, Config.Strength - offlineStrengthDecay);
                Config.Health = Mathf.Max(0, Config.Health - offlineHealthDecay);
                Config.Gold = Mathf.Max(0, Config.Gold - offlineGoldDecay);

                elapsedDecayTime += timeAwaySeconds;
            }
        }

        // Update last save time
        PlayerPrefs.SetString(lastSaveTimeKey, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();
    }
}
