using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseEvent { 
    public string eventName;
    public DateTime startTime;
    public DateTime endTime;
    public bool IsActive => DateTime.Now >= startTime && DateTime.Now <= endTime;

    public abstract void Initialize();
    public abstract void UpdateProgress();
    public abstract bool IsCompleted();
    public abstract void ClaimReward();
}
