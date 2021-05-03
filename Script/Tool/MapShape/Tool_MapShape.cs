using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_MapShape : MonoBehaviour
{
    private Text[]          m_pText_EditSlotFillWay     = new Text[(int)eSlotFillWay.Max];
    private Text[]          m_pText_Barrigate           = new Text[(int)eBarrigate.Max];

    private GameObject      m_pGameObject_Slots         = null;
    private GameObject      m_pGameObject_BaseSlot      = null;
    private GameObject      m_pGameObject_OringBaseSlot = null;

    private Color []        m_clrSlot                   = new Color[(int)eSlotFillWay.Max];

    private float           m_fSlotSize                 = 52;

    private Text[]          m_pText_SlotMoveIndex       = new Text[GameDefine.ms_nSlotMoveMaxCount];

    void Start()
    {
        m_clrSlot[(int)eSlotFillWay.Close]              = Color.grey;
        m_clrSlot[(int)eSlotFillWay.Normal]             = Color.white;
        m_clrSlot[(int)eSlotFillWay.Create]             = Color.red;

        GameObject ob;
        Button pButton;

        ob = Helper.FindChildGameObject(gameObject, "Button_Close");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_SlotFillWay((int)eSlotFillWay.Close); });
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_EditSlotFillWay[(int)eSlotFillWay.Close] = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_Normal");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_SlotFillWay((int)eSlotFillWay.Normal); });
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_EditSlotFillWay[(int)eSlotFillWay.Normal] = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_Create");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_SlotFillWay((int)eSlotFillWay.Create); });
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_EditSlotFillWay[(int)eSlotFillWay.Create] = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_Link");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_SlotFillWay((int)eSlotFillWay.Link_ForTool); });
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_EditSlotFillWay[(int)eSlotFillWay.Link_ForTool] = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_InVisibleLink");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_SlotFillWay((int)eSlotFillWay.InVisibleLink_ForTool); });
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_EditSlotFillWay[(int)eSlotFillWay.InVisibleLink_ForTool] = ob.GetComponent<Text>();

        m_pText_EditSlotFillWay[(int)eSlotFillWay.Close].color = new Color(1, 0, 0);

        m_pGameObject_Slots = Helper.FindChildGameObject(gameObject, "Slots");
        m_pGameObject_BaseSlot = Helper.FindChildGameObject(m_pGameObject_Slots, "Slot_Base");
        RectTransform rtf = m_pGameObject_BaseSlot.GetComponent<RectTransform>();
        m_fSlotSize = rtf.rect.width;
        m_pGameObject_BaseSlot.SetActive(false);

        m_pGameObject_OringBaseSlot = Resources.Load<GameObject>("Tool/Prefabs/Slot_Base");

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Barrigate
        /// 
        GameObject ob_Barrigate = Helper.FindChildGameObject(gameObject, "Barrigate");
        ob = Helper.FindChildGameObject(ob_Barrigate, "Button_Left");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Barrigate((int)eBarrigate.Barrigate_Left); });
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Barrigate[(int)eBarrigate.Barrigate_Left] = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(ob_Barrigate, "Button_Right");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Barrigate((int)eBarrigate.Barrigate_Right); });
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Barrigate[(int)eBarrigate.Barrigate_Right] = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(ob_Barrigate, "Button_Bottom");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Barrigate((int)eBarrigate.Barrigate_Bottom); });
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Barrigate[(int)eBarrigate.Barrigate_Bottom] = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(ob_Barrigate, "Button_LeftRight");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Barrigate((int)eBarrigate.Barrigate_LeftRight); });
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Barrigate[(int)eBarrigate.Barrigate_LeftRight] = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(ob_Barrigate, "Button_LeftBottom");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Barrigate((int)eBarrigate.Barrigate_LeftBottom); });
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Barrigate[(int)eBarrigate.Barrigate_LeftBottom] = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(ob_Barrigate, "Button_RightBottom");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Barrigate((int)eBarrigate.Barrigate_RightBottom); });
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Barrigate[(int)eBarrigate.Barrigate_RightBottom] = ob.GetComponent<Text>();

        ob = Helper.FindChildGameObject(ob_Barrigate, "Button_LeftRightBottom");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Barrigate((int)eBarrigate.Barrigate_LeftRightBottom); });
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Barrigate[(int)eBarrigate.Barrigate_LeftRightBottom] = ob.GetComponent<Text>();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// SlotMove
        /// 
        GameObject ob_SlotMove = Helper.FindChildGameObject(gameObject, "SlotMove");

        for (int i = 0; i < GameDefine.ms_nSlotMoveMaxCount; ++i)
        {
            ob = Helper.FindChildGameObject(ob_SlotMove, "Button_Move_0" + (i+1).ToString());
            ob = Helper.FindChildGameObject(ob, "Text");
            m_pText_SlotMoveIndex[i] = ob.GetComponent<Text>();
        }

        EventDelegateManager_ForTool.Instance.OnEventUpdateMap += OnUpdateMap;
    }

	private void OnDestroy()
	{
        EventDelegateManager_ForTool.Instance.OnEventUpdateMap -= OnUpdateMap;
    }

    public void OnButtonClick_SlotFillWay(int nSlotFillWay)
    {
        eSlotFillWay slotFillway = (eSlotFillWay)nSlotFillWay;

        if (Tool_Info.Instance.m_eCurrEditBarrigate != eBarrigate.None)
        {
            m_pText_Barrigate[(int)Tool_Info.Instance.m_eCurrEditBarrigate].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eCurrEditDisturb = eDisturb.None;
        Tool_Info.Instance.m_eCurrEditBarrigate = eBarrigate.None;

        if (Tool_Info.Instance.m_nCurrEditSlotMoveIndex != -1)
        {
            m_pText_SlotMoveIndex[Tool_Info.Instance.m_nCurrEditSlotMoveIndex].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_nCurrEditSlotMoveIndex = -1;

        if (Tool_Info.Instance.m_eCurrEditSlotFillWay != eSlotFillWay.None)
        {
            m_pText_EditSlotFillWay[(int)Tool_Info.Instance.m_eCurrEditSlotFillWay].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eCurrEditSlotFillWay = slotFillway;
        m_pText_EditSlotFillWay[(int)Tool_Info.Instance.m_eCurrEditSlotFillWay].color = new Color(1, 0, 0);

        if (slotFillway != eSlotFillWay.Link_ForTool && slotFillway != eSlotFillWay.InVisibleLink_ForTool)
        {
            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eNone;
        }
        else if (slotFillway == eSlotFillWay.Link_ForTool)
        {
            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eSlotLink;
        }
        else if (slotFillway == eSlotFillWay.InVisibleLink_ForTool)
        {
            Tool_Info.Instance.m_eEditMode = eTool_EditMode.eInVisibleSlotLink;
        }
    }

    public void OnButtonClick_Barrigate(int nBarrigate)
    {
        if (Tool_Info.Instance.m_eCurrEditSlotFillWay != eSlotFillWay.None)
        {
            m_pText_EditSlotFillWay[(int)Tool_Info.Instance.m_eCurrEditSlotFillWay].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eCurrEditDisturb = eDisturb.None;
        Tool_Info.Instance.m_eCurrEditSlotFillWay = eSlotFillWay.None;

        if (Tool_Info.Instance.m_nCurrEditSlotMoveIndex != -1)
        {
            m_pText_SlotMoveIndex[Tool_Info.Instance.m_nCurrEditSlotMoveIndex].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_nCurrEditSlotMoveIndex = -1;

        if (Tool_Info.Instance.m_eCurrEditBarrigate != eBarrigate.None)
        {
            m_pText_Barrigate[(int)Tool_Info.Instance.m_eCurrEditBarrigate].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eCurrEditBarrigate = (eBarrigate)nBarrigate;
        m_pText_Barrigate[(int)Tool_Info.Instance.m_eCurrEditBarrigate].color = new Color(1, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eBarrigate;
    }

    public void OnButtonClick_Slot(int nSlotIndex)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            MapDataSlotInfo pMapDataSlotInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotInfoTable[nSlotIndex];

            if (pMapDataSlotInfo != null &&
                Tool_Info.Instance.m_eCurrEditSlotFillWay != eSlotFillWay.None &&
                Tool_Info.Instance.m_eCurrEditSlotFillWay != eSlotFillWay.Link_ForTool)
            {
                if (pMapDataSlotInfo.m_eSlotFillWay != Tool_Info.Instance.m_eCurrEditSlotFillWay)
                {
                    pMapDataSlotInfo.m_eSlotFillWay = Tool_Info.Instance.m_eCurrEditSlotFillWay;
                    Tool_Info.Instance.m_IsEditing = true;

                    if (Tool_Info.Instance.m_eCurrEditSlotFillWay == eSlotFillWay.Close)
                    {
                        // 워프 체크
                        foreach (MapDataSlotLinkInfo pInfo in SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotLinkInfoList)
                        {
                            if (pInfo.m_nSlotIndex_In == nSlotIndex || pInfo.m_nSlotIndex_Out == nSlotIndex)
                            {
                                SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotLinkInfoList.Remove(pInfo);
                                break;
                            }
                        }

                        foreach (MapDataSlotLinkInfo pInfo in SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataInVisibleSlotLinkInfoList)
                        {
                            if (pInfo.m_nSlotIndex_In == nSlotIndex || pInfo.m_nSlotIndex_Out == nSlotIndex)
                            {
                                SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataInVisibleSlotLinkInfoList.Remove(pInfo);
                                break;
                            }
                        }
                    }

                    EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                    EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                }
            }
        }
    }

    public void OnButtonClick_SlotMove(int nIndex)
    {
        if (Tool_Info.Instance.m_eCurrEditSlotFillWay != eSlotFillWay.None)
        {
            m_pText_EditSlotFillWay[(int)Tool_Info.Instance.m_eCurrEditSlotFillWay].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eCurrEditDisturb = eDisturb.None;
        Tool_Info.Instance.m_eCurrEditSlotFillWay = eSlotFillWay.None;

        if (Tool_Info.Instance.m_eCurrEditBarrigate != eBarrigate.None)
        {
            m_pText_Barrigate[(int)Tool_Info.Instance.m_eCurrEditBarrigate].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eCurrEditBarrigate = eBarrigate.None;

        if (Tool_Info.Instance.m_nCurrEditSlotMoveIndex != -1)
        {
            m_pText_SlotMoveIndex[Tool_Info.Instance.m_nCurrEditSlotMoveIndex].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_nCurrEditSlotMoveIndex = nIndex;
        m_pText_SlotMoveIndex[Tool_Info.Instance.m_nCurrEditSlotMoveIndex].color = new Color(1, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eSlotMove;
    }

    public void OnButtonClick_SlotMove_Delete(int nIndex)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable.ContainsKey(nIndex) == true)
            {
                SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable.Remove(nIndex);
                
                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
            }
        }
    }

    private void RemoveChildAll(GameObject go, GameObject pExceptionGameObject)
    {
        int nCount = go.transform.childCount;

        for (int i = nCount - 1; i >= 0; --i)
        {
            Transform tf = go.transform.GetChild(i);

            if(tf.gameObject != pExceptionGameObject)
                GameObject.Destroy(tf.gameObject);
        }
    }

    private void OnUpdateMap(bool IsChangeMap)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            RemoveChildAll(m_pGameObject_Slots, m_pGameObject_BaseSlot);

            Vector3 vPos = m_pGameObject_BaseSlot.transform.localPosition;
            Vector3 vPlusPos = Vector3.zero;

            for (int y = 1; y < GameDefine.ms_nInGameSlot_Y - 1; ++y)
            {
                vPlusPos.x = 0;
                for (int x = 1; x < GameDefine.ms_nInGameSlot_X - 1; ++x)
                {
                    int nSlotIndex = Helper.GetSlotIndex(x, y);
                    GameObject ob = GameObject.Instantiate(m_pGameObject_OringBaseSlot) as GameObject;
                    ob.transform.SetParent(m_pGameObject_Slots.transform);
                    ob.transform.localPosition = vPos + vPlusPos;
                    ob.transform.localScale = Vector3.one;

                    MapDataSlotInfo pMapDataSlotInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotInfoTable[nSlotIndex];

                    Image pImage = ob.GetComponent<Image>();
                    pImage.color = m_clrSlot[(int)pMapDataSlotInfo.m_eSlotFillWay];

                    Button pButton = ob.GetComponent<Button>();
                    pButton.onClick.AddListener(delegate { OnButtonClick_Slot(nSlotIndex); });

                    vPlusPos.x += m_fSlotSize;
                }

                vPlusPos.y -= m_fSlotSize;
            }
        }
    }
}

#endif