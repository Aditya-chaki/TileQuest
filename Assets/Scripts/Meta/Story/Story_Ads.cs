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

        if (Config.Magic <= 0)
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
        Config.Magic = Config.Magic + 10;
    }
    void Ads(){
        Notification.SetActive(false);
        Config.Magic = Config.Magic + 5;
    }


}
