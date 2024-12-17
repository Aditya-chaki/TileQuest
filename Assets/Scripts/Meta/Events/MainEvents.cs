using UnityEngine;
using UnityEngine.UI;

public class MainEvents : MonoBehaviour
{
    public Button payButton; // Assign your button in the inspector
    public int foodToAdd = 10;     // Default values to add
    public int strengthToAdd = 5;
    public int goldToAdd = 100;
    public int healthToAdd = 20;

    void Start()
    {
        // Attach the PayButton method to the button's onClick event
        if (payButton != null)
        {
            payButton.onClick.AddListener(PayButton);
        }
        else
        {
            Debug.LogError("Pay Button is not assigned.");
        }
    }

    void PayButton()
    {
        // Update Config metrics safely with clamped values
        Config.Food += foodToAdd;
        Config.Strength += strengthToAdd;
        Config.Gold += goldToAdd;
        Config.Health += healthToAdd;

        Debug.Log($"Updated Values - Food: {Config.Food}, Strength: {Config.Strength}, Gold: {Config.Gold}, Health: {Config.Health}");
        Destroy(this.gameObject);
    }
}
