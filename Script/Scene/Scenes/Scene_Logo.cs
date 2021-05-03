using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Scene_Logo : CScene
{
	private Transformer_Scalar	m_pTimer						= new Transformer_Scalar(0);
	private bool				m_IsTimerDone					= false;
	private bool				m_IsGoogleSavedGame				= false;
	private bool				m_IsGoogleAchievement			= false;
	private bool				m_IsGoogleLeaderboard_MyRank	= false;

	protected override void Initialize()
	{
		AppInstance.Instance.m_GooglePlayUrl = m_GooglePlayUrl;
        AppInstance.Instance.m_pSceneManager.SetCurrScene(this);
        AppInstance.Instance.m_pSceneManager.SetCurrSceneType(eSceneType.Scene_Logo);

#if UNITY_ANDROID
        OutputLog.Log("Save CountryCode : " + AppInstance.Instance.m_pOptionInfo.m_strCountryCode);

        if (AppInstance.Instance.m_pOptionInfo.m_strCountryCode == "None")
        {
            string strCode = CustomAndroidJava.Instance.GetCountryCode();

            OutputLog.Log("CountryCode : " + strCode);

            AppInstance.Instance.m_pOptionInfo.m_strCountryCode = strCode;
            AppInstance.Instance.m_pOptionInfo.Save();
        }
#endif

        Application.targetFrameRate = 120;

        AppInstance.Instance.m_pSoundPlayer.CreateSound();

        OutputLog.Log(Application.internetReachability.ToString());

        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlayLoginResult += OnGooglePlayLoginResult;
		AppInstance.Instance.m_pEventDelegateManager.OnEventLeaderboard_GetMyRank += OnLeaderboard_GetMyRank;
		AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Load += OnGooglePlaySavedGameDone_Load;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlayAchievementLoaded += OnGooglePlayAchievementLoaded;

        //GoogleAdsManager.Instance.Initialize();

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            GameInfo.Instance.m_IsOfflineModeGameEnter = true;
            m_IsGoogleAchievement = true;
			m_IsGoogleLeaderboard_MyRank = true;

			TimeServer.Instance.GetNISTDate();
            SavedGameDataInfo.Instance.Load();

            GoogleIAPManager.Instance.OnInitialize();
            HeartChargeTimer.Instance.OnInitialize();

			m_pTimer.OnReset();
			TransformerEvent_Scalar eventValue = new TransformerEvent_Scalar(1, 0);
			m_pTimer.AddEvent(eventValue);
			m_pTimer.SetCallback(null, OnTimerDone);
			m_pTimer.OnPlay();
		}
        else
        {
			//OutputLog.Log("Logo 01");
			Helper.FirebaseLogEvent("Logo_Initialize");
			//OutputLog.Log("Logo 02");

			TimeServer.Instance.GetNISTDate();
            HeartChargeTimer.Instance.OnInitialize();

			AppInstance.Instance.m_pEventDelegateManager.OnEventStoreVersionCheckComplete += OnStoreVersionCheckComplete;
			VersionCheck.Instance.OnVersionCheck();
		}

	}

	protected override void Destroy()
    {
		AppInstance.Instance.m_pEventDelegateManager.OnEventStoreVersionCheckComplete -= OnStoreVersionCheckComplete;

		AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlayLoginResult -= OnGooglePlayLoginResult;
		AppInstance.Instance.m_pEventDelegateManager.OnEventLeaderboard_GetMyRank -= OnLeaderboard_GetMyRank;
		AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Load -= OnGooglePlaySavedGameDone_Load;
		AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlayAchievementLoaded -= OnGooglePlayAchievementLoaded;
	}

    protected override void Inner_Update()
	{
		m_pTimer.Update(Time.deltaTime);
	}

	protected override void Inner_FixedUpdate()
	{	
	}

	public void OnStoreVersionCheckComplete(bool IsSameVersion)
	{
		if (IsSameVersion == true)
		{
			OutputLog.Log("Logo SameVersion");
			Helper.FirebaseLogEvent("Logo_VersionCheckComplete");

			GooglePlayGamesClient.Instance.Login();

			m_pTimer.OnReset();
			TransformerEvent_Scalar eventValue = new TransformerEvent_Scalar(1, 0);
			m_pTimer.AddEvent(eventValue);
			m_pTimer.SetCallback(null, OnTimerDone);
			m_pTimer.OnPlay();
		}
		else
		{
			GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
			ob = GameObject.Instantiate(ob);
			MessageBox pMessageBox = ob.GetComponent<MessageBox>();
			pMessageBox.Initialize(MessageBox.eMessageBoxType.OKCancel,
									Helper.GetTextDataString(eTextDataType.Error),
									Helper.GetTextDataString(eTextDataType.InValid_Version),
									Helper.GetTextDataString(eTextDataType.App_Quit),
									Helper.GetTextDataString(eTextDataType.OK));

			pMessageBox.m_Callback_OK += AppQuit;
			pMessageBox.m_Callback_Cancel += GoPlayStore;
		}
	}

	private void AppQuit()
	{
		AppInstance.Instance.OnExitGame();
	}

	private void GoPlayStore()
	{
		AppInstance.Instance.OnExitGame();
		Application.OpenURL(AppInstance.Instance.m_GooglePlayUrl);
	}

	//public void OnDone_Timer_NotReachableMessage(TransformerEvent eventValue)
	//{
	//	// 메시지 창을 띄워 다시 시도할수 있도록 한다.
	//	GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
	//	ob = GameObject.Instantiate(ob);
	//	MessageBox pMessageBox = ob.GetComponent<MessageBox>();
	//	pMessageBox.Initialize(MessageBox.eMessageBoxType.OKCancel,
	//							Helper.GetTextDataString(eTextDataType.Error),
	//							Helper.GetTextDataString(eTextDataType.InValid_Internet_Desc),
	//							Helper.GetTextDataString(eTextDataType.InValid_Network_ReExecute),
	//							Helper.GetTextDataString(eTextDataType.InValid_Network_Retry));

	//	pMessageBox.m_Callback_OK += ReExecute;
	//	pMessageBox.m_Callback_Cancel += Retry;
	//}

	private void ReExecute()
	{
		AppInstance.Instance.ReExecute();
	}

	//private void Retry()
	//{
	//	if (Application.internetReachability == NetworkReachability.NotReachable)
	//	{
	//		m_pTimer.OnReset();
	//		TransformerEvent_Scalar eventValue = new TransformerEvent_Scalar(2, 0);
	//		m_pTimer.AddEvent(eventValue);
	//		m_pTimer.SetCallback(null, OnDone_Timer_NotReachableMessage);
	//		m_pTimer.OnPlay();
	//	}
	//	else
	//	{
	//		TimeServer.Instance.GetNISTDate();
	//		GooglePlayGamesClient.Instance.Initialize();
	//		GooglePlayGamesClient.Instance.Login();

	//		m_pTimer.OnReset();
	//		TransformerEvent_Scalar eventValue = new TransformerEvent_Scalar(4, 0);
	//		m_pTimer.AddEvent(eventValue);
	//		m_pTimer.SetCallback(null, OnTimerDone);
	//		m_pTimer.OnPlay();
	//	}
	//}

	public void OnTimerDone(TransformerEvent eventValue)
	{
#if UNITY_EDITOR
		//AppInstance.Instance.m_pOptionInfo.m_strCountryCode = GameDefine.ms_UnityCountryCode;
		SavedGameDataInfo.Instance.Load();
		AppInstance.Instance.m_pSceneManager.ChangeScene(eSceneType.Scene_Middle, false);
#elif UNITY_ANDROID
		m_IsTimerDone = true;
		if (m_IsGoogleSavedGame == true && m_IsGoogleAchievement == true && m_IsGoogleLeaderboard_MyRank == true)
		{
		    AppInstance.Instance.m_pSceneManager.ChangeScene(eSceneType.Scene_Middle, false);
		}
#endif
	}

	public void OnGooglePlayLoginResult(bool IsResult, string strUserID, string strEmail)
	{
		OutputLog.Log("OnGooglePlayLoginResult");
#if UNITY_ANDROID
        if (GameDefine.ms_IsGoogleLogin == true)
        {
            if (IsResult == true)
            {
				Helper.FirebaseLogEvent("Logo_GoogleLogin_S");

				GoogleIAPManager.Instance.OnInitialize();
                SavedGameDataInfo.Instance.Load();
				GooglePlayLeaderboard.Instance.GetMyRank();
				GooglePlayLeaderboard.Instance.GetRanker();
                GooglePlayAchievement.Instance.LoadAchievements();
            }
            else
            {
				Helper.FirebaseLogEvent("Logo_GoogleLogin_F");

				GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
                ob = GameObject.Instantiate(ob);
                MessageBox pMessageBox = ob.GetComponent<MessageBox>();
                pMessageBox.Initialize(MessageBox.eMessageBoxType.OKCancel,
                                        Helper.GetTextDataString(eTextDataType.Error),
                                        Helper.GetTextDataString(eTextDataType.InValid_GoogleLogin_Desc),
                                        Helper.GetTextDataString(eTextDataType.InValid_Network_ReExecute),
                                        Helper.GetTextDataString(eTextDataType.InValid_Network_Retry));

                pMessageBox.m_Callback_OK += ReExecute;
                pMessageBox.m_Callback_Cancel += OnReGoogleLogin;
            }
        }
        else
        {
            m_IsGoogleSavedGame = true;
            m_IsGoogleAchievement = true;
            SavedGameDataInfo.Instance.Load();
        }
#endif
	}

	private void OnLeaderboard_GetMyRank()
	{
		Helper.FirebaseLogEvent("Logo_MyRankLoad");

		m_IsGoogleLeaderboard_MyRank = true;
		if (m_IsTimerDone == true && m_IsGoogleSavedGame == true && m_IsGoogleAchievement == true)
		{
			AppInstance.Instance.m_pSceneManager.ChangeScene(eSceneType.Scene_Middle, false);
		}
	}

	private void OnReGoogleLogin()
    {
        TimeServer.Instance.GetNISTDate();
        GooglePlayGamesClient.Instance.Initialize();
        GooglePlayGamesClient.Instance.Login();
    }

	public void OnGooglePlaySavedGameDone_Load()
	{
		Helper.FirebaseLogEvent("Logo_SavedGameLoad");

		OutputLog.Log("OnGooglePlaySavedGameDone_Load 01");
		m_IsGoogleSavedGame = true;
		if (m_IsTimerDone == true && m_IsGoogleAchievement == true && m_IsGoogleLeaderboard_MyRank == true)
		{
			OutputLog.Log("OnGooglePlaySavedGameDone_Load 02");

			AppInstance.Instance.m_pSceneManager.ChangeScene(eSceneType.Scene_Middle, false);
		}
	}

	public void OnGooglePlayAchievementLoaded()
	{
		Helper.FirebaseLogEvent("Logo_AchievementLoad");

		OutputLog.Log("OnGooglePlayAchievementLoaded 01");
		m_IsGoogleAchievement = true;
		if (m_IsTimerDone == true && m_IsGoogleSavedGame == true && m_IsGoogleLeaderboard_MyRank == true)
		{
			OutputLog.Log("OnGooglePlayAchievementLoaded 02");

			AppInstance.Instance.m_pSceneManager.ChangeScene(eSceneType.Scene_Middle, false);
		}
	}
}