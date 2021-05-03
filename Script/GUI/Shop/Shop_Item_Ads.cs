using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Item_Ads : Shop_Item_Base
{
    void Start()
    {
        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(gameObject, "Text_Coin");
        pText = ob.GetComponent<Text>();
        pText.text = m_pIAPDataInfo.m_nCoin.ToString();

        ob = Helper.FindChildGameObject(gameObject, "Text_Desc");
        pText = ob.GetComponent<Text>();
        string strDesc = Helper.GetTextDataString(eTextDataType.Shop_Item_Ads_Desc);
        strDesc += " (";
        strDesc += (GameDefine.ms_nShopItemFree_Max- SavedGameDataInfo.Instance.m_byGetAdsCoinCount).ToString();
        strDesc += "/";
        strDesc += GameDefine.ms_nShopItemFree_Max.ToString();
        strDesc += ")";
        pText.text = strDesc;
    }

	private void OnDestroy()
	{
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsLoadFailed -= OnGoogleRewardsAdsLoadFailed;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleRewardsAdsComplete -= OnGoogleRewardsAdsComplete;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;
    }

	void Update()
    {
        
    }

	public void OnButtonClick_Ads()
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

        SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Coin] += m_pIAPDataInfo.m_nCoin;
        SavedGameDataInfo.Instance.m_byGetAdsCoinCount += 1;

        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save += OnGooglePlaySavedGameDone_Save;
        SavedGameDataInfo.Instance.Save();
    }

    public void OnGooglePlaySavedGameDone_Save()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;

        //GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
        //ob = GameObject.Instantiate(ob);
        //MessageBox pMessageBox = ob.GetComponent<MessageBox>();
        //string strDesc = string.Format(Helper.GetTextDataString(eTextDataType.Shop_RewardAdsCoin_Receive_Desc), m_pIAPDataInfo.m_nCoin);
        //pMessageBox.Initialize(MessageBox.eMessageBoxType.OK, Helper.GetTextDataString(eTextDataType.Shop_RewardAdsCoin_Receive_Title), strDesc, Helper.GetTextDataString(eTextDataType.OK), "");

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/RewardPopup");
        ob = GameObject.Instantiate(ob);
        RewardPopup pRewardPopup = ob.GetComponent<RewardPopup>();
        pRewardPopup.SetRewardInfo(eRewardSubject.eShop, eItemType.Coin, m_pIAPDataInfo.m_nCoin);

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();

        AppInstance.Instance.m_pEventDelegateManager.OnOnUpdateShopItem();
        AppInstance.Instance.m_pEventDelegateManager.OnUpdateCoinInfo();

        AppInstance.Instance.m_pEventDelegateManager.OnShopItemAdsComplete();
    }
}
