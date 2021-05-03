using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Item_Single : Shop_Item_Base
{
    void Start()
    {
        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(gameObject, "Text_Coin");
        pText = ob.GetComponent<Text>();
        pText.text = m_pIAPDataInfo.m_nCoin.ToString();

        ob = Helper.FindChildGameObject(gameObject, "Button_Buy");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();

        if (m_pIAPDataInfo.m_dePrice != 0)
        {
            pText.text = string.Format("{0} {1}", m_pIAPDataInfo.m_dePrice, m_pIAPDataInfo.m_strCountryCode);
        }
        else
        {
            pText.text = m_pIAPDataInfo.m_strOfflineMode_Price;
        }
    }

	private void OnDestroy()
	{
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleProcessPurchaseFailed -= OnGoogleProcessPurchaseFailed;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleProcessPurchaseDone -= OnGoogleProcessPurchaseDone;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;
    }

	void Update()
    {
        
    }

    public void OnButtonClick_Buy()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleProcessPurchaseFailed += OnGoogleProcessPurchaseFailed;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleProcessPurchaseDone += OnGoogleProcessPurchaseDone;

        GoogleIAPManager.Instance.Buy(m_pIAPDataInfo);
    }

    public void OnGoogleProcessPurchaseFailed(ExcelData_Google_IAPDataInfo pIAPDataInfo)
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleProcessPurchaseFailed -= OnGoogleProcessPurchaseFailed;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleProcessPurchaseDone -= OnGoogleProcessPurchaseDone;

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
        ob = GameObject.Instantiate(ob);
        MessageBox pMessageBox = ob.GetComponent<MessageBox>();
        pMessageBox.Initialize(MessageBox.eMessageBoxType.OK, Helper.GetTextDataString(eTextDataType.Shop_IAPBuyFailed_Title), Helper.GetTextDataString(eTextDataType.Shop_IAPBuyFailed_Desc), Helper.GetTextDataString(eTextDataType.OK), "");

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();
    }

    public void OnGoogleProcessPurchaseDone(ExcelData_Google_IAPDataInfo pIAPDataInfo)
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleProcessPurchaseFailed -= OnGoogleProcessPurchaseFailed;
        AppInstance.Instance.m_pEventDelegateManager.OnEventGoogleProcessPurchaseDone -= OnGoogleProcessPurchaseDone;

        SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Coin] += m_pIAPDataInfo.m_nCoin;

        AppInstance.Instance.m_pEventDelegateManager.OnCreateLoading();
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save += OnGooglePlaySavedGameDone_Save;
        SavedGameDataInfo.Instance.Save();
    }

    public void OnGooglePlaySavedGameDone_Save()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;

        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
        ob = GameObject.Instantiate(ob);
        MessageBox pMessageBox = ob.GetComponent<MessageBox>();
        string strDesc = string.Format(Helper.GetTextDataString(eTextDataType.Shop_IAPBuySuccess_Single_Desc), m_pIAPDataInfo.m_nCoin);
        pMessageBox.Initialize(MessageBox.eMessageBoxType.OK, Helper.GetTextDataString(eTextDataType.Shop_IAPBuySuccess_Title), strDesc, Helper.GetTextDataString(eTextDataType.OK), "");

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();

        AppInstance.Instance.m_pEventDelegateManager.OnUpdateCoinInfo();
    }
}
