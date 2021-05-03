using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Linq;

public class Helper
{
	public Helper()
	{
	}

	static public short				ConvertStringToShort(string strData)
	{
		if (strData == "")
			return 0;

		int len = strData.Length;

		for (int i = 0; i < len; ++i)
		{
			char s = strData[i];

			if ((s < '0' || s > '9'))
			{
				return 0;
			}
		}

		return Convert.ToInt16(strData);
	}

	static public int				ConvertStringToInt(string strData)
	{
		if (strData == "")
			return 0;

		int len = strData.Length;

		for (int i = 0; i < len; ++i)
		{
			char s = strData[i];

			//if ((s < '0' || s > '9'))
			if (s != '.' && s != '-' && (s < '0' || s > '9'))
			{
				return 0;
			}
		}

		return Convert.ToInt32(strData);
	}

	static public float				ConvertStringToFloat(string strData)
	{
		if (strData == "")
			return 0.0f;

		int len = strData.Length;

		for (int i = 0; i < len; ++i)
		{
			char s = strData[i];

			if (s != '.' && s != '-' && (s < '0' || s > '9'))
			{
				return 0;
			}

			if (s == '.' && i == 0)
			{
				return 0;
			}

			if (s == '-' && i != 0)
			{
				return 0;
			}
		}

		if (len == 1 && strData[0] == '-')
			return 0;

		return Convert.ToSingle(strData);
	}

	static public bool				ConvertStringToBool(string strData)
	{
		if (strData == "")
			return false;

		return Convert.ToBoolean(strData);
	}
	
	static public uint 				ConvertStringToUint(string strData)
	{
		if( strData == "" || strData == "0" )
			return 0U;
		
		return Convert.ToUInt32(strData);
	}
	
	static public int 				ConvertToInt(object uData)
	{
		return Convert.ToInt32(uData);
	}

	static public ulong 			ConverToUlong(string strData)
	{
		return Convert.ToUInt64(strData);
	}
	
	static public long				ConverTolong(string strData)
	{
		return Convert.ToInt64(strData);
	}
	
	static public string 			ConvertByteToString(byte[] buffer)
	{
		string data = System.Text.Encoding.Default.GetString(buffer);

		char char_emtpy = (char)0x00;
		char[] tmp = new char[data.Length];
		tmp = data.ToCharArray();
		
		string result = "";
		int data_count = 0;
		
		foreach( char char_data in tmp )
		{
			if( char_data == char_emtpy )
				break;
			
			string str = tmp[data_count].ToString();
			result += str;
			++data_count;
		}
		
		return result;
	}

	public static Vector2 Rotate(Vector2 v, float delta)
	{
		return new Vector2(
			v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
			v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
		);
	}

	static public GameObject		FindChildGameObject(GameObject go, string Name)
	{
		if (go != null)
		{
			return RecursiveFindGameObject(go, Name);
		}

		return null;
	}

	static private GameObject		RecursiveFindGameObject(GameObject go, string name)
	{
		int Count = go.transform.childCount;

		for (int i = 0; i < Count; ++i)
		{
			Transform child = go.transform.GetChild(i);

			if (child != null)
			{
				if (child.gameObject.name == name)
				{
					return child.gameObject;
				}
				else
				{
					GameObject Find = RecursiveFindGameObject(child.gameObject, name);

					if (Find != null)
					{
						return Find;
					}
				}
			}
		}

		return null;
	}

	static public void				RemoveChildAll(GameObject go)
	{
		int nCount = go.transform.childCount;

		for (int i = nCount - 1; i >= 0; --i)
		{
			Transform tf = go.transform.GetChild(i);
			GameObject.Destroy(tf.gameObject);
		}
	}

	static public void				SetLayer(GameObject go, int layer)
	{
		go.layer = layer;

		Transform t = go.transform;

		for (int i = 0, imax = t.childCount; i < imax; ++i)
		{
			Transform child = t.GetChild(i);
			SetLayer(child.gameObject, layer);
		}
	}

