using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiveEventUIManager : MonoBehaviour
{
    public GameObject mileStoneUI;
    public GameObject customLevelEventUI;
    public GameObject minigameEventUI;
    public GameObject eventPanel;
    public Button liveEventButton;

    private string activeEvents;
    private GameObject currentEventUI;
    // Start is called before the first frame update
    void Start()
    {
        activeEvents = PlayerPrefs.GetString("ActiveEvent");
        
        liveEventButton.onClick.AddListener(ShowActiveEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowActiveEvent()
    {
        
        eventPanel.SetActive(true);
        if(activeEvents=="MileStone")
        {
            currentEventUI = mileStoneUI;
            mileStoneUI.SetActive(true);
        }
        else if(activeEvents=="CustomLevel")
        {
            customLevelEventUI.SetActive(true);
            currentEventUI = customLevelEventUI;
            Debug.Log(activeEvents+" LiveUi"+currentEventUI.name);
        }
        else if(activeEvents=="MinigamesEvent")
        {
            currentEventUI = minigameEventUI;
            minigameEventUI.SetActive(true);
        }

    }

    public void Back()
    {
        currentEventUI.SetActive(false);
        eventPanel.SetActive(false);
        currentEventUI = null;
    }
}
