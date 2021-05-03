using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_Mission_02 : MonoBehaviour
{
    protected eMissionType m_eMissionType = eMissionType.None;

    private InputField      m_pInputField_HideBellCount                 = null;
    private Dropdown        m_pDropdown_HideBellAppearType              = null;
    private InputField      m_pInputField_ChangeSlot_Create_Count_Min   = null;
    private InputField      m_pInputField_ChangeSlot_Create_Count_Max   = null;

    private Text            m_pText_Button_Bell_Create                  = null;
    private Text            m_pText_Button_Bell_Goal                    = null;
    private Text            m_pText_Button_Bell_Machine                 = null;

    void Start()
    {
        m_eMissionType = eMissionType.Bell;

        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "InputField_HideBellCount");
        m_pInputField_HideBellCount = ob.GetComponent<InputField>();

        ob = Helper.FindChildGameObject(gameObject, "Dropdown_HideBellAppearType");
        m_pDropdown_HideBellAppearType = ob.GetComponent<Dropdown>();

        ob = Helper.FindChildGameObject(gameObject, "InputField_ChangeSlot_Create_Count_Min");
        m_pInputField_ChangeSlot_Create_Count_Min = ob.GetComponent<InputField>();

        ob = Helper.FindChildGameObject(gameObject, "InputField_ChangeSlot_Create_Count_Max");
        m_pInputField_ChangeSlot_Create_Count_Max = ob.GetComponent<InputField>();

        ob = Helper.FindChildGameObject(gameObject, "Button_Bell_Create");
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Button_Bell_Create = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_Bell_Goal");
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Button_Bell_Goal = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_Bell_Machine");
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Button_Bell_Machine = ob.GetComponent<Text>();

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

    public void OnButtonClick_BellCreate()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == false)
            {
                MapDataMissionInfo_Bell pMissionInfo = new MapDataMissionInfo_Bell();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(m_eMissionType, pMissionInfo);
            }

            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMission;
            Tool_Info.Instance.m_eCurrEditMissionType = m_eMissionType;
            Tool_Info.Instance.m_eBell_EditMode = eTool_Bell_EditMode.eBell_Create;

            m_pText_Button_Bell_Create.color = new Color(1, 0, 0);
            m_pText_Button_Bell_Goal.color = new Color(0, 0, 0);
            m_pText_Button_Bell_Machine.color = new Color(0, 0, 0);

            EventDelegateManager_ForTool.Instance.OnMission_Common_UnCheck();
        }
    }

    public void OnButtonClick_BellGoal()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == false)
            {
                MapDataMissionInfo_Bell pMissionInfo = new MapDataMissionInfo_Bell();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(m_eMissionType, pMissionInfo);
            }

            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMission;
            Tool_Info.Instance.m_eCurrEditMissionType = m_eMissionType;
            Tool_Info.Instance.m_eBell_EditMode = eTool_Bell_EditMode.eBell_Goal;

            m_pText_Button_Bell_Goal.color = new Color(1, 0, 0);
            m_pText_Button_Bell_Create.color = new Color(0, 0, 0);
            m_pText_Button_Bell_Machine.color = new Color(0, 0, 0);

            EventDelegateManager_ForTool.Instance.OnMission_Common_UnCheck();
        }
    }

    public void OnButtonClick_BellMachine()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(m_eMissionType) == false)
            {
                MapDataMissionInfo_Bell pMissionInfo = new MapDataMissionInfo_Bell();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(m_eMissionType, pMissionInfo);
            }

            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMission;
            Tool_Info.Instance.m_eCurrEditMissionType = m_eMissionType;
            Tool_Info.Instance.m_eBell_EditMode = eTool_Bell_EditMode.eBell_Machine;

            m_pText_Button_Bell_Machine.color = new Color(1, 0, 0);
            m_pText_Button_Bell_Goal.color = new Color(0, 0, 0);
            m_pText_Button_Bell_Create.color = new Color(0, 0, 0);
            EventDelegateManager_ForTool.Instance.OnMission_Common_UnCheck();
        }
    }

    public void OnMission_UnCheck()
    {
        m_pText_Button_Bell_Machine.color = new Color(0, 0, 0);
        m_pText_Button_Bell_Create.color = new Color(0, 0, 0);
        m_pText_Button_Bell_Goal.color = new Color(0, 0, 0);
        Tool_Info.Instance.m_eBell_EditMode = eTool_Bell_EditMode.eNone;
    }

    public void OnInputField_EndEdit_HideBellCount(string strData)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            int nCount = Helper.ConvertStringToInt(m_pInputField_HideBellCount.text);

            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eMissionType.Bell) == true)
            {
                MapDataMissionInfo_Bell pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eMissionType.Bell] as MapDataMissionInfo_Bell;

                if (pMissionInfo != null)
                {
                    pMissionInfo.m_nHideBellCount = nCount;
                }
            }
            else
            {
                MapDataMissionInfo_Bell pMissionInfo = new MapDataMissionInfo_Bell();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                pMissionInfo.m_nHideBellCount = nCount;
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(eMissionType.Bell, pMissionInfo);
            }

            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
        }
    }

    public void OnDropdown_Value_Changed_HideBellAppearType(int nValue)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            MapDataMissionInfo_Bell.eHideBellAppearType eType = (MapDataMissionInfo_Bell.eHideBellAppearType)m_pDropdown_HideBellAppearType.value;
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eMissionType.Bell) == true)
            {
                MapDataMissionInfo_Bell pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eMissionType.Bell] as MapDataMissionInfo_Bell;

                if (pMissionInfo != null)
                {
                    pMissionInfo.m_eHideBellAppearType = eType;
                }
            }
            else
            {
                MapDataMissionInfo_Bell pMissionInfo = new MapDataMissionInfo_Bell();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                pMissionInfo.m_eHideBellAppearType = eType;
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(eMissionType.Bell, pMissionInfo);
            }

            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
        }
    }

    public void OnInputField_EndEdit_ChangeSlot_Create_Count_Min(string strData)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            int nCount = Helper.ConvertStringToInt(m_pInputField_ChangeSlot_Create_Count_Min.text);

            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eMissionType.Bell) == true)
            {
                MapDataMissionInfo_Bell pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eMissionType.Bell] as MapDataMissionInfo_Bell;

                if (pMissionInfo != null)
                {
                    pMissionInfo.m_nCount_ChangeSlot_Create_Min = nCount;
                }
            }
            else
            {
                MapDataMissionInfo_Bell pMissionInfo = new MapDataMissionInfo_Bell();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                pMissionInfo.m_nCount_ChangeSlot_Create_Min = nCount;
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(eMissionType.Bell, pMissionInfo);
            }

            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
        }
    }

    public void OnInputField_EndEdit_ChangeSlot_Create_Count_Max(string strData)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            int nCount = Helper.ConvertStringToInt(m_pInputField_ChangeSlot_Create_Count_Max.text);

            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eMissionType.Bell) == true)
            {
                MapDataMissionInfo_Bell pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eMissionType.Bell] as MapDataMissionInfo_Bell;

                if (pMissionInfo != null)
                {
                    pMissionInfo.m_nCount_ChangeSlot_Create_Max = nCount;
                }
            }
            else
            {
                MapDataMissionInfo_Bell pMissionInfo = new MapDataMissionInfo_Bell();
                pMissionInfo.SetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
                pMissionInfo.m_nCount_ChangeSlot_Create_Max = nCount;
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.Add(eMissionType.Bell, pMissionInfo);
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
                MapDataMissionInfo_Bell pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Bell;

                if (pMissionInfo != null)
                {
                    m_pInputField_HideBellCount.text = pMissionInfo.m_nHideBellCount.ToString();
                    m_pDropdown_HideBellAppearType.value = (int)pMissionInfo.m_eHideBellAppearType;
                    m_pInputField_ChangeSlot_Create_Count_Min.text = pMissionInfo.m_nCount_ChangeSlot_Create_Min.ToString();
                    m_pInputField_ChangeSlot_Create_Count_Max.text = pMissionInfo.m_nCount_ChangeSlot_Create_Max.ToString();
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
                m_pInputField_HideBellCount.text = "0";
                m_pDropdown_HideBellAppearType.value = 0;
                m_pInputField_ChangeSlot_Create_Count_Min.text = "0";
                m_pInputField_ChangeSlot_Create_Count_Max.text = "0";
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
                MapDataMissionInfo_Bell pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[m_eMissionType] as MapDataMissionInfo_Bell;

                pMissionInfo.m_AppearBellSlotIndexList.Clear();
                pMissionInfo.m_BellGoalSlotIndexList.Clear();
                pMissionInfo.m_nHideBellCount = 0;
                pMissionInfo.m_HideBellCreateSlotIndexList.Clear();
            }
        }
    }
}

#endif