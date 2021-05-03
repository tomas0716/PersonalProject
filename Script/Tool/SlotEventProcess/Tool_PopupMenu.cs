using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

#if UNITY_EDITOR

public class Tool_PopupMenu : MonoBehaviour
{
    private Slot        m_pSlot                 = null;
    private Vector3     m_vPos                  = Vector3.zero;
    private bool        m_IsClose               = false;

    private GameObject  m_pGameObject_Root      = null;
    private GameObject  m_pGameObject_Button    = null;

    enum eMenu
    {
        eBarrigateDelete    = 0,
        eWarpDelete         = 1,
        eMission_BellDelete = 2,
    }


    void Start()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo == null)
            return;

        MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

        m_pGameObject_Root = Helper.FindChildGameObject(gameObject, "Root");
        m_pGameObject_Button = Helper.FindChildGameObject(gameObject, "Button");

        m_pGameObject_Root.transform.localPosition = m_vPos;

        List<GameObject> MenuList = new List<GameObject>();
        GameObject ob;
        Button pButton;
        Text pText;

        for (int i = (int)eMissionType.None; i < (int)eMissionType.Max; ++i)
        {
            // Mission
            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.IsMissionSlot(m_pSlot.GetSlotIndex(), (eMissionType)i) == true)
            {
                eMissionType eType = (eMissionType)i;
                ob = GameObject.Instantiate(m_pGameObject_Button);
                MenuList.Add(ob);
                ob.transform.SetParent(m_pGameObject_Root.transform);
                ob.transform.localScale = Vector3.one;
                pButton = ob.GetComponent<Button>();
                pButton.onClick.AddListener(delegate { OnButtonClick_MissionDeleteMenu((int)eType); });
                ob = Helper.FindChildGameObject(ob, "Text");
                pText = ob.GetComponent<Text>();
                pText.text = eType.ToString() + " Delete";
            }
        }

        if (m_pSlot.IsExistSlotDisturb() == true &&
            m_pSlot.IsExistSlotDisturb(eDisturb.Dish) == false)
        {
            Dictionary<eDisturb, Disturb_Base> slotDisturbTable = m_pSlot.GetSlotDisturbTable();
            foreach (KeyValuePair<eDisturb, Disturb_Base> item in slotDisturbTable.Reverse())
            {
                if (!((item.Key == eDisturb.Frozen_02 || item.Key == eDisturb.Frozen_03 || item.Key == eDisturb.Frozen_04) &&
                    SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.IsMissionSlot(m_pSlot.GetSlotIndex(), eMissionType.Mouse) == true))
                {
                    ob = GameObject.Instantiate(m_pGameObject_Button);
                    MenuList.Add(ob);
                    ob.transform.SetParent(m_pGameObject_Root.transform);
                    ob.transform.localScale = Vector3.one;
                    pButton = ob.GetComponent<Button>();
                    pButton.onClick.AddListener(delegate { OnButtonClick_MissionAtDisturbDeleteMenu((int)item.Key); });
                    ob = Helper.FindChildGameObject(ob, "Text");
                    pText = ob.GetComponent<Text>();
                    pText.text = item.Key.ToString() + " Delete";

                    ob = GameObject.Instantiate(m_pGameObject_Button);
                    MenuList.Add(ob);
                    ob.transform.SetParent(m_pGameObject_Root.transform);
                    ob.transform.localScale = Vector3.one;
                    pButton = ob.GetComponent<Button>();
                    pButton.onClick.AddListener(delegate { OnButtonClick_MissionAtDisturbOneLayerDeleteMenu((int)item.Key); });
                    ob = Helper.FindChildGameObject(ob, "Text");
                    pText = ob.GetComponent<Text>();
                    pText.text = item.Key.ToString() + " Layer Delete";
                }

                break;
            }
        }

        if (m_pSlot.GetSlotUnit() != null && (SlotManager.isCombinationAtPossibleChangeSpecialUnit(m_pSlot) == false || m_pSlot.GetSlotUnit().GetUnitShape() == eUnitShape.TimeBomb))
        {
            if (pMapDataMissionBaseInfo.m_SlotUnitTypeTable.ContainsKey(m_pSlot.GetSlotIndex()) == true)
            {
                ob = GameObject.Instantiate(m_pGameObject_Button);
                MenuList.Add(ob);
                ob.transform.SetParent(m_pGameObject_Root.transform);
                ob.transform.localScale = Vector3.one;
                pButton = ob.GetComponent<Button>();
                pButton.onClick.AddListener(delegate { OnButtonClick_MissionAtUnitTypeDeleteMenu((int)pMapDataMissionBaseInfo.m_SlotUnitTypeTable[m_pSlot.GetSlotIndex()]); });
                ob = Helper.FindChildGameObject(ob, "Text");
                pText = ob.GetComponent<Text>();
                pText.text = m_pSlot.GetSlotUnit().GetUnitType().ToString() + " Delete";
            }

            switch (m_pSlot.GetSlotUnit().GetUnitShape())
            {
                case eUnitShape.Horizontal:
                case eUnitShape.Vertical:
                case eUnitShape.Cross:
                case eUnitShape.Square:
                case eUnitShape.TimeBomb:
                    {
                        if (pMapDataMissionBaseInfo.m_SlotUnitShapeTable.ContainsKey(m_pSlot.GetSlotIndex()) == true)
                        {
                            ob = GameObject.Instantiate(m_pGameObject_Button);
                            MenuList.Add(ob);
                            ob.transform.SetParent(m_pGameObject_Root.transform);
                            ob.transform.localScale = Vector3.one;
                            pButton = ob.GetComponent<Button>();
                            pButton.onClick.AddListener(delegate { OnButtonClick_MissionAtUnitShapeDeleteMenu((int)pMapDataMissionBaseInfo.m_SlotUnitShapeTable[m_pSlot.GetSlotIndex()]); });
                            ob = Helper.FindChildGameObject(ob, "Text");
                            pText = ob.GetComponent<Text>();
                            pText.text = m_pSlot.GetSlotUnit().GetUnitShape().ToString() + " Delete";
                        }
                    }
                    break;

            }
        }

        // Item Machine
        if (m_pSlot.GetNumUnitMachine() != 0)
        {
            ob = GameObject.Instantiate(m_pGameObject_Button);
            MenuList.Add(ob);
            ob.transform.SetParent(m_pGameObject_Root.transform);
            ob.transform.localScale = Vector3.one;
            pButton = ob.GetComponent<Button>();
            pButton.onClick.AddListener(delegate { OnButtonClick_UnitMachineDelete(); });
            ob = Helper.FindChildGameObject(ob, "Text");
            pText = ob.GetComponent<Text>();
            pText.text = "Item Machine Delete";
        }

        if (m_pSlot.IsBellGoal() == true)
        {
            ob = GameObject.Instantiate(m_pGameObject_Button);
            MenuList.Add(ob);
            ob.transform.SetParent(m_pGameObject_Root.transform);
            ob.transform.localScale = Vector3.one;
            pButton = ob.GetComponent<Button>();
            pButton.onClick.AddListener(delegate { OnButtonClick_BellGoalDelete(); });
            ob = Helper.FindChildGameObject(ob, "Text");
            pText = ob.GetComponent<Text>();
            pText.text = "Bell Goal Delete";
        }

        // Disturb_Dish
        if (m_pSlot.GetDisturb_DishRoad() == eDisturb_DishRoad.Ender)
        {
            ob = GameObject.Instantiate(m_pGameObject_Button);
            MenuList.Add(ob);
            ob.transform.SetParent(m_pGameObject_Root.transform);
            ob.transform.localScale = Vector3.one;
            pButton = ob.GetComponent<Button>();
            pButton.onClick.AddListener(delegate { OnButtonClick_Disturb_DishDelete(); });
            ob = Helper.FindChildGameObject(ob, "Text");
            pText = ob.GetComponent<Text>();
            pText.text = "Dish Delete";
        }

        // Barrigate 
        if (m_pSlot.IsExistSlotBarrigate_Mine() == true)
        {
            ob = GameObject.Instantiate(m_pGameObject_Button);
            MenuList.Add(ob);
            ob.transform.SetParent(m_pGameObject_Root.transform);
            ob.transform.localScale = Vector3.one;
            pButton = ob.GetComponent<Button>();
            pButton.onClick.AddListener(delegate { OnButtonClick_Menu((int)eMenu.eBarrigateDelete); });
            ob = Helper.FindChildGameObject(ob, "Text");
            pText = ob.GetComponent<Text>();
            pText.text = "Barrigate Delete";
        }

        // LinkSlot
        if (m_pSlot.IsSlotLink() == true)
        {
            ob = GameObject.Instantiate(m_pGameObject_Button);
            MenuList.Add(ob);
            ob.transform.SetParent(m_pGameObject_Root.transform);
            ob.transform.localScale = Vector3.one;
            pButton = ob.GetComponent<Button>();
            pButton.onClick.AddListener(delegate { OnButtonClick_Menu((int)eMenu.eWarpDelete); });
            ob = Helper.FindChildGameObject(ob, "Text");
            pText = ob.GetComponent<Text>();
            pText.text = "Warp Delete";
        }

        // SlotMove
        if (m_pSlot.GetSlotMoveRoad() == eSlotMoveRoad.Ender)
        {
            ob = GameObject.Instantiate(m_pGameObject_Button);
            MenuList.Add(ob);
            ob.transform.SetParent(m_pGameObject_Root.transform);
            ob.transform.localScale = Vector3.one;
            pButton = ob.GetComponent<Button>();
            pButton.onClick.AddListener(delegate { OnButtonClick_SlotMoveDelete(); });
            ob = Helper.FindChildGameObject(ob, "Text");
            pText = ob.GetComponent<Text>();
            pText.text = "Slot Move Delete";
        }

        Vector3 vPos = new Vector3(135, -25,0f);
        foreach (GameObject pMenu in MenuList)
        {
            pMenu.transform.localPosition = vPos;
            vPos.y -= 40;
        }

        m_pGameObject_Button.SetActive(false);
    }

    void Update()
    {
        if (m_IsClose == true)
        {
            GameObject.Destroy(gameObject);
        }

        if (Input.GetMouseButtonUp(0) == true || Input.GetMouseButtonUp(1) == true)
        {
            m_IsClose = true;
        }
    }

    public void Show(Slot pSlot, Vector3 vPos)
    {
        m_pSlot = pSlot;
        m_vPos = vPos;
    }

    public void Hide()
    {
        GameObject.Destroy(gameObject);
    }

	public void OnButtonClick_Menu(int nMenu)
	{
        eMenu eMenu = (eMenu)nMenu;

        switch (eMenu)
        {
            case eMenu.eBarrigateDelete:
                {
                    foreach (MapDataSlotBarrigateInfo pInfo in SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotBarrigateInfoList)
                    {
                        if (pInfo.m_nSlotIndex == m_pSlot.GetSlotIndex())
                        {
                            SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotBarrigateInfoList.Remove(pInfo);
                            m_pSlot.RemoveBarrigateAll();
                            return;
                        }
                    }
                }
                break;

            case eMenu.eWarpDelete:
                {
					if (m_pSlot.IsSlotLink() == true)
					{
						bool IsReflesh = false;
                        for (int i = SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotLinkInfoList.Count - 1; i >= 0; --i)
                        {
                            MapDataSlotLinkInfo pInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotLinkInfoList[i];

                            if (pInfo.m_nSlotIndex_In == m_pSlot.GetSlotIndex() || pInfo.m_nSlotIndex_Out == m_pSlot.GetSlotIndex())
                            {
                                SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotLinkInfoList.Remove(pInfo);
                                IsReflesh = true;
                            }
                        }

                        for (int i = SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataInVisibleSlotLinkInfoList.Count - 1; i >= 0; --i)
                        {
                            MapDataSlotLinkInfo pInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataInVisibleSlotLinkInfoList[i];

                            if (pInfo.m_nSlotIndex_In == m_pSlot.GetSlotIndex() || pInfo.m_nSlotIndex_Out == m_pSlot.GetSlotIndex())
							{
								SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataInVisibleSlotLinkInfoList.Remove(pInfo);
								IsReflesh = true;
							}
						}

                        if (IsReflesh == true)
                        {
                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
				    }
                }
                break;
        }

        GameObject.Destroy(gameObject);
    }

    // 미션 추가시 추가
    public void OnButtonClick_MissionDeleteMenu(int nMissionType)
    {
        eMissionType eType = (eMissionType)nMissionType;

        switch (eType)
        {
            case eMissionType.Bell:
                {
                    if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                    {
                        MapDataMissionInfo_Bell pMapDataMissionInfo_Bell = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Bell;

                        if (pMapDataMissionInfo_Bell != null)
                        {
                            pMapDataMissionInfo_Bell.m_AppearBellSlotIndexList.Remove(m_pSlot.GetSlotIndex());

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                    }
                }
                break;
            case eMissionType.Mouse:
                {
                    if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                    {
                        MapDataMissionInfo_Mouse pMapDataMissionInfo_Mouse = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Mouse;

                        if (pMapDataMissionInfo_Mouse != null)
                        {
                            pMapDataMissionInfo_Mouse.m_AppearDisturbSlotIndexTable.Remove(m_pSlot.GetSlotIndex());

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                    }
                }
                break;
            case eMissionType.Apple:
                {
                    if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                    {
                        MapDataMissionInfo_Apple pMapDataMissionInfo_Apple = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Apple;

                        if (pMapDataMissionInfo_Apple != null)
                        {
                            pMapDataMissionInfo_Apple.m_AppearAppleTable.Remove(m_pSlot.GetSlotIndex());

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                    }
                }
                break;
            case eMissionType.Rock:
                {
                    if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                    {
                        MapDataMissionInfo_Rock pMapDataMissionInfo_Rock = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Rock;

                        if (pMapDataMissionInfo_Rock != null)
                        {
                            pMapDataMissionInfo_Rock.m_AppearRockTable.Remove(m_pSlot.GetSlotIndex());

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                    }
                }
                break;
            case eMissionType.Bread:
                {
                    if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                    {
                        MapDataMissionInfo_Bread pMapDataMissionInfo_Bread = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Bread;

                        if (pMapDataMissionInfo_Bread != null)
                        {
                            pMapDataMissionInfo_Bread.m_AppearBreadLeftTopSlotIndexList.Remove(m_pSlot.GetSlotIndex());

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                    }
                }
                break;
            case eMissionType.Jelly:
                {
                    if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                    {
                        MapDataMissionInfo_Jelly pMapDataMissionInfo_Jelly = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Jelly;

                        if (pMapDataMissionInfo_Jelly != null)
                        {
                            pMapDataMissionInfo_Jelly.m_AppearJellyLeftTopTable.Remove(m_pSlot.GetSlotIndex());

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                    }
                }
                break;
            case eMissionType.Fish:
                {
                    if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                    {
                        MapDataMissionInfo_Fish pMapDataMissionInfo_Fish = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Fish;

                        if (pMapDataMissionInfo_Fish != null)
                        {
                            pMapDataMissionInfo_Fish.m_AppearFishSlotIndexList.Remove(m_pSlot.GetSlotIndex());

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                    }
                }
                break;
            case eMissionType.Number:
                {
                    if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                    {
                        MapDataMissionInfo_Number pMapDataMissionInfo_Number = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Number;

                        if (pMapDataMissionInfo_Number != null)
                        {
                            int nNumber = m_pSlot.GetNumberMission_Number();

                            if (nNumber != -1)
                            {
                                pMapDataMissionInfo_Number.m_AppearNumberTable.Remove(nNumber);

                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);

                            }
					    }
                    }
                }
                break;
            case eMissionType.Octopus:
                {
                    if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                    {
                        MapDataMissionInfo_Octopus pMapDataMissionInfo_Octopus = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Octopus;

                        if (pMapDataMissionInfo_Octopus != null)
                        {
                            pMapDataMissionInfo_Octopus.m_AppearOctopusSlotIndexList.Remove(m_pSlot.GetSlotIndex());

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                    }
                }
                break;
            case eMissionType.Knit:
                {
                    if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                    {
                        MapDataMissionInfo_Knit pMapDataMissionInfo_Knit = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Knit;

                        if (pMapDataMissionInfo_Knit != null)
                        {
                            pMapDataMissionInfo_Knit.m_AppearKnitSlotIndexList.Remove(m_pSlot.GetSlotIndex());

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                    }
                }
                break;
            case eMissionType.Can:
                {
                    if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                    {
                        MapDataMissionInfo_Can pMapDataMissionInfo_Can = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Can;

                        if (pMapDataMissionInfo_Can != null)
                        {
                            pMapDataMissionInfo_Can.m_AppearCanSlotIndexList.Remove(m_pSlot.GetSlotIndex());

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                    }
                }
                break;
            case eMissionType.Butterfly:
                {
                    if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                    {
                        MapDataMissionInfo_Butterfly pMapDataMissionInfo_Butterfly = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Butterfly;

                        if (pMapDataMissionInfo_Butterfly != null)
                        {
                            pMapDataMissionInfo_Butterfly.m_AppearButterflySlotIndexList.Remove(m_pSlot.GetSlotIndex());

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                    }
                }
                break;
        }

        GameObject.Destroy(gameObject);
    }

    public void OnButtonClick_MissionAtDisturbOneLayerDeleteMenu(int nDist)
    {
        MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

        eDisturb eDisturb = (eDisturb)nDist;

        if (pMapDataMissionBaseInfo.m_DisturbTable.ContainsKey(m_pSlot.GetSlotIndex()) == true)
        {
            if (eDisturb == pMapDataMissionBaseInfo.m_DisturbTable[m_pSlot.GetSlotIndex()])
            {
                switch (eDisturb)
                {
                    case eDisturb.Fixing_01:    pMapDataMissionBaseInfo.m_DisturbTable.Remove(m_pSlot.GetSlotIndex());                  break;
                    case eDisturb.Fixing_02:    pMapDataMissionBaseInfo.m_DisturbTable[m_pSlot.GetSlotIndex()] = eDisturb.Fixing_01;    break;
                    case eDisturb.Frozen_01:    pMapDataMissionBaseInfo.m_DisturbTable.Remove(m_pSlot.GetSlotIndex());                  break;
                    case eDisturb.Frozen_02:    pMapDataMissionBaseInfo.m_DisturbTable[m_pSlot.GetSlotIndex()] = eDisturb.Frozen_01;    break;
                    case eDisturb.Frozen_03:    pMapDataMissionBaseInfo.m_DisturbTable[m_pSlot.GetSlotIndex()] = eDisturb.Frozen_02;    break;
                    case eDisturb.Frozen_04:    pMapDataMissionBaseInfo.m_DisturbTable[m_pSlot.GetSlotIndex()] = eDisturb.Frozen_03;    break;
                    case eDisturb.Box_01:       pMapDataMissionBaseInfo.m_DisturbTable.Remove(m_pSlot.GetSlotIndex());                  break;
                    case eDisturb.Box_02:       pMapDataMissionBaseInfo.m_DisturbTable[m_pSlot.GetSlotIndex()] = eDisturb.Box_01;       break;
                    case eDisturb.Box_03:       pMapDataMissionBaseInfo.m_DisturbTable[m_pSlot.GetSlotIndex()] = eDisturb.Box_02;       break;
                    case eDisturb.Box_04:       pMapDataMissionBaseInfo.m_DisturbTable[m_pSlot.GetSlotIndex()] = eDisturb.Box_03;       break;
                    case eDisturb.Box_05:       pMapDataMissionBaseInfo.m_DisturbTable[m_pSlot.GetSlotIndex()] = eDisturb.Box_04;       break;
                    case eDisturb.Hide_Show:    pMapDataMissionBaseInfo.m_DisturbTable.Remove(m_pSlot.GetSlotIndex());                  break;
                }

                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
            }
        }

        GameObject.Destroy(gameObject);
    }

    public void OnButtonClick_MissionAtDisturbDeleteMenu(int nDist)
    {
        MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

        eDisturb eDisturb = (eDisturb)nDist;

        if (pMapDataMissionBaseInfo.m_DisturbTable.ContainsKey(m_pSlot.GetSlotIndex()) == true)
        {
            if (eDisturb == pMapDataMissionBaseInfo.m_DisturbTable[m_pSlot.GetSlotIndex()])
            {
                pMapDataMissionBaseInfo.m_DisturbTable.Remove(m_pSlot.GetSlotIndex());
                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
            }
        }

        GameObject.Destroy(gameObject);
    }

    public void OnButtonClick_MissionAtUnitTypeDeleteMenu(int nUnitType)
    {
        MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

        eUnitType eType = (eUnitType)nUnitType;

        if (pMapDataMissionBaseInfo.m_SlotUnitTypeTable.ContainsKey(m_pSlot.GetSlotIndex()) == true)
        {
            if (eType == pMapDataMissionBaseInfo.m_SlotUnitTypeTable[m_pSlot.GetSlotIndex()])
            {
                pMapDataMissionBaseInfo.m_SlotUnitTypeTable.Remove(m_pSlot.GetSlotIndex());

                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
            }
        }

        GameObject.Destroy(gameObject);
    }

    public void OnButtonClick_MissionAtUnitShapeDeleteMenu(int nUnitShape)
    {
        MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

        eUnitShape eShape = (eUnitShape)nUnitShape;

        if (pMapDataMissionBaseInfo.m_SlotUnitShapeTable.ContainsKey(m_pSlot.GetSlotIndex()) == true)
        {
            if (eShape == pMapDataMissionBaseInfo.m_SlotUnitShapeTable[m_pSlot.GetSlotIndex()])
            {
                pMapDataMissionBaseInfo.m_SlotUnitShapeTable.Remove(m_pSlot.GetSlotIndex());

                if (eShape == eUnitShape.TimeBomb)
                {
                    pMapDataMissionBaseInfo.m_SlotUnitTimeBomb_NumberTable.Remove(m_pSlot.GetSlotIndex());
                }

                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
            }
        }

        GameObject.Destroy(gameObject);
    }

    public void OnButtonClick_UnitMachineDelete()
    {
        MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

        pMapDataMissionBaseInfo.m_UnitMachineTable.Remove(m_pSlot.GetSlotIndex());

        if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eMissionType.Bell) == true)
        {
            MapDataMissionInfo_Bell pMapDataMissionInfo_Bell = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eMissionType.Bell] as MapDataMissionInfo_Bell;

            if (pMapDataMissionInfo_Bell != null && pMapDataMissionInfo_Bell.m_HideBellCreateSlotIndexList.Contains(m_pSlot.GetSlotIndex()) == true)
            {
                pMapDataMissionInfo_Bell.m_HideBellCreateSlotIndexList.Remove(m_pSlot.GetSlotIndex());
            }
        }

        if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eMissionType.Fish) == true)
        {
            MapDataMissionInfo_Fish pMapDataMissionInfo_Fish = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eMissionType.Fish] as MapDataMissionInfo_Fish;

            if (pMapDataMissionInfo_Fish != null && pMapDataMissionInfo_Fish.m_HideFishCreateSlotIndexList.Contains(m_pSlot.GetSlotIndex()) == true)
            {
                pMapDataMissionInfo_Fish.m_HideFishCreateSlotIndexList.Remove(m_pSlot.GetSlotIndex());
            }
        }

        EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
        EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);

        GameObject.Destroy(gameObject);
    }

    public void OnButtonClick_BellGoalDelete()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eMissionType.Bell) == true)
        {
            MapDataMissionInfo_Bell pMapDataMissionInfo_Bell = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eMissionType.Bell] as MapDataMissionInfo_Bell;

            if (pMapDataMissionInfo_Bell != null && pMapDataMissionInfo_Bell.m_BellGoalSlotIndexList.Contains(m_pSlot.GetSlotIndex()) == true)
            {
                pMapDataMissionInfo_Bell.m_BellGoalSlotIndexList.Remove(m_pSlot.GetSlotIndex());

                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
            }
        }

        GameObject.Destroy(gameObject);
    }

    public void OnButtonClick_Disturb_DishDelete()
    {
        MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

        int nIndex = m_pSlot.GetDisturb_DishIndex();
        if (pMapDataMissionBaseInfo.m_DishGroupTable.ContainsKey(nIndex) == true)
        {
            pMapDataMissionBaseInfo.m_DishGroupTable[nIndex].Remove(m_pSlot.GetSlotIndex());

            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
        }

        GameObject.Destroy(gameObject);
    }

    public void OnButtonClick_SlotMoveDelete()
    {
        int nSlotMoveIndex = m_pSlot.GetSlotMoveIndex();

        if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable.ContainsKey(nSlotMoveIndex) == true)
        {
            foreach (KeyValuePair<int, List<int>> item in SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable)
            {
                for (int i = item.Value.Count - 1; i >= 0; --i)
                {
                    int nSlotIndex = item.Value[i];
                    if (nSlotIndex == m_pSlot.GetSlotIndex())
                    {
                        item.Value.RemoveAt(i);
						EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
						EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        return;
					}
                }
            }
        }

        GameObject.Destroy(gameObject);
    }
}

#endif