using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TradeOffer
{
    public string tradeName;
    public ResourceType giveResource;
    public int giveAmount;
    public ResourceType getResource;
    public int getAmount;
    public int maxTrades; // Maximum number of times this trade can be used
    public int currentTrades; // Current number of times used
}

public enum ResourceType
{
    Influence,
    Gold,
    Magic,
    Opinion
}

public class MarketplaceManager : MonoBehaviour
{
    public static MarketplaceManager Instance { get; private set; }

    [Header("Marketplace Settings")]
    public float marketResetTime = 300f; // 5 minutes in seconds
    public int numberOfTradesPerCycle = 3; // How many trades are available at once

    [Header("Current Market Offers")]
    public List<TradeOffer> currentOffers = new List<TradeOffer>();

    [Header("Debug")]
    public bool enableDebugLogs = true;

    // All possible trades that can appear
    private List<TradeOffer> allPossibleTrades = new List<TradeOffer>();
    private string lastMarketResetKey = "LastMarketReset";
    private float timeUntilReset;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePossibleTrades();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadMarketState();
        GenerateNewMarket();
        InvokeRepeating(nameof(UpdateMarketTimer), 1f, 1f);
    }

    void InitializePossibleTrades()
    {
        allPossibleTrades = new List<TradeOffer>
        {
            // Influence to Gold trades
            new TradeOffer { tradeName = "Influence to Gold (Royal Treasury)", giveResource = ResourceType.Influence, giveAmount = 500, getResource = ResourceType.Gold, getAmount = 200, maxTrades = 5 },
            new TradeOffer { tradeName = "Influence to Gold (Quick Sale)", giveResource = ResourceType.Influence, giveAmount = 200, getResource = ResourceType.Gold, getAmount = 60, maxTrades = 10 },
            
            // Gold to Influence trades
            new TradeOffer { tradeName = "Gold to Influence (Bribes)", giveResource = ResourceType.Gold, giveAmount = 300, getResource = ResourceType.Influence, getAmount = 800, maxTrades = 4 },
            new TradeOffer { tradeName = "Gold to Influence (Donations)", giveResource = ResourceType.Gold, giveAmount = 100, getResource = ResourceType.Influence, getAmount = 250, maxTrades = 8 },
            
            // Gold to Magic trades
            new TradeOffer { tradeName = "Gold to Magic (Arcane Artifacts)", giveResource = ResourceType.Gold, giveAmount = 400, getResource = ResourceType.Magic, getAmount = 150, maxTrades = 3 },
            new TradeOffer { tradeName = "Gold to Magic (Spell Components)", giveResource = ResourceType.Gold, giveAmount = 150, getResource = ResourceType.Magic, getAmount = 80, maxTrades = 7 },
            
            // Magic to Gold trades
            new TradeOffer { tradeName = "Magic to Gold (Enchanted Items)", giveResource = ResourceType.Magic, giveAmount = 120, getResource = ResourceType.Gold, getAmount = 350, maxTrades = 4 },
            new TradeOffer { tradeName = "Magic to Gold (Potion Sales)", giveResource = ResourceType.Magic, giveAmount = 60, getResource = ResourceType.Gold, getAmount = 140, maxTrades = 9 },
            
            // Influence to Magic trades
            new TradeOffer { tradeName = "Influence to Magic (Court Wizards)", giveResource = ResourceType.Influence, giveAmount = 600, getResource = ResourceType.Magic, getAmount = 180, maxTrades = 5 },
            new TradeOffer { tradeName = "Influence to Magic (Guild Favors)", giveResource = ResourceType.Influence, giveAmount = 300, getResource = ResourceType.Magic, getAmount = 70, maxTrades = 8 },
            
            // Magic to Influence trades
            new TradeOffer { tradeName = "Magic to Influence (Mystical Display)", giveResource = ResourceType.Magic, giveAmount = 100, getResource = ResourceType.Influence, getAmount = 400, maxTrades = 6 },
            new TradeOffer { tradeName = "Magic to Influence (Healing Services)", giveResource = ResourceType.Magic, giveAmount = 50, getResource = ResourceType.Influence, getAmount = 180, maxTrades = 10 },
            
            // For now, let's remove Opinion trades until we know how your Config handles Opinion
            // We can add them back once we understand your Config structure better
        };
    }

    void LoadMarketState()
    {
        if (PlayerPrefs.HasKey(lastMarketResetKey))
        {
            try
            {
                DateTime lastReset = DateTime.Parse(PlayerPrefs.GetString(lastMarketResetKey));
                TimeSpan timePassed = DateTime.UtcNow - lastReset;
                float timePassedSeconds = (float)timePassed.TotalSeconds;

                if (timePassedSeconds >= marketResetTime)
                {
                    if (enableDebugLogs)
                        Debug.Log("Market reset time exceeded. Generating new market.");
                    timeUntilReset = 0;
                }
                else
                {
                    timeUntilReset = marketResetTime - timePassedSeconds;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading market state: {e.Message}");
                timeUntilReset = 0;
            }
        }
        else
        {
            timeUntilReset = 0;
        }
    }

    void UpdateMarketTimer()
    {
        timeUntilReset -= 1f;
        
        if (timeUntilReset <= 0)
        {
            GenerateNewMarket();
            timeUntilReset = marketResetTime;
        }
    }

    void GenerateNewMarket()
    {
        currentOffers.Clear();
        
        List<TradeOffer> availableTrades = new List<TradeOffer>(allPossibleTrades);
        
        for (int i = 0; i < numberOfTradesPerCycle && availableTrades.Count > 0; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableTrades.Count);
            TradeOffer selectedTrade = availableTrades[randomIndex];
            
            TradeOffer newOffer = new TradeOffer
            {
                tradeName = selectedTrade.tradeName,
                giveResource = selectedTrade.giveResource,
                giveAmount = selectedTrade.giveAmount,
                getResource = selectedTrade.getResource,
                getAmount = selectedTrade.getAmount,
                maxTrades = selectedTrade.maxTrades,
                currentTrades = 0
            };
            
            currentOffers.Add(newOffer);
            availableTrades.RemoveAt(randomIndex);
        }
        
        PlayerPrefs.SetString(lastMarketResetKey, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();
        
        if (enableDebugLogs)
        {
            Debug.Log($"New market generated with {currentOffers.Count} offers:");
            foreach (var offer in currentOffers)
            {
                Debug.Log($"- {offer.tradeName}: {offer.giveAmount} {offer.giveResource} → {offer.getAmount} {offer.getResource} (Max: {offer.maxTrades})");
            }
        }
    }

    public bool MakeTrade(int offerIndex)
    {
        if (offerIndex < 0 || offerIndex >= currentOffers.Count)
        {
            Debug.LogError("Invalid trade offer index");
            return false;
        }

        TradeOffer offer = currentOffers[offerIndex];
        
        if (offer.currentTrades >= offer.maxTrades)
        {
            if (enableDebugLogs)
                Debug.Log($"Trade '{offer.tradeName}' is sold out!");
            return false;
        }

        int currentResource = GetResourceValue(offer.giveResource);
        if (currentResource < offer.giveAmount)
        {
            if (enableDebugLogs)
                Debug.Log($"Not enough {offer.giveResource}. Need {offer.giveAmount}, have {currentResource}");
            return false;
        }

        // Execute the trade
        SetResourceValue(offer.giveResource, currentResource - offer.giveAmount);
        int currentGetResource = GetResourceValue(offer.getResource);
        SetResourceValue(offer.getResource, currentGetResource + offer.getAmount);
        
        offer.currentTrades++;
        
        if (enableDebugLogs)
        {
            Debug.Log($"Trade completed: Gave {offer.giveAmount} {offer.giveResource}, received {offer.getAmount} {offer.getResource}");
            Debug.Log($"Remaining trades for '{offer.tradeName}': {offer.maxTrades - offer.currentTrades}");
        }
        
        return true;
    }

    int GetResourceValue(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Influence: return Config.Influence;
            case ResourceType.Gold: return Config.Gold;
            case ResourceType.Magic: return Config.Magic;
            case ResourceType.Opinion: 
                // For now, return a placeholder value until we know how Opinion works in your Config
                // You'll need to replace this with your actual Opinion calculation
                return 50; // Placeholder value
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
            case ResourceType.Opinion: 
                // For now, do nothing with Opinion until we know how it works
                // You'll need to implement this based on your Config structure
                break;
        }
    }

    // Public getters
    public float GetTimeUntilReset() => timeUntilReset;
    public string GetFormattedTimeUntilReset()
    {
        int minutes = Mathf.FloorToInt(timeUntilReset / 60);
        int seconds = Mathf.FloorToInt(timeUntilReset % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }
    
    public List<TradeOffer> GetCurrentOffers() => currentOffers;
    
    public void ForceResetMarket()
    {
        GenerateNewMarket();
        timeUntilReset = marketResetTime;
    }
}