using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleBannerAdsManager : MonoBehaviour
{
    private static GoogleBannerAdsManager m_pInstance = null;
    public static GoogleBannerAdsManager Instance { get { return m_pInstance; } }

    public string m_strAdUnitID = "";

    // 배너광고 높이는 약 157

    void Awake()
    {
        m_pInstance = this;
    }

    public void RequestBanner()
    {
        //if (GameDefine.ms_IsUseAds == true)
        {
            BannerView bannerview = new BannerView(m_strAdUnitID, AdSize.SmartBanner, AdPosition.Bottom);

            AdRequest request = new AdRequest.Builder().Build();
            bannerview.LoadAd(request);
        }
    }
}
