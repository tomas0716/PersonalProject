using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_Mission_14 : MonoBehaviour
{
    protected eMissionType m_eMissionType = eMissionType.None;

    private Text        m_pText_Button_Octopus          = null;
    private InputField  m_pInputField_OctopusInkCount   = null;

    void Start()
    {
        m_eMissionType = eMissionType.Octopus;

        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Button_Create");
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Button_Octopus = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "InputField_OctopusInkCount");
        m_pInputField_OctopusInkCount = ob.GetComponent<InputField>();

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
                MapDataMissionInfo_Octopus pMissionInfo = new MapDataMissionInfo_Octopus();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(m_eMissionType, pMissionInfo);
            }

            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMission;
            Tool_Info.Instance.m_eCurrEditMissionType = m_eMissionType;

            m_pText_Button_Octopus.color = new Color(1, 0, 0);

            EventDelegateManager_ForTool.Instance.OnMission_Common_UnCheck();
        }
    }

    public void OnMission_UnCheck()
    {
        m_pText_Button_Octopus.color = new Color(0, 0, 0);
    }

    public void OnInputField_EndEdit_OctopusInkCount(string strData)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            int nCount = Helper.ConvertStringToInt(m_pInputField_OctopusInkCount.text);

            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eMissionType.Octopus) == true)
            {
                MapDataMissionInfo_Octopus pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eMissionType.Octopus] as MapDataMissionInfo_Octopus;

                if (pMissionInfo != null)
                {
                    pMissionInfo.m_nOctopusInkCount = nCount;
                }
            }
            else
            {
                MapDataMissionInfo_Octopus pMissionInfo = new MapDataMissionInfo_Octopus();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                pMissionInfo.m_nOctopusInkCount = nCount;
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(eMissionType.Octopus, pMissionInfo);
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
                MapDataMissionInfo_Octopus pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Octopus;

                if (pMissionInfo != null)
                {
                    m_pInputField_OctopusInkCount.text = pMissionInfo.m_nOctopusInkCount.ToString();
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
                m_pInputField_OctopusInkCount.text = "0";
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
                MapDataMissionInfo_Octopus pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Octopus;

                pMissionInfo.m_AppearOctopusSlotIndexList.Clear();
            }
        }
    }
}

#endif