using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class Shop_Item_Package : Shop_Item_Base
{
    private SpriteAtlas     m_pSpriteAtlas_Package  = null;
    private string          m_strPackageName        = "";

    void Start()
    {
        GameObject ob;
        Image pImage;
        Text pText;

        ob = Helper.FindChildGameObject(gameObject, "Image_Product");
        pImage = ob.GetComponent<Image>();
        pImage.sprite = m_pSpriteAtlas_Package.GetSprite(m_pIAPDataInfo.m_strIconFileName);

        ob = Helper.FindChildGameObject(gameObject, "Text_Name");
        pText = ob.GetComponent<Text>();
        m_strPackageName = Helper.GetTextDataString(m_pIAPDataInfo.m_nPackageName_TextTableID);
        pText.text = m_strPackageName;

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

    public void SetAtlas_Package(SpriteAtlas pAtlas)
    {
        m_pSpriteAtlas_Package = pAtlas;
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

        foreach (KeyValuePair<int, int> item in pIAPDataInfo.m_IncludeItemTable)
        {
            SavedGameDataInfo.Instance.m_nItemCounts[item.Key] += item.Value;
        }

        if (m_pIAPDataInfo.m_eIAPType == eIAPType.Package_Beginner)
        {
            SavedGameDataInfo.Instance.m_IsBeginnerPackageBuy = true;
        }

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
        string strDesc = string.Format(Helper.GetTextDataString(eTextDataType.Shop_IAPBuySuccess_PackageDesc), m_strPackageName);
        pMessageBox.Initialize(MessageBox.eMessageBoxType.OK, Helper.GetTextDataString(eTextDataType.Shop_IAPBuySuccess_Title), strDesc, Helper.GetTextDataString(eTextDataType.OK), "");

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();
        AppInstance.Instance.m_pEventDelegateManager.OnUpdateCoinInfo();
        AppInstance.Instance.m_pEventDelegateManager.OnUpdateItemState();

        if (m_pIAPDataInfo.m_eIAPType == eIAPType.Package_Beginner)
        {
            AppInstance.Instance.m_pEventDelegateManager.OnOnUpdateShopItem();
        }
    }
}
