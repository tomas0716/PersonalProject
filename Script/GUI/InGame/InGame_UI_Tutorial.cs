using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI_Tutorial : MonoBehaviour
{
    private GameObject[]        m_pGameObjet_Image      = new GameObject[5];
    private Transformer_Timer   m_pTimer_SpriteImage    = new Transformer_Timer();

    public delegate void Callback_OK();
    public event Callback_OK m_Callback_OK;

    void Start()
    {
        GameInfo.Instance.m_IsInGame_Tutorial = true;

        GameObject ob;
        Text pText;

        int nLevel = 0;

        if (GameInfo.Instance.m_IsCurrLevelPlay == false && GameInfo.Instance.m_nPrevLevelPlayLevel != -1)
        {
            nLevel = GameInfo.Instance.m_nPrevLevelPlayLevel;
        }
        else
        {
            nLevel = SavedGameDataInfo.Instance.m_nLevel;
        }

        ExcelData_LevelFixedMissionDataInfo pLevelFixedMissionDataInfo = ExcelDataManager.Instance.m_pExcelData_LevelFixedMissionData.GetLevelFixedMissionDataInfo_byLevel(nLevel);
        string strTitle = Helper.GetTextDataString(pLevelFixedMissionDataInfo.m_nTutorial_Title_TableID);
        string strDesc = Helper.GetTextDataString(pLevelFixedMissionDataInfo.m_nTutorial_Desc_TableID);

        ob = Helper.FindChildGameObject(gameObject, "Text_Title");
        pText = ob.GetComponent<Text>();
        pText.text = strTitle;

        ob = Helper.FindChildGameObject(gameObject, "Text_Desc");
        pText = ob.GetComponent<Text>();
        pText.text = strDesc;

        GameObject ob_Tutorial = Helper.FindChildGameObject(gameObject, "Tutorial");

        for (int i = 0; i < 5; ++i)
        {
            if (nLevel != i + 1)
            {
                ob = Helper.FindChildGameObject(ob_Tutorial, (i+1).ToString());
                ob.SetActive(false);
            }
        }

        ob = Helper.FindChildGameObject(ob_Tutorial, nLevel.ToString());
		for (int i = 0; i < 5; ++i)
		{
            m_pGameObjet_Image[i] = Helper.FindChildGameObject(ob, "Image_0" + (i+1).ToString());

            if (i != 0)
            {
                m_pGameObjet_Image[i].SetActive(false);
            }
        }

        float fAnimTime = 0.18f;
        TransformerEvent eventValue;
        eventValue = new TransformerEvent_Timer(0, 0);
        m_pTimer_SpriteImage.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fAnimTime * 1, 1);
        m_pTimer_SpriteImage.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fAnimTime * 2, 2);
        m_pTimer_SpriteImage.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fAnimTime * 3, 3);
        m_pTimer_SpriteImage.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fAnimTime * 4, 4);
        m_pTimer_SpriteImage.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fAnimTime * 7);
        m_pTimer_SpriteImage.AddEvent(eventValue);
        m_pTimer_SpriteImage.SetCallback(OnOneEventDone_Timer_SpriteImage, null);
        m_pTimer_SpriteImage.SetLoop(true);
        m_pTimer_SpriteImage.OnPlay();

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

    private void OnDestroy()
    {
        GameInfo.Instance.m_IsInGame_Tutorial = false;

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
    }

    void Update()
    {
        m_pTimer_SpriteImage.Update(Time.deltaTime);
    }

    private void OnOneEventDone_Timer_SpriteImage(int nIndex, TransformerEvent eventValue)
    {
        if (eventValue.m_pParameta != null)
        {
            nIndex = (int)eventValue.m_pParameta;

            for (int i = 0; i < 5; ++i)
            {
                if (i != nIndex)
                {
                    m_pGameObjet_Image[i].SetActive(false);
                }
                else
                {
                    m_pGameObjet_Image[i].SetActive(true);
                }
            }
        }
    }
        
    public void OnButtonClick_OK()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        m_Callback_OK?.Invoke();
        GameObject.Destroy(gameObject);

        int nLevel = SavedGameDataInfo.Instance.m_nLevel;
        Helper.FirebaseLogEvent("InGame_Tuto_" + nLevel.ToString());
    }

    public void OnHardwareBackButtonClick()
    {
        if (GameInfo.Instance.m_IsHardwareBackButtonProcess == false &&
            GameInfo.Instance.m_IsShopOpen == false &&
            GameInfo.Instance.m_IsItemBuyOpen == false &&
            GameInfo.Instance.m_nMessageBoxOpenCount == 0 &&
            GameInfo.Instance.m_nRewardPopupOpenCount == 0)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = true;
            OnButtonClick_OK();
        }
    }
}
