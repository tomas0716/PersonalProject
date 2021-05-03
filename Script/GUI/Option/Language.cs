using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    void Start()
    {
        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(gameObject, "Text_Title");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.LanguageSelect);

        ob = Helper.FindChildGameObject(gameObject, "Scroll View");
        ob = Helper.FindChildGameObject(ob, "Viewport");
        ob = Helper.FindChildGameObject(ob, "Content");

        GameObject ob_Item;
        LanguageItem pLanguageItem;

        ob_Item = Resources.Load<GameObject>("Gui/Prefabs/LanguageItem");
        ob_Item = GameObject.Instantiate(ob_Item);
        ob_Item.transform.SetParent(ob.transform);
        ob_Item.transform.localScale = Vector3.one;
        pLanguageItem = ob_Item.GetComponent<LanguageItem>();
        pLanguageItem.SetInfo(AppInstance.Instance.m_pOptionInfo.m_strCountryCode, true);

        for (int i = 0; i < (int)eLanguage.Max; ++i)
        {
            eLanguage eLang = (eLanguage)i;

            if (eLang.ToString() != AppInstance.Instance.m_pOptionInfo.m_strCountryCode)
            {
                ob_Item = Resources.Load<GameObject>("Gui/Prefabs/LanguageItem");
                ob_Item = GameObject.Instantiate(ob_Item);
                ob_Item.transform.SetParent(ob.transform);
                ob_Item.transform.localScale = Vector3.one;
                pLanguageItem = ob_Item.GetComponent<LanguageItem>();
                pLanguageItem.SetInfo(eLang.ToString(), false);
            }
        }
    }
    
    void Update()
    {
        
    }

    public void OnButtonClick_Close()
    {
        GameObject.Destroy(gameObject);
    }
}
