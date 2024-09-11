using UnityEngine;
using UnityEngine.UI;

public class ToggleObjects : MonoBehaviour
{
    public GameObject imageObject; // Assign the Image GameObject in the Inspector
    private bool isImageVisible = false; // Track whether the image is visible

    // This method will be called when the button is clicked
    public void ToggleImageVisibility()
    {
        isImageVisible = !isImageVisible; // Toggle the boolean value
        imageObject.SetActive(isImageVisible); // Show or hide the image based on the boolean value
    }
}
