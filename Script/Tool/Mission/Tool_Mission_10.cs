using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_Mission_10 : MonoBehaviour
{
    protected eMissionType m_eMissionType = eMissionType.None;

    private Text m_pText_Button_Bread = null;

    void Start()
    {
        m_eMissionType = eMissionType.Bread;

        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Button_Create");
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Button_Bread = ob.GetComponent<Text>();

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
                MapDataMissionInfo_Bread pMissionInfo = new MapDataMissionInfo_Bread();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(m_eMissionType, pMissionInfo);
            }

            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMission;
            Tool_Info.Instance.m_eCurrEditMissionType = m_eMissionType;

            m_pText_Button_Bread.color = new Color(1, 0, 0);

            EventDelegateManager_ForTool.Instance.OnMission_Common_UnCheck();
        }
    }

    public void OnMission_UnCheck()
    {
        m_pText_Button_Bread.color = new Color(0, 0, 0);
    }

    private void OnUpdateMap(bool IsChangeMap)
    {
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
                MapDataMissionInfo_Bread pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Bread;

                pMissionInfo.m_AppearBreadLeftTopSlotIndexList.Clear();
            }
        }
    }
}

#endif