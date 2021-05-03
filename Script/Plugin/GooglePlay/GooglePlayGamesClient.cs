using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayGamesClient : MonoBehaviour
{
	private static GooglePlayGamesClient m_pInstance = null;
	public static GooglePlayGamesClient Instance { get { return m_pInstance; } }

	private string m_strUserID = "";
	private string m_strEmail = "";
	public string UserID { get { return m_strUserID; } }
	public string Email { get { return m_strEmail; } }

	void Awake()
	{
		m_pInstance = this;

#if UNITY_ANDROID && !UNITY_EDITOR
		//if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			OutputLog.Log("GooglePlayGamesClient Awake 01");

			Initialize();

			OutputLog.Log("GooglePlayGamesClient Awake 02");
		}
#endif
    }

    public void Initialize()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        if(GameDefine.ms_IsUseSavedGame == true)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).RequestIdToken().EnableSavedGames().Build();
		    PlayGamesPlatform.InitializeInstance(config);
		    PlayGamesPlatform.DebugLogEnabled = true;
		    PlayGamesPlatform.Activate();
        }
        else 
        {
        	PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).RequestIdToken().Build();
		    PlayGamesPlatform.InitializeInstance(config);
		    PlayGamesPlatform.DebugLogEnabled = true;
		    PlayGamesPlatform.Activate();
        }
#endif
    }

    public void Login()
	{
		OutputLog.Log("Google Login 01");

//#if UNITY_ANDROID && !UNITY_EDITOR
        if(GameDefine.ms_IsGoogleLogin == true)
        {
		    if (Social.localUser.authenticated == false)
		    {
			    OutputLog.Log("Google Login 02");

			    Social.localUser.Authenticate(AuthenticateCallback);
            }
		    else
		    {
			    OutputLog.Log("Google Login 03");

			    //AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayLoginResult(false, "");
			    // 이미 로그인 되어 있는 경우
		    }
        }
        else 
        {
            AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayLoginResult(true, m_strUserID, m_strEmail);
        }
//#endif
    }

    public void LogOut()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		((PlayGamesPlatform)Social.Active).SignOut();
#endif
    }

    void AuthenticateCallback(bool sucess)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        if (sucess == true)
		{
			OutputLog.Log("Google Login AuthenticateCallback true");

			m_strUserID = ((PlayGamesLocalUser)Social.localUser).id;
			m_strEmail = ((PlayGamesLocalUser)Social.localUser).Email;
			AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayLoginResult(true, m_strUserID, m_strEmail);
		}
		else
		{
			OutputLog.Log("Google Login AuthenticateCallback false");
			AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayLoginResult(false, "", "");
		}
#endif
	}
}