	static public Vector2			Convert2DTo3DPosition(Vector2 vPos)
	{
		switch (GameDefine.ms_eScreenAnchorPosition)
		{
			case eScreenAnchorPosition.UpperLeft:		return new Vector2(vPos.x - AppInstance.Instance.m_vBaseResolution.x * 0.5f, AppInstance.Instance.m_vBaseResolution.y - vPos.y);
			case eScreenAnchorPosition.UpperCenter:		return new Vector2(vPos.x, AppInstance.Instance.m_vBaseResolution.y - vPos.y);
			case eScreenAnchorPosition.UpperRight:		return new Vector2(AppInstance.Instance.m_vBaseResolution.x * 0.5f - vPos.x, AppInstance.Instance.m_vBaseResolution.y - vPos.y);
			case eScreenAnchorPosition.MiddleLeft:		return new Vector2(vPos.x - AppInstance.Instance.m_vBaseResolution.x * 0.5f, vPos.y + AppInstance.Instance.m_vBaseResolution.y * 0.5f);
			case eScreenAnchorPosition.MiddleCenter:	return new Vector2(vPos.x, vPos.y + AppInstance.Instance.m_vBaseResolution.y * 0.5f);
			case eScreenAnchorPosition.MiddleRight:		return new Vector2(AppInstance.Instance.m_vBaseResolution.x * 0.5f - vPos.x, vPos.y + AppInstance.Instance.m_vBaseResolution.y * 0.5f);
			case eScreenAnchorPosition.LowerLeft:		return new Vector2(vPos.x - AppInstance.Instance.m_vBaseResolution.x * 0.5f, vPos.y);
			case eScreenAnchorPosition.LowerCenter:		return vPos;
			case eScreenAnchorPosition.LowerRight:		return new Vector2(AppInstance.Instance.m_vBaseResolution.x * 0.5f - vPos.x, vPos.y);
		}

		return Vector2.zero;
	}

	static public Vector2 Convert3DTo2DPosition(Vector2 vPos)
	{
		switch (GameDefine.ms_eScreenAnchorPosition)
		{
			case eScreenAnchorPosition.UpperLeft:		return new Vector2(vPos.x + AppInstance.Instance.m_vBaseResolution.x * 0.5f, AppInstance.Instance.m_vBaseResolution.y - vPos.y);
			case eScreenAnchorPosition.UpperCenter:		return new Vector2(vPos.x, AppInstance.Instance.m_vBaseResolution.y - vPos.y);
			case eScreenAnchorPosition.UpperRight:		return new Vector2(AppInstance.Instance.m_vBaseResolution.x * 0.5f - vPos.x, AppInstance.Instance.m_vBaseResolution.y - vPos.y);
			case eScreenAnchorPosition.MiddleLeft:		return new Vector2(vPos.x + AppInstance.Instance.m_vBaseResolution.x * 0.5f, AppInstance.Instance.m_vBaseResolution.y * 0.5f - vPos.y);
			case eScreenAnchorPosition.MiddleCenter:	return new Vector2(vPos.x, AppInstance.Instance.m_vBaseResolution.y * 0.5f - vPos.y);
			case eScreenAnchorPosition.MiddleRight:		return new Vector2(AppInstance.Instance.m_vBaseResolution.x * 0.5f - vPos.x, AppInstance.Instance.m_vBaseResolution.y * 0.5f - vPos.y);
			case eScreenAnchorPosition.LowerLeft:		return new Vector2(vPos.x + AppInstance.Instance.m_vBaseResolution.x * 0.5f, vPos.y);
			case eScreenAnchorPosition.LowerCenter:		return vPos;
			case eScreenAnchorPosition.LowerRight:		return new Vector2(AppInstance.Instance.m_vBaseResolution.x * 0.5f - vPos.x, vPos.y);
		}

		return Vector2.zero;
	}

	static public string GetTextDataString(eTextDataType eType)
	{
		if (ExcelDataManager.Instance.m_pExcelData_TextData.FindTextInfo(eType).m_TextTable.ContainsKey(AppInstance.Instance.m_pOptionInfo.m_strCountryCode) == true)
		{
			return ExcelDataManager.Instance.m_pExcelData_TextData.FindTextInfo(eType).m_TextTable[AppInstance.Instance.m_pOptionInfo.m_strCountryCode];
		}

		return ExcelDataManager.Instance.m_pExcelData_TextData.FindTextInfo(eType).m_TextTable["us"];
	}

    static public string GetTextDataString(eTextDataType eType, string strCountCode)
    {
        if (ExcelDataManager.Instance.m_pExcelData_TextData.FindTextInfo(eType).m_TextTable.ContainsKey(strCountCode) == true)
        {
            return ExcelDataManager.Instance.m_pExcelData_TextData.FindTextInfo(eType).m_TextTable[strCountCode];
        }

        return ExcelDataManager.Instance.m_pExcelData_TextData.FindTextInfo(eType).m_TextTable["us"];
    }

    static public string GetTextDataString(int nID)
	{
		if (ExcelDataManager.Instance.m_pExcelData_TextData.FindTextInfo(nID).m_TextTable.ContainsKey(AppInstance.Instance.m_pOptionInfo.m_strCountryCode) == true)
		{
			return ExcelDataManager.Instance.m_pExcelData_TextData.FindTextInfo(nID).m_TextTable[AppInstance.Instance.m_pOptionInfo.m_strCountryCode];
		}

		return ExcelDataManager.Instance.m_pExcelData_TextData.FindTextInfo(nID).m_TextTable["us"];
	}

