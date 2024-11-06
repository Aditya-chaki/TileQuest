using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    public float highlightScale = 1.1f;    // Scale multiplier for highlighting
    private GameObject highlightedBuilding = null; // Track the currently highlighted building
    private Vector3 originalScale;         // Store the original scale of the highlighted building

    void Update()
    {
        // Detect touch or mouse input
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            // Get the touch position or mouse position
            Vector2 touchPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

            // Perform a raycast at the touch position
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Building"))
            {
                // Highlight the tapped building
                HighlightBuilding(hit.collider.gameObject);
            }
            else
            {
                // Reset highlight if tapped outside any building
                ResetHighlight();
            }
        }
    }

    private void HighlightBuilding(GameObject building)
    {
        // If a different building is already highlighted, reset it
        if (highlightedBuilding != null && highlightedBuilding != building)
        {
            ResetHighlight();
        }

        // Highlight the new building if it’s not already highlighted
        if (highlightedBuilding != building)
        {
            highlightedBuilding = building;
            originalScale = highlightedBuilding.transform.localScale;
            highlightedBuilding.transform.localScale = originalScale * highlightScale;
        }
    }

    private void ResetHighlight()
    {
        if (highlightedBuilding != null)
        {
            // Reset the scale of the currently highlighted building
            highlightedBuilding.transform.localScale = originalScale;
            highlightedBuilding = null;
        }
    }
}
