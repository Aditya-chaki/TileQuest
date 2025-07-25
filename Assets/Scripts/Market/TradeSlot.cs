using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeSlot : MonoBehaviour
{
    [Header("UI References")]
    public Image backgroundImage;
    public TextMeshProUGUI tradeDescriptionText;
    public Button rewardButton;
    public Image giveResourceIcon;
    public Image getResourceIcon;
    
    [Header("Resource Icons")]
    public Sprite influenceIcon;
    public Sprite goldIcon;
    public Sprite magicIcon;
    public Sprite opinionIcon;
    
    [Header("Button States")]
    public Color availableColor = Color.white;
    public Color unavailableColor = Color.gray;
    public Color soldOutColor = Color.red;
    
    private ResourceType giveResource;
    private int giveAmount;
    private ResourceType getResource;
    private int getAmount;
    private int maxTrades;
    private int currentTrades = 0;
    private SimpleMarketplaceUI marketplaceUI;
    
    public void SetupTrade(ResourceType giveType, int giveAmt, ResourceType getType, int getAmt, int maxTradeCount, SimpleMarketplaceUI ui)
    {
        giveResource = giveType;
        giveAmount = giveAmt;
        getResource = getType;
        getAmount = getAmt;
        maxTrades = maxTradeCount;
        currentTrades = 0;
        marketplaceUI = ui;
        
        UpdateDisplay();
        
        if (rewardButton != null)
        {
            rewardButton.onClick.RemoveAllListeners();
            rewardButton.onClick.AddListener(OnRewardButtonClicked);
        }
    }
    
    public void UpdateDisplay()
    {
        if (tradeDescriptionText != null)
        {
            string description = $"Trade {giveAmount} {GetResourceName(giveResource)}\nfor {getAmount} {GetResourceName(getResource)}\n({maxTrades - currentTrades} left)";
            tradeDescriptionText.text = description;
        }
        
        SetResourceIcon(giveResourceIcon, giveResource);
        SetResourceIcon(getResourceIcon, getResource);
        UpdateButtonState();
    }
    
    string GetResourceName(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Influence: return "Influence";
            case ResourceType.Gold: return "Gold";
            case ResourceType.Magic: return "Magic";
            case ResourceType.Opinion: return "Opinion";
            default: return "Unknown";
        }
    }
    
    void SetResourceIcon(Image iconImage, ResourceType resourceType)
    {
        if (iconImage == null) return;
        
        switch (resourceType)
        {
            case ResourceType.Influence:
                if (influenceIcon != null) iconImage.sprite = influenceIcon;
                break;
            case ResourceType.Gold:
                if (goldIcon != null) iconImage.sprite = goldIcon;
                break;
            case ResourceType.Magic:
                if (magicIcon != null) iconImage.sprite = magicIcon;
                break;
            case ResourceType.Opinion:
                if (opinionIcon != null) iconImage.sprite = opinionIcon;
                break;
        }
    }
    
    void UpdateButtonState()
    {
        bool soldOut = currentTrades >= maxTrades;
        bool canAfford = GetResourceValue(giveResource) >= giveAmount;
        
        if (rewardButton != null)
        {
            rewardButton.interactable = !soldOut && canAfford;
            
            TextMeshProUGUI buttonText = rewardButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                if (soldOut)
                    buttonText.text = "SOLD OUT";
                else if (!canAfford)
                    buttonText.text = "CAN'T AFFORD";
                else
                    buttonText.text = "Reward";
            }
        }
        
        if (backgroundImage != null)
        {
            if (soldOut)
                backgroundImage.color = soldOutColor;
            else if (!canAfford)
                backgroundImage.color = unavailableColor;
            else
                backgroundImage.color = availableColor;
        }
    }
    
    void OnRewardButtonClicked()
    {
        if (currentTrades >= maxTrades || GetResourceValue(giveResource) < giveAmount)
            return;
        
        if (marketplaceUI != null)
        {
            marketplaceUI.ExecuteTrade(giveResource, giveAmount, getResource, getAmount);
            currentTrades++;
            UpdateDisplay();
        }
    }
    
    int GetResourceValue(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Influence: return Config.Influence;
            case ResourceType.Gold: return Config.Gold;
            case ResourceType.Magic: return Config.Magic;
            case ResourceType.Opinion: return 100;
            default: return 0;
        }
    }
}