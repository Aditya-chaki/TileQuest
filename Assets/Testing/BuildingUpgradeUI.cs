using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUpgradeUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject upgradePanel;
    public Button openUpgradeButton;
    public Button closeUpgradeButton;
    
    [Header("Main Building UI")]
    public BuildingUpgradeSlot mainBuildingSlot;
    
    [Header("Other Buildings UI")]
    public Transform otherBuildingsContainer;
    public GameObject buildingSlotPrefab;
    
    [Header("Resource Display")]
    public TextMeshProUGUI goldAmountText;
    
    private BuildingManager buildingManager;
    private List<BuildingUpgradeSlot> otherBuildingSlots = new List<BuildingUpgradeSlot>();
    
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
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
        
        // Setup buttons
        if (openUpgradeButton != null)
            openUpgradeButton.onClick.AddListener(OpenUpgradePanel);
            
        if (closeUpgradeButton != null)
            closeUpgradeButton.onClick.AddListener(CloseUpgradePanel);
        
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
        // Clear existing slots
        foreach (var slot in otherBuildingSlots)
        {
            if (slot != null)
                Destroy(slot.gameObject);
        }
        otherBuildingSlots.Clear();
        
        // Create slots for other buildings
        for (int i = 0; i < buildingManager.otherBuildings.Length; i++)
        {
            if (buildingSlotPrefab != null && otherBuildingsContainer != null)
            {
                GameObject slotGO = Instantiate(buildingSlotPrefab, otherBuildingsContainer);
                BuildingUpgradeSlot slot = slotGO.GetComponent<BuildingUpgradeSlot>();
                
                if (slot != null)
                {
                    slot.SetupOtherBuilding(buildingManager.otherBuildings[i], buildingManager, i);
                    otherBuildingSlots.Add(slot);
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
        if (upgradePanel != null)
            upgradePanel.SetActive(true);
    }
    
    public void CloseUpgradePanel()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }
    
    void OnDestroy()
    {
        CancelInvoke(nameof(UpdateUI));
    }
}