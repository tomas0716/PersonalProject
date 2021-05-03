using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageItem : MonoBehaviour
{
    private string m_strLanguage = "";
    private bool m_IsSelect = false;

    void Start()
    {
        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(gameObject, "Text_Country");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Country, m_strLanguage);

        ob = Helper.FindChildGameObject(gameObject, "Image_Select");
        ob.SetActive(m_IsSelect);

        ob = Helper.FindChildGameObject(gameObject, "Text_CountrySelect");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.CountrySelect, m_strLanguage);
        ob.SetActive(!m_IsSelect);
    }

    void Update()
    {
        
    }

    public void SetInfo(string strLanguage, bool IsSelect)
    {
        m_strLanguage = strLanguage;
        m_IsSelect = IsSelect;
    }

    public void OnButtonClick_Select()
    {
        if (m_IsSelect == false)
        {
            GameInfo.Instance.m_IsChangeLanguage = true;
            AppInstance.Instance.m_pOptionInfo.m_strCountryCode = m_strLanguage;
            AppInstance.Instance.m_pOptionInfo.Save();

            AppInstance.Instance.m_pSceneManager.ChangeScene(eSceneType.Scene_Middle, false);
        }
    }
}
