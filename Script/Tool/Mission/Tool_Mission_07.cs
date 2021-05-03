using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_Mission_07 : MonoBehaviour
{
    protected eMissionType m_eMissionType = eMissionType.None;

    private InputField m_pInputField_Cross = null;

    void Start()
    {
        m_eMissionType = eMissionType.Cross;

        GameObject ob;

        ob = Helper.FindChildGameObject(gameObject, "InputField_Cross");
        m_pInputField_Cross = ob.GetComponent<InputField>();

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

    public void OnInputField_EndEdit(string strData)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            int nCount = Helper.ConvertStringToInt(m_pInputField_Cross.text);

            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == true)
            {
                MapDataMissionInfo_Cross pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Cross;

                if (pMissionInfo != null)
                {
                    pMissionInfo.m_nCount = nCount;
                }
            }
            else
            {
                MapDataMissionInfo_Cross pMissionInfo = new MapDataMissionInfo_Cross();
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
                MapDataMissionInfo_Cross pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Cross;

                if (pMissionInfo != null)
                {
                    m_pInputField_Cross.text = pMissionInfo.m_nCount.ToString();
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
                m_pInputField_Cross.text = "0";
            }
        }
    }

    private void OnPostUpdateMap(bool IsChangeMap)
    {
    }

    public void OnAllMissionRemove()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == true && Tool_Info.Instance.m_eCurrMissionTabType == m_eMissionType)
        {
            MapDataMissionInfo_Cross pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Cross;

            if (pMissionInfo != null)
            {
                pMissionInfo.m_nCount = 0;
                m_pInputField_Cross.text = "0";
            }
        }
    }
}

#endif