using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI_ItemUse_Tooltip : MonoBehaviour
{
    private eItemType   m_eItemType = eItemType.None;

    void Start()
    {
        GameInfo.Instance.m_IsInGame_UseItemTooltip = true;

        GameObject ob;
        Text pText;

        GameObject ob_Top = Helper.FindChildGameObject(gameObject, "Top");

        switch (m_eItemType)
        {
            case eItemType.Swap:
                {
                    ob = Helper.FindChildGameObject(ob_Top, "Image_SlotAttack");
                    ob.SetActive(false);
                    ob = Helper.FindChildGameObject(ob_Top, "Image_StripeHV");
                    ob.SetActive(false);

                    ob = Helper.FindChildGameObject(ob_Top, "Text_Desc");
                    pText = ob.GetComponent<Text>();
                    pText.text = Helper.GetTextDataString(eTextDataType.InGame_Item_ChangeUnit_Tooltip_Desc);
                }
                break;

            case eItemType.CatPunch:
                {
                    ob = Helper.FindChildGameObject(ob_Top, "Image_ChangeUnit");
                    ob.SetActive(false);
                    ob = Helper.FindChildGameObject(ob_Top, "Image_StripeHV");
                    ob.SetActive(false);

                    ob = Helper.FindChildGameObject(ob_Top, "Text_Desc");
                    pText = ob.GetComponent<Text>();
                    pText.text = Helper.GetTextDataString(eTextDataType.InGame_Item_SlotAttack_Tooltip_Desc);
                }
                break;

            case eItemType.RunningCat:
                {
                    ob = Helper.FindChildGameObject(ob_Top, "Image_ChangeUnit");
                    ob.SetActive(false);
                    ob = Helper.FindChildGameObject(ob_Top, "Image_SlotAttack");
                    ob.SetActive(false);

                    ob = Helper.FindChildGameObject(ob_Top, "Text_Desc");
                    pText = ob.GetComponent<Text>();
                    pText.text = Helper.GetTextDataString(eTextDataType.InGame_Item_StripeHV_Tooltip_Desc);
                }
                break;
        }


        ob = Helper.FindChildGameObject(gameObject, "Button_Cancel");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Cancel);

        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_ItemUse_Tooltip_Destroy += OnInGame_ItemUse_Tooltip_Destroy;
        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

	private void OnDestroy()
	{
        GameInfo.Instance.m_IsInGame_UseItemTooltip = false;

        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_ItemUse_Tooltip_Destroy -= OnInGame_ItemUse_Tooltip_Destroy;
        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
    }

	public void Init(eItemType eType)
    {
        m_eItemType = eType;
    }

    void Update()
    {
        
    }

    public void OnInGame_ItemUse_Tooltip_Destroy()
    {
        GameObject.Destroy(gameObject);
    }

    public void OnButtonClick_Cancel()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        AppInstance.Instance.m_pEventDelegateManager.OnInGame_ItemUseCancel();
        AppInstance.Instance.m_pEventDelegateManager.OnInGame_UI_Bottom_Visible(true);
        GameObject.Destroy(gameObject);
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

            OnButtonClick_Cancel();
        }
    }
}
