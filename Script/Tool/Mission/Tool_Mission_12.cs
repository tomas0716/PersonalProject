using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_Mission_12 : MonoBehaviour
{
    protected eMissionType m_eMissionType = eMissionType.None;

    private Text        m_pText_Button_Fish                 = null; 
    private Text        m_pText_Button_Machine              = null;

    private InputField  m_pInputField_HideFishCount         = null;
    private InputField  m_pInputField_HideFishCreatePercent = null;

    void Start()
    {
        m_eMissionType = eMissionType.Fish;

        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Button_Create");
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Button_Fish = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_Fish_Machine");
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Button_Machine = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "InputField_HideFishCount");
        m_pInputField_HideFishCount = ob.GetComponent<InputField>();

        ob = Helper.FindChildGameObject(gameObject, "InputField_HideFishCreatePercent");
        m_pInputField_HideFishCreatePercent = ob.GetComponent<InputField>();

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

    public void OnButtonClick_Create()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == false)
            {
                MapDataMissionInfo_Fish pMissionInfo = new MapDataMissionInfo_Fish();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(m_eMissionType, pMissionInfo);
            }

            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMission;
            Tool_Info.Instance.m_eCurrEditMissionType = m_eMissionType;
            Tool_Info.Instance.m_eFish_EditMode = eTool_Fish_EditMode.eFish_Create;

            m_pText_Button_Fish.color = new Color(1, 0, 0);
            m_pText_Button_Machine.color = new Color(0, 0, 0);

            EventDelegateManager_ForTool.Instance.OnMission_Common_UnCheck();
        }
    }

    public void OnButtonClick_Fish_Machine()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == false)
            {
                MapDataMissionInfo_Fish pMissionInfo = new MapDataMissionInfo_Fish();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(m_eMissionType, pMissionInfo);
            }

            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMission;
            Tool_Info.Instance.m_eCurrEditMissionType = m_eMissionType;
            Tool_Info.Instance.m_eFish_EditMode = eTool_Fish_EditMode.eFish_Machine;

            m_pText_Button_Machine.color = new Color(1, 0, 0);
            m_pText_Button_Fish.color = new Color(0, 0, 0);

            EventDelegateManager_ForTool.Instance.OnMission_Common_UnCheck();
        }
    }

    public void OnMission_UnCheck()
    {
        m_pText_Button_Fish.color = new Color(0, 0, 0);
        m_pText_Button_Machine.color = new Color(0, 0, 0);
        Tool_Info.Instance.m_eFish_EditMode = eTool_Fish_EditMode.eNone;
    }

    public void OnInputField_EndEdit_HideFishCount(string strData)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            int nCount = Helper.ConvertStringToInt(m_pInputField_HideFishCount.text);

            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eMissionType.Fish) == true)
            {
                MapDataMissionInfo_Fish pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eMissionType.Fish] as MapDataMissionInfo_Fish;

                if (pMissionInfo != null)
                {
                    pMissionInfo.m_nHideFishCount = nCount;
                }
            }
            else
            {
                MapDataMissionInfo_Fish pMissionInfo = new MapDataMissionInfo_Fish();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                pMissionInfo.m_nHideFishCount = nCount;
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(eMissionType.Fish, pMissionInfo);
            }

            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
        }
    }

    public void OnInputField_EndEdit_HideFishCreatePercent(string strData)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            int nCount = Helper.ConvertStringToInt(m_pInputField_HideFishCreatePercent.text);

            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eMissionType.Fish) == true)
            {
                MapDataMissionInfo_Fish pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eMissionType.Fish] as MapDataMissionInfo_Fish;

                if (pMissionInfo != null)
                {
                    pMissionInfo.m_nHideFishCreatePercent = nCount;
                }
            }
            else
            {
                MapDataMissionInfo_Fish pMissionInfo = new MapDataMissionInfo_Fish();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                pMissionInfo.m_nHideFishCreatePercent = nCount;
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(eMissionType.Fish, pMissionInfo);
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
                MapDataMissionInfo_Fish pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Fish;

                if (pMissionInfo != null)
                {
                    m_pInputField_HideFishCount.text = pMissionInfo.m_nHideFishCount.ToString();
                    m_pInputField_HideFishCreatePercent.text = pMissionInfo.m_nHideFishCreatePercent.ToString();
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
                m_pInputField_HideFishCount.text = "0";
                m_pInputField_HideFishCreatePercent.text = "0";
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
                MapDataMissionInfo_Fish pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Fish;

                pMissionInfo.m_AppearFishSlotIndexList.Clear();
            }
        }
    }
}

#endif