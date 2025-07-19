using UnityEngine;

public class DecayDebugger : MonoBehaviour
{
    void Start()
    {
        // Test if Config values can be changed at all
        Debug.Log($"Initial Config values - Influence: {Config.Influence}, Magic: {Config.Magic}, Gold: {Config.Gold}");
        
        // Try to manually change values
        Config.Influence = 50;
        Config.Magic = 50;
        Config.Gold = 50;
        
        Debug.Log($"After manual change - Influence: {Config.Influence}, Magic: {Config.Magic}, Gold: {Config.Gold}");
        
        // Start a test decay every 2 seconds
        InvokeRepeating(nameof(TestDecay), 2f, 2f);
    }
    
    void TestDecay()
    {
        Debug.Log($"Before test decay - Influence: {Config.Influence}, Magic: {Config.Magic}, Gold: {Config.Gold}");
        
        Config.Influence -= 5;
        Config.Magic -= 5;
        Config.Gold -= 5;
        
        Debug.Log($"After test decay - Influence: {Config.Influence}, Magic: {Config.Magic}, Gold: {Config.Gold}");
    }
}