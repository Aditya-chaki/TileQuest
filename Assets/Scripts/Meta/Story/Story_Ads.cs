 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Story_Ads : MonoBehaviour
{
    public GameObject Notification;
    public Button PayToPlay;
    public Button WatchAds;
    void Start()
    {

        if (Config.Energy <= 0)
        {
            Notification.SetActive(true);
        }

        if (PayToPlay != null)
        {
            PayToPlay.onClick.AddListener(Pay);
        }
        if (WatchAds != null)
        {
            WatchAds.onClick.AddListener(Ads);
        }
    }
    void Pay()
    {
         if (Config.currCoin >= 10){
            Notification.SetActive(false);
        }
        
        Config.SetCoin(Config.currCoin - 10 );
        Config.Energy = Config.Energy + 10;
    }
    void Ads(){
        Notification.SetActive(false);
        Config.Energy = Config.Energy + 5;
    }


}
