using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
    private GameObject                  m_pGameObject_Parent    = null;
    private Transformer_Scalar          m_pScale                = new Transformer_Scalar(0);

    private GameObject                  m_pGameObject_Board     = null;
    
    private GameObject                  m_pGameObject_CirclePoint_Fx = null;
    private bool                        m_IsCirclePoint_Fx      = true;

    private bool                        m_IsDisable             = false;
    private Text                        m_pText_Disable         = null;
    private string                      m_strDisableMessage     = "";

    private bool                        m_IsDoingAction         = false;
    private bool                        m_IsRouletteFree        = true;

    private ExcelData_Event_RouletteDataInfo                m_pRouletteDataInfo = null;
    private ExcelData_Event_RouletteDataInfo.RouletteItem   m_pSelRouletteItem  = null;

    private Transformer_Scalar          m_pRotation             = new Transformer_Scalar(0);
    private Transformer_Scalar          m_pSpeed                = new Transformer_Scalar(1);
    private Transformer_Timer           m_pTimer_Delay          = new Transformer_Timer();

    private Transformer_Timer           m_pTimer_Circle_Point_Fx = new Transformer_Timer();

    void Start()
    {
        m_pGameObject_Parent = Helper.FindChildGameObject(gameObject, "Parent");
        m_pGameObject_Parent.transform.localScale = new Vector3(0, 0, 1);

        GameObject ob;
        Text pText;
        Image pImage;

        ob = Helper.FindChildGameObject(gameObject, "Text_Title");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Roulette_Title);

        ob = Helper.FindChildGameObject(gameObject, "Button_Free");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Roulette_Free);

        ob = Helper.FindChildGameObject(gameObject, "Button_Ads");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Roulette_Ads);

        ob = Helper.FindChildGameObject(gameObject, "Button_Disable");
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Disable = ob.GetComponent<Text>();
        m_pText_Disable.text = "";
        m_strDisableMessage = Helper.GetTextDataString(eTextDataType.Roulette_Disable) + "\n";

        m_pRouletteDataInfo = ExcelDataManager.Instance.m_pExcelData_Event_RouletteData.GetDataInfo_byDayOfWeek(TimeServer.Instance.GetNISTDate().DayOfWeek);
        Texture2D pTex = Resources.Load<Texture2D>("Gui/Roulette/" + m_pRouletteDataInfo.m_strBoardImage);
        ob = Helper.FindChildGameObject(gameObject, "Board");
        m_pGameObject_Board = Helper.FindChildGameObject(ob, "Image_Board");
        pImage = m_pGameObject_Board.GetComponent<Image>();
        pImage.sprite = Sprite.Create(pTex, new Rect(0, 0, pTex.width, pTex.height), new Vector2(0.5f, 0.5f), 100.0f);
        m_pGameObject_CirclePoint_Fx = Helper.FindChildGameObject(ob, "Image_Circle_Point_Fx");

        if (SavedGameDataInfo.Instance.m_IsGetFreeRoulette == false)
        {
            ob = Helper.FindChildGameObject(gameObject, "Button_Ads");
            ob.SetActive(false);
            ob = Helper.FindChildGameObject(gameObject, "Button_Disable");
            ob.SetActive(false);
        }
        else if (SavedGameDataInfo.Instance.m_IsGetAdsRoulette == false)
        {
            if (SavedGameDataInfo.Instance.m_nLevel < GameDefine.ms_nRouletteAdsShowLevel)
            {
                m_IsDisable = true;

                ob = Helper.FindChildGameObject(gameObject, "Button_Ads");
                ob.SetActive(false);
                ob = Helper.FindChildGameObject(gameObject, "Button_Free");
                ob.SetActive(false);
            }
            else
            {
                ob = Helper.FindChildGameObject(gameObject, "Button_Free");
                ob.SetActive(false);
                ob = Helper.FindChildGameObject(gameObject, "Button_Disable");
                ob.SetActive(false);
            }
        }
        else
        {
            m_IsDisable = true;

            ob = Helper.FindChildGameObject(gameObject, "Button_Ads");
            ob.SetActive(false);
            ob = Helper.FindChildGameObject(gameObject, "Button_Free");
            ob.SetActive(false);
        }

        TransformerEvent eventValue;
        eventValue = new TransformerEvent_Scalar(0.2f, 1.0f);
        m_pScale.AddEvent(eventValue);
        m_pScale.OnPlay();

        eventValue = new TransformerEvent_Timer(0.5f);
        m_pTimer_Circle_Point_Fx.AddEvent(eventValue);
        m_pTimer_Circle_Point_Fx.SetCallback(null, OnDone_Timer_CirclePoint_Fx);
        m_pTimer_Circle_Point_Fx.OnPlay();

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

	private void OnDestroy()
	{
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsLoadFailed -= OnGoogleRewardsAdsLoadFailed;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsComplete -= OnGoogleRewardsAdsComplete;

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;

        AppInstance.Instance.m_pEventDelegateManager.OnEventRewardPopupClosed -= OnRewardPopupClosed;
    }

	void Update()
    {
        m_pScale.Update(Time.deltaTime);
        float fScale = m_pScale.GetCurScalar();
        m_pGameObject_Parent.transform.localScale = new Vector3(fScale, fScale, 1);

        if (m_IsDisable == true)
        {
            int nNextDayRemainTime = (int)TimeServer.Instance.GetNextDayRemainTime();

            int nMinute = nNextDayRemainTime / 60;
            int nHour = nMinute / 60;
            int nSecond = nNextDayRemainTime % 60;
            nMinute = nMinute % 60;

            m_pText_Disable.text = m_strDisableMessage + string.Format("{0}:{1}:{2}", nHour.ToString("D2"), nMinute.ToString("D2"), nSecond.ToString("D2"));
        }

        m_pSpeed.Update(Time.deltaTime);
        float fSpeed = m_pSpeed.GetCurScalar();
        m_pRotation.Update(Time.deltaTime * fSpeed);
        float fAngle = m_pRotation.GetCurScalar();
        m_pGameObject_Board.transform.localEulerAngles = new Vector3(0,0,fAngle);

        m_pTimer_Delay.Update(Time.deltaTime);
        m_pTimer_Circle_Point_Fx.Update(Time.deltaTime);
    }

    public void OnButtonClick_Close()
    {
        if (m_IsDoingAction == false)
        {
            Helper.OnSoundPlay(eSoundType.Button, false);

            GameObject.Destroy(gameObject);
        }
    }

    public void OnButtonClick_Free()
    {
        if (m_IsDoingAction == false)
        {
            Helper.OnSoundPlay(eSoundType.Button, false);

            m_IsRouletteFree = true;
            m_IsDoingAction = true;

            OnStartRoulette();

            int nLevel = SavedGameDataInfo.Instance.m_nLevel;
            if (nLevel < 20)
            {
                Helper.FirebaseLogEvent("Roulette_" + nLevel.ToString());
            }
            else
            {
                Helper.FirebaseLogEvent("Roulette");
            }
        }
    }

    public void OnButtonClick_Ads()
    {
        if (m_IsDoingAction == false)
        {
            Helper.OnSoundPlay(eSoundType.Button, false);

            m_IsRouletteFree = false;
            m_IsDoingAction = true;

            Helper.OnSoundPlay(eSoundType.Button, false);

            AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsLoadFailed -= OnGoogleRewardsAdsLoadFailed;
            AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsComplete -= OnGoogleRewardsAdsComplete;

            AppInstance.Instance.m_pEventDelegateManager.OnCreateLoading();
            AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsLoadFailed += OnGoogleRewardsAdsLoadFailed;
            AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsComplete += OnGoogleRewardsAdsComplete;
            //GoogleRewardedAdsManager.Instance.Show();
            UnityRewardAds.Instance.Show();

            int nLevel = SavedGameDataInfo.Instance.m_nLevel;
            if (nLevel < 20)
            {
                Helper.FirebaseLogEvent("RouletteAds_" + nLevel.ToString());
            }
            else
            {
                Helper.FirebaseLogEvent("RouletteAds");
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
        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsLoadFailed -= OnGoogleRewardsAdsLoadFailed;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsComplete -= OnGoogleRewardsAdsComplete;

        OnStartRoulette();
    }

    private void OnStartRoulette()
    {
        Helper.OnSoundPlay(eSoundType.Roulette, false);

        List<ExcelData_Event_RouletteDataInfo.RouletteItem> itemList = new List<ExcelData_Event_RouletteDataInfo.RouletteItem>();

        for (int i = 0; i < GameDefine.ms_nMaxItemRoulette; ++i)
        {
            ExcelData_Event_RouletteDataInfo.RouletteItem pRouletteItem = m_pRouletteDataInfo.m_RouletteItems[i];

            for (int j = 0; j < pRouletteItem.m_nPercentage; ++j)
            {
                itemList.Add(pRouletteItem);
            }
        }

        Helper.ShuffleList(itemList);
        int nMaxCount = itemList.Count;
        int nSelIndex = UnityEngine.Random.Range(0, nMaxCount);
        m_pSelRouletteItem = itemList[nSelIndex];

        float fOneCircleTime = 1.0f;
        float fCircleCount = 3;
        int nIndex = GameDefine.ms_nMaxItemRoulette - m_pSelRouletteItem.m_nIndex;
        float fTime = fOneCircleTime * fCircleCount + nIndex / 8.0f;
        float fAngle = 360 * fCircleCount + nIndex * 45 + UnityEngine.Random.Range(-15, 15);
        fAngle *= -1;

        float fCurrAngle = 360 - (m_pRotation.GetCurScalar() % 360);
        fTime += fCurrAngle / 360 * fOneCircleTime;

        m_pRotation.OnReset();
        TransformerEvent_Scalar eventValue;
        eventValue = new TransformerEvent_Scalar(0, fCurrAngle);
        m_pRotation.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(fTime, fAngle);
        m_pRotation.AddEvent(eventValue);
        m_pRotation.SetCallback(null, OnDone_Rotation);
        m_pRotation.OnPlay();

        float fDecreaseTimeRate = 0.9f;

        m_pSpeed.OnReset();
        eventValue = new TransformerEvent_Scalar(0, 1);
        m_pSpeed.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(fTime - fOneCircleTime * fDecreaseTimeRate, 1);
        m_pSpeed.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar((fTime - fOneCircleTime * fDecreaseTimeRate) + (fOneCircleTime * fDecreaseTimeRate) * 2.0f, 0.01f);
        m_pSpeed.AddEvent(eventValue);
        m_pSpeed.OnPlay();
    }

    private void OnDone_Rotation(TransformerEvent eventValue)
    {
        m_pTimer_Delay.OnReset();
        eventValue = new TransformerEvent_Timer(1.2f);
        m_pTimer_Delay.AddEvent(eventValue);
        m_pTimer_Delay.SetCallback(null, OnDone_Timer_Delay);
        m_pTimer_Delay.OnPlay();
    }

    private void OnDone_Timer_Delay(TransformerEvent eventValue)
    {
        GameObject ob;

        if (m_IsRouletteFree == true)
        {
            SavedGameDataInfo.Instance.m_IsGetFreeRoulette = true;

            if (SavedGameDataInfo.Instance.m_nLevel < GameDefine.ms_nRouletteAdsShowLevel)
            {
                SavedGameDataInfo.Instance.m_IsGetAdsRoulette = true;
                m_IsDisable = true;

                ob = Helper.FindChildGameObject(gameObject, "Button_Free");
                ob.SetActive(false);
                ob = Helper.FindChildGameObject(gameObject, "Button_Ads");
                ob.SetActive(false);
                ob = Helper.FindChildGameObject(gameObject, "Button_Disable");
                ob.SetActive(true);
            }
            else
            {
                ob = Helper.FindChildGameObject(gameObject, "Button_Free");
                ob.SetActive(false);
                ob = Helper.FindChildGameObject(gameObject, "Button_Ads");
                ob.SetActive(true);
                ob = Helper.FindChildGameObject(gameObject, "Button_Disable");
                ob.SetActive(false);
            }
        }
        else
        {
            SavedGameDataInfo.Instance.m_IsGetAdsRoulette = true;
            m_IsDisable = true;

            ob = Helper.FindChildGameObject(gameObject, "Button_Free");
            ob.SetActive(false);
            ob = Helper.FindChildGameObject(gameObject, "Button_Ads");
            ob.SetActive(false);
            ob = Helper.FindChildGameObject(gameObject, "Button_Disable");
            ob.SetActive(true);
        }

        SavedGameDataInfo.Instance.m_nItemCounts[(int)m_pSelRouletteItem.m_ItemType] += m_pSelRouletteItem.m_nCount;

        AppInstance.Instance.m_pEventDelegateManager.OnCreateLoading();
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save += OnGooglePlaySavedGameDone_Save;
        SavedGameDataInfo.Instance.Save();
    }

    public void OnGooglePlaySavedGameDone_Save()
    {
        m_IsDoingAction = false;

        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();

        AppInstance.Instance.m_pEventDelegateManager.OnUpdateHeartInfo();
        AppInstance.Instance.m_pEventDelegateManager.OnUpdateCoinInfo();
        AppInstance.Instance.m_pEventDelegateManager.OnUpdateItemState();

        AppInstance.Instance.m_pEventDelegateManager.OnEventRewardPopupClosed += OnRewardPopupClosed;

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/RewardPopup");
        ob = GameObject.Instantiate(ob);
        RewardPopup pRewardPopup = ob.GetComponent<RewardPopup>();
        pRewardPopup.SetRewardInfo(eRewardSubject.eRoulette, m_pSelRouletteItem.m_ItemType, m_pSelRouletteItem.m_nCount);

        AppInstance.Instance.m_pEventDelegateManager.OnRouletteComplete();
    }

    private void OnDone_Timer_CirclePoint_Fx(TransformerEvent eventValue)
    {
        m_IsCirclePoint_Fx = !m_IsCirclePoint_Fx;
        m_pGameObject_CirclePoint_Fx.SetActive(m_IsCirclePoint_Fx);

        m_pTimer_Circle_Point_Fx.OnPlay();
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
            OnButtonClick_Close();
        }
    }

    public void OnRewardPopupClosed()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventRewardPopupClosed -= OnRewardPopupClosed;

        if (SavedGameDataInfo.Instance.m_nLevel < GameDefine.ms_nRouletteAdsShowLevel)
        {
            GameObject.Destroy(gameObject);
        }       
    }
}
