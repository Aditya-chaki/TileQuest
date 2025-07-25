using System;

[System.Serializable]
public class TradeOffer
{
    public string tradeName;
    public ResourceType giveResource;
    public int giveAmount;
    public ResourceType getResource;
    public int getAmount;
    public int maxTrades;
    public int currentTrades;
}

public enum ResourceType
{
    Influence,
    Gold,
    Magic,
    Opinion
}