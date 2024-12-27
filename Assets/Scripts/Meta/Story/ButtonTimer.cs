using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ButtonTimer : MonoBehaviour
{
    public Button myButton;
    public TextMeshProUGUI countdownText;
    [SerializeField] float countdown = 72.0f; // Countdown time in seconds
    private bool isCountingDown = false;
    private DateTime startTime;

    void Start()
    {
        if (PlayerPrefs.HasKey("StartTime"))
        {
            startTime = DateTime.Parse(PlayerPrefs.GetString("StartTime"));
            float elapsed = (float)(DateTime.Now - startTime).TotalSeconds;
            countdown -= elapsed;

            if (countdown > 0)
            {
                StartCountdown();
            }
            else
            {
                ResetButton();
            }
        }

        myButton.onClick.AddListener(OnButtonClicked);
    }

    void Update()
    {
        if (isCountingDown)
        {
            if (countdown > 0)
            {
                countdown -= Time.deltaTime;
                countdownText.text = " " + FormatTime((int)countdown); // Cast countdown to int for formatting
            }
            else
            {
                ResetButton();
            }
        }
      
    }

    void OnButtonClicked()
    {
        
        StartCountdown();
        countdown = 7200.0f; // Reset the countdown to its initial value
        countdownText.text = FormatTime((int)countdown); // Cast countdown to int for formatting
        startTime = DateTime.Now;
        PlayerPrefs.SetString("StartTime", startTime.ToString());
        PlayerPrefs.Save();
    }

    void StartCountdown()
    {
        isCountingDown = true;
        myButton.interactable = false; // Disable the button
    }

    void ResetButton()
    {
        isCountingDown = false;
        myButton.interactable = true; // Re-enable the button
        countdownText.text = "";
    }

    string FormatTime(int timeInSeconds) // Changed parameter type to int
    {
        int hours = timeInSeconds / 3600;
        int minutes = (timeInSeconds % 3600) / 60;
        int seconds = timeInSeconds % 60;
        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }
}
