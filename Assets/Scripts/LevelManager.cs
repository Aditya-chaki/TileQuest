using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI castleLevelText;
    public Button upgradeButton;  // Reference to the Upgrade button
    public int baseFoodIncrease = 10;  // Base food increase per level
    public int baseGoldIncrease = 20;  // Base gold increase per level
    public float collectionMultiplier = 1.1f; // 10% increase per level

    private CollectingItems collectingItems;
    private CollectingFood collectingFood;



    void Start()
    {

        collectingItems = FindObjectOfType<CollectingItems>();
        collectingFood = FindObjectOfType<CollectingFood>();
        ApplyResourceBoosts();
        
    
    }

    void Update()
    {
        UpdateCastleLevel();
       
    }

    void UpdateCastleLevel()
    {
        int castleLevel = Config.GetCastleLevel();
        castleLevelText.text = "Castle Level " + castleLevel;
    }

    

    public void IncreaseCastleLevel()
    {
        int currentLevel = Config.GetCastleLevel();
        int newLevel = currentLevel + 1;
        Config.SetCastleLevel(newLevel);

        ApplyResourceBoosts();
        UpdateCollectionAmounts();

        Debug.Log($"Castle level increased to {newLevel}. Food and Gold collection boosted!");
    }

    private void ApplyResourceBoosts()
    {
        int castleLevel = Config.GetCastleLevel();
        int magicBoost = baseFoodIncrease * castleLevel;
        int goldBoost = baseGoldIncrease * castleLevel;

        Config.Magic += magicBoost;
        Config.Gold += goldBoost;

        Debug.Log($"Resources updated: +{magicBoost} Food, +{goldBoost} Gold.");
    }

    private void UpdateCollectionAmounts()
    {
        int castleLevel = Config.GetCastleLevel();

        if (collectingItems != null)
        {
            collectingItems.goldToCollect = Mathf.RoundToInt(collectingItems.goldToCollect * collectionMultiplier);
            Debug.Log($"Gold collection increased to {collectingItems.goldToCollect} per cycle.");
        }

        if (collectingFood != null)
        {
            collectingFood.foodToCollect = Mathf.RoundToInt(collectingFood.foodToCollect * collectionMultiplier);
            Debug.Log($"Food collection increased to {collectingFood.foodToCollect} per cycle.");
        }
    }


}
