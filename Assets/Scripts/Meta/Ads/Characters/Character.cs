using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private string characterId;

    void Start()
    {
        // Set the current character ID in the Config class
        Config.SetCurrentCharacterId(characterId);
    }

    // Method to set the character ID through code if needed
    public void SetCharacterId(string newCharacterId)
    {
        characterId = newCharacterId;
        Config.SetCurrentCharacterId(characterId);
    }

    public string GetCharacterId()
    {
        return characterId;
    }

    public int GetOpinion()
    {
        return Config.GetOpinionMeter(characterId);
    }

    public void SetOpinion(int opinionValue)
    {
        Config.SetOpinionMeter(characterId, opinionValue);
    }
}
