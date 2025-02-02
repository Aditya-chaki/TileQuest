using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventTrigger : MonoBehaviour
{
    public string folderPath = "PrefabsFolder"; // Folder path inside Resources
    private GameObject[] prefabs; // Array to store loaded prefabs
    private int currentIndex = 0; // Index of the current prefab
    private GameObject instantiatedPrefab; // Currently instantiated prefab
    [SerializeField] private int energyCostPerClick = 5; // Energy cost for each click
    [SerializeField] private float instantiateDelay = 0f; // Delay in seconds before instantiating the prefab

    private Coroutine instantiateCoroutine; // To manage ongoing coroutines

    [SerializeField] private Button[] triggerButtons; // Array of buttons to trigger the action

    void Start()
    {
       
        // Load all prefabs from the specified folder in Resources
        prefabs = Resources.LoadAll<GameObject>(folderPath);

        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError("No prefabs found in folder: " + folderPath);
        }

        // Assign ShowNextPrefab to each button
        foreach (var button in triggerButtons)
        {
            button.onClick.AddListener(ShowNextPrefab);
        }
    }

    public void ShowNextPrefab()
    {
        // Check if there's enough energy
        if (Config.Energy < energyCostPerClick)
        {
            Debug.LogWarning("Not enough energy to perform this action.");
            return;
        }

        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning("No prefabs to cycle through.");
            return;
        }

        // Deduct energy
        Config.Energy -= energyCostPerClick;
        Debug.Log($"Energy deducted: {energyCostPerClick}. Remaining energy: {Config.Energy}");

        // Stop any ongoing instantiation coroutine
        if (instantiateCoroutine != null)
        {
            StopCoroutine(instantiateCoroutine);
        }

        // Start the coroutine to instantiate the prefab with a delay
        instantiateCoroutine = StartCoroutine(InstantiatePrefabWithDelay());
    }

    private IEnumerator InstantiatePrefabWithDelay()
    {
        // If there's already a prefab, destroy it
        if (instantiatedPrefab != null)
        {
            Destroy(instantiatedPrefab);
        }

        // Wait for the specified delay
        yield return new WaitForSeconds(instantiateDelay);

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

        // Reset the coroutine reference
        instantiateCoroutine = null;
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
