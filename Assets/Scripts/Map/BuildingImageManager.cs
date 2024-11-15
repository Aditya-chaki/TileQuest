using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingImageManager : MonoBehaviour
{
    public Image displayImage;         // Reference to the UI Image component
    public TextMeshProUGUI displayText; // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI foodText;    // UI element to show food deduction
    public TextMeshProUGUI strengthText; // UI element to show strength deduction
    public TextMeshProUGUI healthText;   // UI element to show health deduction
    public TextMeshProUGUI goldText;     // UI element to show gold deduction
    public Button completeButton;        // UI button for completing the event

    private EventData currentEventData;  // Currently shown event data

    void Start()
    {
        if (completeButton != null)
        {
            // Link the button's onClick event to the CompleteEvent method
            completeButton.onClick.AddListener(CompleteEvent);
            completeButton.gameObject.SetActive(false); // Hide the button initially
        }
    }
    private void Update()
    {
        // Detect touch input for hiding the image when tapping outside
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                // Check if the touch is outside any building (hit is null or not a building)
                if (hit.collider == null || !hit.collider.CompareTag("Building"))
                {
                    HideContent();
                }
            }
        }
    }

    public void ShowContent(EventData eventData)
    {
        currentEventData = eventData;

        if (displayImage != null && displayText != null)
        {
            
            displayText.text = eventData.eventText;          // Set the text

            // Set the metric values in the UI
            foodText.text = $"Food: {eventData.food}";
            strengthText.text = $"Strength: {eventData.strength}";
            healthText.text = $"Health: {eventData.health}";
            goldText.text = $"Gold: {eventData.gold}";

            // Make the UI elements visible
            displayImage.gameObject.SetActive(true);
            displayText.gameObject.SetActive(true);
            foodText.gameObject.SetActive(true);
            strengthText.gameObject.SetActive(true);
            healthText.gameObject.SetActive(true);
            goldText.gameObject.SetActive(true);

            // Show the complete button
            if (completeButton != null)
            {
                completeButton.gameObject.SetActive(true);
            }
        }
    }

    public void HideContent()
    {
        if (displayImage != null && displayText != null)
        {
            displayImage.gameObject.SetActive(false);
            displayText.gameObject.SetActive(false);
            foodText.gameObject.SetActive(false);
            strengthText.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
            goldText.gameObject.SetActive(false);

            // Hide the complete button
            if (completeButton != null)
            {
                completeButton.gameObject.SetActive(false);
            }
        }
    }

    // Called when the "Complete" button is clicked
    public void CompleteEvent()
    {
        if (currentEventData != null)
        {
            // Deduct the metric values from the player's resources
            Config.Food -= currentEventData.food;
            Config.Strength -= currentEventData.strength;
            Config.Health -= currentEventData.health;
            Config.Gold -= currentEventData.gold;

            Debug.Log("Event completed, metrics deducted.");
            HideContent(); // Hide the card after completing the event
        }
    }
}

