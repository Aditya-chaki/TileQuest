
// using UnityEngine;
// using TMPro;
// using System.Collections.Generic;

// public class MetricsDisplay : MonoBehaviour
// {
//     public TextMeshProUGUI influenceText;
//     public TextMeshProUGUI magicText;
//     public TextMeshProUGUI goldText;
//     public TextMeshProUGUI opinionText;

//     //[Header("Decay Timer (Optional)")]
//     //public TextMeshProUGUI decayTimerText; // Show remaining decay time

//     private int lastInfluence, lastMagic, lastGold;
//     private float lastOpinion;

//     void Start()
//     {
//         // Initialize texts and previous values
//         lastInfluence = Config.Influence;
//         lastMagic = Config.Magic;
//         lastGold = Config.Gold;
//         lastOpinion = GetAverageOpinion();

//         UpdateMetrics();
//     }

//     // void Update()
//     // {
//     //     UpdateMetrics();

//     //     if (decayTimerText != null && MetricsDecay.Instance != null)
//     //     {
//     //         decayTimerText.text = $"Decay ends in: {MetricsDecay.Instance.GetFormattedRemainingTime()}";
//     //     }
//     // }

//     void UpdateMetrics()
//     {
//         UpdateMetric(influenceText, Config.Influence, ref lastInfluence);
//         UpdateMetric(magicText, Config.Magic, ref lastMagic);
//         UpdateMetric(goldText, Config.Gold, ref lastGold);
//         UpdateOpinionMetric(opinionText);
//     }

//     void UpdateMetric(TextMeshProUGUI textElement, int currentValue, ref int lastValue)
//     {
//         if (textElement != null)
//         {
//             textElement.text = $"{currentValue}";
//             lastValue = currentValue;
//         }
//     }

//     void UpdateOpinionMetric(TextMeshProUGUI textElement)
//     {
//         if (textElement != null)
//         {
//             float avgOpinion = GetAverageOpinion();
//             textElement.text = $"{avgOpinion:F1}";
//             lastOpinion = avgOpinion;
//         }
//     }

//     float GetAverageOpinion()
//     {
//         List<string> factions = Config.InitialFactions;
//         float total = 0f;

//         foreach (var faction in factions)
//         {
//             total += Config.GetFactionOpinion(faction);
//         }

//         return factions.Count > 0 ? total / factions.Count : 0f;
//     }
// }



using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MetricsDisplay : MonoBehaviour
{
    public TextMeshProUGUI influenceText;
    public TextMeshProUGUI magicText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI opinionText;

    [Header("Update Settings")]
    public float updateInterval = 0.5f; // Update every 0.5 seconds

    //[Header("Decay Timer (Optional)")]
    //public TextMeshProUGUI decayTimerText; // Show remaining decay time

    private int lastInfluence, lastMagic, lastGold;
    private float lastOpinion;

    void Start()
    {
        // Initialize texts and previous values
        lastInfluence = Config.Influence;
        lastMagic = Config.Magic;
        lastGold = Config.Gold;
        lastOpinion = GetAverageOpinion();

        // Update immediately on start
        UpdateMetrics();
        
        // Then update regularly using InvokeRepeating
        InvokeRepeating(nameof(UpdateMetrics), updateInterval, updateInterval);
    }

    // Alternative: If you prefer using Update instead of InvokeRepeating, uncomment this:
    // void Update()
    // {
    //     UpdateMetrics();
    //
    //     if (decayTimerText != null && MetricsDecay.Instance != null)
    //     {
    //         decayTimerText.text = $"Decay ends in: {MetricsDecay.Instance.GetFormattedRemainingTime()}";
    //     }
    // }

    void UpdateMetrics()
    {
        UpdateMetric(influenceText, Config.Influence, ref lastInfluence);
        UpdateMetric(magicText, Config.Magic, ref lastMagic);
        UpdateMetric(goldText, Config.Gold, ref lastGold);
        UpdateOpinionMetric(opinionText);
    }

    void UpdateMetric(TextMeshProUGUI textElement, int currentValue, ref int lastValue)
    {
        if (textElement != null)
        {
            textElement.text = $"{currentValue}";
            lastValue = currentValue;
        }
    }

    void UpdateOpinionMetric(TextMeshProUGUI textElement)
    {
        if (textElement != null)
        {
            float avgOpinion = GetAverageOpinion();
            textElement.text = $"{avgOpinion:F1}";
            lastOpinion = avgOpinion;
        }
    }

    float GetAverageOpinion()
    {
        List<string> factions = Config.InitialFactions;
        float total = 0f;

        foreach (var faction in factions)
        {
            total += Config.GetFactionOpinion(faction);
        }

        return factions.Count > 0 ? total / factions.Count : 0f;
    }

    // Optional: Call this method to force an immediate update
    public void ForceUpdate()
    {
        UpdateMetrics();
    }

    // Clean up when the object is destroyed
    void OnDestroy()
    {
        CancelInvoke(nameof(UpdateMetrics));
    }
}