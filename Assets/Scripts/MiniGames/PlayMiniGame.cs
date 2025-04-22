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
    float requiredValue;
     void Start()
    {
        if (miniGameButton != null)
        {
            miniGameButton.onClick.AddListener(LoadMiniGame);
        }
        if(Metric == "Food")
        {
          requiredValue = Config.Food + metricValue;
        }
        if(Metric == "Health")
        {
            requiredValue = Config.Health + metricValue;   
        }
        if(Metric == "Strength")
        {
            requiredValue = Config.Strength + metricValue;
        }
        if(Metric == "Gold")
        {
            requiredValue = Config.Gold + metricValue;
        }
        if(Metric == "Energy")
        {
            requiredValue = Config.Energy + metricValue;   
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
                ActiveButton();
            }
        }
        if(Metric == "Food"&&Config.Food>=requiredValue)
        {
          ActiveButton();
        }
        if(Metric == "Health"&&Config.Health>=requiredValue)
        {
            ActiveButton();   
        }
        if(Metric == "Strength"&&Config.Strength>=requiredValue)
        {
            ActiveButton();
        }
        if(Metric == "Gold"&&Config.Gold>=requiredValue)
        {
           ActiveButton();
        }
        if(Metric == "Energy"&&Config.Energy>=requiredValue)
        {
            ActiveButton();   
        }


    }

    void ActiveButton()
    {
        miniGameButton.gameObject.SetActive(true);
        miniGameButton.GetComponent<RectTransform>().DOShakePosition(0.25f,25f,5,45,false,true);
           
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
                if(Config.Food>=requiredValue)
                {
                    SceneManager.LoadSceneAsync(miniGameSceneName);
                }
            }
            if(Metric == "Health")
            {
             if(Config.Health>=requiredValue)
                {
                   SceneManager.LoadSceneAsync(miniGameSceneName); 
                }   
            }
            if(Metric == "Strength")
            {
                if(Config.Strength>=requiredValue)
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
            if(Metric == "Energy")
            {
             if(Config.Energy>=requiredValue)
                {
                    SceneManager.LoadSceneAsync(miniGameSceneName);
                }   
            }
        }
    }

}
