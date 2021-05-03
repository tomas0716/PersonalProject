using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class Shop : MonoBehaviour
{
    private GameObject          m_pGameObject_ScrollViewContent     = null;
    private GridLayoutGroup     m_pGridLayoutGroup                  = null;

    private SpriteAtlas         m_pSpriteAtlas_Package              = null;

	private void Awake()
	{
        GameInfo.Instance.m_IsShopOpen = true;
    }

	void Start()
    {
        SpriteAtlas sa = Resources.Load<SpriteAtlas>("Gui/Shop/Atlas_Package");
        m_pSpriteAtlas_Package = SpriteAtlas.Instantiate(sa);

        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(gameObject, "Scroll View");
        ob = Helper.FindChildGameObject(ob, "Viewport");
        ob = Helper.FindChildGameObject(ob, "Content");
        m_pGridLayoutGroup = ob.GetComponent<GridLayoutGroup>();
        ob = Helper.FindChildGameObject(ob, "IAPs");
        m_pGameObject_ScrollViewContent = Helper.FindChildGameObject(ob, "GameObject");

        ob = Helper.FindChildGameObject(gameObject, "Image_Title");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Shop_Title);

        OnUpdateShopItem();

        AppInstance.Instance.m_pEventDelegateManager.OnShopOpen();

        AppInstance.Instance.m_pEventDelegateManager.OnEventUpdateShopItem += OnUpdateShopItem;
        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

	private void OnDestroy()
	{
        GameInfo.Instance.m_IsShopOpen = false;

        AppInstance.Instance.m_pEventDelegateManager.OnEventUpdateShopItem -= OnUpdateShopItem;
        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
    }

	void Update()
    {

    }

    private void OnUpdateShopItem()
    {
#if UNITY_EDITOR || UNITY_ANDROID
        Helper.RemoveChildAll(m_pGameObject_ScrollViewContent);

        List <Shop_Item_Base> itemContentList = new List<Shop_Item_Base>();

        int nCount = ExcelDataManager.Instance.m_pExcelData_Google_IAPData.GetNumData();

        for (int i = 0; i < nCount; ++i)
        {
            ExcelData_Google_IAPDataInfo pIAPDataInfo = ExcelDataManager.Instance.m_pExcelData_Google_IAPData.GetIAPDataInfo_byIndex(i);

            if (pIAPDataInfo != null)
            {
                switch (pIAPDataInfo.m_eIAPType)
                {
                    case eIAPType.Package_Beginner:
                        {
                            if (SavedGameDataInfo.Instance.m_IsBeginnerPackageBuy == false)
                            {
                                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/" + pIAPDataInfo.m_strPrefabName);
                                ob = GameObject.Instantiate(ob);
                                Shop_Item_Package pItem = ob.GetComponent<Shop_Item_Package>();
                                pItem.Init(pIAPDataInfo);
                                pItem.SetAtlas_Package(m_pSpriteAtlas_Package);
                                itemContentList.Add(pItem);
                            }
                        }
                        break;

                    case eIAPType.Single_Free:
                        {
                            if (SavedGameDataInfo.Instance.m_IsGetFreeCoin == false)
                            {
                                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/" + pIAPDataInfo.m_strPrefabName);
                                ob = GameObject.Instantiate(ob);
                                Shop_Item_Free pItem = ob.GetComponent<Shop_Item_Free>();
                                pItem.Init(pIAPDataInfo);
                                itemContentList.Add(pItem);
                            }
                        }
                        break;

                    case eIAPType.Single_Ads:
                        {
                            if (SavedGameDataInfo.Instance.m_IsGetFreeCoin == true && SavedGameDataInfo.Instance.m_byGetAdsCoinCount < GameDefine.ms_nShopItemFree_Max)
                            {
                                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/" + pIAPDataInfo.m_strPrefabName);
                                ob = GameObject.Instantiate(ob);
                                Shop_Item_Ads pItem = ob.GetComponent<Shop_Item_Ads>();
                                pItem.Init(pIAPDataInfo);
                                itemContentList.Add(pItem);
                            }
                        }
                        break;

                    case eIAPType.Single:
                        {
                            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/" + pIAPDataInfo.m_strPrefabName);
                            ob = GameObject.Instantiate(ob);
                            Shop_Item_Single pItem = ob.GetComponent<Shop_Item_Single>();
                            pItem.Init(pIAPDataInfo);
                            itemContentList.Add(pItem);
                        }
                        break;

                    case eIAPType.Package:
                        {
                            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/" + pIAPDataInfo.m_strPrefabName);
                            ob = GameObject.Instantiate(ob);
                            Shop_Item_Package pItem = ob.GetComponent<Shop_Item_Package>();
                            pItem.Init(pIAPDataInfo);
                            pItem.SetAtlas_Package(m_pSpriteAtlas_Package);
                            itemContentList.Add(pItem);
                        }
                        break;
                }
            }
        }

        float fInterval = 10;

        float fTotalHeight = 30;
        foreach (Shop_Item_Base pItem in itemContentList)
        {
            fTotalHeight += pItem.GetHeight();
            fTotalHeight += fInterval;
        }

        fTotalHeight += 30;

        Vector2 vSize = m_pGridLayoutGroup.cellSize;
        vSize.y = fTotalHeight;
        m_pGridLayoutGroup.cellSize = vSize;

        float fNextHeight = 30;
        foreach (Shop_Item_Base pItem in itemContentList)
        {
            pItem.transform.SetParent(m_pGameObject_ScrollViewContent.transform);
            float fHeight = pItem.GetHeight();
            pItem.transform.localPosition = new Vector3(0,fHeight * -0.5f + -fNextHeight, 0);
            fNextHeight += fHeight;
            fNextHeight += fInterval;
            pItem.transform.localScale = Vector3.one;
        }
#endif
    }

    public void OnButtonClick_Close()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        //PopupManager.Instance.ShowLastPopup();
        GameObject.Destroy(gameObject);
    }

    public void OnHardwareBackButtonClick()
    {
        if (GameInfo.Instance.m_IsHardwareBackButtonProcess == false &&
            GameInfo.Instance.m_nMessageBoxOpenCount == 0 &&
            GameInfo.Instance.m_nRewardPopupOpenCount == 0)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = true;
            OnButtonClick_Close();
        }
    }
}
