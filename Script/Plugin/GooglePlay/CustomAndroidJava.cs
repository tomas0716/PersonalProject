using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomAndroidJava : MonoBehaviour
{
	private static CustomAndroidJava m_pInstance = null;
	public static CustomAndroidJava Instance { get { return m_pInstance; } }

#if !UNITY_EDITOR
	private AndroidJavaObject m_pMainJavaObject = null;
	private AndroidJavaObject m_pAndroidJavaObject = null;
#endif

	void Awake()
	{
		m_pInstance = this;
	}

	void Start()
    {
#if !UNITY_EDITOR
		OutputLog.Log("CustomAndroidJava Start 01");
		
		m_pAndroidJavaObject = new AndroidJavaObject("com.thomasstudio.threematch.MainActivity");

		OutputLog.Log("CustomAndroidJava Start 02");

		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

		OutputLog.Log("CustomAndroidJava Start 03");

		m_pMainJavaObject = jc.GetStatic<AndroidJavaObject>("currentActivity");

		OutputLog.Log("CustomAndroidJava Start 04");
#endif
	}

	public string GetUniqueIdentifier()
	{
#if !UNITY_EDITOR
		OutputLog.Log("CustomAndroidJava GetUniqueIdentifier 01");

		AndroidJavaObject context = m_pMainJavaObject.Call<AndroidJavaObject>("getApplicationContext");

		OutputLog.Log("CustomAndroidJava GetUniqueIdentifier 02");

		return m_pAndroidJavaObject.Call<string>("GetDeviceUUID", context);;
#else
		return "UnityEditor_4";
#endif
	}

	public string GetCountryCode()
	{
#if !UNITY_EDITOR
		OutputLog.Log("CustomAndroidJava GetCountryCode 01");

		AndroidJavaObject context = m_pMainJavaObject.Call<AndroidJavaObject>("getApplicationContext");

		OutputLog.Log("CustomAndroidJava GetCountryCode 02");

		return m_pAndroidJavaObject.Call<string>("GetCountryCode", context);;
#else
		return "kr";
#endif
	}
}
