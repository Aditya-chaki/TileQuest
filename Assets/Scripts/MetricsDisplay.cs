// using UnityEngine;
// using TMPro;

// public class MetricsDisplay : MonoBehaviour
// {
//     public TextMeshProUGUI foodText;
//     //public TextMeshProUGUI strengthText;
//     public TextMeshProUGUI healthText;
//     public TextMeshProUGUI goldText;
//     public TextMeshProUGUI energyText;

//     private int lastFood, lastStrength, lastHealth, lastGold, lastEnergy;

//     void Start()
//     {
//         // Initialize texts and previous values
//         lastFood = Config.Food;
//         lastStrength = Config.Strength;
//         lastHealth = Config.Health;
//         lastGold = Config.Gold;
//         lastEnergy = Config.Energy;

//         UpdateMetrics();
//     }

//     void Update()
//     {
//         // Continuously update texts
//         UpdateMetrics();
//     }

//     void UpdateMetrics()
//     {
//         // Update Food
//         UpdateMetric(foodText, Config.Food, ref lastFood, "Food");

//         // Update Strength
//         //UpdateMetric(strengthText, Config.Strength, ref lastStrength, "Strength");

//         // Update Health
//         UpdateMetric(healthText, Config.Health, ref lastHealth, "Health");

//         // Update Gold
//         UpdateMetric(goldText, Config.Gold, ref lastGold, "Gold");

//         // Update Energy
//         UpdateMetric(energyText, Config.Energy, ref lastEnergy, "Energy");
//     }

//     void UpdateMetric(TextMeshProUGUI textElement, int currentValue, ref int lastValue, string metricName)
//     {
//         // Update the text
//         textElement.text = $"{currentValue}";

//         // Update the last value
//         lastValue = currentValue;
//     }
// }

using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MetricsDisplay : MonoBehaviour
{
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI opinionText;  // Replacing Strength with Opinion
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI energyText;

    private int lastFood, lastHealth, lastGold, lastEnergy;
    private float lastOpinion;

    void Start()
    {
        // Initialize texts and previous values
        lastFood = Config.Food;
        lastOpinion = GetAverageOpinion();
        lastHealth = Config.Health;
        lastGold = Config.Gold;
        lastEnergy = Config.Energy;

        UpdateMetrics();
    }

    void Update()
    {
        // Continuously update texts
        UpdateMetrics();
    }

    void UpdateMetrics()
    {
        // Update Food
        UpdateMetric(foodText, Config.Food, ref lastFood);

        // Update Opinion
        UpdateOpinionMetric(opinionText);

        // Update Health
        UpdateMetric(healthText, Config.Health, ref lastHealth);

        // Update Gold
        UpdateMetric(goldText, Config.Gold, ref lastGold);

        // Update Energy
        UpdateMetric(energyText, Config.Energy, ref lastEnergy);
    }

    void UpdateMetric(TextMeshProUGUI textElement, int currentValue, ref int lastValue)
    {
        textElement.text = $"{currentValue}";
        lastValue = currentValue;
    }

    void UpdateOpinionMetric(TextMeshProUGUI textElement)
    {
        float avgOpinion = GetAverageOpinion();
        textElement.text = $"{avgOpinion:F1}";
        lastOpinion = avgOpinion;
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
}
