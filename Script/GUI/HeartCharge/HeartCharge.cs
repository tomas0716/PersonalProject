using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartCharge : MonoBehaviour
{
    private GameObject              m_pGameObject_Parent    = null;
    private Transformer_Scalar      m_pScale                = new Transformer_Scalar(0);

    public bool                     m_IsShowRewardAds       = false;

    private ExcelData_ItemDataInfo  m_pItemDataInfo         = null;

    void Start()
    {
        m_pGameObject_Parent = Helper.FindChildGameObject(gameObject, "Parent");
        m_pGameObject_Parent.transform.localScale = new Vector3(0, 0, 1);

        m_pItemDataInfo = ExcelDataManager.Instance.m_pExcelData_ItemData.GetItemDataInfo_byItemType(eItemType.HeartCharge);

        GameObject ob;
        Text pText;
        Image pImage;

        ob = Helper.FindChildGameObject(gameObject, "Text_ItemName");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(m_pItemDataInfo.m_nItemName_TextTableID);

        ob = Helper.FindChildGameObject(gameObject, "Text_Desc");
        pText = ob.GetComponent<Text>();

        if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] == 0)
        {
            pText.text = Helper.GetTextDataString(m_pItemDataInfo.m_nDesc_TextTableID);
        }
        else
        {
            pText.text = Helper.GetTextDataString(eTextDataType.NotEnough_HeartCharge);
        }

        ob = Helper.FindChildGameObject(gameObject, "Text_ItemCount");
        pText = ob.GetComponent<Text>();
        pText.text = "x" + m_pItemDataInfo.m_nItemCount.ToString();

        GameObject ob_Buy = Helper.FindChildGameObject(gameObject, "Button_Buy");
        if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] != 0)
        {
            Button pButton = ob_Buy.GetComponent<Button>();
            pButton.interactable = false;
        }

        ob = Helper.FindChildGameObject(ob_Buy, "Text_Coin");
        pText = ob.GetComponent<Text>();
        pText.text = m_pItemDataInfo.m_nNeedCoin.ToString();

        if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] != 0)
        {
            pText.color = new Color(0.5f, 0.5f, 0.5f, 1);

            ob = Helper.FindChildGameObject(ob_Buy, "Image_CoinIcon");
            pImage = ob.GetComponent<Image>();
            pImage.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        if (m_IsShowRewardAds == true)
        {
            ob = Helper.FindChildGameObject(gameObject, "Text_RewardAds");
            pText = ob.GetComponent<Text>();
            pText.text = Helper.GetTextDataString(eTextDataType.HeartCharge_RewardAds);
        }

        TransformerEvent_Scalar eventValue;
        eventValue = new TransformerEvent_Scalar(0.2f, 1.0f);
        m_pScale.AddEvent(eventValue);
        m_pScale.OnPlay();

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

	private void OnDestroy()
	{
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
    }

    public void OnButtonClick_Close()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        PopupManager.Instance.RemovePopup(gameObject);
        PopupManager.Instance.ShowLastPopup();
    }

    public void OnButtonClick_Buy()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Coin] >= m_pItemDataInfo.m_nNeedCoin)
        {
            SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Coin] -= m_pItemDataInfo.m_nNeedCoin;
            SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] += m_pItemDataInfo.m_nItemCount;

            AppInstance.Instance.m_pEventDelegateManager.OnCreateLoading();
            AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save += OnGooglePlaySavedGameDone_Save;
            SavedGameDataInfo.Instance.Save();
        }
        else
        {
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/Shop");
            GameObject.Instantiate(ob);
        }
    }

    public void OnButtonClick_RewardAds()
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

    public void OnGoogleRewardsAdsLoadFailed()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsLoadFailed -= OnGoogleRewardsAdsLoadFailed;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsComplete -= OnGoogleRewardsAdsComplete;
    }

    public void OnGoogleRewardsAdsComplete()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsLoadFailed -= OnGoogleRewardsAdsLoadFailed;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsComplete -= OnGoogleRewardsAdsComplete;

        AppInstance.Instance.m_pEventDelegateManager.OnCreateLoading();

        SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] += 1;
        SavedGameDataInfo.Instance.m_byGetFreeHeartCountForAds += 1;

        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save += OnGooglePlaySavedGameDone_Save;
        SavedGameDataInfo.Instance.Save();
    }

    public void OnGooglePlaySavedGameDone_Save()
    {
        InGameInfo.Instance.m_IsInGameClick = true;

        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();

        AppInstance.Instance.m_pEventDelegateManager.OnUpdateHeartInfo();
        AppInstance.Instance.m_pEventDelegateManager.OnUpdateCoinInfo();
        AppInstance.Instance.m_pEventDelegateManager.OnUpdateItemState();
        AppInstance.Instance.m_pEventDelegateManager.OnItemBuy(m_pItemDataInfo.m_ItemType);

        PopupManager.Instance.RemovePopup(gameObject);
        PopupManager.Instance.ShowLastPopup();
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
