using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabGenerator : MonoBehaviour
{
    public string folderName = "MyPrefabs"; // Folder inside Resources
    private int prefabsGeneratedCount = 0;  // Counter to track prefabs generated

    void Start()
    {
        Config.UpdateTalkVariable();
        
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

            // Increment the prefab generation count
            prefabsGeneratedCount++;

            // Check if we need to increase the character's level
            if (prefabsGeneratedCount >= 5)
            {
                int currentLevel = Config.GetCharacterLevel(characterId);
                int maxLevel = Config.GetMaxCharacterLevel(Config.GetCastleLevel());

                if (currentLevel < maxLevel)
                {
                    Config.SetCharacterLevel(characterId, currentLevel + 1);
                    Debug.Log("Character level increased to: " + (currentLevel + 1));
                }

                // Reset the counter
                prefabsGeneratedCount = 0;
            }
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
