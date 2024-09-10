using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyLevelBar : MonoBehaviour
{
    public Slider energySlider;
    public TextMeshProUGUI energyText;

    void Start()
    {
        
        if (energySlider == null)
        {
            Debug.LogError("Energy Slider is not assigned.");
            return;
        }
        

        // Initialize the energy bar
        UpdateEnergyBar();

        // Optionally, subscribe to an event that updates the energy bar when the energy changes
        // You need to create an event in the Config class and invoke it whenever energy changes
    }

    void Update()
    {
        // Continuously update the energy bar
        UpdateEnergyBar();
    }

    void UpdateEnergyBar()
    {
        // Get the current energy and max energy
        int currentEnergy = Config.Energy;
        int maxEnergy = Config.GetMaxEnergy();

        // Update the slider's value and max value
        energySlider.maxValue = maxEnergy;
        energySlider.value = currentEnergy;

        // Update the energy text, if applicable
        if (energyText != null)
        {
            energyText.text = $"{currentEnergy}/{maxEnergy}";
        }
    }
}
