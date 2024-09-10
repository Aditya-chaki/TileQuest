/*
 * Created on 2022
 *
 * Copyright (c) 2022 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */

//using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using GoogleMobileAds.Api;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager instance;
    



    public enum ADS_CALLBACK_STATE
    {
        SUCCESS,
        FAIL
    }

   

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        

        Advertisements.Instance.Initialize();

        //Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM, BannerType.Banner);

    }

   


 
}
