using UnityEngine;
using TMPro;

public class MetricsDisplay : MonoBehaviour
{
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI energyText;

    private int lastFood, lastStrength, lastHealth, lastGold, lastEnergy;

    void Start()
    {
        // Initialize texts and previous values
        lastFood = Config.Food;
        lastStrength = Config.Strength;
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
        UpdateMetric(foodText, Config.Food, ref lastFood, "Food");

        // Update Strength
        UpdateMetric(strengthText, Config.Strength, ref lastStrength, "Strength");

        // Update Health
        UpdateMetric(healthText, Config.Health, ref lastHealth, "Health");

        // Update Gold
        UpdateMetric(goldText, Config.Gold, ref lastGold, "Gold");

        // Update Energy
        UpdateMetric(energyText, Config.Energy, ref lastEnergy, "Energy");
    }

    void UpdateMetric(TextMeshProUGUI textElement, int currentValue, ref int lastValue, string metricName)
    {
        // Update the text
        textElement.text = $"{metricName}: {currentValue}";

        // Update the last value
        lastValue = currentValue;
    }
}
