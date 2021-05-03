using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarMap : MonoBehaviour
{
    private GameObject          m_pGameObject_Parent                = null;
    private Transformer_Scalar  m_pScale                            = new Transformer_Scalar(0);

    private GameObject          m_pGameObject_Image_Entire          = null;
    private GameObject          m_pGameObject_Image_InComplete      = null;

    private GameObject          m_pGameObject_ListView_Entire       = null;
    private GameObject          m_pGameObject_ListView_InComplete   = null;

    private bool                m_IsInitScroll                      = false;


    void Start()
    {
        GameObject ob;
        Text pText;

        m_pGameObject_Parent = Helper.FindChildGameObject(gameObject, "Parent");
        m_pGameObject_Parent.transform.localScale = new Vector3(0, 0, 1);

        ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Text_Title");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.StarMap);
        
        ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Button_Entire");
        m_pGameObject_Image_Entire = Helper.FindChildGameObject(ob, "Image_Select");
        ob = Helper.FindChildGameObject(ob, "Text_Name");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.StarMap_Entire);

        ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Button_InComplete");
        m_pGameObject_Image_InComplete = Helper.FindChildGameObject(ob, "Image_Select");
        m_pGameObject_Image_InComplete.SetActive(false);
        ob = Helper.FindChildGameObject(ob, "Text_Name");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.StarMap_InComplete);

        ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Text_Desc");
        pText = ob.GetComponent<Text>();
        string strDesc = Helper.GetTextDataString(eTextDataType.StarMap_Desc);
        string strReplace = strDesc.Replace("\\n", "\n");
        pText.text = strReplace;

        m_pGameObject_ListView_Entire = Helper.FindChildGameObject(m_pGameObject_Parent, "Scroll View_Entire");
        GameObject ob_Content = Helper.FindChildGameObject(m_pGameObject_ListView_Entire, "Content");

        for (int i = 0; i < SavedGameDataInfo.Instance.m_nLevel - 1; ++i)
        {
            ob = Resources.Load<GameObject>("Gui/Prefabs/StarMapItem");
            GameObject pGameObject_StarMapItem = GameObject.Instantiate(ob);
            StarMapItem pStarMapItem = pGameObject_StarMapItem.GetComponent<StarMapItem>();
            pStarMapItem.SetLevel(this, i + 1);

            pGameObject_StarMapItem.transform.SetParent(ob_Content.transform);
            pGameObject_StarMapItem.transform.localScale = Vector3.one;
        }

        m_pGameObject_ListView_InComplete = Helper.FindChildGameObject(m_pGameObject_Parent, "Scroll View_InComplete");
        ob_Content = Helper.FindChildGameObject(m_pGameObject_ListView_InComplete, "Content");

        for (int i = 0; i < SavedGameDataInfo.Instance.m_nLevel - 1; ++i)
        {
            if (SavedGameDataInfo.Instance.m_LevelStarCountList[i] < 3)
            {
                ob = Resources.Load<GameObject>("Gui/Prefabs/StarMapItem");
                GameObject pGameObject_StarMapItem = GameObject.Instantiate(ob);
                StarMapItem pStarMapItem = pGameObject_StarMapItem.GetComponent<StarMapItem>();
                pStarMapItem.SetLevel(this, i + 1);

                pGameObject_StarMapItem.transform.SetParent(ob_Content.transform);
                pGameObject_StarMapItem.transform.localScale = Vector3.one;
            }
        }

        m_pGameObject_ListView_InComplete.SetActive(false);

        PopupManager.Instance.AddPopup(gameObject);

        TransformerEvent_Scalar eventValue;
        eventValue = new TransformerEvent_Scalar(0.2f, 1.0f);
        m_pScale.AddEvent(eventValue);
        m_pScale.OnPlay();

        if (GameInfo.Instance.m_IsOpenStarMapInGameStart == true)
        {
            if (GameInfo.Instance.m_IsStarMapEntireTab == false)
            {
                OnButtonClick_InComplete();
            }
        }
    }

    void Update()
    {
        m_pScale.Update(Time.deltaTime);
        float fScale = m_pScale.GetCurScalar();
        m_pGameObject_Parent.transform.localScale = new Vector3(fScale, fScale, 1);

        if (m_IsInitScroll == false)
        {
            m_IsInitScroll = true;
            InitScroll();
        }
    }

    private void InitScroll()
    {
        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Scroll View_Entire");
        ob = Helper.FindChildGameObject(ob, "Scrollbar Vertical");
        Scrollbar pScrollbar = ob.GetComponent<Scrollbar>();

        if (GameInfo.Instance.m_IsOpenStarMapInGameStart == true)
        {
            pScrollbar.value = GameInfo.Instance.m_fListView_Entire_ScrollValue;
        }
        else
        {
            pScrollbar.value = 0.0f;
        }

        ob = Helper.FindChildGameObject(gameObject, "Scroll View_InComplete");
        ob = Helper.FindChildGameObject(ob, "Scrollbar Vertical");
        pScrollbar = ob.GetComponent<Scrollbar>();

        if (GameInfo.Instance.m_IsOpenStarMapInGameStart == true)
        {
            pScrollbar.value = GameInfo.Instance.m_fListView_InComplete_ScrollValue;
        }
        else
        {
            pScrollbar.value = 0.0f;
        }
    }

    private void NewGameOpenScrollSave()
    {
        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Scroll View_Entire");
        ob = Helper.FindChildGameObject(ob, "Scrollbar Vertical");
        Scrollbar pScrollbar = ob.GetComponent<Scrollbar>();
        GameInfo.Instance.m_fListView_Entire_ScrollValue = pScrollbar.value;

        ob = Helper.FindChildGameObject(gameObject, "Scroll View_InComplete");
        ob = Helper.FindChildGameObject(ob, "Scrollbar Vertical");
        pScrollbar = ob.GetComponent<Scrollbar>();
        GameInfo.Instance.m_fListView_InComplete_ScrollValue = pScrollbar.value;
    }

    public void OnButtonClick_Close()
    {
        GameInfo.Instance.m_IsOpenStarMapInGameStart = false;

        PopupManager.Instance.RemovePopup(gameObject);
        PopupManager.Instance.ShowLastPopup();
    }

    public void OnButtonClick_Entire()
    {
        m_pGameObject_Image_Entire.SetActive(true);
        m_pGameObject_Image_InComplete.SetActive(false);

        m_pGameObject_ListView_Entire.SetActive(true);
        m_pGameObject_ListView_InComplete.SetActive(false);
    }

    public void OnButtonClick_InComplete()
    {
        m_pGameObject_Image_Entire.SetActive(false);
        m_pGameObject_Image_InComplete.SetActive(true);

        m_pGameObject_ListView_Entire.SetActive(false);
        m_pGameObject_ListView_InComplete.SetActive(true);
    }

    public void OnNewGameOpen(int nLevel)
    {
        GameInfo.Instance.m_IsOpenStarMapInGameStart = true;
        GameInfo.Instance.m_IsStarMapEntireTab = m_pGameObject_Image_Entire.activeSelf;
        NewGameOpenScrollSave();

        GameInfo.Instance.m_IsCurrLevelPlay = false;
        GameInfo.Instance.m_nPrevLevelPlayLevel = nLevel;
        SavedGameDataInfo.Instance.OnPrevLevelPlay(nLevel);

        if (SavedGameDataInfo.Instance.m_nLevel < GameDefine.ms_nNewGameAdsShowLevel)
        {
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/NewGame_Lobby_NoAds");
            ob = GameObject.Instantiate(ob) as GameObject;
            PopupManager.Instance.AddPopup(ob);
        }
        else
        {
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/NewGame_Lobby");
            ob = GameObject.Instantiate(ob) as GameObject;
            PopupManager.Instance.AddPopup(ob);
        }
    }
}
