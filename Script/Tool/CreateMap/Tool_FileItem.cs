using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
public class Tool_FileItem : MonoBehaviour
{
    private Tool_CreateMap m_pCreateMap = null;
    private GameObject m_pGameObject_Sel = null;
    private Text m_pText_FileName = null;

    private float m_fPrevButtonClick = 0;

    public void Init(Tool_CreateMap pCreateMap, string strFileName)
    {
        m_pCreateMap = pCreateMap;

        m_pGameObject_Sel = Helper.FindChildGameObject(gameObject, "Image_Sel");
        GameObject ob = Helper.FindChildGameObject(gameObject, "Text");
        m_pText_FileName = ob.GetComponent<Text>();
        m_pText_FileName.text = strFileName;
    }

    public string GetFileName()
    {
        return m_pText_FileName.text;
    }

    public void OnButtonClick()
    {
        //if (Time.time - m_fPrevButtonClick < 0.25f)
        //{
        //    m_fPrevButtonClick = 0;
        //    m_pCreateMap.OnFileSel(this);

        //    m_pGameObject_Sel.SetActive(true);
        //}
        //else
        //{
        //    m_fPrevButtonClick = Time.time;
        //}

        m_pCreateMap.OnFileSel(this);
        m_pGameObject_Sel.SetActive(true);
    }

    public void Able_Sel()
    {
        m_pGameObject_Sel.SetActive(true);
    }

    public void Disable_Sel()
    {
        m_pGameObject_Sel.SetActive(false);
    }
}

#endif