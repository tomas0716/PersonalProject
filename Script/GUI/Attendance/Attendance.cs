using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attendance : MonoBehaviour
{
    private GameObject          m_pGameObject_Parent        = null;
    private Transformer_Scalar  m_pScale                    = new Transformer_Scalar(0);

    private GameObject []       m_pGameObject_Completes     = new GameObject[7];

    private bool                m_IsValid                   = true;

    private Transformer_Scalar  m_pScale_Check              = new Transformer_Scalar(1.2f);
    private GameObject          m_pGameObject_Check         = null;

    private bool                m_IsFirstOpen               = false;

    private bool                m_IsCloseButtonVisible      = false;

    void Start()
    {
        m_pGameObject_Parent = Helper.FindChildGameObject(gameObject, "Parent");
        m_pGameObject_Parent.transform.localScale = new Vector3(0, 0, 1);

        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(gameObject, "Text_Title");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Attendance_Title);

        ob = Helper.FindChildGameObject(gameObject, "Text_Desc");
        pText = ob.GetComponent<Text>();
        string strDesc = Helper.GetTextDataString(eTextDataType.Attendance_Desc);
        string strReplace = strDesc.Replace("\\n", "\n");
        pText.text = strReplace;

        ob = Helper.FindChildGameObject(gameObject, "Button_Get");
        Button pButton = ob.GetComponent<Button>();
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Attendance_Recieve);

        if (SavedGameDataInfo.Instance.m_IsGetAttendance == true)
        {
            pButton.interactable = false;
            pText.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            m_IsCloseButtonVisible = true;
        }
        else
        {
            ob = Helper.FindChildGameObject(gameObject, "Button_Close");
            ob.SetActive(false);
            m_IsCloseButtonVisible = false;
        }

        GameObject ob_Items = Helper.FindChildGameObject(gameObject, "Items");

        for (int i = 0; i < 7; ++i)
        {
            ExcelData_Event_AttendanceDataInfo pAttendanceDataInfo = ExcelDataManager.Instance.m_pExcelData_Event_AttendanceData.GetDataInfo_byDay(i+1);

            GameObject ob_Day = Helper.FindChildGameObject(ob_Items, (i+1).ToString());
            ob = Helper.FindChildGameObject(ob_Day, "Text_Day");
            pText = ob.GetComponent<Text>();

            if (AppInstance.Instance.m_pOptionInfo.m_strCountryCode == "us")
            {
                pText.text = Helper.GetTextDataString(eTextDataType.Attendance_Day) + (i+1).ToString();
            }
            else
            {
                pText.text = (i + 1).ToString() + Helper.GetTextDataString(eTextDataType.Attendance_Day);
            }

            ob = Helper.FindChildGameObject(ob_Day, "Text_ItemDesc");
            pText = ob.GetComponent<Text>();
            pText.text = Helper.GetTextDataString(pAttendanceDataInfo.m_nDesc);

            m_pGameObject_Completes[i] = Helper.FindChildGameObject(ob_Day, "Image_Complete");

            if (i < SavedGameDataInfo.Instance.m_byGetAttendanceDay)
            {
                m_pGameObject_Completes[i].SetActive(true);
            }
            else
            {
                m_pGameObject_Completes[i].SetActive(false);
            }
        }

        TransformerEvent_Scalar eventValue;
        eventValue = new TransformerEvent_Scalar(0.3f, 1.0f);
        m_pScale.AddEvent(eventValue);
        m_pScale.OnPlay();

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

	private void OnDestroy()
	{
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;
        AppInstance.Instance.m_pEventDelegateManager.OnEventRewardPopupClosed -= OnRewardPopupClosed;

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
    }

	void Update()
    {
        m_pScale.Update(Time.deltaTime);
        float fScale = m_pScale.GetCurScalar();
        m_pGameObject_Parent.transform.localScale = new Vector3(fScale, fScale, 1);

        m_pScale_Check.Update(Time.deltaTime);
        if (m_pGameObject_Check != null)
        {
            fScale = m_pScale_Check.GetCurScalar();
            m_pGameObject_Check.transform.localScale = new Vector3(fScale, fScale, 1);
        }
    }

    public void SetFirstOpen()
    {
        m_IsFirstOpen = true;
    }

    public void OnButtonClick_Close()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        GameObject.Destroy(gameObject);
    }

    public void OnButtonClick_Check()
    {
        if (m_IsValid == true)
        {
            Helper.OnSoundPlay(eSoundType.Attendance, false);

            m_IsValid = false;

            m_pGameObject_Check = m_pGameObject_Completes[SavedGameDataInfo.Instance.m_byGetAttendanceDay];
            m_pGameObject_Check.SetActive(true);

            TransformerEvent_Scalar eventValue;
            eventValue = new TransformerEvent_Scalar(0.2f, 1.0f);
            m_pScale_Check.AddEvent(eventValue);
            eventValue = new TransformerEvent_Scalar(0.5f, 1.0f);
            m_pScale_Check.AddEvent(eventValue);
            m_pScale_Check.SetCallback(null, OnDone_Check);
            m_pScale_Check.OnPlay();

            int nLevel = SavedGameDataInfo.Instance.m_nLevel;
            if (nLevel < 20)
            {
                Helper.FirebaseLogEvent("Attendance_" + nLevel.ToString());
            }
            else
            {
                Helper.FirebaseLogEvent("Attendance");
            }
        }
    }

    private void OnDone_Check(TransformerEvent eventVlaue)
    {
        SavedGameDataInfo.Instance.m_IsGetAttendance = true;
        ++SavedGameDataInfo.Instance.m_byGetAttendanceDay;
        ExcelData_Event_AttendanceDataInfo pAttendanceDataInfo = ExcelDataManager.Instance.m_pExcelData_Event_AttendanceData.GetDataInfo_byDay(SavedGameDataInfo.Instance.m_byGetAttendanceDay);
        SavedGameDataInfo.Instance.m_nItemCounts[(int)pAttendanceDataInfo.m_ItemType] += pAttendanceDataInfo.m_nCount;

        AppInstance.Instance.m_pEventDelegateManager.OnCreateLoading();
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save += OnGooglePlaySavedGameDone_Save;
        SavedGameDataInfo.Instance.Save();
    }

    public void OnGooglePlaySavedGameDone_Save()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();

        AppInstance.Instance.m_pEventDelegateManager.OnUpdateHeartInfo();
        AppInstance.Instance.m_pEventDelegateManager.OnUpdateCoinInfo();
        AppInstance.Instance.m_pEventDelegateManager.OnUpdateItemState();

        m_pGameObject_Completes[SavedGameDataInfo.Instance.m_byGetAttendanceDay-1].SetActive(true);

        AppInstance.Instance.m_pEventDelegateManager.OnEventRewardPopupClosed += OnRewardPopupClosed;

        ExcelData_Event_AttendanceDataInfo pAttendanceDataInfo = ExcelDataManager.Instance.m_pExcelData_Event_AttendanceData.GetDataInfo_byDay(SavedGameDataInfo.Instance.m_byGetAttendanceDay);

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/RewardPopup");
        ob = GameObject.Instantiate(ob);
        RewardPopup pRewardPopup = ob.GetComponent<RewardPopup>();
        pRewardPopup.SetRewardInfo(eRewardSubject.eAttendance, pAttendanceDataInfo.m_ItemType, pAttendanceDataInfo.m_nCount);

        AppInstance.Instance.m_pEventDelegateManager.OnAttendaceComplete();
    }

    public void OnRewardPopupClosed()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventRewardPopupClosed -= OnRewardPopupClosed;

        if (m_IsFirstOpen == true && SavedGameDataInfo.Instance.m_IsGetFreeRoulette == false)
        {
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/Roulette");
            GameObject.Instantiate(ob);
        }

        GameObject.Destroy(gameObject);
    }

    public void OnHardwareBackButtonClick()
    {
        if (GameInfo.Instance.m_IsHardwareBackButtonProcess == false &&
            GameInfo.Instance.m_IsShopOpen == false &&
            GameInfo.Instance.m_IsItemBuyOpen == false &&
            GameInfo.Instance.m_nMessageBoxOpenCount == 0 &&
            GameInfo.Instance.m_nRewardPopupOpenCount == 0 &&
            m_IsCloseButtonVisible == true)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = true;
            OnButtonClick_Close();
        }
    }
}
