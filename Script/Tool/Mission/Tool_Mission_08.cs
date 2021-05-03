using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_Mission_08 : MonoBehaviour
{
    protected eMissionType m_eMissionType = eMissionType.None;

    private Toggle m_pToggle_ActiveGrass = null;

    void Start()
    {
        m_eMissionType = eMissionType.Grass;

        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Toggle_ActiveGrass");
        m_pToggle_ActiveGrass = ob.GetComponent<Toggle>();
        m_pToggle_ActiveGrass.isOn = false;

        EventDelegateManager_ForTool.Instance.OnEventUpdateMap += OnUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventPostUpdateMap += OnPostUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventOnAllMissionRemove += OnAllMissionRemove;
    }

    private void OnDestroy()
    {
        EventDelegateManager_ForTool.Instance.OnEventUpdateMap -= OnUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventPostUpdateMap -= OnPostUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventOnAllMissionRemove -= OnAllMissionRemove;
    }

    void Update()
    {

    }

    public void OnToggle_ValueChanged_ActiveGrass(bool IsCheck)
    {
        if (SavedGameDataInfo.Instance != null && SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == false)
            {
                MapDataMissionInfo_Grass pMissionInfo = new MapDataMissionInfo_Grass();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                pMissionInfo.m_IsActiveMission = m_pToggle_ActiveGrass.isOn;
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(m_eMissionType, pMissionInfo);
            }
            else
            {
                MapDataMissionInfo_Grass pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Grass;
                pMissionInfo.m_IsActiveMission = m_pToggle_ActiveGrass.isOn;
            }

            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
        }
    }

    private void OnUpdateMap(bool IsChangeMap)
    {
        if (SavedGameDataInfo.Instance != null && SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            bool IsValid = true;
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == true)
            {
                MapDataMissionInfo_Grass pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Grass;

                if (pMissionInfo != null)
                {
                    m_pToggle_ActiveGrass.isOn = pMissionInfo.m_IsActiveMission;
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
                m_pToggle_ActiveGrass.isOn = false;
            }
        }
        else
        {
            m_pToggle_ActiveGrass.isOn = false;
        }
    }

    private void OnPostUpdateMap(bool IsChangeMap)
    {
    }

    public void OnAllMissionRemove()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == true && Tool_Info.Instance.m_eCurrMissionTabType == m_eMissionType)
        {
            MapDataMissionInfo_Grass pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Grass;
            if (pMissionInfo != null)
            {
                pMissionInfo.m_IsActiveMission = false;
                m_pToggle_ActiveGrass.isOn = false;
            }
        }
    }
}

#endif