using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI_Mission_Guide : MonoBehaviour
{
    public delegate void Callback_Close();
    public event Callback_Close m_Callback_Close;

    void Start()
    {
        GameInfo.Instance.m_IsInGame_MissionGuide = true;

        GameObject ob;
        Text pText;

        int nLevel = 0;

        if (GameInfo.Instance.m_IsCurrLevelPlay == false && GameInfo.Instance.m_nPrevLevelPlayLevel != -1)
        {
            nLevel = GameInfo.Instance.m_nPrevLevelPlayLevel;
        }
        else
        {
            nLevel = SavedGameDataInfo.Instance.m_nLevel;
        }

        ExcelData_LevelFixedMissionDataInfo pLevelFixedMissionDataInfo = ExcelDataManager.Instance.m_pExcelData_LevelFixedMissionData.GetLevelFixedMissionDataInfo_byLevel(nLevel);

        if (pLevelFixedMissionDataInfo == null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            ob = Helper.FindChildGameObject(gameObject, "Text_Title");
            pText = ob.GetComponent<Text>();
            pText.text = Helper.GetTextDataString(pLevelFixedMissionDataInfo.m_nTutorial_Title_TableID);

            ob = Helper.FindChildGameObject(gameObject, "Text_Desc");
            pText = ob.GetComponent<Text>();
            pText.text = Helper.GetTextDataString(pLevelFixedMissionDataInfo.m_nTutorial_Desc_TableID);
        }

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

    private void OnDestroy()
    {
        GameInfo.Instance.m_IsInGame_MissionGuide = false;

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
    }

    void Update()
    {
    }

    public void OnButtonClick_Close()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        m_Callback_Close?.Invoke();
        GameObject.Destroy(gameObject);
    }

    public void OnHardwareBackButtonClick()
    {
        if (GameInfo.Instance.m_IsHardwareBackButtonProcess == false &&
            GameInfo.Instance.m_IsShopOpen == false &&
            GameInfo.Instance.m_IsItemBuyOpen == false &&
            GameInfo.Instance.m_nMessageBoxOpenCount == 0 &&
            GameInfo.Instance.m_nRewardPopupOpenCount == 0)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = true;

            OnButtonClick_Close();
        }
    }
}
