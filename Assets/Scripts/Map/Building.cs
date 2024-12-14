using UnityEngine;
using UnityEngine.UI; // Include this for working with UI components
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    public List<EventData> eventList; // List of event data for the building
    public Button eventButton; // Reference to the event button in the UI
    

    private int currentEventIndex = 0; // Tracks the current event index
    private bool isEventComplete = false; // Flag to check if the current event is complete

    void Start()
    {
        if (eventButton != null)
        {
            // Add a listener to the button's onClick event
            eventButton.onClick.AddListener(() => TriggerCurrentEvent());
        }
       
    }

    void Update()
    {
        // Detect touch input for mobile
        // if (Input.touchCount > 0)
        // {
        //     Touch touch = Input.GetTouch(0);

        //     if (touch.phase == TouchPhase.Began)
        //     {
        //         Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

        //         // Perform a raycast to detect if this building was tapped
        //         RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

        //         if (hit.collider != null && hit.collider.gameObject == this.gameObject)
        //         {
        //             TriggerCurrentEvent();
        //         }
        //     }
        // }
    }

    void TriggerCurrentEvent()
    {
        if (currentEventIndex < eventList.Count)
        {
            EventData currentEvent = eventList[currentEventIndex];

            // Find the BuildingDisplayManager in the scene and call ShowContent
            BuildingImageManager displayManager = FindObjectOfType<BuildingImageManager>();
            if (displayManager != null && currentEvent != null)
            {
                displayManager.ShowContent(currentEvent);
                isEventComplete = false; // Reset event completion status
            }
        }
        else
        {
            Debug.Log("All events for this building have been triggered.");
        }
    }

    public void CompleteEvent()
    {
        if (currentEventIndex < eventList.Count)
        {
            isEventComplete = true; // Mark the current event as complete
            currentEventIndex++; // Move to the next event
            Debug.Log("Event completed. Ready for the next event.");
        }
    }
}
