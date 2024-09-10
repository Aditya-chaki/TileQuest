using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class ButtonTimer : MonoBehaviour
{
    public Button myButton;
    public TextMeshProUGUI countdownText;
    [SerializeField] float countdown = 72.0f; // Countdown time in seconds
    private bool isCountingDown = false;
    private DateTime startTime;
    private string characterId;

    void Start()
    {
        // Assuming Character component is on the same GameObject
        Character character = GetComponent<Character>();
        if (character != null)
        {
            characterId = character.GetCharacterId();
        }

        myButton.onClick.AddListener(OnButtonClicked);

        if (!string.IsNullOrEmpty(characterId) && PlayerPrefs.HasKey("StartTime_" + characterId))
        {
            startTime = DateTime.Parse(PlayerPrefs.GetString("StartTime_" + characterId));
            float elapsed = (float)(DateTime.Now - startTime).TotalSeconds;
            countdown -= elapsed;

            if (countdown > 0)
            {
                isCountingDown = true;
                myButton.interactable = false;
                countdownText.text = " " + FormatTime(countdown);
            }
            else
            {
                countdown = 0;
                myButton.interactable = true;
                countdownText.text = "";
            }
        }
    }

    void Update()
    {
        if (isCountingDown)
        {
            if (countdown > 0)
            {
                countdown -= Time.deltaTime;
                countdownText.text = " " + FormatTime(countdown);
            }
            else
            {
                countdownText.text = "";
                isCountingDown = false;
                myButton.interactable = true;
                countdown = 7200.0f; // reset countdown
            }
        }
    }

    void OnButtonClicked()
    {
        myButton.interactable = false;
        isCountingDown = true;
        startTime = DateTime.Now;
        if (!string.IsNullOrEmpty(characterId))
        {
            PlayerPrefs.SetString("StartTime_" + characterId, startTime.ToString());
            PlayerPrefs.Save();
        }
        countdown = 7200.0f; // Reset the countdown to its initial value
        countdownText.text = "" + FormatTime(countdown);
    }

    string FormatTime(float timeInSeconds)
    {
        int hours = (int)(timeInSeconds / 3600);
        int minutes = (int)((timeInSeconds % 3600) / 60);
        int seconds = (int)(timeInSeconds % 60);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }
}
