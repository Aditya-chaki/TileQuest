using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import the TextMeshPro namespace

public class BuildingImageManager : MonoBehaviour
{
    public Image displayImage;         // Reference to the UI Image component
    public TextMeshProUGUI displayText; // Reference to the TextMeshProUGUI component

    void Update()
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
        if (displayImage != null && displayText != null)
        {
            displayImage.sprite = eventData.backgroundImage; // Set the image
            displayText.text = eventData.eventText;          // Set the text
            displayImage.gameObject.SetActive(true);         // Show the image
            displayText.gameObject.SetActive(true);          // Show the text
        }
    }

    public void HideContent()
    {
        if (displayImage != null && displayText != null)
        {
            displayImage.gameObject.SetActive(false); // Hide the image
            displayText.gameObject.SetActive(false);  // Hide the text
        }
    }
}
