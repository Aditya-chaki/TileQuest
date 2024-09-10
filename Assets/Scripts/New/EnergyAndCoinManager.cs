using UnityEngine;
using UnityEngine.UI;

public class EnergyAndCoinManager : MonoBehaviour
{
    public Button gainEnergyButton;
    public Button AdButton;
    public GameObject EnergyShop;
    private int energyGain = 20;
    private int coinDeduction = 100;

    void Start()
    {


        // Add a listener for the button click event
        gainEnergyButton.onClick.AddListener(OnGainEnergyClick);
        AdButton.onClick.AddListener(OnAdsGainEnergyClick);
    }
    void Update()
    {
        if (Config.Energy <= 0)
        {
            EnergyShop.SetActive(true);
        }
        else
        {
            EnergyShop.SetActive(false);
        }
    }

    // Method called when the button is clicked
    void OnGainEnergyClick()
    {
        int currentCoins = Config.GetCoin();
        int currentEnergy = Config.Energy;

        // Check if the player has enough coins
        if (currentCoins >= coinDeduction)
        {
            // Deduct coins and add energy
            Config.SetCoin(currentCoins - coinDeduction);
            Config.Energy = currentEnergy + energyGain;

            Debug.Log("Energy gained: " + energyGain + ", Coins deducted: " + coinDeduction);
        }
        else
        {
            Debug.Log("Not enough coins to gain energy.");
        }
    }
    void OnAdsGainEnergyClick()
    {

        int currentEnergy = Config.Energy;
        Config.Energy = currentEnergy + energyGain;


    }

}
