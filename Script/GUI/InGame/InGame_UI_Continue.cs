using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI_Continue : MonoBehaviour
{
    private GameObject          m_pGameObject_Parent    = null;
    private Transformer_Scalar  m_pScale                = new Transformer_Scalar(0);

    private CoinInfo            m_pCoinInfo             = null;

    private bool                m_IsValid               = true;

    void Start()
    {
        Helper.OnSoundPlay(eSoundType.Failed, false);

        m_pGameObject_Parent = Helper.FindChildGameObject(gameObject, "Parent");
        m_pGameObject_Parent.transform.localScale = new Vector3(0, 0, 1);

        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(gameObject, "Text_Title");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.InGame_Continue_Title);

        ob = Helper.FindChildGameObject(gameObject, "Text_Title_Desc");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.InGame_Continue_Title_Desc);

        ob = Helper.FindChildGameObject(gameObject, "Text_Desc");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.InGame_Continue_Desc);

        ob = Helper.FindChildGameObject(gameObject, "Text_Continue");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.InGame_Continue);

        ExcelData_ItemDataInfo pItemDataInfo = ExcelDataManager.Instance.m_pExcelData_ItemData.GetItemDataInfo_byItemType(eItemType.Continue);
        ob = Helper.FindChildGameObject(gameObject, "Text_Coin");
        pText = ob.GetComponent<Text>();
        pText.text = pItemDataInfo.m_nNeedCoin.ToString();

        TransformerEvent_Scalar eventValue;
        eventValue = new TransformerEvent_Scalar(0.2f, 1.0f);
        m_pScale.AddEvent(eventValue);
        m_pScale.OnPlay();

        ob = Resources.Load<GameObject>("Gui/Prefabs/CoinInfo_InGame");
        ob = GameObject.Instantiate(ob);
        m_pCoinInfo = ob.GetComponent<CoinInfo>();

        Update();
    }

	private void OnDestroy()
	{
        GameObject.Destroy(m_pCoinInfo.gameObject);

        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;
    }

	void Update()
    {
        m_pScale.Update(Time.deltaTime);
        float fScale = m_pScale.GetCurScalar();
        m_pGameObject_Parent.transform.localScale = new Vector3(fScale, fScale, 1);
    }

    public void OnGooglePlaySavedGameDone_Save()
    {
        m_IsValid = true;

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();

        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;

        ++InGameInfo.Instance.m_nContinueCount;
        AppInstance.Instance.m_pEventDelegateManager.OnInGame_ItemBuy_Complete(eItemType.Continue);

        PopupManager.Instance.RemovePopup(gameObject);
    }

    public void OnButtonClick_Close()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        PopupManager.Instance.RemovePopup(gameObject);
        AppInstance.Instance.m_pEventDelegateManager.OnInGame_ItemBuy_Continue_Cancel();
    }

    public void OnButtonClick_Continue()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        ExcelData_ItemDataInfo pItemDataInfo = ExcelDataManager.Instance.m_pExcelData_ItemData.GetItemDataInfo_byItemType(eItemType.Continue);
        if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Coin] >= pItemDataInfo.m_nNeedCoin)
        {
            if (m_IsValid == true)
            {
                AppInstance.Instance.m_pEventDelegateManager.OnCreateLoading();

                m_IsValid = false;
                AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save += OnGooglePlaySavedGameDone_Save;

                SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Coin] -= pItemDataInfo.m_nNeedCoin;
                SavedGameDataInfo.Instance.Save();
            }
        }
        else
        {
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/Shop");
            GameObject.Instantiate(ob);
        }
    }
}
