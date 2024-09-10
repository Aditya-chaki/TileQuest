using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEnabler : MonoBehaviour
{
    public GameObject scene;
   
    public void SceneDisable()
    {
        scene.SetActive(false);
    }
    public void SceneEnable()
    {
        scene.SetActive(true);
    }
    
    public void MenuEnabler()
    {
         SceneManager.LoadScene("Menu");
    }
    public void Pay(){
        
        if (Config.currCoin >= 10){
            scene.SetActive(false);
        }
        
        Config.SetCoin(Config.currCoin - 10 );
    }

}
