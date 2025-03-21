﻿/*
 * Created on 2022
 *
 * Copyright (c) 2022 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //  Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE, 1);
        //  Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST,  1);
        //  Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO,  1);

        // Config.Food = 500;
        //     Config.Strength =500;
        //     Config.Gold =500;
        //     Config.Health = 500;
         //Config.Energy=30;
        // Config.SetCoin(1000);
        // Config.ResetLevel();
        Config.SetCurrLevel(1);
        Config.GetSound();
        Config.GetMusic();
        Config.currCoin = Config.GetCoin();
        Config.GetCurrLevel();
        Config.currPiggyBankCoin = Config.GetPiggyBank();
        Config.GetLevelStar();
        if (Config.isMusic)
        {
            MusicManager.instance.PlayMusicBG();
        }
      
        //TODO: Check Chest Star
        //Config.SetChestCountStar(15);
        StartCoroutine(LoadMenuScene_IEnumerator());
          
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator LoadMenuScene_IEnumerator()
    {
        yield return new WaitForSeconds(2f);
        LoadMenuScene();
    }
    bool isLoadMenu = false;
    public void LoadMenuScene()
    {
        if (!isLoadMenu)
        {
            isLoadMenu = true;
           
            if (Config.currLevel == 1)
            {
                SceneManager.LoadSceneAsync("Play");
                
            }
            else
            {
                SceneManager.LoadSceneAsync("Menu");
                
            }
            //SceneManager.LoadSceneAsync("Menu");
        }
    }
     // Public method to load any scene
    public void LoadScene(string sceneName)
    {
        // isLoadMenu = false;
        SceneManager.LoadSceneAsync(sceneName);
    }
}
