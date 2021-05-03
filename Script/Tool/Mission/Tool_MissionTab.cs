using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_MissionToggle
{
    private Tool_MissionTab     m_pMissionTab   = null;
    private eMissionType        m_eMissionType  = eMissionType.None;
    public Tool_MissionToggle(Tool_MissionTab pMissionTab, eMissionType eType)
    {
        m_pMissionTab = pMissionTab;
        m_eMissionType = eType; 
    }

    public void OnToggle_ValueChanged(bool IsCheck)
    {
        m_pMissionTab.OnToggle_ValueChanged(IsCheck, m_eMissionType);
    }
}

public class Tool_MissionTab : MonoBehaviour
{
    public class  Tool_MissionObject
    {
        public GameObject   m_pGameObject_Button    = null;
        public Text         m_pText_MissionType     = null;
        public GameObject   m_pGameObject_Toggle    = null;
        public GameObject   m_pGameObject_Panel     = null;
    }

    private Dictionary<eMissionType, Tool_MissionObject>    m_MissionTable = new Dictionary<eMissionType, Tool_MissionObject>(); 

    private Tool_Mission_Base [] m_pMission_Base    = new Tool_Mission_Base[(int)eMissionType.Max];

    void Start()
    {
        for (int i = ((int)eMissionType.None) + 1; i < (int)eMissionType.Max; ++i)
        {
            Tool_MissionObject pMissionObject = new Tool_MissionObject();
            string strMissionName = "Mission_" + i.ToString("D2");
            GameObject ob = Helper.FindChildGameObject(gameObject, strMissionName);
            m_pMission_Base[i] = ob.GetComponent<Tool_Mission_Base>();
            pMissionObject.m_pGameObject_Button = Helper.FindChildGameObject(ob, "Button");
            GameObject ob_Text = Helper.FindChildGameObject(pMissionObject.m_pGameObject_Button, "Text");
            pMissionObject.m_pText_MissionType = ob_Text.GetComponent<Text>();
            pMissionObject.m_pGameObject_Panel= Helper.FindChildGameObject(ob, "Panel");
            pMissionObject.m_pGameObject_Toggle = Helper.FindChildGameObject(pMissionObject.m_pGameObject_Button, "Toggle");

            Button pButton = pMissionObject.m_pGameObject_Button.GetComponent<Button>();
            int nMissionType = i;
            pButton.onClick.AddListener(delegate { OnButtonClick_Mission(nMissionType); });

            Toggle pToggle = pMissionObject.m_pGameObject_Toggle.GetComponent<Toggle>();
            pToggle.onValueChanged.AddListener((new Tool_MissionToggle(this, (eMissionType)i)).OnToggle_ValueChanged);

            if (i == 1)
            {
                pMissionObject.m_pGameObject_Panel.SetActive(true);
                pMissionObject.m_pText_MissionType.color = new Color(1,0,0);
            }
            else
            {
                pMissionObject.m_pGameObject_Panel.SetActive(false);
            }

            m_MissionTable.Add((eMissionType)i, pMissionObject);
        }

        EventDelegateManager_ForTool.Instance.OnEventUpdateMap += OnUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventPostUpdateMap += OnPostUpdateMap;
    }

    private void OnDestroy()
    {
        EventDelegateManager_ForTool.Instance.OnEventUpdateMap -= OnUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventPostUpdateMap -= OnPostUpdateMap;
    }

    void Update()
    {
    }

    public void OnButtonClick_Mission(int nMission)
    {
        if (m_MissionTable.ContainsKey(Tool_Info.Instance.m_eCurrMissionTabType) == true)
        {
            m_MissionTable[Tool_Info.Instance.m_eCurrMissionTabType].m_pGameObject_Panel.SetActive(false);
            m_MissionTable[Tool_Info.Instance.m_eCurrMissionTabType].m_pText_MissionType.color = new Color(1,1,1);
        }

        EventDelegateManager_ForTool.Instance.OnMission_UnCheck();

        Tool_Info.Instance.m_eCurrMissionTabType = (eMissionType)nMission;

        if (m_MissionTable.ContainsKey(Tool_Info.Instance.m_eCurrMissionTabType) == true)
        {
            m_MissionTable[Tool_Info.Instance.m_eCurrMissionTabType].m_pGameObject_Panel.SetActive(true);
            m_MissionTable[Tool_Info.Instance.m_eCurrMissionTabType].m_pText_MissionType.color = new Color(1, 0, 0);
        }

        Tool_Info.Instance.m_eCurrEditMissionType = eMissionType.None;

        EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
        EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
    }

    public void OnToggle_ValueChanged(bool IsCheck, eMissionType eType)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
            {
                MapDataMissionInfo pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType];

                if (pMissionInfo != null && pMissionInfo.GetMissionCount() != 0)
                {
                    Tool_Info.Instance.m_IsActiveMission[(int)eType] = IsCheck;
                }
                else
                {
                    if (m_MissionTable.ContainsKey(eType) == true)
                    {
                        Toggle pToggle = m_MissionTable[eType].m_pGameObject_Toggle.GetComponent<Toggle>();
                        pToggle.isOn = false;
                    }

                    Tool_Info.Instance.m_IsActiveMission[(int)eType] = false;
                }
            }
            else
            {
                if (m_MissionTable.ContainsKey(eType) == true)
                {
                    Toggle pToggle = m_MissionTable[eType].m_pGameObject_Toggle.GetComponent<Toggle>();
                    pToggle.isOn = false;
                }

                Tool_Info.Instance.m_IsActiveMission[(int)eType] = false;
            }
        }
    }

    private void OnUpdateMap(bool IsChangeMap)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            foreach (KeyValuePair<eMissionType, Tool_MissionObject> item in m_MissionTable)
            {
                bool IsValid = false;
                if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(item.Key) == true)
                {
                    MapDataMissionInfo pMapDataMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[item.Key];
                    if (pMapDataMissionInfo != null && pMapDataMissionInfo.GetMissionCount() != 0)
                    {
                        IsValid = true;
                    }
                }

                Image pImage = item.Value.m_pGameObject_Button.GetComponent<Image>();
                
                if (IsValid == true)
                {
                    pImage.color = new Color(137.0f/255.0f, 74.0f / 255.0f, 132.0f / 255.0f);
                }
                else
                {
                    pImage.color = new Color(128.0f / 255.0f, 128.0f / 255.0f, 128.0f / 255.0f);
                }
            }
        }
    }

    private void OnPostUpdateMap(bool IsChangeMap)
    {
        if (IsChangeMap == true)
        {
            Tool_Info.Instance.InActiveMission();

            foreach (KeyValuePair<eMissionType, Tool_MissionObject> item in m_MissionTable)
            {
                Toggle pToggle = item.Value.m_pGameObject_Toggle.GetComponent<Toggle>();
                pToggle.isOn = false;
            }
        }
    }
}

#endif