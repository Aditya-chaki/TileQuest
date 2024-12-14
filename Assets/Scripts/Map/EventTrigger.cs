using UnityEngine;
using UnityEngine.UI;

public class EventTrigger : MonoBehaviour
{
    public string folderPath = "PrefabsFolder"; // Folder path inside Resources
    private GameObject[] prefabs; // Array to store loaded prefabs
    private int currentIndex = 0; // Index of the current prefab
    private GameObject instantiatedPrefab; // Currently instantiated prefab

    void Start()
    {
        // Load all prefabs from the specified folder in Resources
        prefabs = Resources.LoadAll<GameObject>(folderPath);

        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError("No prefabs found in folder: " + folderPath);
        }
    }

    // Call this method when the main button is clicked
    public void ShowNextPrefab()
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning("No prefabs to cycle through.");
            return;
        }

        // If there's already a prefab, destroy it
        if (instantiatedPrefab != null)
        {
            Destroy(instantiatedPrefab);
        }

        // Instantiate the next prefab
        instantiatedPrefab = Instantiate(prefabs[currentIndex]);

        // Optional: Set its parent or position
        instantiatedPrefab.transform.SetParent(this.transform, false);
        instantiatedPrefab.transform.localPosition = Vector3.zero;

        // Find the Close Button in the prefab
        Button closeButton = instantiatedPrefab.GetComponentInChildren<Button>();
        if (closeButton != null)
        {
            // Add listener to the Close Button
            closeButton.onClick.AddListener(ClosePrefab);
        }
        else
        {
            Debug.LogWarning("No Button component found in prefab: " + prefabs[currentIndex].name);
        }

        // Move to the next prefab in the sequence
        currentIndex = (currentIndex + 1) % prefabs.Length;
    }

    // Close the currently instantiated prefab
    public void ClosePrefab()
    {
        if (instantiatedPrefab != null)
        {
            Destroy(instantiatedPrefab);
            instantiatedPrefab = null;
        }
        else
        {
            Debug.LogWarning("No instantiated prefab to destroy.");
        }
    }
}
