using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEnabler : MonoBehaviour
{
<<<<<<< HEAD
    public GameObject scene;
   
    public void SceneDisable()
    {
        scene.SetActive(false);
    }
    public void SceneEnable()
    {
        scene.SetActive(true);
    }
    
=======
    public GameObject Daily_Quest_scene;
    public GameObject Weekly_Quest_scene;
    public GameObject Quest_List_scene;



    public void Open_Daily_Quest_Scene()
    {
        Scene_Enable(true, false, false);
    }
    public void Open_Weekly_Quest_Scene()
    {
        Scene_Enable(false, true, false);
    }
    public void Quest_List_Scene_Open()
    {
        Scene_Enable(false, false, true);
        Debug.Log("Open quest Scene");
    }


>>>>>>> b97c403d (Daily Quest reset update & Managed scenes)
    public void MenuEnabler()
    {
         SceneManager.LoadScene("Menu");
    }
<<<<<<< HEAD
    public void Pay(){
        
        if (Config.currCoin >= 10){
            scene.SetActive(false);
=======


    public void Pay(){
        
        if (Config.currCoin >= 10){
            Daily_Quest_scene.SetActive(false);
>>>>>>> b97c403d (Daily Quest reset update & Managed scenes)
        }
        
        Config.SetCoin(Config.currCoin - 10 );
    }

<<<<<<< HEAD
=======

    public void Scene_Enable(bool daily_quest_Scene, bool weekly_quest_Scene,bool quest_list)
    {
        Daily_Quest_scene.SetActive(daily_quest_Scene);
        Weekly_Quest_scene.SetActive(weekly_quest_Scene);
        Quest_List_scene.SetActive(quest_list);
    } 
    
  
   
>>>>>>> b97c403d (Daily Quest reset update & Managed scenes)
}
