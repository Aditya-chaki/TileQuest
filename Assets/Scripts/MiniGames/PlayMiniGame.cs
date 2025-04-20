using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
<<<<<<< HEAD
using DG.Tweening;
=======
>>>>>>> b97c403d (Daily Quest reset update & Managed scenes)
public class PlayMiniGame : MonoBehaviour
{
    public Button miniGameButton; 
    public string miniGameSceneName;
<<<<<<< HEAD
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
=======

    
    public enum METRIC_TYPE
    {
        NONE,
        FOOD,
        STRENGTH,
        HEALTH,
        GOLD,
        ENERGY,
    }

    public METRIC_TYPE metricType;
>>>>>>> b97c403d (Daily Quest reset update & Managed scenes)

     void Start()
    {
        if (miniGameButton != null)
        {
            miniGameButton.onClick.AddListener(LoadMiniGame);
        }
<<<<<<< HEAD
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
=======
    }


    public void LoadMiniGame()
    {
        SceneManager.LoadSceneAsync(miniGameSceneName);
>>>>>>> b97c403d (Daily Quest reset update & Managed scenes)
    }

}
