using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleMarketplaceUI : MonoBehaviour
{
    [Header("Main UI")]
    public Button marketplaceOpenButton;
    public GameObject marketplacePanel;
    public Button closeButton;
    
    [Header("Resource Display at Top (4 metrics only)")]
    public TextMeshProUGUI influenceValueText;
    public TextMeshProUGUI goldValueText;
    public TextMeshProUGUI magicValueText;
    public TextMeshProUGUI opinionValueText;
    
    [Header("Trade Slots - Assign the 4 trade panels here")]
    public GameObject tradeSlot1Panel;
    public GameObject tradeSlot2Panel;
    public GameObject tradeSlot3Panel;
    public GameObject tradeSlot4Panel;
    
    private List<TradeSlot> tradeSlots = new List<TradeSlot>();
    
    void Start()
    {
        // Setup button listeners
        if (marketplaceOpenButton != null)
            marketplaceOpenButton.onClick.AddListener(OpenMarketplace);
            
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMarketplace);
        
        // Start with marketplace closed
        if (marketplacePanel != null)
            marketplacePanel.SetActive(false);
        
        // Get TradeSlot components from the assigned panels
        SetupTradeSlotReferences();
        
        // Setup initial trades
        SetupInitialTrades();
        
        // Update UI regularly
        InvokeRepeating(nameof(UpdateUI), 0f, 0.5f);
    }
    
    void SetupTradeSlotReferences()
    {
        tradeSlots.Clear();
        
        if (tradeSlot1Panel != null)
            tradeSlots.Add(tradeSlot1Panel.GetComponent<TradeSlot>());
            
        if (tradeSlot2Panel != null)
            tradeSlots.Add(tradeSlot2Panel.GetComponent<TradeSlot>());
            
        if (tradeSlot3Panel != null)
            tradeSlots.Add(tradeSlot3Panel.GetComponent<TradeSlot>());
            
        if (tradeSlot4Panel != null)
            tradeSlots.Add(tradeSlot4Panel.GetComponent<TradeSlot>());
    }
    
    void SetupInitialTrades()
    {
        if (tradeSlots.Count >= 4)
        {
            // Trade Slot 1: 100 Gold → 150 Influence
            if (tradeSlots[0] != null)
                tradeSlots[0].SetupTrade(ResourceType.Gold, 100, ResourceType.Influence, 150, 3, this);
            
            // Trade Slot 2: 200 Influence → 80 Magic
            if (tradeSlots[1] != null)
                tradeSlots[1].SetupTrade(ResourceType.Influence, 200, ResourceType.Magic, 80, 5, this);
            
            // Trade Slot 3: 120 Magic → 250 Gold
            if (tradeSlots[2] != null)
                tradeSlots[2].SetupTrade(ResourceType.Magic, 120, ResourceType.Gold, 250, 4, this);
            
            // Trade Slot 4: 300 Gold → 50 Opinion
            if (tradeSlots[3] != null)
                tradeSlots[3].SetupTrade(ResourceType.Gold, 300, ResourceType.Opinion, 50, 2, this);
        }
    }
    
    void UpdateUI()
    {
        // Update only the 4 resource displays
        if (influenceValueText != null)
            influenceValueText.text = Config.Influence.ToString();
            
        if (goldValueText != null)
            goldValueText.text = Config.Gold.ToString();
            
        if (magicValueText != null)
            magicValueText.text = Config.Magic.ToString();
            
        if (opinionValueText != null)
            opinionValueText.text = "100"; // Placeholder for opinion
        
        // Update all trade slots
        foreach (TradeSlot slot in tradeSlots)
        {
            if (slot != null)
                slot.UpdateDisplay();
        }
    }
    
    public void OpenMarketplace()
    {
        if (marketplacePanel != null)
            marketplacePanel.SetActive(true);
    }
    
    public void CloseMarketplace()
    {
        if (marketplacePanel != null)
            marketplacePanel.SetActive(false);
    }
    
    public void ExecuteTrade(ResourceType giveType, int giveAmount, ResourceType getType, int getAmount)
    {
        // Check if player has enough resources
        int currentGiveResource = GetResourceValue(giveType);
        if (currentGiveResource < giveAmount)
        {
            Debug.Log($"Not enough {giveType}! Need {giveAmount}, have {currentGiveResource}");
            return;
        }
        
        // Execute the trade
        SetResourceValue(giveType, currentGiveResource - giveAmount);
        int currentGetResource = GetResourceValue(getType);
        SetResourceValue(getType, currentGetResource + getAmount);
        
        Debug.Log($"Trade completed: Gave {giveAmount} {giveType}, received {getAmount} {getType}");
    }
    
    int GetResourceValue(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Influence: return Config.Influence;
            case ResourceType.Gold: return Config.Gold;
            case ResourceType.Magic: return Config.Magic;
            case ResourceType.Opinion: return 100; // Placeholder
            default: return 0;
        }
    }
    
    void SetResourceValue(ResourceType resourceType, int value)
    {
        switch (resourceType)
        {
            case ResourceType.Influence: Config.Influence = value; break;
            case ResourceType.Gold: Config.Gold = value; break;
            case ResourceType.Magic: Config.Magic = value; break;
            case ResourceType.Opinion: break; // Handle opinion later
        }
    }
    
    void OnDestroy()
    {
        CancelInvoke(nameof(UpdateUI));
    }
}