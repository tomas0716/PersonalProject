using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleRewardedAdsManager : MonoBehaviour
{
    private static GoogleRewardedAdsManager m_pInstance = null;
    public static GoogleRewardedAdsManager Instance { get { return m_pInstance; } }

    private RewardedAd          m_RewardedAd;
    public string               m_strAdUnitID           = "";

    private Coroutine           m_pCoroutine_Show       = null;
    private Transformer_Timer   m_pTimer_ShowTimeout    = new Transformer_Timer();

    private bool                m_IsRequestShow         = false;
    private bool                m_IsSuccess             = false;

    private void Awake()
    {
        m_pInstance = this;
    }

    public void Start()
    {
    }

    public void Initialize()
    {
        if (GameDefine.ms_IsUseAds == true)
        {
            ReloadAd();
        }
    }

    private void Update()
	{
        m_pTimer_ShowTimeout.Update(Time.deltaTime);
    }

	private void Handle(RewardedAd videoAd)
    {
        videoAd.OnAdLoaded += HandleOnAdLoaded;
        videoAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        videoAd.OnAdFailedToShow += HandleOnAdFailedToShow;
        videoAd.OnAdOpening += HandleOnAdOpening;
        videoAd.OnAdClosed += HandleOnAdClosed;
        videoAd.OnUserEarnedReward += HandleOnUserEarnedReward;
    }

    public void ReloadAd()
    {
        OutputLog.Log("GoogleRewardedAdsManager ReloadAd 01");

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            OutputLog.Log("GoogleRewardedAdsManager ReloadAd 02");

            m_RewardedAd = new RewardedAd(m_strAdUnitID);
            Handle(m_RewardedAd);
            AdRequest request = new AdRequest.Builder().Build();
            m_RewardedAd.LoadAd(request);

            OutputLog.Log("GoogleRewardedAdsManager ReloadAd 03");
        }
        else
        {
            AppInstance.Instance.m_pEventDelegateManager.OnGoogleRewardsAdsLoadFailed();

            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
            ob = GameObject.Instantiate(ob);
            MessageBox pMessageBox = ob.GetComponent<MessageBox>();
            pMessageBox.Initialize(MessageBox.eMessageBoxType.OK, "", Helper.GetTextDataString(eTextDataType.RewardAdsLoadFailed_Desc), Helper.GetTextDataString(eTextDataType.OK), "");
        }
    }

    public void Show()
    {
        if (GameDefine.ms_IsUseAds == true)
        {
            m_IsSuccess = false;

#if UNITY_EDITOR
            AppInstance.Instance.m_pEventDelegateManager.OnGoogleRewardsAdsComplete();
#else
            ReloadAd();
#endif
        }
        else
        {
            AppInstance.Instance.m_pEventDelegateManager.OnGoogleRewardsAdsComplete();
        }
    }

    private void Show_Android()
    {
        m_IsRequestShow = true;
        
        m_pTimer_ShowTimeout.OnReset();
        TransformerEvent_Timer eventValue;
        eventValue = new TransformerEvent_Timer(10);
        m_pTimer_ShowTimeout.AddEvent(eventValue);
        m_pTimer_ShowTimeout.SetCallback(null, OnDone_Timer_ShowTimeout);
        m_pTimer_ShowTimeout.OnPlay();

        m_pCoroutine_Show = StartCoroutine("ShowRewardAd");
    }

    private void OnDone_Timer_ShowTimeout(TransformerEvent eventValue)
    {
        StopCoroutine(m_pCoroutine_Show);
        m_pCoroutine_Show = null;

        AppInstance.Instance.m_pEventDelegateManager.OnGoogleRewardsAdsLoadFailed();

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
        ob = GameObject.Instantiate(ob);
        MessageBox pMessageBox = ob.GetComponent<MessageBox>();
        pMessageBox.Initialize(MessageBox.eMessageBoxType.OK, "", Helper.GetTextDataString(eTextDataType.RewardAdsLoadFailed_Desc), Helper.GetTextDataString(eTextDataType.OK), "");
        m_IsSuccess = false;

        //ReloadAd();
    }

    private IEnumerator ShowRewardAd()
    {
        m_IsSuccess = false;

        while (!m_RewardedAd.IsLoaded())
        {
            yield return null;
        }

        m_pTimer_ShowTimeout.OnStop();
        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();
        m_RewardedAd.Show();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        // 동영상 로드 완료
        Show_Android();
    }

    public void HandleOnAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        // 동영상 로드 실패
        AppInstance.Instance.m_pEventDelegateManager.OnGoogleRewardsAdsLoadFailed();

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
        ob = GameObject.Instantiate(ob);
        MessageBox pMessageBox = ob.GetComponent<MessageBox>();
        pMessageBox.Initialize(MessageBox.eMessageBoxType.OK, "", Helper.GetTextDataString(eTextDataType.RewardAdsLoadFailed_Desc), Helper.GetTextDataString(eTextDataType.OK), "");
    }

    public void HandleOnAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        // 동영상 시청 실패
        AppInstance.Instance.m_pEventDelegateManager.OnGoogleRewardsAdsLoadFailed();

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
        ob = GameObject.Instantiate(ob);
        MessageBox pMessageBox = ob.GetComponent<MessageBox>();
        pMessageBox.Initialize(MessageBox.eMessageBoxType.OK, "", Helper.GetTextDataString(eTextDataType.RewardAdsLoadFailed_Desc), Helper.GetTextDataString(eTextDataType.OK), "");
    }

    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        // 동영상 시청 시작
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        OutputLog.Log("HandleOnAdClosed");

        if (m_IsSuccess == true)
        {
            OutputLog.Log("HandleOnAdClosed : m_IsSuccess = true");

            m_IsSuccess = false;
            AppInstance.Instance.m_pEventDelegateManager.OnGoogleRewardsAdsComplete();
        }

        // 동영상 종료 되었을때
        //ReloadAd();
    }

    public void HandleOnUserEarnedReward(object sender, EventArgs args)
    {
        OutputLog.Log("HandleOnUserEarnedReward");
        // 동영상 시청 완료
        if (m_IsRequestShow == true)
        {
            m_IsSuccess = true;
            m_IsRequestShow = false;

            OutputLog.Log("HandleOnUserEarnedReward : m_IsSuccess = true");
        }
    }
}