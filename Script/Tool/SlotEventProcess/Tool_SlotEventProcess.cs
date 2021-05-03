using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR

public class Tool_SlotEventProcess : MonoBehaviour
{
    private bool m_WasCtrlKeyDown = false;

    void Start()
    {
        EventDelegateManager_ForTool.Instance.OnEventPostUpdateMap += OnPostUpdateMap;
    }

    private void OnDestroy()
    {
        EventDelegateManager_ForTool.Instance.OnEventPostUpdateMap -= OnPostUpdateMap;
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.LeftControl) == true)
        {
            m_WasCtrlKeyDown = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) == true)
        {
            m_WasCtrlKeyDown = false;
        }
    }

	private void OnPostUpdateMap(bool IsChangeMap)
    {
        Scene_Tool pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_Tool;

        if (pScene != null)
        {
            Dictionary<int, Slot>  pSlotTable = pScene.m_pGameController.m_pSlotManager.GetSlotTable();

            foreach (KeyValuePair<int, Slot> item in pSlotTable)
            {
                Plane2D pPlane = item.Value.GetPlane_Slot();
                pPlane.SetLayerName("Slot");
                pPlane.AddCallback_LButtonDown(OnCallback_LButtonDown);
                pPlane.AddCallback_LButtonUp(OnCallback_LButtonUp);
                pPlane.AddCallback_RButtonDown(OnCallback_RButtonDown);
                pPlane.AddCallback_RButtonUp(OnCallback_RButtonUp);
                pPlane.GetPickingComponent().SetParameta(item.Key);
            }
        }
    }

    public void OnCallback_LButtonDown(GameObject gameObject, Vector3 vPos, object ob, int nFingerID)
    {
        int nSlotIndex = (int)ob;
    }

    public void OnCallback_LButtonUp(GameObject gameObject, Vector3 vPos, object ob, int nFingerID, bool IsDown)
    {
        if(EventSystem.current.IsPointerOverGameObject() == true)
            return;

        if(SavedGameDataInfo.Instance.m_pMapDataInfo == null)
            return;

        int nSlotIndex = (int)ob;
        Slot pSlot = null;
        Scene_Tool pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_Tool;

        if (pScene != null && m_WasCtrlKeyDown == true)
        {
            pSlot = pScene.m_pGameController.m_pSlotManager.GetSlot_bySlotIndex(nSlotIndex);

            if (pSlot != null)
            {
                switch (Tool_Info.Instance.m_eEditMode)
                {
                    case eTool_EditMode.eSlotLink:
                        {
                            if (Tool_Info.Instance.m_pSlotLink_In == null)
                            {
                                if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotLinkInfoList.Count < GameDefine.ms_nLinkSlotMax)
                                {
                                    int nIndex = SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotLinkInfoList.Count;
                                    Tool_Info.Instance.m_pSlotLink_In = pSlot;
                                    pSlot.SetSlotLinkType_In(eSlotLinkType.In, nIndex);
                                }
                            }
                            else if (Tool_Info.Instance.m_pSlotLink_In != null)
                            {
                                int nIndex = SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotLinkInfoList.Count;
                                pSlot.SetSlotLink(Tool_Info.Instance.m_pSlotLink_In);
                                pSlot.SetSlotLinkType_Out(eSlotLinkType.Out, nIndex);

                                MapDataSlotLinkInfo pMapDataSlotLinkInfo = new MapDataSlotLinkInfo();
                                pMapDataSlotLinkInfo.m_nSlotIndex_In = Tool_Info.Instance.m_pSlotLink_In.GetSlotIndex();
                                pMapDataSlotLinkInfo.m_nSlotIndex_Out = pSlot.GetSlotIndex();
                                SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotLinkInfoList.Add(pMapDataSlotLinkInfo);

                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                            }
                        }
                        break;

                    case eTool_EditMode.eInVisibleSlotLink:
                        {
                            if (Tool_Info.Instance.m_pSlotLink_In == null)
                            {
                                if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataInVisibleSlotLinkInfoList.Count < GameDefine.ms_nLinkSlotMax_InVisible)
                                {
                                    int nIndex = SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataInVisibleSlotLinkInfoList.Count;
                                    Tool_Info.Instance.m_pSlotLink_In = pSlot;
                                    pSlot.SetSlotLinkType_In_InVisible(eSlotLinkType.In, nIndex);
                                }
                            }
                            else if (Tool_Info.Instance.m_pSlotLink_In != null)
                            {
                                int nIndex = SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataInVisibleSlotLinkInfoList.Count;
                                pSlot.SetSlotLink(Tool_Info.Instance.m_pSlotLink_In);
                                pSlot.SetSlotLinkType_Out_InVisible(eSlotLinkType.Out, nIndex);

                                MapDataSlotLinkInfo pMapDataSlotLinkInfo = new MapDataSlotLinkInfo();
                                pMapDataSlotLinkInfo.m_nSlotIndex_In = Tool_Info.Instance.m_pSlotLink_In.GetSlotIndex();
                                pMapDataSlotLinkInfo.m_nSlotIndex_Out = pSlot.GetSlotIndex();
                                SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataInVisibleSlotLinkInfoList.Add(pMapDataSlotLinkInfo);

                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                            }
                        }
                        break;

                    case eTool_EditMode.eBarrigate:
                        {
                            pSlot.ChangeSlotBarrigate(SlotManager.GetIncludeBarrigate(Tool_Info.Instance.m_eCurrEditBarrigate), true);

                            SavedGameDataInfo.Instance.m_pMapDataInfo.m_MapDataSlotBarrigateInfoList.Add(new MapDataSlotBarrigateInfo() { m_nSlotIndex = pSlot.GetSlotIndex(), m_eBarrigate = Tool_Info.Instance.m_eCurrEditBarrigate });
                        }
                        break;

                    case eTool_EditMode.eSlotMove:
						{
							int nIndex = Tool_Info.Instance.m_nCurrEditSlotMoveIndex;
                            if (nIndex != -1)
                            {
                                //if(pSlot.GetSlotMoveIndex() != -1)
                                //    return;

                                if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable.ContainsKey(nIndex) == true)
                                {
                                    int nLastIndex = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable[nIndex].Count - 1;

                                    if (nLastIndex != 0 && 
                                        SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable[nIndex][0] == SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable[nIndex][nLastIndex])
                                        return;
                                }

                                if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable.ContainsKey(nIndex) == true)
                                {
                                    int nCount = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable[nIndex].Count;

                                    if (nCount == 0)
                                    {
                                        SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.AddSlotMove(nIndex, pSlot.GetSlotIndex());
                                    }
                                    else
                                    {
                                        if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable[nIndex].Contains(pSlot.GetSlotIndex()) == false)
                                        {
                                            int nLastSlotIndex = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable[nIndex][nCount - 1];
                                            Slot pSlot_Last = pScene.m_pGameController.m_pSlotManager.GetSlot_bySlotIndex(nLastSlotIndex);
                                            if (pSlot_Last != null)
                                            {
                                                bool IsNeighbor = false;
                                                Slot pSlot_Neighbor;

                                                pSlot_Neighbor = pSlot_Last.GetNeighborSlot(eNeighbor.eNeighbor_10);
                                                if (pSlot_Neighbor == pSlot) IsNeighbor = true;
                                                pSlot_Neighbor = pSlot_Last.GetNeighborSlot(eNeighbor.eNeighbor_01);
                                                if (pSlot_Neighbor == pSlot) IsNeighbor = true;
                                                pSlot_Neighbor = pSlot_Last.GetNeighborSlot(eNeighbor.eNeighbor_21);
                                                if (pSlot_Neighbor == pSlot) IsNeighbor = true;
                                                pSlot_Neighbor = pSlot_Last.GetNeighborSlot(eNeighbor.eNeighbor_12);
                                                if (pSlot_Neighbor == pSlot) IsNeighbor = true;

                                                if (IsNeighbor == true)
                                                {
                                                    SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.AddSlotMove(nIndex, pSlot.GetSlotIndex());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable[nIndex][0] == pSlot.GetSlotIndex())
                                            {
                                                int nLastSlotIndex = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable[nIndex][nCount - 1];
                                                Slot pSlot_Last = pScene.m_pGameController.m_pSlotManager.GetSlot_bySlotIndex(nLastSlotIndex);
                                                if (pSlot_Last != null)
                                                {
                                                    bool IsNeighbor = false;
                                                    Slot pSlot_Neighbor;

                                                    pSlot_Neighbor = pSlot_Last.GetNeighborSlot(eNeighbor.eNeighbor_10);
                                                    if (pSlot_Neighbor == pSlot) IsNeighbor = true;
                                                    pSlot_Neighbor = pSlot_Last.GetNeighborSlot(eNeighbor.eNeighbor_01);
                                                    if (pSlot_Neighbor == pSlot) IsNeighbor = true;
                                                    pSlot_Neighbor = pSlot_Last.GetNeighborSlot(eNeighbor.eNeighbor_21);
                                                    if (pSlot_Neighbor == pSlot) IsNeighbor = true;
                                                    pSlot_Neighbor = pSlot_Last.GetNeighborSlot(eNeighbor.eNeighbor_12);
                                                    if (pSlot_Neighbor == pSlot) IsNeighbor = true;

                                                    if (IsNeighbor == true)
                                                    {
                                                        SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.AddSlotMove(nIndex, pSlot.GetSlotIndex());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
								else if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable.ContainsKey(nIndex) == false)
								{
									SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.AddSlotMove(nIndex, pSlot.GetSlotIndex());
								}

                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                            }
						}
						break;

					// 미션 추가시 추가
					case eTool_EditMode.eMission:
                        {
                            eMissionType eType = Tool_Info.Instance.m_eCurrMissionTabType;

                            if (Tool_Info.Instance.m_eUnitMachine != eUnitMachine.None)
                            {
                                MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

                                pMapDataMissionBaseInfo.AddUnitMachine(pSlot.GetSlotIndex(), Tool_Info.Instance.m_eUnitMachine);

                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                            }
                            else
                            {
                                switch (eType)
                                {
                                    case eMissionType.Bell:
                                        {
                                            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                                            {
                                                MapDataMissionInfo_Bell pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Bell;

                                                if (pMissionInfo != null)
                                                {
                                                    switch (Tool_Info.Instance.m_eBell_EditMode)
                                                    {
                                                        case eTool_Bell_EditMode.eBell_Create:
                                                            {
                                                                if (pMissionInfo.m_AppearBellSlotIndexList.Contains(pSlot.GetSlotIndex()) == false)
                                                                {
                                                                    pMissionInfo.m_AppearBellSlotIndexList.Add(pSlot.GetSlotIndex());

                                                                    EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                                                    EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                                                                }
                                                            }
                                                            break;

                                                        case eTool_Bell_EditMode.eBell_Goal:
                                                            {
                                                                if (pMissionInfo.m_BellGoalSlotIndexList.Contains(pSlot.GetSlotIndex()) == false)
                                                                {
                                                                    pMissionInfo.m_BellGoalSlotIndexList.Add(pSlot.GetSlotIndex());

                                                                    EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                                                    EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                                                                }
                                                            }
                                                            break;

                                                        case eTool_Bell_EditMode.eBell_Machine:
                                                            {
                                                                pMissionInfo.m_HideBellCreateSlotIndexList.Add(pSlot.GetSlotIndex());

                                                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case eMissionType.Mouse:
                                        {
                                            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                                            {
                                                MapDataMissionInfo_Mouse pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Mouse;

                                                if (pMissionInfo != null && Tool_Info.Instance.m_eDisturb_Mouse != eDisturb.None && pMissionInfo.m_AppearDisturbSlotIndexTable.ContainsKey(pSlot.GetSlotIndex()) == false)
                                                {
                                                    pMissionInfo.m_AppearDisturbSlotIndexTable.Add(pSlot.GetSlotIndex(), Tool_Info.Instance.m_eDisturb_Mouse);

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
                                                MapDataMissionInfo_Apple pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Apple;

                                                if (pMissionInfo != null && Tool_Info.Instance.m_nAppleCount != -1 && pMissionInfo.m_AppearAppleTable.ContainsKey(pSlot.GetSlotIndex()) == false)
                                                {
                                                    pMissionInfo.m_AppearAppleTable.Add(pSlot.GetSlotIndex(), Tool_Info.Instance.m_nAppleCount);

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
                                                MapDataMissionInfo_Rock pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Rock;

                                                if (pMissionInfo != null && Tool_Info.Instance.m_nRockCount != -1 && pMissionInfo.m_AppearRockTable.ContainsKey(pSlot.GetSlotIndex()) == false)
                                                {
                                                    pMissionInfo.m_AppearRockTable.Add(pSlot.GetSlotIndex(), Tool_Info.Instance.m_nRockCount);

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
                                                MapDataMissionInfo_Bread pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Bread;

                                                if (pMissionInfo != null && pMissionInfo.m_AppearBreadLeftTopSlotIndexList.Contains(pSlot.GetSlotIndex()) == false)
                                                {
                                                    pMissionInfo.m_AppearBreadLeftTopSlotIndexList.Add(pSlot.GetSlotIndex());

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
                                                MapDataMissionInfo_Jelly pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Jelly;

                                                if (pMissionInfo != null && pMissionInfo.m_AppearJellyLeftTopTable.ContainsKey(pSlot.GetSlotIndex()) == false)
                                                {
                                                    pMissionInfo.m_AppearJellyLeftTopTable.Add(pSlot.GetSlotIndex(), Tool_Info.Instance.m_nJelly_Pattern);

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
                                                MapDataMissionInfo_Fish pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Fish;

                                                if (pMissionInfo != null)
                                                {
                                                    switch (Tool_Info.Instance.m_eFish_EditMode)
                                                    {
                                                        case eTool_Fish_EditMode.eFish_Create:
                                                            {
                                                                if (pMissionInfo.m_AppearFishSlotIndexList.Contains(pSlot.GetSlotIndex()) == false)
                                                                {
                                                                    pMissionInfo.m_AppearFishSlotIndexList.Add(pSlot.GetSlotIndex());

                                                                    EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                                                    EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                                                                }
                                                            }
                                                            break;
                                                        case eTool_Fish_EditMode.eFish_Machine:
                                                            {
                                                                if (pMissionInfo.m_HideFishCreateSlotIndexList.Contains(pSlot.GetSlotIndex()) == false)
                                                                {
                                                                    pMissionInfo.m_HideFishCreateSlotIndexList.Add(pSlot.GetSlotIndex());

                                                                    EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                                                    EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case eMissionType.Number:
                                        {
                                            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                                            {
                                                MapDataMissionInfo_Number pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Number;

                                                if (pMissionInfo != null && Tool_Info.Instance.m_nCurrNumber != -1 && pMissionInfo.m_AppearNumberTable.ContainsValue(pSlot.GetSlotIndex()) == false)
                                                {
                                                    pMissionInfo.m_AppearNumberTable.Add(Tool_Info.Instance.m_nCurrNumber, pSlot.GetSlotIndex());

                                                    EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                                    EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                                                }
                                            }
                                        }
                                        break;
                                    case eMissionType.Octopus:
                                        {
                                            if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable.ContainsKey(eType) == true)
                                            {
                                                MapDataMissionInfo_Octopus pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Octopus;

                                                if (pMissionInfo != null && pMissionInfo.m_AppearOctopusSlotIndexList.Contains(pSlot.GetSlotIndex()) == false)
                                                {
                                                    pMissionInfo.m_AppearOctopusSlotIndexList.Add(pSlot.GetSlotIndex());

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
                                                MapDataMissionInfo_Knit pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Knit;

                                                if (pMissionInfo != null && pMissionInfo.m_AppearKnitSlotIndexList.Contains(pSlot.GetSlotIndex()) == false)
                                                {
                                                    pMissionInfo.m_AppearKnitSlotIndexList.Add(pSlot.GetSlotIndex());

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
                                                MapDataMissionInfo_Can pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Can;

                                                if (pMissionInfo != null && pMissionInfo.m_AppearCanSlotIndexList.Contains(pSlot.GetSlotIndex()) == false)
                                                {
                                                    pMissionInfo.m_AppearCanSlotIndexList.Add(pSlot.GetSlotIndex());

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
                                                MapDataMissionInfo_Butterfly pMissionInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eType] as MapDataMissionInfo_Butterfly;

                                                if (pMissionInfo != null && pMissionInfo.m_AppearButterflySlotIndexList.Contains(pSlot.GetSlotIndex()) == false)
                                                {
                                                    pMissionInfo.m_AppearButterflySlotIndexList.Add(pSlot.GetSlotIndex());

                                                    EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                                    EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case eTool_EditMode.eChangeFixedUnit:
                        {
                            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

                            eUnitType unitType = Tool_Info.Instance.m_eCurrEditFixedUnitType;

                            if (unitType != eUnitType.Random)
                            {
                                if (pMapDataMissionBaseInfo.m_SlotUnitTypeTable.ContainsKey(pSlot.GetSlotIndex()) == false)
                                {
                                    pMapDataMissionBaseInfo.m_SlotUnitTypeTable.Add(pSlot.GetSlotIndex(), unitType);
                                }
                                else
                                {
                                    pMapDataMissionBaseInfo.m_SlotUnitTypeTable[pSlot.GetSlotIndex()] = unitType;
                                }
                            }
                            else
                            {
                                if (pMapDataMissionBaseInfo.m_SlotUnitTypeTable.ContainsKey(pSlot.GetSlotIndex()) == true)
                                {
                                    pMapDataMissionBaseInfo.m_SlotUnitTypeTable.Remove(pSlot.GetSlotIndex());
                                }
                            }

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                        break;
                    case eTool_EditMode.eMissionAtDisturb:
                        {
                            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

                            if (pMapDataMissionBaseInfo.AddDisturb(pSlot.GetSlotIndex(), Tool_Info.Instance.m_eCurrMissionEditDisturb) == true)
                            {
                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                            }
                        }
                        break;
                    case eTool_EditMode.eMissionAtUnitType_Magician:
                        {
                            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

                            if (pMapDataMissionBaseInfo.m_SlotUnitTypeTable.ContainsKey(pSlot.GetSlotIndex()) == false)
                            {
                                pMapDataMissionBaseInfo.m_SlotUnitTypeTable.Add(pSlot.GetSlotIndex(), eUnitType.Magician);

                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                            }
                        }
                        break;
                    case eTool_EditMode.eMissionAtUnitType_Block:
                        {
                            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

                            if (pMapDataMissionBaseInfo.m_SlotUnitTypeTable.ContainsKey(pSlot.GetSlotIndex()) == false)
                            {
                                pMapDataMissionBaseInfo.m_SlotUnitTypeTable.Add(pSlot.GetSlotIndex(), eUnitType.Block);

                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                            }
                        }
                        break;
                    case eTool_EditMode.eMissionAtUnitShape_Horizontal:
                        {
                            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

                            if (pMapDataMissionBaseInfo.m_SlotUnitShapeTable.ContainsKey(pSlot.GetSlotIndex()) == false)
                            {
                                pMapDataMissionBaseInfo.m_SlotUnitShapeTable.Add(pSlot.GetSlotIndex(), eUnitShape.Horizontal);

                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                            }
                        }
                        break;
                    case eTool_EditMode.eMissionAtUnitShape_Vertical:
                        {
                            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

                            if (pMapDataMissionBaseInfo.m_SlotUnitShapeTable.ContainsKey(pSlot.GetSlotIndex()) == false)
                            {
                                pMapDataMissionBaseInfo.m_SlotUnitShapeTable.Add(pSlot.GetSlotIndex(), eUnitShape.Vertical);

                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                            }
                        }
                        break;
                    case eTool_EditMode.eMissionAtUnitShape_Cross:
                        {
                            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

                            if (pMapDataMissionBaseInfo.m_SlotUnitShapeTable.ContainsKey(pSlot.GetSlotIndex()) == false)
                            {
                                pMapDataMissionBaseInfo.m_SlotUnitShapeTable.Add(pSlot.GetSlotIndex(), eUnitShape.Cross);

                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                            }
                        }
                        break;
                    case eTool_EditMode.eMissionAtUnitShape_Square:
                        {
                            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

                            if (pMapDataMissionBaseInfo.m_SlotUnitShapeTable.ContainsKey(pSlot.GetSlotIndex()) == false)
                            {
                                pMapDataMissionBaseInfo.m_SlotUnitShapeTable.Add(pSlot.GetSlotIndex(), eUnitShape.Square);

                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                            }
                        }
                        break;
                    case eTool_EditMode.eMissionAtUnitShape_TimeBomb:
                        {
                            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

                            if (pMapDataMissionBaseInfo.m_SlotUnitShapeTable.ContainsKey(pSlot.GetSlotIndex()) == false &&
                                pMapDataMissionBaseInfo.m_SlotUnitTimeBomb_NumberTable.ContainsKey(pSlot.GetSlotIndex()) == false)
                            {
                                pMapDataMissionBaseInfo.m_SlotUnitShapeTable.Add(pSlot.GetSlotIndex(), eUnitShape.TimeBomb);
                                pMapDataMissionBaseInfo.m_SlotUnitTimeBomb_NumberTable.Add(pSlot.GetSlotIndex(), Tool_Info.Instance.m_nTimeBombUnit_Number);

                                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                            }
                        }
                        break;
                    case eTool_EditMode.eDisturb_Dish:
                        {
                            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

                            if (pSlot.IsExistFourSlotMission() == true || pSlot.IsExistDisturb() == true || pSlot.IsExistUnitMission() == true)
                            {
                                return;
                            }

                            int nIndex = Tool_Info.Instance.m_nDisturb_Dish_Add_Index;

                            if (pMapDataMissionBaseInfo.m_DishGroupTable.ContainsKey(nIndex) == true)
                            {
                                int nCount = pMapDataMissionBaseInfo.m_DishGroupTable[nIndex].Count;

                                if (nCount == 0)
                                {
                                    pMapDataMissionBaseInfo.m_DishGroupTable[nIndex].Add(pSlot.GetSlotIndex());
                                }
                                else
                                {
                                    if (pMapDataMissionBaseInfo.m_DishGroupTable[nIndex].Contains(pSlot.GetSlotIndex()) == false)
                                    {
                                        int nLastSlotIndex = pMapDataMissionBaseInfo.m_DishGroupTable[nIndex][nCount - 1];
                                        Slot pSlot_Last = pScene.m_pGameController.m_pSlotManager.GetSlot_bySlotIndex(nLastSlotIndex);
                                        if (pSlot_Last != null)
                                        {
                                            bool IsNeighbor = false;
                                            Slot pSlot_Neighbor;

                                            pSlot_Neighbor = pSlot_Last.GetNeighborSlot(eNeighbor.eNeighbor_10);
                                            if (pSlot_Neighbor == pSlot) IsNeighbor = true;
                                            pSlot_Neighbor = pSlot_Last.GetNeighborSlot(eNeighbor.eNeighbor_01);
                                            if (pSlot_Neighbor == pSlot) IsNeighbor = true;
                                            pSlot_Neighbor = pSlot_Last.GetNeighborSlot(eNeighbor.eNeighbor_21);
                                            if (pSlot_Neighbor == pSlot) IsNeighbor = true;
                                            pSlot_Neighbor = pSlot_Last.GetNeighborSlot(eNeighbor.eNeighbor_12);
                                            if (pSlot_Neighbor == pSlot) IsNeighbor = true;

                                            if (IsNeighbor == true)
                                            {
                                                pMapDataMissionBaseInfo.m_DishGroupTable[nIndex].Add(pSlot.GetSlotIndex());
                                            }
                                        }
                                    }
                                }
                            }
                            else if (SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataSlotMove.m_SlotMoveTable.ContainsKey(nIndex) == false)
                            {
                                List<int> newSlotIndexList = new List<int>();
                                newSlotIndexList.Add(pSlot.GetSlotIndex());
                                pMapDataMissionBaseInfo.m_DishGroupTable.Add(nIndex, newSlotIndexList);
                            }

                            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
                        }
                        break;
                }
            }
        }
    }

    public void OnCallback_RButtonDown(GameObject gameObject, Vector3 vPos, object ob, int nFingerID)
    {
        int nSlotIndex = (int)ob;
    }

    public void OnCallback_RButtonUp(GameObject gameObject, Vector3 vPos, object ob, int nFingerID)
    {
        if (EventSystem.current.IsPointerOverGameObject() == true)
            return;

        int nSlotIndex = (int)ob;

        Slot pSlot = null;
        Scene_Tool pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_Tool;

        if (pScene != null)
        {
            pSlot = pScene.m_pGameController.m_pSlotManager.GetSlot_bySlotIndex(nSlotIndex);

            if (pSlot != null)
            {
                GameObject ob_Menu = Resources.Load<GameObject>("Tool/Prefabs/PopupMenu");
                ob_Menu = GameObject.Instantiate(ob_Menu);
                Tool_PopupMenu pPopupMenu = ob_Menu.GetComponent<Tool_PopupMenu>();
                Vector3 vUIPos = Helper.Get3DPosToUIPos(ob_Menu, Camera.main, vPos);
                pPopupMenu.Show(pSlot, vUIPos);
            }
        }
    }
}

#endif