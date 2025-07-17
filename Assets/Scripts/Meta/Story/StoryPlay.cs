using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryPlay : MonoBehaviour
{
    [System.Serializable]
    public class ButtonPrefabPair
    {
        public Button button;
        public GameObject prefab;
        public string characterId; // Unique identifier for each character
        public int maxIndex; // Maximum index for this prefab
    }

    public List<ButtonPrefabPair> buttonPrefabPairs;
    public Button resetButton;
    public Transform spawnParent;

    private Dictionary<string, int> prefabChildIndexTracker = new Dictionary<string, int>();

    void Start()
    {
        Debug.Log("StoryPlay Start");

        foreach (var pair in buttonPrefabPairs)
        {
            string key = pair.characterId + "_Index"; // Use characterId to create a unique key
            if (PlayerPrefs.HasKey(key))
            {
                prefabChildIndexTracker[pair.characterId] = PlayerPrefs.GetInt(key);
            }
            else
            {
                prefabChildIndexTracker[pair.characterId] = 0;
            }

            pair.button.onClick.AddListener(() => InstantiatePrefab(pair));

            // Disable the button if Config.Magic is 0
            if (Config.Magic == 0)
            {
                pair.button.interactable = false;
            }
        }

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetProgress);
        }
    }

    void InstantiatePrefab(ButtonPrefabPair pair)
    {
        // Check if there is enough Magic before instantiating the prefab
        if (Config.Magic > 0)
        {
            if (pair.prefab != null)
            {
                Debug.Log("Instantiating prefab: " + pair.prefab.name);
                GameObject instance = Instantiate(pair.prefab, Vector3.zero, Quaternion.identity, spawnParent);

                string characterId = pair.characterId;
                if (!prefabChildIndexTracker.ContainsKey(characterId))
                {
                    prefabChildIndexTracker[characterId] = 0;
                }

                int currentIndex = prefabChildIndexTracker[characterId];
                ActivateChild(instance, currentIndex);

                currentIndex++; // Increment the child index
                if (currentIndex >= pair.maxIndex) // Check if it exceeds maxIndex
                {
                    pair.button.interactable = false; // Disable the button
                }

                prefabChildIndexTracker[characterId] = currentIndex; // Update the tracker

                PlayerPrefs.SetInt(characterId + "_Index", currentIndex); // Save updated index
                PlayerPrefs.Save();
            }
            else
            {
                Debug.LogWarning("Prefab is null, cannot instantiate.");
            }
        }
        else
        {
            Debug.LogWarning("Not enough Magic to instantiate prefab.");
        }
    }

    void ActivateChild(GameObject instance, int index)
    {
        if (index < instance.transform.childCount)
        {
            instance.transform.GetChild(index).gameObject.SetActive(true);
            Config.Magic = Config.Magic - 4;

            // Disable all buttons if Magic reaches 0
            if (Config.Magic <= 0)
            {
                foreach (var pair in buttonPrefabPairs)
                {
                    pair.button.interactable = false;
                }
            }
        }
    }

    void ResetProgress()
    {
        foreach (var pair in buttonPrefabPairs)
        {
            string key = pair.characterId + "_Index";
            prefabChildIndexTracker[pair.characterId] = 0;
            PlayerPrefs.DeleteKey(key);
            pair.button.interactable = true; // Re-enable the button
        }

        PlayerPrefs.Save();
        Debug.Log("Progress has been reset.");
    }

    public int GetMaxIndex(string characterId)
    {
        foreach (var pair in buttonPrefabPairs)
        {
            if (pair.characterId == characterId)
            {
                return pair.maxIndex;
            }
        }
        Debug.LogWarning("Character ID not found: " + characterId);
        return -1; // Return -1 or an appropriate value to indicate that the character ID was not found
    }

    public void SetMaxIndex(string characterId, int newMaxIndex)
    {
        foreach (var pair in buttonPrefabPairs)
        {
            if (pair.characterId == characterId)
            {
                pair.maxIndex = newMaxIndex;
                Debug.Log("Max index for " + characterId + " has been set to " + newMaxIndex);
                return;
            }
        }
        Debug.LogWarning("Character ID not found: " + characterId);
    }

    public int GetCurrentIndex(string characterId)
    {
        if (prefabChildIndexTracker.TryGetValue(characterId, out int currentIndex))
        {
            return currentIndex;
        }
        else
        {
            Debug.LogWarning("Character ID not found: " + characterId);
            return -1; // Return -1 or an appropriate value to indicate that the character ID was not found
        }
    }

    public void SetCurrentIndex(string characterId, int newIndex)
    {
        if (prefabChildIndexTracker.ContainsKey(characterId))
        {
            prefabChildIndexTracker[characterId] = newIndex;
            PlayerPrefs.SetInt(characterId + "_Index", newIndex); // Save updated index
            PlayerPrefs.Save();
            Debug.Log("Current index for " + characterId + " has been set to " + newIndex);
        }
        else
        {
            Debug.LogWarning("Character ID not found: " + characterId);
        }
    }
}
