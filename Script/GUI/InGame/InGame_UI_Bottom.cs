using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI_Bottom : MonoBehaviour
{
    public class ItemContent
    {
        public GameObject   m_pGameObject_Add       = null;
        public GameObject   m_pGameObject_Got       = null;
        public Text         m_pText_Item_Count      = null;
    }

    private ItemContent[]   m_pItemContents         = new ItemContent[3];

    void Start()
    {
        GameObject ob;

        GameObject ob_items = Helper.FindChildGameObject(gameObject, "Items");
        for (int i = 0; i < 3; ++i)
        {
            string strName = "Button_Item_0" + (i + 1).ToString();
            ob = Helper.FindChildGameObject(ob_items, strName);

            m_pItemContents[i] = new ItemContent();
            m_pItemContents[i].m_pGameObject_Add = Helper.FindChildGameObject(ob, "State_Add");
            m_pItemContents[i].m_pGameObject_Got = Helper.FindChildGameObject(ob, "State_Got");
            m_pItemContents[i].m_pText_Item_Count = Helper.FindChildGameObject(m_pItemContents[i].m_pGameObject_Got, "Text_GotCount").GetComponent<Text>();
        }

        OnUpdateItemState();

        AppInstance.Instance.m_pEventDelegateManager.OnEventUpdataItemState += OnUpdateItemState;
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UsedItem += OnInGame_UsedItem;

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

    private void OnDestroy()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventUpdataItemState -= OnUpdateItemState;
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UsedItem -= OnInGame_UsedItem;

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
    }

    void Update()
    {
        
    }

    private void OnUpdateItemState()
    {
        if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Swap] > 0)
        {
            m_pItemContents[0].m_pGameObject_Add.SetActive(false);
            m_pItemContents[0].m_pGameObject_Got.SetActive(true);
            m_pItemContents[0].m_pText_Item_Count.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Swap].ToString();
        }
        else
        {
            m_pItemContents[0].m_pGameObject_Add.SetActive(true);
            m_pItemContents[0].m_pGameObject_Got.SetActive(false);
        }

        if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.CatPunch] > 0)
        {
            m_pItemContents[1].m_pGameObject_Add.SetActive(false);
            m_pItemContents[1].m_pGameObject_Got.SetActive(true);
            m_pItemContents[1].m_pText_Item_Count.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.CatPunch].ToString();
        }
        else
        {
            m_pItemContents[1].m_pGameObject_Add.SetActive(true);
            m_pItemContents[1].m_pGameObject_Got.SetActive(false);
        }

        if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.RunningCat] > 0)
        {
            m_pItemContents[2].m_pGameObject_Add.SetActive(false);
            m_pItemContents[2].m_pGameObject_Got.SetActive(true);
            m_pItemContents[2].m_pText_Item_Count.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.RunningCat].ToString();
        }
        else
        {
            m_pItemContents[2].m_pGameObject_Add.SetActive(true);
            m_pItemContents[2].m_pGameObject_Got.SetActive(false);
        }
    }

    public void OnButtonClick_Item_01()
    {
        if (InGameInfo.Instance.m_IsInGameClick == true && InGameInfo.Instance.m_eCurrItemUse_ItemType == eItemType.None && InGameInfo.Instance.m_IsInGameIntroing == false)
        {
            Helper.OnSoundPlay(eSoundType.Button, false);

            if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Swap] > 0)
            {
                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/InGame_ItemUse_Tooltip");
                ob = GameObject.Instantiate(ob);
                InGame_UI_ItemUse_Tooltip pItemUse_Tooltip = ob.GetComponent<InGame_UI_ItemUse_Tooltip>();
                pItemUse_Tooltip.Init(eItemType.Swap);

                // 아이템 발동
                AppInstance.Instance.m_pEventDelegateManager.OnInGame_ItemUse(eItemType.Swap);
            }
            else
            {
                InGameInfo.Instance.m_IsInGameClick = false;

                // 아이템 구매창 띄움
                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/ItemBuy");
                ob = GameObject.Instantiate(ob);
                ItemBuy pItemBuy = ob.GetComponent<ItemBuy>();
                pItemBuy.Init(ExcelDataManager.Instance.m_pExcelData_ItemData.GetItemDataInfo_byItemType(eItemType.Swap));
                PopupManager.Instance.AddPopup(ob);
            }
        }
    }

    public void OnButtonClick_Item_02()
    {
        if (InGameInfo.Instance.m_IsInGameClick == true && InGameInfo.Instance.m_eCurrItemUse_ItemType == eItemType.None && InGameInfo.Instance.m_IsInGameIntroing == false)
        {
            Helper.OnSoundPlay(eSoundType.Button, false);

            if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.CatPunch] > 0)
            {
                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/InGame_ItemUse_Tooltip");
                ob = GameObject.Instantiate(ob);
                InGame_UI_ItemUse_Tooltip pItemUse_Tooltip = ob.GetComponent<InGame_UI_ItemUse_Tooltip>();
                pItemUse_Tooltip.Init(eItemType.CatPunch);

                // 아이템 발동    
                AppInstance.Instance.m_pEventDelegateManager.OnInGame_ItemUse(eItemType.CatPunch);
            }
            else
            {
                InGameInfo.Instance.m_IsInGameClick = false;

                // 아이템 구매창 띄움
                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/ItemBuy");
                ob = GameObject.Instantiate(ob);
                ItemBuy pItemBuy = ob.GetComponent<ItemBuy>();
                pItemBuy.Init(ExcelDataManager.Instance.m_pExcelData_ItemData.GetItemDataInfo_byItemType(eItemType.CatPunch));
                PopupManager.Instance.AddPopup(ob);
            }
        }
    }

    public void OnButtonClick_Item_03()
    {
        if (InGameInfo.Instance.m_IsInGameClick == true && InGameInfo.Instance.m_eCurrItemUse_ItemType == eItemType.None && InGameInfo.Instance.m_IsInGameIntroing == false)
        {
            Helper.OnSoundPlay(eSoundType.Button, false);

            if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.RunningCat] > 0)
            {
                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/InGame_ItemUse_Tooltip");
                ob = GameObject.Instantiate(ob);
                InGame_UI_ItemUse_Tooltip pItemUse_Tooltip = ob.GetComponent<InGame_UI_ItemUse_Tooltip>();
                pItemUse_Tooltip.Init(eItemType.RunningCat);

                // 아이템 발동
                AppInstance.Instance.m_pEventDelegateManager.OnInGame_ItemUse(eItemType.RunningCat);
            }
            else
            {
                InGameInfo.Instance.m_IsInGameClick = false;

                // 아이템 구매창 띄움
                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/ItemBuy");
                ob = GameObject.Instantiate(ob);
                ItemBuy pItemBuy = ob.GetComponent<ItemBuy>();
                pItemBuy.Init(ExcelDataManager.Instance.m_pExcelData_ItemData.GetItemDataInfo_byItemType(eItemType.RunningCat));
                PopupManager.Instance.AddPopup(ob);
            }
        }
    }

    public void OnInGame_UsedItem(eItemType eType)
    {
        switch (eType)
        {
            case eItemType.Swap:
            case eItemType.CatPunch:
            case eItemType.RunningCat:
                {
                    SavedGameDataInfo.Instance.m_nItemCounts[(int)eType] -= 1;
                    SavedGameDataInfo.Instance.Save(false);
                }
                break;
        }

        OnUpdateItemState();
    }

    public void OnButtonClick_Option()
    {
        if (InGameInfo.Instance.m_IsInGameClick == true && InGameInfo.Instance.m_IsInGameIntroing == false)
        {
            Helper.OnSoundPlay(eSoundType.Button, false);

            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/InGame_Option");
            GameObject.Instantiate(ob);
            InGameInfo.Instance.m_IsInGameClick = false;
        }
    }

    public void OnHardwareBackButtonClick()
    {
        if (GameInfo.Instance.m_IsHardwareBackButtonProcess == false &&
            GameInfo.Instance.m_IsShopOpen == false &&
            GameInfo.Instance.m_IsItemBuyOpen == false &&
            GameInfo.Instance.m_nMessageBoxOpenCount == 0 &&
            GameInfo.Instance.m_nRewardPopupOpenCount == 0 &&
            GameInfo.Instance.m_IsInGame_UseItemTooltip == false &&
            GameInfo.Instance.m_IsInGame_MissionGuide == false &&
            GameInfo.Instance.m_IsInGame_MissionIntro == false &&
            GameInfo.Instance.m_IsInGame_Tutorial == false &&
            GameInfo.Instance.m_IsInGame_Option == false && 
            InGameInfo.Instance.m_IsInGameIntroing == false)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = true;
            OnButtonClick_Option();
        }
    }
}
