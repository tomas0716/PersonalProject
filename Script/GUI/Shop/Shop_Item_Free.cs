using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Item_Free : Shop_Item_Base
{
    void Start()
    {
        GameObject ob;
        Text pText;
        
        ob = Helper.FindChildGameObject(gameObject, "Text_Coin");
        pText = ob.GetComponent<Text>();
        pText.text = m_pIAPDataInfo.m_nCoin.ToString();
    }

	private void OnDestroy()
	{
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;
    }

	void Update()
    {
        
    }

    public void OnButtonClick_Free()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        AppInstance.Instance.m_pEventDelegateManager.OnCreateLoading();

        SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Coin] += m_pIAPDataInfo.m_nCoin;
        SavedGameDataInfo.Instance.m_IsGetFreeCoin = true;

        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save += OnGooglePlaySavedGameDone_Save;
        SavedGameDataInfo.Instance.Save();
    }

    public void OnGooglePlaySavedGameDone_Save()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;

        //GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
        //ob = GameObject.Instantiate(ob);
        //MessageBox pMessageBox = ob.GetComponent<MessageBox>();
        //string strDesc = string.Format(Helper.GetTextDataString(eTextDataType.Shop_FreeCoin_Receive_Desc), m_pIAPDataInfo.m_nCoin);
        //pMessageBox.Initialize(MessageBox.eMessageBoxType.OK, Helper.GetTextDataString(eTextDataType.Shop_FreeCoin_Receive_Title), strDesc, Helper.GetTextDataString(eTextDataType.OK),"");

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/RewardPopup");
        ob = GameObject.Instantiate(ob);
        RewardPopup pRewardPopup = ob.GetComponent<RewardPopup>();
        pRewardPopup.SetRewardInfo(eRewardSubject.eShop, eItemType.Coin, m_pIAPDataInfo.m_nCoin);

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();

        AppInstance.Instance.m_pEventDelegateManager.OnOnUpdateShopItem();
        AppInstance.Instance.m_pEventDelegateManager.OnUpdateCoinInfo();
    }
}
