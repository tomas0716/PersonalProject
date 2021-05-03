using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_FolderItem : MonoBehaviour
{
    private Tool_CreateMap m_pCreateMap = null;
    private GameObject m_pGameObject_Sel = null;
    private Text m_pText_FolderName = null;

    private float m_fPrevButtonClick = 0;

    public void Init(Tool_CreateMap pCreateMap, string strFolderName)
    {
        m_pCreateMap = pCreateMap;

        m_pGameObject_Sel = Helper.FindChildGameObject(gameObject, "Image_Sel");
        m_pGameObject_Sel.SetActive(false);
        GameObject ob = Helper.FindChildGameObject(gameObject, "Text");
        m_pText_FolderName = ob.GetComponent<Text>();
        m_pText_FolderName.text = strFolderName;
    }

    public string GetFolderName()
    {
        return m_pText_FolderName.text;
    }

	public void OnButtonClick()
    {
        //if (Time.time - m_fPrevButtonClick < 0.25f)
        //{
        //    m_fPrevButtonClick = 0;
        //    m_pCreateMap.OnFolderSel(this);

        //    m_pGameObject_Sel.SetActive(true);
        //}
        //else
        //{
        //    m_fPrevButtonClick = Time.time;
        //}

        m_pCreateMap.OnFolderSel(this);
        m_pGameObject_Sel.SetActive(true);
    }

    public void Disable_Sel()
    {
        m_pGameObject_Sel.SetActive(false);
    }
}

#endif