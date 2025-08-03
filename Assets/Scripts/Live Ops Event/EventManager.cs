using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public GameObject eventButton;
    public BaseEvent activeEvents;
    [SerializeField] private TextMeshProUGUI timerText;
    public Button customLevelButton;
    public Button onetMinigameLevelButton;
    [SerializeField] private TextMeshProUGUI eventText;
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
        customLevelButton.onClick.AddListener(PlayCustomLevel);
        onetMinigameLevelButton.onClick.AddListener(PlayCustomOnet);
        nextResetTime = LoadResetTime();
      
        currentEvent = PlayerPrefs.GetString("ActiveEvent","none");
        int startEvent = PlayerPrefs.GetInt("StartEvent",0);
      
        int isEventStart = PlayerPrefs.GetInt(Config.CURR_LEVEL);

        Debug.Log(currentEvent);

        if(startEvent==1 && currentEvent=="none")
        {
            SelectEventAtRandom();
        }
        else if(currentEvent=="MileStone")
        {
            activeEvents = new MilestoneEvent() {
            eventName = "Kingdom Ascent",
            startTime = DateTime.Now,
            endTime = nextResetTime,
            requiredInfluence = 1000,
            requiredGold = 1000,
            requiredMagic = 1000,
                };
            activeEvents.Initialize(); 
            customLevelButton.gameObject.SetActive(true);
        }
        else if(currentEvent=="CustomLevel")
        {
            activeEvents = new CustomLevelEvent()
            {
            eventName = "The Queen's Challange",
            startTime = DateTime.Now,
            endTime = nextResetTime,
            requiredLevelToComplete = 5,
            };
            activeEvents.Initialize();
        }
        else if(currentEvent=="MinigamesEvent")
        {
            activeEvents = new OnetMiniGamesEvent()
            {
            eventName = "Memory of the Relam",
            startTime = DateTime.Now,
            endTime = nextResetTime,
            requiredLevelToCompleteOnet=5,
            };
            activeEvents.Initialize(); 
            onetMinigameLevelButton.gameObject.SetActive(true);
        }
        Debug.Log(activeEvents.eventName);
        eventText.text = activeEvents.eventName;

      if(isEventStart>3)
      {
        eventButton.SetActive(true);
      }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        if(nextTime<Time.time && activeEvents!=null)
        {
            UpdateAllEvents();
            ClaimReward();
            nextTime  = Time.time+1f;
        }

    }

    public void UpdateAllEvents()
    {
        
     activeEvents.UpdateProgress();
        
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

    public void ClaimReward()
    {
       if (activeEvents != null && activeEvents.IsCompleted())
        {
            activeEvents.ClaimReward();
            activeEvents = null;
            customLevelButton.gameObject.SetActive(false);
            onetMinigameLevelButton.gameObject.SetActive(false);
            SelectEventAtRandom();
        }
    }

    void StartMileStoneEvent()
    {
        PlayerPrefs.SetString("ActiveEvent","MileStone");
        var milestone = new MilestoneEvent() {
            eventName = "Kingdom Ascent",
            startTime = DateTime.Now,
            endTime = DateTime.Now.AddDays(3),
            requiredInfluence = 1000,
            requiredGold = 1000,
            requiredMagic = 1000,
        };
        daysForEvent = milestone.endTime;
        milestone.Initialize();
        activeEvents = milestone;
        PlayerPrefs.SetInt("StartEvent",1);
        eventText.text = activeEvents.eventName;
    }

    void StartCustomLevelEvent()
    {
        PlayerPrefs.SetString("ActiveEvent","CustomLevel");
        var customLevel = new CustomLevelEvent()
        {
            eventName = "The Queen's Challange",
            startTime = DateTime.Now,
            endTime = DateTime.Now.AddDays(2),
            requiredLevelToComplete = 5,
        };
        customLevel.Initialize();
        activeEvents = customLevel;
        customLevelButton.gameObject.SetActive(true);
        PlayerPrefs.SetInt("StartEvent",2);
        daysForEvent = customLevel.endTime;
        eventText.text = activeEvents.eventName;
    }

    void StartMinigameEvent()
    {
        PlayerPrefs.SetString("ActiveEvent","MinigamesEvent");
        var minigameEvent = new OnetMiniGamesEvent()
        {
            eventName = "Memory of the Relam",
            startTime = DateTime.Now,
            endTime = DateTime.Now.AddDays(2),
            requiredLevelToCompleteOnet=5,
        };
        minigameEvent.Initialize();
        activeEvents = minigameEvent;
        PlayerPrefs.SetInt("StartEvent",3);
        daysForEvent = minigameEvent.endTime;
        onetMinigameLevelButton.gameObject.SetActive(true);
        eventText.text = activeEvents.eventName;
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
        SelectEventAtRandom();
    }

    public void PlayCustomLevel()
    {
        SceneManager.LoadScene("CustomEventTilematch");
    }

    public void PlayCustomOnet()
    {
        SceneManager.LoadScene("CustomLiveEventOnet");
    }

}
