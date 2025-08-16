using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUpgradeUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject castleUpgradePanel;
    public GameObject treasuryUpgradePanel;
    public GameObject foodUpgradePanel;

    public Button openUpgradeButton;
    public Button closeUpgradeButton;
    
    public Button openTreasuryUpgradeButton;
    public Button closeTreasuryUpgradeButton;

    public Button openFoodUpgradeButton;
    public Button closeFoodUpgradeButton;
   
    [Header("Main Building UI")]
    public BuildingUpgradeSlot mainBuildingSlot;
    
    [Header("Other Buildings UI")]
    public Transform otherBuildingsContainer;
    public GameObject buildingSlotPrefab;
    [Header("Resource Display")]
    public TextMeshProUGUI goldAmountText;
    
    private BuildingManager buildingManager;
    [SerializeField] private List<BuildingUpgradeSlot> otherBuildingSlots = new List<BuildingUpgradeSlot>();
 
    void Start()
    {
        // Find BuildingManager in scene
        buildingManager = FindObjectOfType<BuildingManager>();
        
        if (buildingManager == null)
        {
            Debug.LogError("BuildingManager not found in scene!");
            return;
        }
        
        // Setup UI
        SetupUI();
        
        // Start with panel closed
        if (castleUpgradePanel != null)
            castleUpgradePanel.SetActive(false);
        
        // Setup buttons
        if (openUpgradeButton != null)
            openUpgradeButton.onClick.AddListener(OpenUpgradePanel);
            
        if (closeUpgradeButton != null)
            closeUpgradeButton.onClick.AddListener(CloseUpgradePanel);
        
         if (openTreasuryUpgradeButton != null)
            openTreasuryUpgradeButton.onClick.AddListener(OpenTreasuryUpgradePanel);
            
        if (closeTreasuryUpgradeButton != null)
            closeTreasuryUpgradeButton.onClick.AddListener(CloseTreasuryUpgradePanel);

         if (openFoodUpgradeButton != null)
            openFoodUpgradeButton.onClick.AddListener(OpenFoodUpgradePanel);
            
        if (closeFoodUpgradeButton != null)
            closeFoodUpgradeButton.onClick.AddListener(CloseFoodUpgradePanel);    
        // Update UI regularly
        InvokeRepeating(nameof(UpdateUI), 0f, 0.5f);
    }
    
    void SetupUI()
    {
        // Setup main building slot
        if (mainBuildingSlot != null && buildingManager.mainBuilding != null)
        {
            mainBuildingSlot.SetupMainBuilding(buildingManager.mainBuilding, buildingManager);
        }
        
        // Setup other building slots
        SetupOtherBuildingSlots();
    }
    
    void SetupOtherBuildingSlots()
    {
        for (int i = 0; i < buildingManager.otherBuildings.Length; i++)
        {
            if (buildingSlotPrefab != null && otherBuildingsContainer != null)
            {
                BuildingUpgradeSlot slot = otherBuildingSlots[i];
                if (slot != null)
                {
                    slot.SetupOtherBuilding(buildingManager.otherBuildings[i], buildingManager, i);
                    //otherBuildingSlots.Add(slot);
                }
            }
        }
    }
    
    void UpdateUI()
    {
        // Update gold display
        if (goldAmountText != null)
            goldAmountText.text = Config.Gold.ToString();
        
        // Update main building slot
        if (mainBuildingSlot != null)
            mainBuildingSlot.UpdateDisplay();
        
        // Update other building slots
        foreach (var slot in otherBuildingSlots)
        {
            if (slot != null)
                slot.UpdateDisplay();
        }
    }
    
    public void OpenUpgradePanel()
    {
        if (castleUpgradePanel != null)
            castleUpgradePanel.SetActive(true);
    }
    
    public void CloseUpgradePanel()
    {
        if (castleUpgradePanel != null)
            castleUpgradePanel.SetActive(false);
    }
    
     public void OpenTreasuryUpgradePanel()
    {
        if (treasuryUpgradePanel != null)
            treasuryUpgradePanel.SetActive(true);
    }
    
    public void CloseTreasuryUpgradePanel()
    {
        if (treasuryUpgradePanel != null)
            treasuryUpgradePanel.SetActive(false);
    }
     public void OpenFoodUpgradePanel()
    {
        if (foodUpgradePanel != null)
            foodUpgradePanel.SetActive(true);
    }
    
    public void CloseFoodUpgradePanel()
    {
        if (foodUpgradePanel != null)
            foodUpgradePanel.SetActive(false);
    }

    void OnDestroy()
    {
        CancelInvoke(nameof(UpdateUI));
    }
}