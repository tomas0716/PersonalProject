using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class NewGame : MonoBehaviour
{
    private GameObject          m_pGameObject_Parent            = null;
    private Transformer_Scalar  m_pScale                        = new Transformer_Scalar(0);

    private SpriteAtlas         m_pSpriteAtlas_MIssion          = null;
    private Text                m_pText_Level                   = null;
    private GameObject          m_pGameObject_MissionDummy      = null;

    public class ItemContent
    {
        public GameObject       m_pGameObject_Add               = null;
        public GameObject       m_pGameObject_Select            = null;
        public GameObject       m_pGameObject_Got               = null;
        public Text             m_pText_Item_Count              = null;
    }
    
    private ItemContent []      m_pItemContents                 = new ItemContent[3];

    private Dictionary<eMissionType, NewGame_MissionItem> m_MissionItemTable = new Dictionary<eMissionType, NewGame_MissionItem>();

    private Transformer_Timer   m_pTimer_InGame_Loading_Delete  = new Transformer_Timer();

    void Start()
    {
        Helper.OnSoundPlay(eSoundType.IngamePopup, true);

        m_pGameObject_Parent = Helper.FindChildGameObject(gameObject, "Parent");
        m_pGameObject_Parent.transform.localScale = new Vector3(0, 0, 1);

        SpriteAtlas sa = Resources.Load<SpriteAtlas>("Gui/InGame_MIssion/Atlas_Main");
        m_pSpriteAtlas_MIssion = SpriteAtlas.Instantiate(sa);

        GameObject ob;
        Text pText;

        if (GameInfo.Instance.m_IsCurrLevelPlay == true)
        {
            ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Star");
            ob.SetActive(false);

            ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Image_Score_Back");
            ob.SetActive(false);

            ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Text_Score");
            ob.SetActive(false);
        }
        else
        {
            GameObject ob_Star = Helper.FindChildGameObject(m_pGameObject_Parent, "Star");
            
            int nLevelScore = SavedGameDataInfo.Instance.m_LevelScoreList[GameInfo.Instance.m_nPrevLevelPlayLevel-1];

            for (int i = 0; i < 3; ++i)
            {
                ob = Helper.FindChildGameObject(ob_Star, "Image_Star_0" + (i+1).ToString());

                int nScore = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[i];

                if (nLevelScore < nScore)
                {
                    ob.SetActive(false);
                }
            }

            ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Text_Score");
            pText = ob.GetComponent<Text>();
            pText.text = "Score " + string.Format("{0:n0}", nLevelScore);
        }

        ob = Helper.FindChildGameObject(gameObject,"Text_Level");
        m_pText_Level = ob.GetComponent<Text>();

        if (GameInfo.Instance.m_IsCurrLevelPlay == false && GameInfo.Instance.m_nPrevLevelPlayLevel != -1)
        {
            m_pText_Level.text = "LEVEL " + GameInfo.Instance.m_nPrevLevelPlayLevel.ToString();
        }
        else
        {
            m_pText_Level.text = "LEVEL " + SavedGameDataInfo.Instance.m_nLevel.ToString();
        }

        m_pGameObject_MissionDummy = Helper.FindChildGameObject(gameObject, "MissionDummy");

        GameObject ob_items = Helper.FindChildGameObject(gameObject, "Items");
        for (int i = 0; i < 3; ++i)
        {
            string strName = "Button_Item_0" + (i+1).ToString();
            ob = Helper.FindChildGameObject(ob_items, strName);

            m_pItemContents[i] = new ItemContent();
            m_pItemContents[i].m_pGameObject_Add = Helper.FindChildGameObject(ob, "State_Add");
            m_pItemContents[i].m_pGameObject_Select = Helper.FindChildGameObject(ob, "State_Select");
            m_pItemContents[i].m_pGameObject_Got = Helper.FindChildGameObject(ob, "State_Got");
            m_pItemContents[i].m_pText_Item_Count = Helper.FindChildGameObject(m_pItemContents[i].m_pGameObject_Got, "Text_GotCount").GetComponent<Text>();
        }

        GameObject ob_Parent = Helper.FindChildGameObject(gameObject, "Lobby_Normal");

        ob = Helper.FindChildGameObject(ob_Parent, "Button_GameStart");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.GameStart);

        if (SavedGameDataInfo.Instance.m_nLevel >= GameDefine.ms_nNewGameAdsShowLevel)
        {
            ob = Helper.FindChildGameObject(ob_Parent, "Button_GameStart_Ad");
            ob = Helper.FindChildGameObject(ob, "Text");
            pText = ob.GetComponent<Text>();
            pText.text = Helper.GetTextDataString(eTextDataType.GameStart);
        }

        OnLobbyNewGame_UpdataMissionUI();
        OnUpdateItemState();

        TransformerEvent_Scalar eventValue;
        eventValue = new TransformerEvent_Scalar(0.2f, 1.0f);
        m_pScale.AddEvent(eventValue);
        m_pScale.OnPlay();

        if (SavedGameDataInfo.Instance.m_nLevel <= 5)
        {
            ob = Helper.FindChildGameObject(gameObject, "Button_Close");
            Button pButton = ob.GetComponent<Button>();
            pButton.interactable = false;
        }

        AppInstance.Instance.m_pEventDelegateManager.OnEventLobbyNewGame_UpdataMissionUI += OnLobbyNewGame_UpdataMissionUI;
        AppInstance.Instance.m_pEventDelegateManager.OnEventUpdataItemState += OnUpdateItemState;
        AppInstance.Instance.m_pEventDelegateManager.OnEventItemBuy += OnItemBuy;

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

	private void OnDestroy()
	{
        AppInstance.Instance.m_pEventDelegateManager.OnEventLobbyNewGame_UpdataMissionUI -= OnLobbyNewGame_UpdataMissionUI;
        AppInstance.Instance.m_pEventDelegateManager.OnEventUpdataItemState -= OnUpdateItemState;
        AppInstance.Instance.m_pEventDelegateManager.OnEventItemBuy -= OnItemBuy;

        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsLoadFailed -= OnGoogleRewardsAdsLoadFailed;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsComplete -= OnGoogleRewardsAdsComplete;

        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
    }

	void Update()
    {
        m_pScale.Update(Time.deltaTime);
        float fScale = m_pScale.GetCurScalar();
        m_pGameObject_Parent.transform.localScale = new Vector3(fScale, fScale, 1);

        m_pTimer_InGame_Loading_Delete.Update(Time.deltaTime);
    }

    private void OnLobbyNewGame_UpdataMissionUI()
    {
        m_MissionItemTable.Clear();

        Helper.RemoveChildAll(m_pGameObject_MissionDummy);

        int nCount = SavedGameDataInfo.Instance.m_MissionInfoTable.Count;

        int nX = 0;
        int nInterval = 0;

        if (nCount == 2)
        {
            nX = -90;
            nInterval = 180;
        }

        foreach (KeyValuePair<eMissionType, int> item in SavedGameDataInfo.Instance.m_MissionInfoTable)
        {
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/NewGame_MissionItem") as GameObject;
            ob = GameObject.Instantiate(ob);

            NewGame_MissionItem pInGame_UI_MissionItem = ob.GetComponent<NewGame_MissionItem>();
            pInGame_UI_MissionItem.SetMissionData(item.Key, m_pSpriteAtlas_MIssion, 1);

            ob.transform.SetParent(m_pGameObject_MissionDummy.transform);
            ob.transform.localPosition = new Vector3(nX, 0, 0);
            ob.transform.localScale = new Vector3(1, 1, 1);
            nX += nInterval;

            m_MissionItemTable.Add(item.Key, pInGame_UI_MissionItem);
        }
    }

    private void OnUpdateItemState()
    {
        if (InGameInfo.Instance.m_IsNewGame_Item_Select_Move_3 == true && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Move3] > 0)
        {
            m_pItemContents[0].m_pGameObject_Add.SetActive(false);
            m_pItemContents[0].m_pGameObject_Select.SetActive(true);
            m_pItemContents[0].m_pGameObject_Got.SetActive(false);
        }
        else if (InGameInfo.Instance.m_IsNewGame_Item_Select_Move_3 == false && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Move3] > 0)
        {
            m_pItemContents[0].m_pGameObject_Add.SetActive(false);
            m_pItemContents[0].m_pGameObject_Select.SetActive(false);
            m_pItemContents[0].m_pGameObject_Got.SetActive(true);
            m_pItemContents[0].m_pText_Item_Count.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Move3].ToString();
        }
        else if (InGameInfo.Instance.m_IsNewGame_Item_Select_Move_3 == false && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Move3] == 0)
        {
            m_pItemContents[0].m_pGameObject_Add.SetActive(true);
            m_pItemContents[0].m_pGameObject_Select.SetActive(false);
            m_pItemContents[0].m_pGameObject_Got.SetActive(false);
        }

        if (InGameInfo.Instance.m_IsNewGame_Item_Select_Magician == true && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.RainbowCat] > 0)
        {
            m_pItemContents[1].m_pGameObject_Add.SetActive(false);
            m_pItemContents[1].m_pGameObject_Select.SetActive(true);
            m_pItemContents[1].m_pGameObject_Got.SetActive(false);
        }
        else if (InGameInfo.Instance.m_IsNewGame_Item_Select_Magician == false && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.RainbowCat] > 0)
        {
            m_pItemContents[1].m_pGameObject_Add.SetActive(false);
            m_pItemContents[1].m_pGameObject_Select.SetActive(false);
            m_pItemContents[1].m_pGameObject_Got.SetActive(true);
            m_pItemContents[1].m_pText_Item_Count.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.RainbowCat].ToString();
        }
        else if (InGameInfo.Instance.m_IsNewGame_Item_Select_Magician == false && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.RainbowCat] == 0)
        {
            m_pItemContents[1].m_pGameObject_Add.SetActive(true);
            m_pItemContents[1].m_pGameObject_Select.SetActive(false);
            m_pItemContents[1].m_pGameObject_Got.SetActive(false);
        }

        if (InGameInfo.Instance.m_IsNewGame_Item_Select_SpecialUnit == true && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.StripeToeJelly] > 0)
        {
            m_pItemContents[2].m_pGameObject_Add.SetActive(false);
            m_pItemContents[2].m_pGameObject_Select.SetActive(true);
            m_pItemContents[2].m_pGameObject_Got.SetActive(false);
        }
        else if (InGameInfo.Instance.m_IsNewGame_Item_Select_SpecialUnit == false && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.StripeToeJelly] > 0)
        {
            m_pItemContents[2].m_pGameObject_Add.SetActive(false);
            m_pItemContents[2].m_pGameObject_Select.SetActive(false);
            m_pItemContents[2].m_pGameObject_Got.SetActive(true);
            m_pItemContents[2].m_pText_Item_Count.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.StripeToeJelly].ToString();
        }
        else if (InGameInfo.Instance.m_IsNewGame_Item_Select_SpecialUnit == false && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.StripeToeJelly] == 0)
        {
            m_pItemContents[2].m_pGameObject_Add.SetActive(true);
            m_pItemContents[2].m_pGameObject_Select.SetActive(false);
            m_pItemContents[2].m_pGameObject_Got.SetActive(false);
        }
    }

    public void OnItemBuy(eItemType eType)
    {
        switch (eType)
        {
            case eItemType.Move3:               OnButtonClick_Item_01();    break;
            case eItemType.RainbowCat:          OnButtonClick_Item_02();    break;
            case eItemType.StripeToeJelly:      OnButtonClick_Item_03();    break;
        }
    }

    public void OnButtonClick_Close()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        InGameInfo.Instance.m_IsNewGame_Item_Select_Move_3 = false;
        InGameInfo.Instance.m_IsNewGame_Item_Select_Magician = false;
        InGameInfo.Instance.m_IsNewGame_Item_Select_SpecialUnit = false;

        if (GameInfo.Instance.m_IsCurrLevelPlay == false)
        {
            GameInfo.Instance.m_IsCurrLevelPlay = true;
            SavedGameDataInfo.Instance.OnResetCurrLevel();
        }

        PopupManager.Instance.RemovePopup(gameObject);
        PopupManager.Instance.ShowLastPopup();

        Helper.OnSoundPlay(eSoundType.Lobby, true);
    }

    public void OnButtonClick_Item_01()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        if (InGameInfo.Instance.m_IsNewGame_Item_Select_Move_3 == true && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Move3] > 0)
        {
            InGameInfo.Instance.m_IsNewGame_Item_Select_Move_3 = false;
            m_pItemContents[0].m_pGameObject_Add.SetActive(false);
            m_pItemContents[0].m_pGameObject_Select.SetActive(false);
            m_pItemContents[0].m_pGameObject_Got.SetActive(true);
            m_pItemContents[0].m_pText_Item_Count.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Move3].ToString();
        }
        else if (InGameInfo.Instance.m_IsNewGame_Item_Select_Move_3 == false && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Move3] > 0)
        {
            InGameInfo.Instance.m_IsNewGame_Item_Select_Move_3 = true;
            m_pItemContents[0].m_pGameObject_Add.SetActive(false);
			m_pItemContents[0].m_pGameObject_Select.SetActive(true);
			m_pItemContents[0].m_pGameObject_Got.SetActive(false);
        }
        else if (InGameInfo.Instance.m_IsNewGame_Item_Select_Move_3 == false && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Move3] == 0)
        {
            // 아이템 구매창 띄움
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/ItemBuy");
            ob = GameObject.Instantiate(ob);
            ItemBuy pItemBuy = ob.GetComponent<ItemBuy>();
            pItemBuy.Init(ExcelDataManager.Instance.m_pExcelData_ItemData.GetItemDataInfo_byItemType(eItemType.Move3));
            PopupManager.Instance.AddPopup(ob);
        }
    }

	public void OnButtonClick_Item_02()
	{
        Helper.OnSoundPlay(eSoundType.Button, false);

        if (InGameInfo.Instance.m_IsNewGame_Item_Select_Magician == true && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.RainbowCat] > 0)
        {
            InGameInfo.Instance.m_IsNewGame_Item_Select_Magician = false;
            m_pItemContents[1].m_pGameObject_Add.SetActive(false);
            m_pItemContents[1].m_pGameObject_Select.SetActive(false);
            m_pItemContents[1].m_pGameObject_Got.SetActive(true);
            m_pItemContents[1].m_pText_Item_Count.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.RainbowCat].ToString();
        }
        else if (InGameInfo.Instance.m_IsNewGame_Item_Select_Magician == false && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.RainbowCat] > 0)
        {
            InGameInfo.Instance.m_IsNewGame_Item_Select_Magician = true;
            m_pItemContents[1].m_pGameObject_Add.SetActive(false);
            m_pItemContents[1].m_pGameObject_Select.SetActive(true);
            m_pItemContents[1].m_pGameObject_Got.SetActive(false);
        }
        else if (InGameInfo.Instance.m_IsNewGame_Item_Select_Magician == false && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.RainbowCat] == 0)
        {
            // 아이템 구매창 띄움
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/ItemBuy");
            ob = GameObject.Instantiate(ob);
            ItemBuy pItemBuy = ob.GetComponent<ItemBuy>();
            pItemBuy.Init(ExcelDataManager.Instance.m_pExcelData_ItemData.GetItemDataInfo_byItemType(eItemType.RainbowCat));
            PopupManager.Instance.AddPopup(ob);
        }
    }

    public void OnButtonClick_Item_03()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        if (InGameInfo.Instance.m_IsNewGame_Item_Select_SpecialUnit == true && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.StripeToeJelly] > 0)
        {
            InGameInfo.Instance.m_IsNewGame_Item_Select_SpecialUnit = false;
            m_pItemContents[2].m_pGameObject_Add.SetActive(false);
            m_pItemContents[2].m_pGameObject_Select.SetActive(false);
            m_pItemContents[2].m_pGameObject_Got.SetActive(true);
            m_pItemContents[2].m_pText_Item_Count.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.StripeToeJelly].ToString();
        }
        else if (InGameInfo.Instance.m_IsNewGame_Item_Select_SpecialUnit == false && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.StripeToeJelly] > 0)
        {
            InGameInfo.Instance.m_IsNewGame_Item_Select_SpecialUnit = true;
            m_pItemContents[2].m_pGameObject_Add.SetActive(false);
            m_pItemContents[2].m_pGameObject_Select.SetActive(true);
            m_pItemContents[2].m_pGameObject_Got.SetActive(false);
        }
        else if (InGameInfo.Instance.m_IsNewGame_Item_Select_SpecialUnit == false && SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.StripeToeJelly] == 0)
        {
            // 아이템 구매창 띄움
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/ItemBuy");
            ob = GameObject.Instantiate(ob);
            ItemBuy pItemBuy = ob.GetComponent<ItemBuy>();
            pItemBuy.Init(ExcelDataManager.Instance.m_pExcelData_ItemData.GetItemDataInfo_byItemType(eItemType.StripeToeJelly));
            PopupManager.Instance.AddPopup(ob);
        }
    }

    public void OnButtonClick_GameStart()
    {
        if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] > 0)
        {
            Helper.OnSoundPlay(eSoundType.Button, false);

            OnGameStart_CheckItemUse();
        }
        else
        {
            if (SavedGameDataInfo.Instance.m_byGetFreeHeartCountForAds == GameDefine.ms_nHeartFree_Max)
            {
                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/HeartCharge_01");
                ob = GameObject.Instantiate(ob);
                PopupManager.Instance.AddPopup(ob);
            }
            else
            {
                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/HeartCharge_02");
                ob = GameObject.Instantiate(ob);
                PopupManager.Instance.AddPopup(ob);
            }
        }
    }

    public void OnButtonClick_GameStart_Ad()
    {
        if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] > 0)
        {
            Helper.OnSoundPlay(eSoundType.Button, false);

            AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsLoadFailed -= OnGoogleRewardsAdsLoadFailed;
            AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsComplete -= OnGoogleRewardsAdsComplete;

            AppInstance.Instance.m_pEventDelegateManager.OnCreateLoading();
            AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsLoadFailed += OnGoogleRewardsAdsLoadFailed;
            AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsComplete += OnGoogleRewardsAdsComplete;
            //GoogleRewardedAdsManager.Instance.Show();
            UnityRewardAds.Instance.Show();
        }
        else
        {
            if (SavedGameDataInfo.Instance.m_byGetFreeHeartCountForAds == GameDefine.ms_nHeartFree_Max)
            {
                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/HeartCharge_01");
                ob = GameObject.Instantiate(ob);
                PopupManager.Instance.AddPopup(ob);
            }
            else
            {
                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/HeartCharge_02");
                ob = GameObject.Instantiate(ob);
                PopupManager.Instance.AddPopup(ob);
            }
        }
    }

    public void OnGoogleRewardsAdsLoadFailed()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsLoadFailed -= OnGoogleRewardsAdsLoadFailed;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsComplete -= OnGoogleRewardsAdsComplete;
    }

    public void OnGoogleRewardsAdsComplete()
    {
        StartCoroutine(Coroutine_GoogleRewardsAdsComplete());
    }

    IEnumerator Coroutine_GoogleRewardsAdsComplete()
    {
        yield return new WaitForEndOfFrame();

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsLoadFailed -= OnGoogleRewardsAdsLoadFailed;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsComplete -= OnGoogleRewardsAdsComplete;

        InGameInfo.Instance.m_IsNewGame_Item_Reward = true;

        OnGameStart_CheckItemUse();
    }

    private void OnGameStart_CheckItemUse()
    {
        SavedGameDataInfo.Instance.m_nHeartAtGameStart = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart];

        if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] == GameDefine.ms_nMaxAutoChargeHeart)
        {
            DateTime ServerTime = TimeServer.Instance.GetNISTDate();
            SavedGameDataInfo.Instance.m_sYear_Heart = (short)ServerTime.Year;
            SavedGameDataInfo.Instance.m_byMonth_Heart = (byte)ServerTime.Month;
            SavedGameDataInfo.Instance.m_byDay_Heart = (byte)ServerTime.Day;
            SavedGameDataInfo.Instance.m_byHour_Heart = (byte)ServerTime.Hour;
            SavedGameDataInfo.Instance.m_byMinute_Heart = (byte)ServerTime.Minute;
            SavedGameDataInfo.Instance.m_bySecond_Heart = (byte)ServerTime.Second;
        }

        SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] -= 1;

        AppInstance.Instance.m_pEventDelegateManager.OnInGameStartHeartMinus();

        if (InGameInfo.Instance.m_IsNewGame_Item_Select_Move_3 == true || InGameInfo.Instance.m_IsNewGame_Item_Select_Magician == true || InGameInfo.Instance.m_IsNewGame_Item_Select_SpecialUnit == true)
        {
            if (InGameInfo.Instance.m_IsNewGame_Item_Select_Move_3 == true)
            {
                SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Move3] -= 1;
            }

            if (InGameInfo.Instance.m_IsNewGame_Item_Select_Magician == true)
            {
                SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.RainbowCat] -= 1;
            }

            if (InGameInfo.Instance.m_IsNewGame_Item_Select_SpecialUnit == true)
            {
                SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.StripeToeJelly] -= 1;
            }
        }

        AppInstance.Instance.m_pEventDelegateManager.OnCreateLoading();
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save += OnGooglePlaySavedGameDone_Save;
        SavedGameDataInfo.Instance.Save();
    }

    public void OnGooglePlaySavedGameDone_Save()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();

        AppInstance.Instance.m_pEventDelegateManager.OnUpdateCoinInfo();
        AppInstance.Instance.m_pEventDelegateManager.OnUpdateItemState();

        OnGameStart();
    }

    private void  OnGameStart()
    {
        AppInstance.Instance.m_pSoundPlayer.StopBKG();

        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            SavedGameDataInfo.Instance.m_pMapDataInfo.CalculationBound();

            InGameInfo.Instance.m_nSlotStartIndex_X = SavedGameDataInfo.Instance.m_pMapDataInfo.m_nStartSlotIndex_X;
            InGameInfo.Instance.m_nSlotStartIndex_Y = SavedGameDataInfo.Instance.m_pMapDataInfo.m_nStartSlotIndex_Y;
            InGameInfo.Instance.m_nSlotEndIndex_X = SavedGameDataInfo.Instance.m_pMapDataInfo.m_nEndSlotIndex_X;
            InGameInfo.Instance.m_nSlotEndIndex_Y = SavedGameDataInfo.Instance.m_pMapDataInfo.m_nEndSlotIndex_Y;
            InGameInfo.Instance.m_nSlotCount_X = InGameInfo.Instance.m_nSlotEndIndex_X - InGameInfo.Instance.m_nSlotStartIndex_X + 1;
            InGameInfo.Instance.m_nSlotCount_Y = InGameInfo.Instance.m_nSlotEndIndex_Y - InGameInfo.Instance.m_nSlotStartIndex_Y + 1;
            InGameInfo.Instance.m_fSlotSize = 76;
            InGameInfo.Instance.m_fSlotScale = InGameInfo.Instance.m_fSlotSize / 72.0f;
            InGameInfo.Instance.m_vSlot_LeftTopPos.x = -((InGameInfo.Instance.m_nSlotCount_X * 0.5f) * InGameInfo.Instance.m_fSlotSize - InGameInfo.Instance.m_fSlotSize * 0.5f);
            InGameInfo.Instance.m_vSlot_LeftTopPos.y = (InGameInfo.Instance.m_nSlotCount_Y * 0.5f) * InGameInfo.Instance.m_fSlotSize + GameDefine.ms_vnSlotCenter_Y - InGameInfo.Instance.m_fSlotSize * 0.5f;
        }

        int nLevel = SavedGameDataInfo.Instance.m_nLevel;

        if (nLevel < 30)
        {
            Helper.FirebaseLogEvent("InGame_Start_" + nLevel.ToString());
        }
        else
        {
            Helper.FirebaseLogEvent("InGame_Start", "Level", nLevel);
        }

        InGameInfo.Instance.InGameStart_Reset();
        AppInstance.Instance.m_pInGameRandom.InGamePlay();

        GameInfo.Instance.m_IsInGameEnter = true;
        AppInstance.Instance.m_pEventDelegateManager.OnCreateInGameLoading();
    }

    public void OnButtonClick_Home()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        AppInstance.Instance.m_pSceneManager.ChangeScene(eSceneType.Scene_Lobby, false);
    }

    public void OnHardwareBackButtonClick()
    {
        if (gameObject.activeSelf == true &&
            GameInfo.Instance.m_IsHardwareBackButtonProcess == false &&
            GameInfo.Instance.m_IsShopOpen == false &&
            GameInfo.Instance.m_IsItemBuyOpen == false &&
            GameInfo.Instance.m_nMessageBoxOpenCount == 0 &&
            GameInfo.Instance.m_nRewardPopupOpenCount == 0)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = true;
            OnButtonClick_Close();
        }
    }
}
