using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using System;

public class Lobby_MainUI : MonoBehaviour
{
    private Text        m_pText_Level                        = null;
    private Outline     m_pOutline_Level                     = null;

    private GameObject  m_pGameObject_NewNotice_Shop         = null;
    private GameObject  m_pGameObject_NewNotice_Roulette     = null;
    private GameObject  m_pGameObject_NewNotice_Attendance   = null;

    private Transformer_Scalar m_pAlpha_GameStart_Flash      = new Transformer_Scalar(1);

    void Start()
    {
        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(gameObject, "Button_StarMap");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.StarMap);

        ob = Helper.FindChildGameObject(gameObject, "Text_HighScore_Title");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.HighScore_Title);

        ob = Helper.FindChildGameObject(gameObject, "Text_HighScore");
        pText = ob.GetComponent<Text>();
        pText.text = string.Format("{0:n0}", SavedGameDataInfo.Instance.m_nScore);

        ob = Helper.FindChildGameObject(gameObject, "Text_ScoreRanking_Title");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Ranking);

        if (GameInfo.Instance.m_IsFirstEnterLobby == true)
        {
            Helper.OnSoundPlay(eSoundType.Lobby, true);

            if (SavedGameDataInfo.Instance.m_nRanking != -1 && GooglePlayLeaderboard.Instance.GetScoreRanking() > SavedGameDataInfo.Instance.m_nRanking)
            {
                float fLength = pText.preferredWidth * 0.5f;
                Vector3 vPos = pText.gameObject.transform.localPosition;

                ob = Helper.FindChildGameObject(gameObject, "Change_Ranking");
                float fHeight = ob.transform.localPosition.y;
                ob.transform.localPosition = new Vector3(vPos.x + fLength + 37, fHeight, 1);

                ob = Helper.FindChildGameObject(ob, "Text_DownValue");
                pText = ob.GetComponent<Text>();
                int nCangeValue = GooglePlayLeaderboard.Instance.GetScoreRanking() - SavedGameDataInfo.Instance.m_nRanking;
                pText.text = string.Format("{0:n0}", nCangeValue);
            }
            else
            {
                ob = Helper.FindChildGameObject(gameObject, "Change_Ranking");
                ob.SetActive(false);
            }
        }
        else
        {
            ob = Helper.FindChildGameObject(gameObject, "Change_Ranking");
            ob.SetActive(false);
        }

        ob = Helper.FindChildGameObject(gameObject, "Text_ScoreRanking");
        pText = ob.GetComponent<Text>();

        if (GooglePlayLeaderboard.Instance.GetScoreRanking() != -1)
        {
            int nRanking = GooglePlayLeaderboard.Instance.GetScoreRanking();
            pText.text = Helper.ConvertToOrdinal(nRanking);
        }
        else
        {
            pText.text = "-";
        }

        ob = Helper.FindChildGameObject(gameObject, "Text_Level");
        m_pText_Level = ob.GetComponent<Text>();
        m_pText_Level.text = "LEVEL " + SavedGameDataInfo.Instance.m_nLevel.ToString();

        m_pOutline_Level = ob.GetComponent<Outline>();

        ob = Helper.FindChildGameObject(gameObject, "Button_Achievement");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Achievement_Button);

        ob = Helper.FindChildGameObject(gameObject, "Button_Ranking");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Leaderboard_Button);

        ob = Helper.FindChildGameObject(gameObject, "Button_Shop");
        m_pGameObject_NewNotice_Shop = Helper.FindChildGameObject(ob, "Image_NewNotice");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Shop_Button);

        ob = Helper.FindChildGameObject(gameObject, "Button_Roulette");
        m_pGameObject_NewNotice_Roulette = Helper.FindChildGameObject(ob, "Image_NewNotice");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Roulette_Button);

        ob = Helper.FindChildGameObject(gameObject, "Button_Attendance");
        m_pGameObject_NewNotice_Attendance = Helper.FindChildGameObject(ob, "Image_NewNotice");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Attendance_Button);

        if (SavedGameDataInfo.Instance.m_IsGetFreeCoin == false || SavedGameDataInfo.Instance.m_byGetAdsCoinCount < GameDefine.ms_nShopItemFree_Max)
        {
            m_pGameObject_NewNotice_Shop.SetActive(true);
        }
        else
        {
            m_pGameObject_NewNotice_Shop.SetActive(false);
        }

        if (SavedGameDataInfo.Instance.m_IsGetFreeRoulette == false || SavedGameDataInfo.Instance.m_IsGetAdsRoulette == false)
        {
            m_pGameObject_NewNotice_Roulette.SetActive(true);
        }
        else
        {
            m_pGameObject_NewNotice_Roulette.SetActive(false);
        }

        if (SavedGameDataInfo.Instance.m_IsGetAttendance == false)
        {
            m_pGameObject_NewNotice_Attendance.SetActive(true);
        }
        else
        {
            m_pGameObject_NewNotice_Attendance.SetActive(false);
        }

        float fFlashTime = 7.0f;
        TransformerEvent eventValue;
        eventValue = new TransformerEvent_Scalar(fFlashTime, 1);
        m_pAlpha_GameStart_Flash.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(fFlashTime + 0.15f, 0);
        m_pAlpha_GameStart_Flash.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(fFlashTime + 0.3f, 1);
        m_pAlpha_GameStart_Flash.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(fFlashTime + 0.45f, 0);
        m_pAlpha_GameStart_Flash.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(fFlashTime + 0.6f, 1);
        m_pAlpha_GameStart_Flash.AddEvent(eventValue);
        m_pAlpha_GameStart_Flash.SetLoop(true);
        m_pAlpha_GameStart_Flash.OnPlay();

        if (GameInfo.Instance.m_IsFirstEnterLobby == true)
        {
            Helper.FirebaseLogEvent("Lobby_Enter");
        }

		if (GameInfo.Instance.m_IsFirstEnterLobby == true && SavedGameDataInfo.Instance.m_nLevel <= 5)
		{
            GameInfo.Instance.m_IsCurrLevelPlay = true;
			GameInfo.Instance.m_nPrevLevelPlayLevel = -1;
			SavedGameDataInfo.Instance.OnResetCurrLevel();

			ob = Resources.Load<GameObject>("Gui/Prefabs/NewGame_Lobby_NoAds");
			ob = GameObject.Instantiate(ob) as GameObject;
			PopupManager.Instance.AddPopup(ob);
		}

        GameInfo.Instance.m_IsFirstEnterLobby = false;

        if (GameInfo.Instance.m_IsOpenStarMapInGameStart == true)
        {
            ob = Resources.Load<GameObject>("Gui/Prefabs/StarMap");
            GameObject.Instantiate(ob);
        }

        AppInstance.Instance.m_pEventDelegateManager.OnEventShopItemAdsComplete += OnShopItemAdsComplete;
        AppInstance.Instance.m_pEventDelegateManager.OnEventRouletteComplete += OnRouletteComplete;
        AppInstance.Instance.m_pEventDelegateManager.OnEventAttendaceComplete += OnAttendaceComplete;

        AppInstance.Instance.m_pEventDelegateManager.OnEventNextDay += OnNextDay;

        AppInstance.Instance.m_pEventDelegateManager.OnEventDestroyInGameLoading += OnDestroyInGameLoading;
    }

	private void OnDestroy()
	{
        AppInstance.Instance.m_pEventDelegateManager.OnEventShopItemAdsComplete -= OnShopItemAdsComplete;
        AppInstance.Instance.m_pEventDelegateManager.OnEventRouletteComplete -= OnRouletteComplete;
        AppInstance.Instance.m_pEventDelegateManager.OnEventAttendaceComplete -= OnAttendaceComplete;

        AppInstance.Instance.m_pEventDelegateManager.OnEventNextDay -= OnNextDay;

        AppInstance.Instance.m_pEventDelegateManager.OnEventDestroyInGameLoading -= OnDestroyInGameLoading;
    }

	void Update()
    {
        m_pAlpha_GameStart_Flash.Update(Time.deltaTime);
        float fAlpha = m_pAlpha_GameStart_Flash.GetCurScalar();
        m_pText_Level.color = new Color(1, 1, 1, fAlpha);

        Color outlineColor = m_pOutline_Level.effectColor;
        m_pOutline_Level.effectColor = new Color(outlineColor.r, outlineColor.g, outlineColor.b, 0.5f * fAlpha);
    }

    private void OnDestroyInGameLoading()
    {
        if (SavedGameDataInfo.Instance.m_nLevel <= 5)
        {
            GameInfo.Instance.m_IsCurrLevelPlay = true;
            GameInfo.Instance.m_nPrevLevelPlayLevel = -1;
            SavedGameDataInfo.Instance.OnResetCurrLevel();

            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/NewGame_Lobby_NoAds");
            ob = GameObject.Instantiate(ob) as GameObject;
            PopupManager.Instance.AddPopup(ob);
        }
    }

    public void OnButtonClick_Option()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/OutGame_Option");
        GameObject.Instantiate(ob);
    }

    public void OnButtonClick_StarMap()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/StarMap");
        GameObject.Instantiate(ob);
    }

    public void OnButtonClick_Acheivement()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        GooglePlayAchievement.Instance.ShowAchievement();
    }

    public void OnButtonClick_Leaderboard()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        GooglePlayLeaderboard.Instance.ShowLeaderboard_Score();
    }

    public void OnButtonClick_Shop()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/Shop");
        GameObject.Instantiate(ob);
    }

    public void OnButtonClick_Roulette()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/Roulette");
        GameObject.Instantiate(ob);
    }

    public void OnButtonClick_Attendance()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/Attendance");
        ob = GameObject.Instantiate(ob);
    }

    public void OnButtonClick_Start()
    {
        GameInfo.Instance.m_IsOpenStarMapInGameStart = false;
        GameInfo.Instance.m_fListView_Entire_ScrollValue = 0.0f;
        GameInfo.Instance.m_fListView_InComplete_ScrollValue = 0.0f;

        Helper.OnSoundPlay(eSoundType.Button, false);

        if (SavedGameDataInfo.Instance.m_nLevel > GameDefine.ms_nMaxLevel)
        {
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
            ob = GameObject.Instantiate(ob);
            MessageBox pMessageBox = ob.GetComponent<MessageBox>();
            pMessageBox.Initialize(MessageBox.eMessageBoxType.OK, "", Helper.GetTextDataString(eTextDataType.MaxLevelArrival), Helper.GetTextDataString(eTextDataType.OK), "");
        }
        else
        {
            GameInfo.Instance.m_IsCurrLevelPlay = true;
            GameInfo.Instance.m_nPrevLevelPlayLevel = -1;
            SavedGameDataInfo.Instance.OnResetCurrLevel();

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

    public void OnShopItemAdsComplete()
    {
        if (SavedGameDataInfo.Instance.m_IsGetFreeCoin == false || SavedGameDataInfo.Instance.m_byGetAdsCoinCount < GameDefine.ms_nShopItemFree_Max)
        {
            m_pGameObject_NewNotice_Shop.SetActive(true);
        }
        else
        {
            m_pGameObject_NewNotice_Shop.SetActive(false);
        }
    }

    public void OnRouletteComplete()
    {
        if (SavedGameDataInfo.Instance.m_IsGetFreeRoulette == false || SavedGameDataInfo.Instance.m_IsGetAdsRoulette == false)
        {
            m_pGameObject_NewNotice_Roulette.SetActive(true);
        }
        else
        {
            m_pGameObject_NewNotice_Roulette.SetActive(false);
        }
    }

    public void OnAttendaceComplete()
    {
        if (SavedGameDataInfo.Instance.m_IsGetAttendance == false)
        {
            m_pGameObject_NewNotice_Attendance.SetActive(true);
        }
        else
        {
            m_pGameObject_NewNotice_Attendance.SetActive(false);
        }
    }

    public void OnNextDay()
    {
        m_pGameObject_NewNotice_Shop.SetActive(true);
        m_pGameObject_NewNotice_Roulette.SetActive(true);
        m_pGameObject_NewNotice_Attendance.SetActive(true);
    }

#if _DEBUG
    public void OnButtonClick_TestLevel()
    {
        GameObject ob = GameObject.Find("TestLevel");
        ob = Helper.FindChildGameObject(ob, "InputField");
        InputField pInputField = ob.GetComponent<InputField>();
        string strLevel = pInputField.text;
        int nLevel = Helper.ConvertStringToInt(strLevel);

        if (nLevel != 0 && nLevel != 1)
        {
            SavedGameDataInfo.Instance.m_nLevel = nLevel - 1;
            SavedGameDataInfo.Instance.OnMissionClear();
            m_pText_Level.text = "LEVEL " + nLevel.ToString();
        }
    }

    public void OnButtonClick_TestNextLevel()
    {
        SavedGameDataInfo.Instance.OnMissionClear();
        m_pText_Level.text = "LEVEL " + SavedGameDataInfo.Instance.m_nLevel.ToString();
    }

    public void OnButtonClick_TestPrevLevel()
    {
        if (SavedGameDataInfo.Instance.m_nLevel != 1)
        {
            SavedGameDataInfo.Instance.m_nLevel = SavedGameDataInfo.Instance.m_nLevel - 2;
            SavedGameDataInfo.Instance.OnMissionClear();
            m_pText_Level.text = "LEVEL " + SavedGameDataInfo.Instance.m_nLevel.ToString();
        }
    }
#endif
}
