using UnityEngine;
using UnityEngine.UI;

public class ToggleObjects : MonoBehaviour
{
    public GameObject imageObject1;    // First GameObject to toggle
    public GameObject imageObject2;    // Second GameObject to toggle
    private bool isImageVisible = false;  // Tracks the toggle state

    // This method will be called when the button is clicked
    public void ToggleImageVisibility()
    {
        isImageVisible = !isImageVisible;  // Toggle the boolean value

        // Set opposite states for the two objects
        imageObject1.SetActive(isImageVisible);    // Enable/disable first object
        imageObject2.SetActive(!isImageVisible);   // Enable/disable second object (opposite state)
    }
}