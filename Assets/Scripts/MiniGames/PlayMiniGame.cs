using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayMiniGame : MonoBehaviour
{
    public Button miniGameButton; 
    public string miniGameSceneName;

    
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

     void Start()
    {
        if (miniGameButton != null)
        {
            miniGameButton.onClick.AddListener(LoadMiniGame);
        }
    }


    public void LoadMiniGame()
    {
        SceneManager.LoadSceneAsync(miniGameSceneName);
    }

}
