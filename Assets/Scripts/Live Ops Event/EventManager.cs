using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public GameObject eventButton;
    public List<BaseEvent> activeEvents = new List<BaseEvent>();

    public GameObject customLevelButton;

    private string currentEvent;

    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
      currentEvent = PlayerPrefs.GetString("ActiveEvent");
      int startEvent = PlayerPrefs.GetInt("StartEvent",0);
      
      int isEventStart = PlayerPrefs.GetInt(Config.CURR_LEVEL);
      
      if(startEvent==1 && activeEvents.Count == 0)
      {
        Debug.Log("Mile stone event start");
        StartMileStoneEvent();
      }
      if(isEventStart>3)
      {
        eventButton.SetActive(true);
      }

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void UpdateAllEvents()
    {
        foreach (var e in activeEvents)
        {
            if (e.IsActive)
                e.UpdateProgress();
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
    }

}
