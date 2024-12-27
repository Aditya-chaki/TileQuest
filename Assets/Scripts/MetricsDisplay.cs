using UnityEngine;
using TMPro;

public class MetricsDisplay : MonoBehaviour
{
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI energyText; // Added for energy display

    void Start()
    {
        // Initialize texts
        UpdateMetrics();
    }

    void Update()
    {
        // Continuously update texts in case the metrics change during runtime
        UpdateMetrics();
    }

    void UpdateMetrics()
    {
        // Update Food
        foodText.text = $"Food: {Config.Food}";

        // Update Strength
        strengthText.text = $"Strength: {Config.Strength}";

        // Update Health
        healthText.text = $"Health: {Config.Health}";

        // Update Gold
        goldText.text = $"Gold: {Config.Gold}";

        // Update Energy
        energyText.text = $"Energy: {Config.Energy}"; // New energy text
    }
}
