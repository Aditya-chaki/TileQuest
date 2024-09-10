using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MetricsDisplay : MonoBehaviour
{
    public Slider foodSlider;
    public TextMeshProUGUI foodText;
    public Slider strengthSlider;
    public TextMeshProUGUI strengthText;
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public Slider goldSlider;
    public TextMeshProUGUI goldText;

    void Start()
    {
        // Initialize sliders and texts
        UpdateSliders();
        InitializeSliders();

    }

    void Update()
    {
        // Continuously update sliders and texts in case the metrics change during runtime
        UpdateSliders();
    }
    void InitializeSliders()
    {
        // Set the max values for sliders based on Config
        foodSlider.maxValue = Config.GetMaxFood();
        strengthSlider.maxValue = Config.GetMaxStrength();
        healthSlider.maxValue = Config.GetMaxHealth();
        goldSlider.maxValue = Config.GetMaxGold();
    }

    void UpdateSliders()
    {
        // Update Food
        foodSlider.value = Config.Food;
        foodText.text = "Food";

        // Update Strength
        strengthSlider.value = Config.Strength;
        strengthText.text = "Strength";

        // Update Health
        healthSlider.value = Config.Health;
        healthText.text = "Health";

        // Update Gold
        goldSlider.value = Config.Gold;
        goldText.text = "Gold";
    }
}
