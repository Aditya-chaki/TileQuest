using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryInitiator : MonoBehaviour
{
    public string folderName = "MyPrefabs"; // Folder inside Resources

    void Start()
    {
        string characterId = Config.GetCurrentCharacterId();

        if (string.IsNullOrEmpty(characterId))
        {
            Debug.LogError("Character ID is not set.");
            return;
        }

        // Load all prefabs from the specified folder in Resources
        Object[] prefabs = Resources.LoadAll(folderName, typeof(GameObject));

        // Get the current prefab index for the character from PlayerPrefs
        int currentIndex = Config.GetPrefabIndex(characterId);

        // Instantiate the current prefab
        if (prefabs.Length > 0)
        {
            Instantiate(prefabs[currentIndex], GetRandomPosition(), Quaternion.identity);

            // Update the index for the next scene load
            currentIndex = (currentIndex + 1) % prefabs.Length;
            Config.SetPrefabIndex(characterId, currentIndex);
        }
    }

    public void ResetPrefabSequence()
    {
        string characterId = Config.GetCurrentCharacterId();
        Config.SetPrefabIndex(characterId, 0);
    }

    // Function to generate a random position for instantiation
    private Vector3 GetRandomPosition()
    {
        return new Vector3(0, 0, 0); // You can implement a more complex logic here if needed
    }
}
