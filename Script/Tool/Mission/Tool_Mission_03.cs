using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_Mission_03 : MonoBehaviour
{
    protected eMissionType m_eMissionType = eMissionType.None;

    private Text[]      m_pText_Disturb         = new Text[4];
    private InputField  m_pInputField_Count     = null;

    void Start()
    {
        m_eMissionType = eMissionType.Mouse;

        GameObject ob;

        for (int i = 0; i < 4; ++i)
        {
            string strButtonName = "Button_" + (i + 1).ToString();
            ob = Helper.FindChildGameObject(gameObject, strButtonName);
            ob = Helper.FindChildGameObject(ob, "Text");
            m_pText_Disturb[i] = ob.GetComponent<Text>();
        }

        ob = Helper.FindChildGameObject(gameObject, "InputField_Mouse");
        m_pInputField_Count = ob.GetComponent<InputField>();

        EventDelegateManager_ForTool.Instance.OnEventUpdateMap += OnUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventPostUpdateMap += OnPostUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventMission_UnCheck += OnMission_UnCheck;
        EventDelegateManager_ForTool.Instance.OnEventOnAllMissionRemove += OnAllMissionRemove;
    }

    private void OnDestroy()
    {
        EventDelegateManager_ForTool.Instance.OnEventUpdateMap -= OnUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventPostUpdateMap -= OnPostUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventMission_UnCheck -= OnMission_UnCheck;
        EventDelegateManager_ForTool.Instance.OnEventOnAllMissionRemove -= OnAllMissionRemove;
    }

    void Update()
    {

    }

    public void OnButtonClick_Disturb(int nDisturb)
    {
        Tool_Info.Instance.m_eDisturb_Mouse = (eDisturb)(nDisturb + eDisturb.Frozen_01 - 1);

        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == false)
            {
                MapDataMissionInfo_Mouse pMissionInfo = new MapDataMissionInfo_Mouse();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(m_eMissionType, pMissionInfo);
            }

            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMission;
            Tool_Info.Instance.m_eCurrEditMissionType = m_eMissionType;
        }

        if (Tool_Info.Instance.m_nMouseDisturb != -1)
        {
            m_pText_Disturb[Tool_Info.Instance.m_nMouseDisturb - 1].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_nMouseDisturb = nDisturb;
        m_pText_Disturb[Tool_Info.Instance.m_nMouseDisturb - 1].color = new Color(1, 0, 0);

        EventDelegateManager_ForTool.Instance.OnMission_Common_UnCheck();
    }

    public void OnMission_UnCheck()
    {
        if (Tool_Info.Instance.m_nMouseDisturb != -1)
        {
            m_pText_Disturb[Tool_Info.Instance.m_nMouseDisturb - 1].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eDisturb_Mouse = eDisturb.None;
        Tool_Info.Instance.m_nMouseDisturb = -1;
    }

    public void OnInputField_EndEdit(string strData)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            int nCount = Helper.ConvertStringToInt(m_pInputField_Count.text);

            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == true)
            {
                MapDataMissionInfo_Mouse pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Mouse;

                if (pMissionInfo != null)
                {
                    pMissionInfo.m_nCount = nCount;
                }
            }
            else
            {
                MapDataMissionInfo_Mouse pMissionInfo = new MapDataMissionInfo_Mouse();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                pMissionInfo.m_nCount = nCount;
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(m_eMissionType, pMissionInfo);
            }

            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
        }
    }

    private void OnUpdateMap(bool IsChangeMap)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            bool IsValid = true;
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == true)
            {
                MapDataMissionInfo_Mouse pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Mouse;

                if (pMissionInfo != null)
                {
                    m_pInputField_Count.text = pMissionInfo.m_nCount.ToString();
                }
                else
                {
                    IsValid = false;
                }
            }
            else
            {
                IsValid = false;
            }

            if (IsValid == false)
            {
                m_pInputField_Count.text = "0";
            }
        }
    }

    private void OnPostUpdateMap(bool IsChangeMap)
    {
    }

    private void OnAllMissionRemove()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == true && Tool_Info.Instance.m_eCurrMissionTabType == m_eMissionType)
            {
                MapDataMissionInfo_Mouse pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Mouse;

                pMissionInfo.m_AppearDisturbSlotIndexTable.Clear();
            }
        }
    }
}

#endif