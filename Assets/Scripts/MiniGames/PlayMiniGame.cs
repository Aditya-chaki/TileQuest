using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class PlayMiniGame : MonoBehaviour
{
    [SerializeField] private Button miniGameButton; 
    [SerializeField] private string miniGameSceneName;
    [SerializeField] private bool isValueType;
    [SerializeField] private bool isRandomTime;
    [SerializeField] private int minTime = 20;
    [SerializeField] private int maxTime = 150;
    [SerializeField] private float metricValue;
    [SerializeField] private string Metric;
    private int randtime;
    private float nexttime;
    private float requiredValue;
    
    private void Start()
    {
        if (miniGameButton != null)
        {
            miniGameButton.onClick.AddListener(LoadMiniGame);
        }
        if(Metric == "Magic")
        {
          requiredValue = Config.Magic + metricValue;
        }
        if(Metric == "Influence")
        {
            requiredValue = Config.Influence + metricValue;   
        }
        if(Metric == "Gold")
        {
            requiredValue = Config.Gold + metricValue;
        }
        randtime = UnityEngine.Random.Range(minTime,maxTime);
    }

    void Update()
    {
        if(isRandomTime)
        {
            if(Time.time>nexttime)
            {
                randtime--;
                nexttime = Time.time+1;
                if(randtime<=0)
                    ActiveButton();
            }
        }
        if(isValueType)
        {
            if(Metric == "Magic"&&Config.Magic>=requiredValue)
            {
              ActiveButton();
            }
            if(Metric == "Gold"&&Config.Gold>=requiredValue)
            {
                ActiveButton();
            }
            if(Metric == "Influence"&&Config.Influence>=requiredValue)
            {
                ActiveButton();   
            }
        }

    }

    void ActiveButton()
    {
        miniGameButton.gameObject.SetActive(true);
        miniGameButton.GetComponent<RectTransform>().DOShakePosition(0.5f,5f,10,90,false,true);
           
    }

    public void LoadMiniGame()
    {
        if(isRandomTime){
            randtime =  UnityEngine.Random.Range(minTime,maxTime);
            SceneManager.LoadSceneAsync(miniGameSceneName);
        }
        if(isValueType)
        {
            if(Metric == "Influence")
            {
                if(Config.Influence>=requiredValue)
                {
                    SceneManager.LoadSceneAsync(miniGameSceneName);
                }
            }
            if(Metric == "Magic")
            {
                if(Config.Magic>=requiredValue)
                {
                  SceneManager.LoadSceneAsync(miniGameSceneName);  
                }
            }
            if(Metric == "Gold")
            {
                if(Config.Gold>=requiredValue)
                {
                    SceneManager.LoadSceneAsync(miniGameSceneName);
                }
            }
        }
    }

}
