using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_Mission_05 : MonoBehaviour
{
    protected eMissionType m_eMissionType = eMissionType.None;

    private Text[]      m_pText_RockCount  = new Text[4];

    void Start()
    {
        m_eMissionType = eMissionType.Rock;

        GameObject ob;

        for (int i = 0; i < 4; ++i)
        {
            string strButtonName = "Button_" + (i + 1).ToString();
            ob = Helper.FindChildGameObject(gameObject, strButtonName);
            ob = Helper.FindChildGameObject(ob, "Text");
            m_pText_RockCount[i] = ob.GetComponent<Text>();
        }

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

    public void OnButtonClick_RockCount(int nCount)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == false)
            {
                MapDataMissionInfo_Rock pMissionInfo = new MapDataMissionInfo_Rock();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(m_eMissionType, pMissionInfo);
            }

            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMission;
            Tool_Info.Instance.m_eCurrEditMissionType = m_eMissionType;
        }

        if (Tool_Info.Instance.m_nRockCount != -1)
        {
            m_pText_RockCount[Tool_Info.Instance.m_nRockCount - 1].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_nRockCount = nCount;
        m_pText_RockCount[Tool_Info.Instance.m_nRockCount - 1].color = new Color(1, 0, 0);

        EventDelegateManager_ForTool.Instance.OnMission_Common_UnCheck();
    }

    public void OnMission_UnCheck()
    {
        if (Tool_Info.Instance.m_nRockCount != -1)
        {
            m_pText_RockCount[Tool_Info.Instance.m_nRockCount - 1].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_nAppleCount = -1;
    }

    private void OnUpdateMap(bool IsChangeMap)
    {
        if (Tool_Info.Instance.m_eCurrMissionTabType != m_eMissionType)
        {
            if (Tool_Info.Instance.m_nRockCount != -1)
            {
                m_pText_RockCount[Tool_Info.Instance.m_nRockCount - 1].color = new Color(0, 0, 0);
            }

            Tool_Info.Instance.m_nRockCount = -1;
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
                MapDataMissionInfo_Rock pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Rock;

                pMissionInfo.m_AppearRockTable.Clear();
            }
        }
    }
}

#endif