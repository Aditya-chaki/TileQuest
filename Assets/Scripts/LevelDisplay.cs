using UnityEngine;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    
    public TextMeshProUGUI characterLevelText;
    public string characterId = "default_character_id"; // Replace with actual character ID

    void Start()
    {
       
        UpdateCharacterLevel();
    }

   

    void UpdateCharacterLevel()
    {
        int characterLevel = Config.GetCharacterLevel(characterId);
        characterLevelText.text = "Level: " + characterLevel;
    }
}
