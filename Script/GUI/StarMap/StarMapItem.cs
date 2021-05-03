using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarMapItem : MonoBehaviour
{
    private StarMap     m_pStarMap  = null;
    private int         m_nLevel    = 0;

    void Start()
    {
        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(gameObject, "Text_Level");
        pText = ob.GetComponent<Text>();
        pText.text = m_nLevel.ToString();

        GameObject [] ob_Star = new GameObject[3];

        for (int i = 0; i < 3; ++i)
        {
            ob_Star[i] = Helper.FindChildGameObject(gameObject, "Image_Star_0" + (i+1).ToString());
            ob_Star[i].SetActive(false);
        }

        int nStarCount = SavedGameDataInfo.Instance.m_LevelStarCountList[m_nLevel-1];

        for (int i = 0; i < nStarCount; ++i)
        {
            ob_Star[i].SetActive(true);
        }
    }

    void Update()
    {
        
    }

    public void SetLevel(StarMap pStarMap, int nLevel)
    {
        m_pStarMap = pStarMap;
        m_nLevel = nLevel;
    }

    public void OnButtonClick_Item()
    {
        m_pStarMap.OnNewGameOpen(m_nLevel);
    }
}
