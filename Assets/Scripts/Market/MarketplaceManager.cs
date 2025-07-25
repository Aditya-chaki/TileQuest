using System;
using UnityEngine;

public class MarketplaceManager : MonoBehaviour
{
    public static MarketplaceManager Instance { get; private set; }

    [Header("Marketplace Settings")]
    public float marketResetTime = 300f;
    public int numberOfTradesPerCycle = 4;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    private float timeUntilReset;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        timeUntilReset = marketResetTime;
        InvokeRepeating(nameof(UpdateMarketTimer), 1f, 1f);
    }

    void UpdateMarketTimer()
    {
        timeUntilReset -= 1f;
        
        if (timeUntilReset <= 0)
        {
            timeUntilReset = marketResetTime;
            if (enableDebugLogs)
                Debug.Log("Market reset! New trades available.");
        }
    }

    public string GetFormattedTimeUntilReset()
    {
        int minutes = Mathf.FloorToInt(timeUntilReset / 60);
        int seconds = Mathf.FloorToInt(timeUntilReset % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }
}