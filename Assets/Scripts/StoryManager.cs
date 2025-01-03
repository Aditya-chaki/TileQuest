using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class StoryManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonSet
    {
        public List<Button> buttons; // A list of buttons in this set
        public Sprite setImage; // Image to display with this set

        // Decay properties specific to this button set
        public int foodDecayRate = 1;
        public int strengthDecayRate = 1;
        public int healthDecayRate = 1;
        public int goldDecayRate = 1;
        public int decayDuration = 600; // Decay duration in seconds
    }

    public List<ButtonSet> buttonSets; // List of all button sets
    public Image displayImage; // The single GameObject used to display images
    public TextMeshProUGUI nextEventTimerText; // TextMeshPro for displaying the timer
    public float NextEventDuration = 90f; // Time to wait before the next set

    private int currentSetIndex = 0; // Tracks the current set index

    void Start()
    {
        // Initialize all button sets
        for (int i = 0; i < buttonSets.Count; i++)
        {
            SetButtonsActive(buttonSets[i], false); // Disable all button sets at start
        }

        // Assign click listeners to each button
        foreach (var set in buttonSets)
        {
            foreach (var button in set.buttons)
            {
                button.onClick.AddListener(() => OnButtonClicked(button));
            }
        }

        // Start with the first set
        StartCoroutine(ActivateSetWithDecay());
    }

    private void SetButtonsActive(ButtonSet buttonSet, bool active)
    {
        foreach (var button in buttonSet.buttons)
        {
            button.gameObject.SetActive(active);
            button.interactable = active; // Reset interactability when activating buttons
        }
    }

    private void OnButtonClicked(Button clickedButton)
    {
        // Disable the clicked button
        clickedButton.interactable = false;

        // Check if all buttons in the current set have been clicked
        if (AllButtonsClicked(buttonSets[currentSetIndex]))
        {
            // Disable the current set
            SetButtonsActive(buttonSets[currentSetIndex], false);

            // Move to the next set if available
            if (currentSetIndex + 1 < buttonSets.Count)
            {
                currentSetIndex++;
                StartCoroutine(ActivateSetWithDecay());
            }
            else
            {
                Debug.Log("All button sets have been completed!");
                displayImage.gameObject.SetActive(false); // Hide the image when all sets are done
                if (nextEventTimerText != null)
                    nextEventTimerText.text = ""; // Clear the timer text
            }
        }
    }

    private bool AllButtonsClicked(ButtonSet buttonSet)
    {
        foreach (var button in buttonSet.buttons)
        {
            if (button.interactable) // Check if any button is still interactable
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator ActivateSetWithDecay()
    {
        float remainingTime = NextEventDuration;

        // Display the timer for the next event
        while (remainingTime > 0)
        {
            if (nextEventTimerText != null)
            {
                nextEventTimerText.text = $"Next Event In: {Mathf.CeilToInt(remainingTime)}s";
            }

            yield return new WaitForSeconds(1);
            remainingTime -= 1;
        }

        if (nextEventTimerText != null)
        {
            nextEventTimerText.text = ""; // Clear the timer text when the event starts
        }

        // Activate the current set
        ButtonSet currentSet = buttonSets[currentSetIndex];
        SetButtonsActive(currentSet, true);

        // Update the image
        if (displayImage != null && currentSet.setImage != null)
        {
            displayImage.sprite = currentSet.setImage;
            displayImage.gameObject.SetActive(true);
        }

        // Apply decay for the current set
        int elapsedDecayTime = 0;
        while (elapsedDecayTime < currentSet.decayDuration)
        {
            yield return new WaitForSeconds(1);
            elapsedDecayTime++;

            // Apply decay logic
            Config.Food = Mathf.Max(0, Config.Food - currentSet.foodDecayRate);
            Config.Strength = Mathf.Max(0, Config.Strength - currentSet.strengthDecayRate);
            Config.Health = Mathf.Max(0, Config.Health - currentSet.healthDecayRate);
            Config.Gold = Mathf.Max(0, Config.Gold - currentSet.goldDecayRate);

            Debug.Log($"Decay applied for Set {currentSetIndex + 1}: Food={Config.Food}, Strength={Config.Strength}, Health={Config.Health}, Gold={Config.Gold}");
        }
    }
}
