using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUpgradeSlot : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI buildingNameText;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI upgradeCostText;
    public Button upgradeButton;
    //public Image buildingIcon;
    public Image backgroundImage;
    
    [Header("Visual States")]
    public Color affordableColor = Color.white;
    public Color unaffordableColor = Color.gray;
    public Color cappedColor = Color.red;
    
    private BuildingData buildingData;
    private BuildingManager buildingManager;
    private bool isMainBuilding;
    private int buildingIndex = -1;
    
    public void SetupMainBuilding(BuildingData building, BuildingManager manager)
    {
        buildingData = building;
        buildingManager = manager;
        isMainBuilding = true;
        buildingIndex = -1;
        
        SetupButton();
        UpdateDisplay();
    }
    
    public void SetupOtherBuilding(BuildingData building, BuildingManager manager, int index)
    {
        buildingData = building;
        buildingManager = manager;
        isMainBuilding = false;
        buildingIndex = index;
        
        SetupButton();
        UpdateDisplay();
    }
    
    void SetupButton()
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }
    }
    
    public void UpdateDisplay()
    {
        if (buildingData == null) return;
        
        // Update texts
        if (buildingNameText != null)
            buildingNameText.text = buildingData.name;
            
        if (currentLevelText != null)
            currentLevelText.text = $"Level {buildingData.level}";
            
        if (upgradeCostText != null)
            upgradeCostText.text = $"{buildingData.GetUpgradeCost()} Gold";
        
        // Update button state
        UpdateButtonState();
    }
    
    void UpdateButtonState()
    {
        if (upgradeButton == null) return;
        
        bool canAfford = Config.Gold >= buildingData.GetUpgradeCost();
        bool isCapped = false;
        
        // Check if other building is capped by main building level
        if (!isMainBuilding && buildingManager != null)
        {
            isCapped = buildingData.level >= buildingManager.mainBuilding.level;
        }
        
        // Set button interactability
        upgradeButton.interactable = canAfford && !isCapped;
        
        // Update button text
        TextMeshProUGUI buttonText = upgradeButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            if (isCapped)
                buttonText.text = "LEVEL CAPPED";
            else if (!canAfford)
                buttonText.text = "CAN'T AFFORD";
            else
                buttonText.text = "UPGRADE";
        }
        
        // Update visual state
        if (backgroundImage != null)
        {
            if (isCapped)
                backgroundImage.color = cappedColor;
            else if (!canAfford)
                backgroundImage.color = unaffordableColor;
            else
                backgroundImage.color = affordableColor;
        }
    }
    
    void OnUpgradeButtonClicked()
    {
        if (buildingManager == null) return;
        
        if (isMainBuilding)
        {
            buildingManager.UpgradeMainBuilding();
            Debug.Log($"Upgrading main building: {buildingData.name}");
        }
        else
        {
            buildingManager.UpgradeOtherBuilding(buildingIndex);
            Debug.Log($"Upgrading building: {buildingData.name}");
        }
    }
}