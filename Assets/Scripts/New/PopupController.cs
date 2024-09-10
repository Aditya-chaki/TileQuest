using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupController : MonoBehaviour
{
    public GameObject popup;
    public float displayDuration = 2.0f; // Duration the pop-up will be visible

    // Function to show the pop-up
    public void ShowPopup()
    {
        popup.SetActive(true); // Activate the pop-up
        StartCoroutine(HidePopupAfterDelay()); // Start the coroutine to hide the pop-up
    }

    // Coroutine to hide the pop-up after the specified duration
    private IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        popup.SetActive(false); // Deactivate the pop-up
    }
   
}
