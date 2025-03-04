using UnityEngine;
using UnityEngine.UI;

public class ToggleGameobject : MonoBehaviour
{
    public GameObject objectToToggle;
    public Button toggleButton;

    void Start()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleGameObject);
        }
        else
        {
            Debug.LogError("Toggle Button is not assigned in the Inspector!");
        }
    }

    void ToggleGameObject()
    {
        if (objectToToggle != null)
        {
            // Toggle the active state (if active -> deactivate, if inactive -> activate)
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }
        else
        {
            Debug.LogError("Object to toggle is not assigned in the Inspector!");
        }
    }
}