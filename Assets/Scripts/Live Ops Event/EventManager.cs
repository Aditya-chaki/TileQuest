using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public GameObject eventButton;
    public List<BaseEvent> activeEvents = new List<BaseEvent>();
    [SerializeField] private TextMeshProUGUI timerText;
    public GameObject customLevelButton;
    
    private const string RESET_TIME_PREF_KEY = "EventRestTime";
    private string currentEvent;
    private DateTime nextResetTime; 
    private DateTime daysForEvent;

    float nextTime;

    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {   
        nextResetTime = LoadResetTime();
        currentEvent = PlayerPrefs.GetString("ActiveEvent","none");
        int startEvent = PlayerPrefs.GetInt("StartEvent",0);
      
        int isEventStart = PlayerPrefs.GetInt(Config.CURR_LEVEL);
      
        if(startEvent==1 && activeEvents.Count == 0)
        {
            SelectEventAtRandom();
        }
        

      if(isEventStart>3)
      {
        eventButton.SetActive(true);
      }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        if(nextTime<Time.time)
        {
            UpdateAllEvents();
            nextTime  = Time.time+1f;
        }

    }

    public void UpdateAllEvents()
    {
        foreach (var e in activeEvents)
        {
            if (e.IsActive)
                e.UpdateProgress();
        }
    }

    void SelectEventAtRandom()
    {
        int rand = UnityEngine.Random.Range(0,4);
        if(rand==0)
        {
            StartMileStoneEvent();
        }
        else if(rand==1)
        {
            StartCustomLevelEvent();
        }
        else
        {
            StartMinigameEvent();
        }
    }

    public void ClaimReward(string eventName)
    {
        var e = activeEvents.Find(evt => evt.eventName == eventName);
        if (e != null && e.IsCompleted())
        {
            e.ClaimReward();
            activeEvents.Remove(e);
            customLevelButton.SetActive(false);
        }
    }

    void StartMileStoneEvent()
    {
        PlayerPrefs.SetString("ActiveEvent","MileStone");
        var milestone = new MilestoneEvent() {
            eventName = "Resource Collection",
            startTime = DateTime.Now,
            endTime = DateTime.Now.AddDays(3),
            requiredFood = 1000,
            requiredGold = 1000,
            requiredMagic = 1000,
        };
        daysForEvent = milestone.endTime;
        milestone.Initialize();
        activeEvents.Add(milestone);
        PlayerPrefs.SetInt("StartEvent",1);
    }

    void StartCustomLevelEvent()
    {
        PlayerPrefs.SetString("ActiveEvent","CustomLevel");
        var customLevel = new CustomLevelEvent()
        {
            eventName = "Custom Level Challange",
            startTime = DateTime.Now,
            endTime = DateTime.Now.AddDays(2),
            requiredLevelToComplete = 5,
        };
        customLevel.Initialize();
        activeEvents.Add(customLevel);
        customLevelButton.SetActive(true);
        PlayerPrefs.SetInt("StartEvent",2);
        daysForEvent = customLevel.endTime;
    }

    void StartMinigameEvent()
    {
        PlayerPrefs.SetString("ActiveEvent","MinigamesEvent");
        var minigameEvent = new MiniGamesEvent()
        {
            eventName = "MinigamesEvent Challange",
            startTime = DateTime.Now,
            endTime = DateTime.Now.AddDays(2),
            requiredLevelToCompleteWaterSort = 5,
            requiredLevelToCompleteOnet=5,
        };
        minigameEvent.Initialize();
        activeEvents.Add(minigameEvent);
        PlayerPrefs.SetInt("StartEvent",3);
        daysForEvent = minigameEvent.endTime;
    }


    private DateTime LoadResetTime()
    {
        string resetTimeString = PlayerPrefs.GetString(RESET_TIME_PREF_KEY, string.Empty);
        if (!string.IsNullOrEmpty(resetTimeString))
        {
            return DateTime.Parse(resetTimeString);
        }

        // If reset time doesn't exist, set the next reset to be 7 days from now
        DateTime newResetTime = daysForEvent;
        SaveResetTime(newResetTime);
        return newResetTime;
    }

    // Method to save the reset time to PlayerPrefs
    private void SaveResetTime(DateTime resetTime)
    {
        PlayerPrefs.SetString(RESET_TIME_PREF_KEY, resetTime.ToString());
        PlayerPrefs.Save();
    }

   
     // Update the timer text
    private void UpdateTimer()
    {
        TimeSpan timeRemaining = nextResetTime - DateTime.UtcNow;

        if (timeRemaining.TotalSeconds > 0)
        {
            timerText.text = $"{timeRemaining.Days:D2}D {timeRemaining.Hours:D2}H {timeRemaining.Minutes:D2}M {timeRemaining.Seconds:D2}s";
        }
        else
        {
            timerText.text = "Resetting...";
            ResetTimer();
            
        }
    }

    private void ResetTimer()
    {
        nextResetTime = daysForEvent;
        SaveResetTime(nextResetTime);
    }


}
