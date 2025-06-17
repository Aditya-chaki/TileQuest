using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public List<BaseEvent> activeEvents = new List<BaseEvent>();

    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }

    void StartMileStoneEvent()
    {
        var milestone = new MilestoneEvent() {
            eventName = "Level Up Challenge",
            startTime = DateTime.Now,
            endTime = DateTime.Now.AddDays(3),
            requiredFood = 1000,
            requiredGold = 1000,
            requiredMagic = 1000,
        };
        milestone.Initialize();
        activeEvents.Add(milestone);
    }

}
