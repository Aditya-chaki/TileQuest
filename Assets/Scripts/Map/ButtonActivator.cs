using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivator : MonoBehaviour
{
    public List<GameObject> buttonObjects; // Assign button GameObjects in Inspector
    public int activeButtonCount = 3; // Number of buttons to be active at a time
    public float updateInterval = 3f; // Time interval to randomize buttons

    private void Start()
    {
        Config.Magic = 30;
        if (buttonObjects.Count == 0)
        {
            Debug.LogError("No buttons assigned!");
            return;
        }
        InvokeRepeating(nameof(RandomizeButtons), 0f, updateInterval);
    }

    private void RandomizeButtons()
    {
        // Deactivate all buttons first
        foreach (GameObject buttonObj in buttonObjects)
        {
            buttonObj.SetActive(false);
        }

        // Select random buttons to activate
        HashSet<int> selectedIndices = new HashSet<int>();
        while (selectedIndices.Count < Mathf.Min(activeButtonCount, buttonObjects.Count))
        {
            int randomIndex = Random.Range(0, buttonObjects.Count);
            selectedIndices.Add(randomIndex);
        }

        // Activate selected buttons
        foreach (int index in selectedIndices)
        {
            buttonObjects[index].SetActive(true);
        }
    }
}
