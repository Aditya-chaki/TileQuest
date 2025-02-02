using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CollectingItems : MonoBehaviour
{
    public Button timerButton;             // Button for timer and gold collection
    public TextMeshProUGUI buttonText;     // TextMeshPro for button text
    public int goldToCollect = 100;        // Amount of gold to collect
    public float timerDuration = 10f;      // Timer duration in seconds

    public CoinAnimationController coinAnimationController; // Reference to CoinAnimationController for animations
  

    private bool isTimerActive = false;    // Flag to check if the timer is active
    private bool canCollect = false;       // Flag to check if collection is allowed
    private float timeRemaining;           // Time remaining on the timer

    private const string TimerStartKey = "TimerStartTime"; // Key for saving start time
    private const string TimerEndKey = "TimerEndTime";     // Key for saving end time

    void Start()
    {
        if (timerButton != null && buttonText != null)
        {
            timerButton.onClick.AddListener(OnButtonClick);
            CheckTimerState(); // Check saved timer state at game start
        }
        else
        {
            Debug.LogError("Button or TextMeshPro is not assigned.");
        }
    }

    void Update()
    {
        if (isTimerActive)
        {
            // Decrease remaining time
            timeRemaining -= Time.deltaTime;

            if (timeRemaining > 0)
            {
                // Update button text with the countdown
                buttonText.text = $"Time Left: {Mathf.CeilToInt(timeRemaining)}s";
            }
            else
            {
                // Timer is finished
                isTimerActive = false;
                canCollect = true;
                buttonText.text = "Tap to Collect";  // Change button text to "Tap to Collect"
            }
        }
    }

    void OnButtonClick()
    {
        if (!isTimerActive && !canCollect && buttonText.text == "")
        {
            // Start the timer
            isTimerActive = true;
            timeRemaining = timerDuration;

            // Save the start and end times
            DateTime startTime = DateTime.UtcNow;
            PlayerPrefs.SetString(TimerStartKey, startTime.ToString());
            PlayerPrefs.SetString(TimerEndKey, startTime.AddSeconds(timerDuration).ToString());
            PlayerPrefs.Save();

            buttonText.text = $"Time Left: {timerDuration}s";  // Initial countdown display
        }
        else if (canCollect)
        {
            // Tap to Collect and trigger coin animation
            CollectGold();

            // Trigger coin animation
            if (coinAnimationController != null)
            {
                coinAnimationController.AnimateCoins(); // Use AnimateCoins function
            }

            buttonText.text = "";  // Reset button text
            canCollect = false;

            // Clear saved timer data
            PlayerPrefs.DeleteKey(TimerStartKey);
            PlayerPrefs.DeleteKey(TimerEndKey);
            PlayerPrefs.Save();
        }
    }

    void CheckTimerState()
    {
        if (PlayerPrefs.HasKey(TimerEndKey))
        {
            DateTime endTime = DateTime.Parse(PlayerPrefs.GetString(TimerEndKey));
            DateTime currentTime = DateTime.UtcNow;

            if (currentTime >= endTime)
            {
                // Timer has already finished
                isTimerActive = false;
                canCollect = true;
                buttonText.text = "Tap to Collect";
            }
            else
            {
                // Timer is still active
                isTimerActive = true;
                timeRemaining = (float)(endTime - currentTime).TotalSeconds;
                buttonText.text = $"Time Left: {Mathf.CeilToInt(timeRemaining)}s";
            }
        }
        else
        {
            // No active timer, show default state
            buttonText.text = "";
            isTimerActive = false;
            canCollect = false;
        }
    }

    void CollectGold()
    {
        // Update the player's gold in Config
        Config.Gold += goldToCollect;
        Debug.Log($"Collected {goldToCollect} gold. Total Gold: {Config.Gold}");
    }
}
