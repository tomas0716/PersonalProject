using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System;

public class VersionCheck : MonoBehaviour
{
    private static VersionCheck m_pInstance;
    public static VersionCheck Instance { get { return m_pInstance; } }

    private void Awake()
    {
        m_pInstance = this;
    }

    public void OnVersionCheck()
    {
        if (GameDefine.ms_IsVersionCheck == true)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            StartCoroutine(ClientVersionCheck());
#else
            AppInstance.Instance.m_pEventDelegateManager.OnStoreVersionCheckComplete(true);
#endif
        }
        else
        {
            // 버전이 같다고 하자
            AppInstance.Instance.m_pEventDelegateManager.OnStoreVersionCheckComplete(true);
        }
    }

    private IEnumerator ClientVersionCheck()
    {
		UnityWebRequest _WebRequest = UnityWebRequest.Get(AppInstance.Instance.m_GooglePlayUrl);
		yield return _WebRequest.SendWebRequest();

		if (_WebRequest.error == null)
		{
			string _Pattern = @"<span class=""htlgb"">[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}<";
			Regex _Regex = new Regex(_Pattern, RegexOptions.IgnoreCase);
			Match _Match = _Regex.Match(_WebRequest.downloadHandler.text);

            if (_Match != null)
            {
                _Match = Regex.Match(_Match.Value, "[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}");

                try
                {
                    if (_Match.ToString().Trim().Equals(Application.version))
                    {
                        AppInstance.Instance.m_pEventDelegateManager.OnStoreVersionCheckComplete(true);

                        yield break;
                    }
                    else
                    {
                        AppInstance.Instance.m_pEventDelegateManager.OnStoreVersionCheckComplete(false);

                        yield break;
                    }
                }
                catch (Exception e)
                {
                    AppInstance.Instance.m_pEventDelegateManager.OnStoreVersionCheckComplete(true);
                }
            }
            else
            {
                AppInstance.Instance.m_pEventDelegateManager.OnStoreVersionCheckComplete(true);
            }
		}
		else
		{
			AppInstance.Instance.m_pEventDelegateManager.OnStoreVersionCheckComplete(true);
		}
	}
}
