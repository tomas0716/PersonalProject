using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_MainMenu_Right : MonoBehaviour
{
    private eTool_MainMenu  m_eCurrMainMenu     = eTool_MainMenu.eMapShape;
    private Text []         m_pText_MainMenu    = new Text[(int)eTool_MainMenu.eMax];

    void Start()
    {
        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Button_MapShape");
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_MainMenu[(int)eTool_MainMenu.eMapShape] = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_Mission");
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_MainMenu[(int)eTool_MainMenu.eMission] = ob.GetComponent<Text>();

        m_pText_MainMenu[(int)eTool_MainMenu.eMapShape].color = new Color(1,0,0);
    }

    public void OnButtonClick_MapShape()
    {
        if (m_eCurrMainMenu != eTool_MainMenu.eMapShape)
        {
            m_pText_MainMenu[(int)m_eCurrMainMenu].color = new Color(0, 0, 0);
            m_eCurrMainMenu = eTool_MainMenu.eMapShape;
            m_pText_MainMenu[(int)m_eCurrMainMenu].color = new Color(1, 0, 0);

            Tool_Info.Instance.m_eCurrMainMenu = eTool_MainMenu.eMapShape;

            Tool_WindowCollect.Instance.m_GameObject_MapShape.SetActive(true);
            Tool_WindowCollect.Instance.m_GameObject_Mission.SetActive(false);

            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eNone;
            Tool_Info.Instance.m_eCurrEditMissionType = eMissionType.None;

            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
        }
    }

    public void OnButtonClick_Mission()
    {
        if (m_eCurrMainMenu != eTool_MainMenu.eMission)
        {
            m_pText_MainMenu[(int)m_eCurrMainMenu].color = new Color(0, 0, 0);
            m_eCurrMainMenu = eTool_MainMenu.eMission;
            m_pText_MainMenu[(int)m_eCurrMainMenu].color = new Color(1, 0, 0);

            Tool_Info.Instance.m_eCurrMainMenu = eTool_MainMenu.eMission;

            Tool_WindowCollect.Instance.m_GameObject_MapShape.SetActive(false);
            Tool_WindowCollect.Instance.m_GameObject_Mission.SetActive(true);

            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMission;
            Tool_Info.Instance.m_eCurrEditMissionType = eMissionType.None;

            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
        }
    }
}

#endif