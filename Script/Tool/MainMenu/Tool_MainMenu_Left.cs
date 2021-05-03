using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_MainMenu_Left : MonoBehaviour
{
    private eTool_MainMenu  m_eCurrMainMenu     = eTool_MainMenu.eFileCreate;
    private Text []         m_pText_MainMenu    = new Text[(int)eTool_MainMenu.eMax];

    void Start()
    {
        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Button_CreateMap");
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_MainMenu[(int)eTool_MainMenu.eFileCreate] = ob.GetComponent<Text>();

        m_pText_MainMenu[(int)eTool_MainMenu.eFileCreate].color = new Color(1,0,0);
    }

    public void OnButtonClick_CreateMap()
    {
        if (m_eCurrMainMenu != eTool_MainMenu.eFileCreate)
        {
            m_pText_MainMenu[(int)m_eCurrMainMenu].color = new Color(0, 0, 0);
            m_eCurrMainMenu = eTool_MainMenu.eFileCreate;
            m_pText_MainMenu[(int)m_eCurrMainMenu].color = new Color(1, 0, 0);

            Tool_WindowCollect.Instance.m_GameObject_CreateMap.SetActive(true);
        }
    }
}

#endif