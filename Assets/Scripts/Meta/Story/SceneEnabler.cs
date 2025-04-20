using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEnabler : MonoBehaviour
{
  
    public GameObject Quest_List_scene;
    public GameObject Daily_Quest_Scene;
    public GameObject Weekly_Quest_scene;

    //**********************************************
    public void Quest_list_Enable()
    {
        Scene_Enable_Disable(true, false, false);
    }
   
   
    //************************************************
    public void Daily_Quest_Enable()
    {
        Scene_Enable_Disable(false, true, false);
    }
  
 
    //*************************************************
    public void Weekly_Quest_Enable()
    {
        Scene_Enable_Disable(false, false, true);
    }

 
   
    //*************************************************
    public void MenuEnabler()
    {
         SceneManager.LoadScene("Menu");
    }

    //************************************************
    public void Pay(){
        
        if (Config.currCoin >= 10){
            Daily_Quest_Scene.SetActive(false);
        }
        
        Config.SetCoin(Config.currCoin - 10 );
    }

    public void Scene_Enable_Disable(bool quest_list,bool daily_quest,bool weekly_quest )
    {
        Quest_List_scene.SetActive(quest_list);
        Daily_Quest_Scene.SetActive(daily_quest);
        Weekly_Quest_scene.SetActive(weekly_quest);
    }
}