	static public void ShuffleList<T>(List<T> list)
	{
		int random1;
		int random2;

		T tmp;

		for (int index = 0; index < list.Count; ++index)
		{
			random1 = UnityEngine.Random.Range(0, list.Count);
			random2 = UnityEngine.Random.Range(0, list.Count);

			tmp = list[random1];
			list[random1] = list[random2];
			list[random2] = tmp;
		}
	}

	static public Dictionary<TKey, TValue> ShuffleDictionary<TKey, TValue>(Dictionary<TKey, TValue> source)
	{
		System.Random r = new System.Random();

		return source.OrderBy(x => r.Next()).ToDictionary(item => item.Key, item => item.Value);
	}

	static public int GetSlotIndex(int x, int y)
	{
		return y * GameDefine.ms_nInGameSlot_X + x;
	}

	static public void GetSlotXY(int nSlotIndex, out int x, out int y)
	{
		x = nSlotIndex % GameDefine.ms_nInGameSlot_X;
		y = nSlotIndex / GameDefine.ms_nInGameSlot_X;
	}

	static public Vector3 Get3DPosToUIPos(GameObject pGameObject_Canvas, Camera pCamera, Vector3 vPos)
	{
		RectTransform rtf = pGameObject_Canvas.GetComponent<RectTransform>();

		Vector3 ViewportPos = pCamera.WorldToViewportPoint(vPos);
		Vector3 vRes = new Vector3(((ViewportPos.x * rtf.sizeDelta.x) - (rtf.sizeDelta.x * 0.5f)), ((ViewportPos.y * rtf.sizeDelta.y) - (rtf.sizeDelta.y * 0.5f)), 0);

		return vRes;
	}

	static public void OnSoundPlay(eSoundType eType, bool IsLoop)
	{
		ExcelData_SoundDataInfo SoundInfo = ExcelDataManager.Instance.m_pExcelData_SoundData.GetRandomSoundInfo_byType(eType);
		AppInstance.Instance.m_pSoundPlayer.Play(SoundInfo, IsLoop);
	}

	public static string ConvertToOrdinal(int nNumber)
	{
		string strNumber = nNumber.ToString();
		if ((nNumber % 100) == 11 || (nNumber % 100) == 12 || (nNumber % 100) == 13)
		{
			return strNumber + "th";
		}

		switch (nNumber % 10)
		{
			case 1:		strNumber += "st";	break;
			case 2:		strNumber += "nd";	break;
			case 3:		strNumber += "rd";	break;
			default:	strNumber += "th";	break;
		}
		return strNumber;
	}

	public static void FirebaseLogEvent(string strName)
	{
		if (AppInstance.Instance.m_pFirebaseApp != null)
		{
			Firebase.Analytics.FirebaseAnalytics.LogEvent(strName);
			OutputLog.Log("Firebase : " + strName);
		}
	}

	public static void FirebaseLogEvent(string strName, string strParemetaName, int nParametaValue)
	{
		if (AppInstance.Instance.m_pFirebaseApp != null)
		{
			Firebase.Analytics.FirebaseAnalytics.LogEvent(strName, strParemetaName, nParametaValue);
			OutputLog.Log("Firebase : " + strName + ", " + strParemetaName + ", " + nParametaValue.ToString());
		}
	}

	public static void OnCameraSmallShaking()
	{
		GameEvent_CameraShaking pGameEvent = new GameEvent_CameraShaking(GameDefine.ms_fNextSpecialSlotRemoveDelayTime, 2 * AppInstance.Instance.m_fMainScale);
		AppInstance.Instance.m_pGameEventManager.AddGameEvent(pGameEvent);
	}

	public static void OnCameraNormalShaking()
	{
		GameEvent_CameraShaking pGameEvent = new GameEvent_CameraShaking(GameDefine.ms_fNextSpecialSlotRemoveDelayTime, 3 * AppInstance.Instance.m_fMainScale);
		AppInstance.Instance.m_pGameEventManager.AddGameEvent(pGameEvent);
	}

	public static void OnCameraBigShaking()
	{
		GameEvent_CameraShaking pGameEvent = new GameEvent_CameraShaking(GameDefine.ms_fNextSpecialSlotRemoveDelayTime * 2, 5 * AppInstance.Instance.m_fMainScale);
		AppInstance.Instance.m_pGameEventManager.AddGameEvent(pGameEvent);
	}

	public static void OnCameraShaking(float fTime, float fAmount)
	{
		GameEvent_CameraShaking pGameEvent = new GameEvent_CameraShaking(fTime, fAmount * AppInstance.Instance.m_fMainScale);
		AppInstance.Instance.m_pGameEventManager.AddGameEvent(pGameEvent);
	}
}
