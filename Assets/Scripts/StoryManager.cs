using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonSet
    {
        public List<Button> buttons; // A list of buttons in this set
        public Sprite setImage; // Image to display with this set
    }

    public List<ButtonSet> buttonSets; // List of all button sets
    public Image displayImage; // The single GameObject used to display images
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
        StartCoroutine(ActivateSetWithDelay());
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
                StartCoroutine(ActivateSetWithDelay());
            }
            else
            {
                Debug.Log("All button sets have been completed!");
                displayImage.gameObject.SetActive(false); // Hide the image when all sets are done
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

    private IEnumerator ActivateSetWithDelay()
    {
        yield return new WaitForSeconds(NextEventDuration); // Wait before activating the next set

        // Activate the current set
        SetButtonsActive(buttonSets[currentSetIndex], true);

        // Update the image
        if (displayImage != null && buttonSets[currentSetIndex].setImage != null)
        {
            displayImage.sprite = buttonSets[currentSetIndex].setImage;
            displayImage.gameObject.SetActive(true);
        }
    }
}
