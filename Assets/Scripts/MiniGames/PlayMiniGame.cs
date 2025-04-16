using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class PlayMiniGame : MonoBehaviour
{
    public Button miniGameButton; 
    public string miniGameSceneName;
    public bool isValueType;
    public bool isRandomTime;
    public float metricValue;
    public enum METRIC_TYPE
    {
        NONE,
        Food,
        STRENGTH,
        GOLD,
        ENERGY,
        HEALTH,
    }

    public METRIC_TYPE metricType;
    public string Metric;
    int randtime;
    float nexttime;

     void Start()
    {
        if (miniGameButton != null)
        {
            miniGameButton.onClick.AddListener(LoadMiniGame);
        }
        randtime = Random.Range(15,55);
    }

    void Update()
    {
        if(isRandomTime)
        {
            if(Time.time>nexttime)
            {
                randtime--;
                nexttime = Time.time;
                miniGameButton.gameObject.SetActive(true);
                miniGameButton.GetComponent<RectTransform>().DOShakePosition(0.5f,5f,10,90,false,true);
            }
        }
    }

    public void LoadMiniGame()
    {
        if(isRandomTime){
            randtime =  Random.Range(15,55);
            SceneManager.LoadSceneAsync(miniGameSceneName);
        }
        if(isValueType)
        {
            if(Metric == "Food")
            {
                if(Config.Food>=metricValue)
                {
                    SceneManager.LoadSceneAsync(miniGameSceneName);
                }
            }
            if(Metric == "Health")
            {
             if(Config.Health>=metricValue)
                {
                   SceneManager.LoadSceneAsync(miniGameSceneName); 
                }   
            }
            if(Metric == "Strength")
            {
                if(Config.Strength>=metricValue)
                {
                  SceneManager.LoadSceneAsync(miniGameSceneName);  
                }
            }
            if(Metric == "Gold")
            {
                if(Config.Gold>=metricValue)
                {
                    SceneManager.LoadSceneAsync(miniGameSceneName);
                }
            }
            if(Metric == "Energy")
            {
             if(Config.Energy>=metricValue)
                {
                    SceneManager.LoadSceneAsync(miniGameSceneName);
                }   
            }
        }
    }

}
