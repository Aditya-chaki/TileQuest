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
    public int minTime = 20;
    public int maxTime = 150;
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
        randtime = Random.Range(minTime,maxTime);
    }

    void Update()
    {
        if(isRandomTime)
        {
            if(Time.time>nexttime)
            {
                randtime--;
                nexttime = Time.time+1;
                ActiveButton();
            }
        }
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

    void ActiveButton()
    {
        miniGameButton.gameObject.SetActive(true);
        miniGameButton.GetComponent<RectTransform>().DOShakePosition(0.5f,5f,10,90,false,true);
           
    }

    public void LoadMiniGame()
    {
        if(isRandomTime){
            randtime =  Random.Range(minTime,maxTime);
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
