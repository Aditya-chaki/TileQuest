using UnityEngine;
using TMPro;
using System.Collections;

public class MetricsDisplay : MonoBehaviour
{
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI energyText;

    public GameObject foodUpSign, foodDownSign;
    public GameObject strengthUpSign, strengthDownSign;
    public GameObject healthUpSign, healthDownSign;
    public GameObject goldUpSign, goldDownSign;
    public GameObject energyUpSign, energyDownSign;

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
        // Continuously update texts and signs
        UpdateMetrics();
    }
   

    void UpdateMetrics()
    {
        // Update Food
        UpdateMetric(foodText, foodUpSign, foodDownSign, Config.Food, ref lastFood, "Food");

        // Update Strength
        UpdateMetric(strengthText, strengthUpSign, strengthDownSign, Config.Strength, ref lastStrength, "Strength");

        // Update Health
        UpdateMetric(healthText, healthUpSign, healthDownSign, Config.Health, ref lastHealth, "Health");

        // Update Gold
        UpdateMetric(goldText, goldUpSign, goldDownSign, Config.Gold, ref lastGold, "Gold");

        // Update Energy
        UpdateMetric(energyText, energyUpSign, energyDownSign, Config.Energy, ref lastEnergy, "Energy");
    }

    void UpdateMetric(TextMeshProUGUI textElement, GameObject upSign, GameObject downSign, int currentValue, ref int lastValue, string metricName)
    {
        // Update the text
        textElement.text = $"{metricName}: {currentValue}";

        // Compare with the last value
        if (currentValue > lastValue)
        {
            StartCoroutine(ShowSignForDuration(upSign, downSign, 30f));
        }
        else if (currentValue < lastValue)
        {
            StartCoroutine(ShowSignForDuration(downSign, upSign, 30f));
        }

        // Update the last value
        lastValue = currentValue;
    }

    IEnumerator ShowSignForDuration(GameObject show, GameObject hide, float duration)
    {
        show.SetActive(true);
        hide.SetActive(false);
        yield return new WaitForSeconds(duration);
        show.SetActive(false);
    }
}
