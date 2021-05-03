using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdsManager : MonoBehaviour
{
    private static GoogleAdsManager m_pInstance = null;
    public static GoogleAdsManager Instance { get { return m_pInstance; } }

    private InterstitialAd m_Interstitial;
    public string strAppID = "";
    public string m_strAdUnitID = "";

    private void Awake()
    {
        m_pInstance = this;
    }

    void Start()
    {

    }

    void Update()
    {

    }

	public void Initialize()
	{
        MobileAds.Initialize(strAppID);

        this.m_Interstitial = new InterstitialAd(m_strAdUnitID);
        this.m_Interstitial.OnAdLoaded += HandleOnAdLoaded;
        this.m_Interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        this.m_Interstitial.OnAdOpening += HandleOnAdOpened;
        this.m_Interstitial.OnAdClosed += HandleOnAdClosed;
        this.m_Interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        RequestInterstitial();
    }

	private void RequestInterstitial()
    {
        AdRequest request = new AdRequest.Builder().Build();

        this.m_Interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        OutputLog.Log("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        OutputLog.Log("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        OutputLog.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        OutputLog.Log("HandleAdClosed event received");
        RequestInterstitial();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        OutputLog.Log("HandleAdLeavingApplication event received");
    }

    public void AdsShow()
    {
        if (m_Interstitial != null)
        {
            if (m_Interstitial.IsLoaded())
            {
                m_Interstitial.Show();
            }
            else
            {
                RequestInterstitial();
            }
        }
    }
}