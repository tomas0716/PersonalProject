using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class ItemBuy : MonoBehaviour
{
    private GameObject              m_pGameObject_Parent    = null;
    private Transformer_Scalar      m_pScale                = new Transformer_Scalar(0);

    private ExcelData_ItemDataInfo  m_pItemDataInfo         = null;

    private HeartInfo               m_pHeartInfo            = null;
    private CoinInfo                m_pCoinInfo             = null;

    void Start()
    {
        GameInfo.Instance.m_IsItemBuyOpen = true;

        m_pGameObject_Parent = Helper.FindChildGameObject(gameObject, "Parent");
        m_pGameObject_Parent.transform.localScale = new Vector3(0, 0, 1);

        GameObject ob;
        Text pText;
        Image pImage;

        ob = Helper.FindChildGameObject(gameObject, "Text_ItemName");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(m_pItemDataInfo.m_nItemName_TextTableID);

        ob = Helper.FindChildGameObject(gameObject, "Text_Desc");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(m_pItemDataInfo.m_nDesc_TextTableID);

        ob = Helper.FindChildGameObject(gameObject, "Text_ItemCount");
        pText = ob.GetComponent<Text>();
		pText.text = "x" + m_pItemDataInfo.m_nItemCount.ToString();

        ob = Helper.FindChildGameObject(gameObject, "Button_Buy");
        ob = Helper.FindChildGameObject(ob, "Text_Coin");
        pText = ob.GetComponent<Text>();
        pText.text = m_pItemDataInfo.m_nNeedCoin.ToString();

        ob = Helper.FindChildGameObject(gameObject, "Image_Icon");
        pImage = ob.GetComponent<Image>();

        Texture2D pTex = Resources.Load<Texture2D>("Gui/ItemIcon/" + m_pItemDataInfo.m_strIconFileName);
        pImage.sprite = Sprite.Create(pTex, new Rect(0, 0, pTex.width, pTex.height), new Vector2(0.5f, 0.5f), 100.0f);

        if (AppInstance.Instance.m_pSceneManager.GetCurrSceneType() == eSceneType.Scene_InGame)
        {
            ob = Resources.Load<GameObject>("Gui/Prefabs/CoinInfo_InGame");
            ob = GameObject.Instantiate(ob);
            m_pCoinInfo = ob.GetComponent<CoinInfo>();
        }

        TransformerEvent_Scalar eventValue;
        eventValue = new TransformerEvent_Scalar(0.2f, 1.0f);
        m_pScale.AddEvent(eventValue);
        m_pScale.OnPlay();

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

	private void OnDestroy()
	{
        GameInfo.Instance.m_IsItemBuyOpen = false;

        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;

        if (AppInstance.Instance.m_pSceneManager.GetCurrSceneType() == eSceneType.Scene_InGame && m_pHeartInfo != null)
        {
            GameObject.Destroy(m_pHeartInfo.gameObject);
        }

        if (AppInstance.Instance.m_pSceneManager.GetCurrSceneType() == eSceneType.Scene_InGame && m_pCoinInfo != null)
        {
            GameObject.Destroy(m_pCoinInfo.gameObject);
        }

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
    }

	void Update()
    {
        m_pScale.Update(Time.deltaTime);
        float fScale = m_pScale.GetCurScalar();
        m_pGameObject_Parent.transform.localScale = new Vector3(fScale, fScale, 1);
    }

    public void Init(ExcelData_ItemDataInfo pItemDataInfo)
    {
        m_pItemDataInfo = pItemDataInfo;
    }

    public void OnButtonClick_Close()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        InGameInfo.Instance.m_IsInGameClick = true;
        PopupManager.Instance.RemovePopup(gameObject);
        PopupManager.Instance.ShowLastPopup();
    }

    public void OnButtonClick_Buy()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Coin] >= m_pItemDataInfo.m_nNeedCoin)
        {
            SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Coin] -= m_pItemDataInfo.m_nNeedCoin;
            SavedGameDataInfo.Instance.m_nItemCounts[(int)m_pItemDataInfo.m_ItemType] += m_pItemDataInfo.m_nItemCount;

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

    public void OnGooglePlaySavedGameDone_Save()
    {
        InGameInfo.Instance.m_IsInGameClick = true;

        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();

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
            GameInfo.Instance.m_nMessageBoxOpenCount == 0 &&
            GameInfo.Instance.m_nRewardPopupOpenCount == 0)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = true;
            OnButtonClick_Close();
        }
    }
}
