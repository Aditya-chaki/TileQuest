// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;

// public class LevelManager : MonoBehaviour
// {
//     public TextMeshProUGUI castleLevelText;
//     public Button upgradeButton;  // Reference to the Upgrade button

//     private int foodRequired;
//     private int strengthRequired;
//     private int goldRequired;
//     private int healthRequired;

//     void Start()
//     {
//         // Set required values to the current maximum values of each metric
//         foodRequired = Config.GetMaxFood();
//         strengthRequired = Config.GetMaxStrength();
//         goldRequired = Config.GetMaxGold();
//         healthRequired = Config.GetMaxHealth();

//         UpdateCastleLevel();
//         UpdateUpgradeButtonVisibility();  // Update button visibility at start
//     }

//     void Update()
//     {
//         UpdateCastleLevel();
//         UpdateUpgradeButtonVisibility();  // Update button visibility continuously
//     }

//     void UpdateCastleLevel()
//     {
//         int castleLevel = Config.GetCastleLevel();
//         castleLevelText.text = "Castle Level " + castleLevel;
//     }

//     public void UpgradeCastleLevel()
//     {
//         if (CanUpgrade())
//         {
//             // Deduct the required resources
//             Config.Food -= foodRequired;
//             Config.Strength -= strengthRequired;
//             Config.Gold -= goldRequired;
//             Config.Health -= healthRequired;

//             // Upgrade the castle level
//             int currentLevel = Config.GetCastleLevel();
//             Config.SetCastleLevel(currentLevel + 1);

//             // Increase the max values of each metric by 1000
//             Config.SetMaxFood(Config.GetMaxFood() + 1000);
//             Config.SetMaxStrength(Config.GetMaxStrength() + 1000);
//             Config.SetMaxGold(Config.GetMaxGold() + 1000);
//             Config.SetMaxHealth(Config.GetMaxHealth() + 1000);

//             // Update required values for the next upgrade
//             foodRequired = Config.GetMaxFood();
//             strengthRequired = Config.GetMaxStrength();
//             goldRequired = Config.GetMaxGold();
//             healthRequired = Config.GetMaxHealth();

//             Debug.Log("Castle upgraded to level " + (currentLevel + 1));
//         }
//         else
//         {
//             Debug.Log("Not enough resources to upgrade the castle.");
//         }
//     }

//     private bool CanUpgrade()
//     {
//         return Config.Food >= foodRequired &&
//                Config.Strength >= strengthRequired &&
//                Config.Gold >= goldRequired &&
//                Config.Health >= healthRequired;
//     }

//     private void UpdateUpgradeButtonVisibility()
//     {
//         // Check if all metrics are full
//         bool canUpgrade = CanUpgrade();
//         upgradeButton.gameObject.SetActive(canUpgrade);
//     }
// }
