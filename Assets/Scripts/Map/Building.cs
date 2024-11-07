using UnityEngine;

public class Building : MonoBehaviour
{
    public EventData eventData; // Reference to the building's event data

    void Update()
    {
        // Detect touch input for mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                // Perform a raycast to detect if this building was tapped
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                {
                    // Find the BuildingDisplayManager in the scene and call ShowContent
                    BuildingImageManager displayManager = FindObjectOfType<BuildingImageManager>();
                    if (displayManager != null && eventData != null)
                    {
                        displayManager.ShowContent(eventData);
                    }
                }
            }
        }
    }
}
