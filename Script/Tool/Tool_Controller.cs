using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

public class Tool_Controller : MonoBehaviour
{
    private Toggle m_pToggle_DisturbPoint_Visible = null;

    private bool [] m_IsVisibleUnit = new bool[(int)eUnitType.Max];

    private Text m_pText_FileName = null;

    private bool m_IsUpdateMissionUI = false;
    private int m_nUpdateMissionUI = -1;

    void Start()
    {
        GameObject ob = Helper.FindChildGameObject(gameObject, "Toggle_DisturbPoint_Visible");
        m_pToggle_DisturbPoint_Visible = ob.GetComponent<Toggle>();

        EventDelegateManager_ForTool.Instance.OnEventUpdateMap += OnUpdateMap;

        for (int i = 0; i < (int)eUnitType.Max; ++i)
        {
            m_IsVisibleUnit[i] = true;
        }

        ob = Helper.FindChildGameObject(gameObject, "Text_FileName");
        m_pText_FileName = ob.GetComponent<Text>();
        m_pText_FileName.text = "";
    }

	private void OnDestroy()
	{
        EventDelegateManager_ForTool.Instance.OnEventUpdateMap -= OnUpdateMap;
    }

	void Update()
    {
        if (m_IsUpdateMissionUI == true)
        {
            --m_nUpdateMissionUI;
            if (m_nUpdateMissionUI == 0)
            {
                m_IsUpdateMissionUI = false;
                AppInstance.Instance.m_pEventDelegateManager.OnInGame_UpdataMissionUI();
            }
        }
        //if (Input.GetKeyDown(KeyCode.Keypad1) == true)
        //{
        //	m_IsVisibleUnit[(int)eUnitType.Red] = !m_IsVisibleUnit[(int)eUnitType.Red];
        //	UpdateVisibleUnit();
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad2) == true)
        //{
        //	m_IsVisibleUnit[(int)eUnitType.Blue] = !m_IsVisibleUnit[(int)eUnitType.Blue];
        //	UpdateVisibleUnit();
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad3) == true)
        //{
        //	m_IsVisibleUnit[(int)eUnitType.Yellow] = !m_IsVisibleUnit[(int)eUnitType.Yellow];
        //	UpdateVisibleUnit();
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad4) == true)
        //{
        //	m_IsVisibleUnit[(int)eUnitType.White] = !m_IsVisibleUnit[(int)eUnitType.White];
        //	UpdateVisibleUnit();
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad5) == true)
        //{
        //	m_IsVisibleUnit[(int)eUnitType.Purple] = !m_IsVisibleUnit[(int)eUnitType.Purple];
        //	UpdateVisibleUnit();
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad6) == true)
        //{
        //	m_IsVisibleUnit[(int)eUnitType.Brown] = !m_IsVisibleUnit[(int)eUnitType.Brown];
        //	UpdateVisibleUnit();
        //}
    }

    private void UpdateVisibleUnit()
    {
        Scene_Tool pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_Tool;

        if (pScene != null)
        {
            SlotManager pSlotManager = pScene.m_pGameController.m_pSlotManager;

            foreach (KeyValuePair<int, Slot> item in pSlotManager.GetSlotTable())
            {
                if (item.Value.GetSlotUnit() != null)
                {
                    item.Value.GetSlotUnit().GetRndSlotUnit().SetActiveColor();

                    if (m_IsVisibleUnit[(int)item.Value.GetSlotUnit().GetUnitType()] == true)
                    {
                        item.Value.GetSlotUnit().GetRndSlotUnit().SetAlpha(1.0f);
                    }
                    else
                    {
                        item.Value.GetSlotUnit().GetRndSlotUnit().SetAlpha(0.0f);
                    }
                }
            }
        }
    }

    private void OnMapBound()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            Tool_Info.Instance.m_pSlotLink_In = null;
            SavedGameDataInfo.Instance.m_pMapDataInfo.CalculationBound();

            InGameInfo.Instance.m_nSlotStartIndex_X = SavedGameDataInfo.Instance.m_pMapDataInfo.m_nStartSlotIndex_X;
            InGameInfo.Instance.m_nSlotStartIndex_Y = SavedGameDataInfo.Instance.m_pMapDataInfo.m_nStartSlotIndex_Y;
            InGameInfo.Instance.m_nSlotEndIndex_X = SavedGameDataInfo.Instance.m_pMapDataInfo.m_nEndSlotIndex_X;
            InGameInfo.Instance.m_nSlotEndIndex_Y = SavedGameDataInfo.Instance.m_pMapDataInfo.m_nEndSlotIndex_Y;
            InGameInfo.Instance.m_nSlotCount_X = InGameInfo.Instance.m_nSlotEndIndex_X - InGameInfo.Instance.m_nSlotStartIndex_X + 1;
            InGameInfo.Instance.m_nSlotCount_Y = InGameInfo.Instance.m_nSlotEndIndex_Y - InGameInfo.Instance.m_nSlotStartIndex_Y + 1;
            InGameInfo.Instance.m_fSlotSize = 76;
            InGameInfo.Instance.m_fSlotScale = InGameInfo.Instance.m_fSlotSize / 72.0f;
            InGameInfo.Instance.m_vSlot_LeftTopPos.x = -((InGameInfo.Instance.m_nSlotCount_X * 0.5f) * InGameInfo.Instance.m_fSlotSize - InGameInfo.Instance.m_fSlotSize * 0.5f);
            InGameInfo.Instance.m_vSlot_LeftTopPos.y = (InGameInfo.Instance.m_nSlotCount_Y * 0.5f) * InGameInfo.Instance.m_fSlotSize + GameDefine.ms_vnSlotCenter_Y - InGameInfo.Instance.m_fSlotSize * 0.5f;
        }
    }

    public void OnButtonClick_Lobby()
    {
        AppInstance.Instance.m_pOptionInfo.m_strCountryCode = GameDefine.ms_UnityCountryCode;
        SavedGameDataInfo.Instance.Load();
        AppInstance.Instance.m_pSceneManager.ChangeScene(eSceneType.Scene_Lobby, false);
    }

    public void OnButtonClick_Save()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            SavedGameDataInfo.Instance.m_pMapDataInfo.SaveFile();
        }
    }

    public void OnToggle_ValueChanged_DisburbPoint_Visible(bool IsCheck)
    {
        Tool_Info.Instance.m_IsDisturbPoint_Visible = m_pToggle_DisturbPoint_Visible.isOn;
        EventDelegateManager_ForTool.Instance.OnDisturbPoint_Visible(Tool_Info.Instance.m_IsDisturbPoint_Visible);
    }

    public void OnButtonClick_CurrentFileReLoad()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            string strFolderName = SavedGameDataInfo.Instance.m_pMapDataInfo.m_strDirectory;
            string strFildName = SavedGameDataInfo.Instance.m_pMapDataInfo.m_strFileName;
            Tool_Info.Instance.m_IsEditing = false;
            SavedGameDataInfo.Instance.m_pMapDataInfo = new MapDataInfo(strFolderName, strFildName);
            SavedGameDataInfo.Instance.m_pMapDataInfo.LoadFile();

            EventDelegateManager_ForTool.Instance.OnUpdateMap(true);
            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(true);
        }
    }

    private void OnUpdateMap(bool IsChangeMap)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            m_pText_FileName.text = SavedGameDataInfo.Instance.m_pMapDataInfo.m_strFileName;

            SavedGameDataInfo.Instance.ResetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
            OnMapBound();

            Scene_Tool pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_Tool;

            if (pScene != null)
            {
                InGameInfo.Instance.InGameStart_Reset();

                Tool_Info.Instance.m_IsMapReflesh = true;
                pScene.m_pGameController.m_pSlotManager.RemoveCurrMap();
                Tool_Info.Instance.m_IsMapReflesh = false;
                pScene.m_pGameController.m_pSlotManager.CreateMap();
                pScene.m_pGameController.m_pSlotManager.CheckSlotMoveInfo();
                pScene.m_pGameController.m_pSlotManager.CreateSlotMove();
                pScene.m_pGameController.m_pSlotManager.CreateMission(SavedGameDataInfo.Instance.m_MapDataMissionList);
                pScene.m_pGameController.m_pSlotManager.CreateMissionAtUnitType();
                pScene.m_pGameController.m_pSlotManager.CreateMissionAtUnitShape();
                pScene.m_pGameController.m_pSlotManager.CreateMissionAtDisturb(SavedGameDataInfo.Instance.m_MapDataMissionList);

                pScene.m_pGameController.m_pSlotManager.Initialize_PossibleMoveSlotCheck(false);

                InGameInfo.Instance.m_IsInGameClick = true;
                InGameInfo.Instance.m_eGameState = eGameState.Playing;

                pScene.m_pGameController.OnResetMainUI();
                m_IsUpdateMissionUI = true;
                m_nUpdateMissionUI = 2;

                AppInstance.Instance.m_pEventDelegateManager.OnInGame_UpdateMoveCount(SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nMoveCount);
            }
        }
    }
}

#endif