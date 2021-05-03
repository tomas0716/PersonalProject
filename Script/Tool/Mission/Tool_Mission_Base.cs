using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

#if UNITY_EDITOR

public class Tool_Mission_Base : MonoBehaviour
{
    private InputField []   m_pInputField_UnitCount         = new InputField[(int)(eUnitType.Brown + 1)];
    private InputField      m_pInputField_MoveCount         = null;

    private InputField[]    m_pInputField_Star              = new InputField[3];

    private Dropdown        m_pDropdown_FixedUnit           = null;
    private Text            m_pText_ChnageFixedUnit         = null;

    private Text []         m_pText_Machine                 = new Text[(int)eUnitMachine.Max];
    private InputField []   m_pInputField_MachinePercent    = new InputField[(int)eUnitMachine.Max];
    private InputField      m_pInputField_TimeBomb_Machine_Number = null;

    private Dropdown        m_pDropdown_Disturb             = null;
    private Text            m_pText_DisturbCreate           = null;

    private Text            m_pText_UnitType_Magician       = null;
    private Text            m_pText_UnitType_Block          = null;

    private Text            m_pText_UnitShape_Horizontal    = null;
    private Text            m_pText_UnitShape_Vertical      = null;
    private Text            m_pText_UnitShape_Cross         = null;
    private Text            m_pText_UnitShape_Square        = null;
    private Text            m_pText_UnitShape_TimeBomb      = null;
    private InputField      m_pInputField_TimeBomb_Number   = null;

    private Dropdown        m_pDropdown_Disturb_Dish        = null;
    private Text            m_pText_Disturb_Dish_Add        = null;
    private Text            m_pText_Disturb_Dish_Remove     = null;

    private List<Plane2D>   m_FixedUnitList                 = new List<Plane2D>();

    void Start()
    {
        GameObject ob;
        Button pButton;
        GameObject ob_Text;
        GameObject ob_InputField;

        for (int i = (int)eUnitType.Red; i <= (int)eUnitType.Brown; ++i)
        {
            eUnitType eType = (eUnitType)i;
            ob = Helper.FindChildGameObject(gameObject, "UnitCount_" + eType.ToString());
            ob = Helper.FindChildGameObject(ob, "InputField");
            m_pInputField_UnitCount[i] = ob.GetComponent<InputField>();
            m_pInputField_UnitCount[i].onEndEdit.AddListener(delegate { OnInputField_EndEdit_UnitCount((int)eType); });
        }

        ob = Helper.FindChildGameObject(gameObject, "MoveCount");
        ob = Helper.FindChildGameObject(ob, "InputField");
        m_pInputField_MoveCount = ob.GetComponent<InputField>();
        m_pInputField_MoveCount.onEndEdit.AddListener(OnInputField_EndEdit_MoveCount);

        for (int i = 0; i < 3; ++i)
        {
            int index = i;
            ob = Helper.FindChildGameObject(gameObject, "Star_0" + (i+1).ToString());
            ob = Helper.FindChildGameObject(ob, "InputField");
            m_pInputField_Star[i] = ob.GetComponent<InputField>();
            m_pInputField_Star[i].onEndEdit.AddListener(delegate { OnInputField_EndEdit_Star(index); });
        }
        
        ob = Helper.FindChildGameObject(gameObject, "Dropdown_FixedUnit");
        m_pDropdown_FixedUnit = ob.GetComponent<Dropdown>();
        m_pDropdown_FixedUnit.onValueChanged.AddListener(OnButtonClick_Dropdown_FixedUnit);

        ob = Helper.FindChildGameObject(gameObject, "Button_ChangeFixedUnit");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(OnButtonClick_ChangeFixedUnit);
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_ChnageFixedUnit = ob_Text.GetComponent<Text>();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Machine
        /// 

        ob = Helper.FindChildGameObject(gameObject, "Button_Unit_Cross");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Machine((int)eUnitMachine.Unit_Cross); });
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_Machine[(int)eUnitMachine.Unit_Cross] = ob_Text.GetComponent<Text>();
        ob_InputField = Helper.FindChildGameObject(ob, "InputField");
        m_pInputField_MachinePercent[(int)eUnitMachine.Unit_Cross] = ob_InputField.GetComponent<InputField>();
        m_pInputField_MachinePercent[(int)eUnitMachine.Unit_Cross].onEndEdit.AddListener(delegate { OnInputField_EndEdit_MachinePercent((int)eUnitMachine.Unit_Cross); });

        ob = Helper.FindChildGameObject(gameObject, "Button_Unit_Stripe");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Machine((int)eUnitMachine.Unit_Stripe); });
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_Machine[(int)eUnitMachine.Unit_Stripe] = ob_Text.GetComponent<Text>();
        ob_InputField = Helper.FindChildGameObject(ob, "InputField");
        m_pInputField_MachinePercent[(int)eUnitMachine.Unit_Stripe] = ob_InputField.GetComponent<InputField>();
        m_pInputField_MachinePercent[(int)eUnitMachine.Unit_Stripe].onEndEdit.AddListener(delegate { OnInputField_EndEdit_MachinePercent((int)eUnitMachine.Unit_Stripe); });

        ob = Helper.FindChildGameObject(gameObject, "Button_Unit_Magician");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Machine((int)eUnitMachine.Unit_Magician); });
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_Machine[(int)eUnitMachine.Unit_Magician] = ob_Text.GetComponent<Text>();
        ob_InputField = Helper.FindChildGameObject(ob, "InputField");
        m_pInputField_MachinePercent[(int)eUnitMachine.Unit_Magician] = ob_InputField.GetComponent<InputField>();
        m_pInputField_MachinePercent[(int)eUnitMachine.Unit_Magician].onEndEdit.AddListener(delegate { OnInputField_EndEdit_MachinePercent((int)eUnitMachine.Unit_Magician); });

        ob = Helper.FindChildGameObject(gameObject, "Button_Unit_Square");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Machine((int)eUnitMachine.Unit_Square); });
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_Machine[(int)eUnitMachine.Unit_Square] = ob_Text.GetComponent<Text>();
        ob_InputField = Helper.FindChildGameObject(ob, "InputField");
        m_pInputField_MachinePercent[(int)eUnitMachine.Unit_Square] = ob_InputField.GetComponent<InputField>();
        m_pInputField_MachinePercent[(int)eUnitMachine.Unit_Square].onEndEdit.AddListener(delegate { OnInputField_EndEdit_MachinePercent((int)eUnitMachine.Unit_Square); });


        ob = Helper.FindChildGameObject(gameObject, "Button_Unit_TimeBomb");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Machine((int)eUnitMachine.Unit_TimeBomb); });
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_Machine[(int)eUnitMachine.Unit_TimeBomb] = ob_Text.GetComponent<Text>();
        ob_InputField = Helper.FindChildGameObject(ob, "InputField");
        m_pInputField_MachinePercent[(int)eUnitMachine.Unit_TimeBomb] = ob_InputField.GetComponent<InputField>();
        m_pInputField_MachinePercent[(int)eUnitMachine.Unit_TimeBomb].onEndEdit.AddListener(delegate { OnInputField_EndEdit_MachinePercent((int)eUnitMachine.Unit_TimeBomb); });

        ob_InputField = Helper.FindChildGameObject(ob, "InputField_Number");
        m_pInputField_TimeBomb_Machine_Number = ob_InputField.GetComponent<InputField>();
        m_pInputField_TimeBomb_Machine_Number.onEndEdit.AddListener(OnInputField_EndEdit_TimeBomb_Machine_Number);

        ob = Helper.FindChildGameObject(gameObject, "Button_Disturb_Block");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(delegate { OnButtonClick_Machine((int)eUnitMachine.Disturb_Block); });
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_Machine[(int)eUnitMachine.Disturb_Block] = ob_Text.GetComponent<Text>();
        ob_InputField = Helper.FindChildGameObject(ob, "InputField");
        m_pInputField_MachinePercent[(int)eUnitMachine.Disturb_Block] = ob_InputField.GetComponent<InputField>();
        m_pInputField_MachinePercent[(int)eUnitMachine.Disturb_Block].onEndEdit.AddListener(delegate { OnInputField_EndEdit_MachinePercent((int)eUnitMachine.Disturb_Block); });

        ob = Helper.FindChildGameObject(gameObject, "Dropdown_Disturb");
        m_pDropdown_Disturb = ob.GetComponent<Dropdown>();
        m_pDropdown_Disturb.onValueChanged.AddListener(OnButtonClick_Dropdown_Disturb);

        ob = Helper.FindChildGameObject(gameObject, "Button_Disturb");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(OnButtonClick_Disturb);
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_DisturbCreate = ob_Text.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_Disturb_DeleteAll");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(OnButtonClick_Disturb_DeleteAll);

        ob = Helper.FindChildGameObject(gameObject, "Button_UnitType_Magician");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(OnButtonClick_UnitType_Magician);
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_UnitType_Magician = ob_Text.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_UnitType_Block");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(OnButtonClick_UnitType_Block);
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_UnitType_Block = ob_Text.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_UnitShape_Horizontal");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(OnButtonClick_UnitShape_Horizontal);
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_UnitShape_Horizontal = ob_Text.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_UnitShape_Vertical");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(OnButtonClick_UnitShape_Vertical);
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_UnitShape_Vertical = ob_Text.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_UnitShape_Cross");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(OnButtonClick_UnitShape_Cross);
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_UnitShape_Cross = ob_Text.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_UnitShape_Square");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(OnButtonClick_UnitShape_Square);
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_UnitShape_Square = ob_Text.GetComponent<Text>();

        ob = Helper.FindChildGameObject(gameObject, "Button_UnitShape_TimeBomb");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(OnButtonClick_UnitShape_TimeBomb);
        ob_Text = Helper.FindChildGameObject(ob, "Text");
        m_pText_UnitShape_TimeBomb = ob_Text.GetComponent<Text>();
        ob_InputField = Helper.FindChildGameObject(ob, "InputField");
        m_pInputField_TimeBomb_Number = ob_InputField.GetComponent<InputField>();
        m_pInputField_TimeBomb_Number.text = "10";
        m_pInputField_TimeBomb_Number.onEndEdit.AddListener(OnInputField_EndEdit_TimeBomb_Number);

        ob = Helper.FindChildGameObject(gameObject, "Button_Unit_TimeBomb_AllDelete");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(OnButtonClick_Unit_TimeBomb_AllDelete);

        ob = Helper.FindChildGameObject(gameObject, "Button_AllMissionRemove");
        pButton = ob.GetComponent<Button>();
        pButton.onClick.AddListener(OnButtonClick_AllMissionRemove);

		ob = Helper.FindChildGameObject(gameObject, "Dropdown_Disturb_Dish");
		m_pDropdown_Disturb_Dish = ob.GetComponent<Dropdown>();
		m_pDropdown_Disturb_Dish.onValueChanged.AddListener(OnButtonClick_Dropdown_Disturb_Dish);

		ob = Helper.FindChildGameObject(gameObject, "Button_Disturb_Dish");
		pButton = ob.GetComponent<Button>();
		pButton.onClick.AddListener(OnButtonClick_Disturb_Dish_Add);
		ob_Text = Helper.FindChildGameObject(ob, "Text");
		m_pText_Disturb_Dish_Add = ob_Text.GetComponent<Text>();

		ob = Helper.FindChildGameObject(gameObject, "Button_Disturb_RemoveDish");
		pButton = ob.GetComponent<Button>();
		pButton.onClick.AddListener(OnButtonClick_Disturb_Dish_Remove);
		ob_Text = Helper.FindChildGameObject(ob, "Text");
		m_pText_Disturb_Dish_Remove = ob_Text.GetComponent<Text>();

		EventDelegateManager_ForTool.Instance.OnEventUpdateMap += OnUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventPostUpdateMap += OnPostUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventMission_Common_UnCheck += OnMission_Common_UnCheck;

        EventDelegateManager_ForTool.Instance.OnEventDisturbPoint_Visible += OnDisturbPoint_Visible;
    }

    void OnDestroy()
    {
        EventDelegateManager_ForTool.Instance.OnEventUpdateMap -= OnUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventPostUpdateMap -= OnPostUpdateMap;
        EventDelegateManager_ForTool.Instance.OnEventMission_Common_UnCheck -= OnMission_Common_UnCheck;

        EventDelegateManager_ForTool.Instance.OnEventDisturbPoint_Visible -= OnDisturbPoint_Visible;
    }

    private void OnUpdateMap(bool IsChangeMap)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

            for (int i = (int)eUnitType.Red; i <= (int)eUnitType.Brown; ++i)
            {
                m_pInputField_UnitCount[i].text = pMapDataMissionBaseInfo.m_nUnitCount[i].ToString();
            }

            m_pInputField_MoveCount.text = pMapDataMissionBaseInfo.m_nMoveCount.ToString();

            for (int i = 0; i < 3; ++i)
            {
                m_pInputField_Star[i].text = pMapDataMissionBaseInfo.m_nStarScore[i].ToString();
            }

            m_pInputField_MachinePercent[(int)eUnitMachine.Unit_Cross].text = pMapDataMissionBaseInfo.m_UnitMachinePercentTable[eUnitMachine.Unit_Cross].ToString();
            m_pInputField_MachinePercent[(int)eUnitMachine.Unit_Stripe].text = pMapDataMissionBaseInfo.m_UnitMachinePercentTable[eUnitMachine.Unit_Stripe].ToString();
            m_pInputField_MachinePercent[(int)eUnitMachine.Unit_Magician].text = pMapDataMissionBaseInfo.m_UnitMachinePercentTable[eUnitMachine.Unit_Magician].ToString();
            m_pInputField_MachinePercent[(int)eUnitMachine.Unit_Square].text = pMapDataMissionBaseInfo.m_UnitMachinePercentTable[eUnitMachine.Unit_Square].ToString();
            m_pInputField_MachinePercent[(int)eUnitMachine.Unit_TimeBomb].text = pMapDataMissionBaseInfo.m_UnitMachinePercentTable[eUnitMachine.Unit_TimeBomb].ToString();
            m_pInputField_TimeBomb_Machine_Number.text = pMapDataMissionBaseInfo.m_nUnitMachine_TimeBomb_Number.ToString();
            m_pInputField_MachinePercent[(int)eUnitMachine.Disturb_Block].text = pMapDataMissionBaseInfo.m_UnitMachinePercentTable[eUnitMachine.Disturb_Block].ToString();
        }
    }

    private void OnPostUpdateMap(bool IsChangeMap)
    {
        foreach(Plane2D pPlane in m_FixedUnitList)
        {
            pPlane.OnDestroy();
        }

        m_FixedUnitList.Clear();

        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null && Tool_WindowCollect.Instance.m_GameObject_Mission.activeSelf == true )
        {
            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

            AtlasGroup pAtlasGroup = AppInstance.Instance.m_pAtlasManager.LoadAtlasGroup(eAtlasFileType.Atlas_2D_Tool, "Tool");

            Scene_Tool pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_Tool;

            if (pScene != null)
            {
                Dictionary<int, Slot> pSlotTable = pScene.m_pGameController.m_pSlotManager.GetSlotTable();

                foreach (KeyValuePair<int, Slot> item in pSlotTable)
                {
                    if (pMapDataMissionBaseInfo.m_SlotUnitTypeTable.ContainsKey(item.Key) == true)
                    {
                        AtlasInfo atlasInfo_slot = pAtlasGroup.FindAtlasInfo("Slot_03");
                        Plane2D pPlane = new Plane2D(atlasInfo_slot, (float)ePlaneOrder.Tool_Slot);
                        pPlane.SetPosition(item.Value.GetPosition());
                        pPlane.SetActiveColor(true);
                        pPlane.SetScale(new Vector3(InGameInfo.Instance.m_fSlotScale, InGameInfo.Instance.m_fSlotScale, InGameInfo.Instance.m_fSlotScale));

                        m_FixedUnitList.Add(pPlane);

                        if (Tool_Info.Instance.m_IsDisturbPoint_Visible == false)
                        {
                            pPlane.SetVisible(false);
                        }
                    }
                }
            }
        }
    }

    public void OnDisturbPoint_Visible(bool IsVisible)
    {
        foreach (Plane2D pPlane in m_FixedUnitList)
        {
            pPlane.SetVisible(IsVisible);
        }
    }

    public void OnInputField_EndEdit_UnitCount(int nUnitType)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

            bool IsSame = false;
            string strUnitCount = m_pInputField_UnitCount[nUnitType].text;
            int nUnitCount = Helper.ConvertStringToInt(strUnitCount);

            if (pMapDataMissionBaseInfo.m_nUnitCount[nUnitType] == nUnitCount)
            {
                IsSame = true;
            }

            pMapDataMissionBaseInfo.m_nUnitCount[nUnitType] = nUnitCount;

            if (IsSame == false)
            {
                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
            }
        }
    }

    public void OnInputField_EndEdit_Star(int nStar)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

            string strStar = m_pInputField_Star[nStar].text;
            int nScore = Helper.ConvertStringToInt(strStar);

            pMapDataMissionBaseInfo.m_nStarScore[nStar] = nScore;

            AppInstance.Instance.m_pEventDelegateManager.OnInGame_UpdataStarUI();
        }
    }

    public void OnInputField_EndEdit_MoveCount(string strData)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

            bool IsSame = false;
            string strMoveCount = m_pInputField_MoveCount.text;
            int nMoveCount = Helper.ConvertStringToInt(strMoveCount);

            if (pMapDataMissionBaseInfo.m_nMoveCount == nMoveCount)
            {
                IsSame = true;
            }

            pMapDataMissionBaseInfo.m_nMoveCount = nMoveCount;

            if (IsSame == false)
            {
                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
            }
        }
    }

    public void OnButtonClick_Dropdown_FixedUnit(int nIndex)
    {
        switch(nIndex)
        {
            case 0: Tool_Info.Instance.m_eCurrEditFixedUnitType = eUnitType.Empty;  break;
            case 1: Tool_Info.Instance.m_eCurrEditFixedUnitType = eUnitType.Red;    break;
            case 2: Tool_Info.Instance.m_eCurrEditFixedUnitType = eUnitType.Blue;   break;
            case 3: Tool_Info.Instance.m_eCurrEditFixedUnitType = eUnitType.Yellow; break;
            case 4: Tool_Info.Instance.m_eCurrEditFixedUnitType = eUnitType.White;  break;
            case 5: Tool_Info.Instance.m_eCurrEditFixedUnitType = eUnitType.Purple; break;
            case 6: Tool_Info.Instance.m_eCurrEditFixedUnitType = eUnitType.Brown;  break;
            case 7: Tool_Info.Instance.m_eCurrEditFixedUnitType = eUnitType.Random; break;
        }

        OnButtonClick_ChangeFixedUnit();
    }
    public void OnButtonClick_ChangeFixedUnit()
    {
        if (m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine] != null)
        {
            m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eUnitMachine = eUnitMachine.None;

        m_pText_UnitType_Magician.color = new Color(0, 0, 0);
        m_pText_UnitType_Block.color = new Color(0, 0, 0);
        m_pText_UnitShape_Horizontal.color = new Color(0, 0, 0);
        m_pText_UnitShape_Vertical.color = new Color(0, 0, 0);
        m_pText_UnitShape_Cross.color = new Color(0, 0, 0);
        m_pText_UnitShape_Square.color = new Color(0, 0, 0);
        m_pText_UnitShape_TimeBomb.color = new Color(0, 0, 0);

        m_pText_DisturbCreate.color = new Color(0, 0, 0);
        m_pText_ChnageFixedUnit.color = new Color(1,0,0);

        m_pText_Disturb_Dish_Add.color = new Color(0, 0, 0);
        m_pText_Disturb_Dish_Remove.color = new Color(0, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eChangeFixedUnit;
        EventDelegateManager_ForTool.Instance.OnMission_UnCheck();
    }

    public void OnButtonClick_Machine(int nUnitMachine)
    {
        eUnitMachine eMachine = (eUnitMachine)nUnitMachine;

        if (m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine] != null)
        {
            m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine].color = new Color(0, 0, 0);
        }

        if (m_pText_Machine[(int)eMachine] != null)
        {
            m_pText_Machine[(int)eMachine].color = new Color(1, 0, 0);
        }

        m_pText_DisturbCreate.color = new Color(0, 0, 0);
        m_pText_UnitType_Magician.color = new Color(0, 0, 0);
        m_pText_UnitType_Block.color = new Color(0, 0, 0);
        m_pText_UnitShape_Horizontal.color = new Color(0, 0, 0);
        m_pText_UnitShape_Vertical.color = new Color(0, 0, 0);
        m_pText_UnitShape_Cross.color = new Color(0, 0, 0);
        m_pText_UnitShape_Square.color = new Color(0, 0, 0);
        m_pText_UnitShape_TimeBomb.color = new Color(0, 0, 0);

        m_pText_ChnageFixedUnit.color = new Color(0, 0, 0);

        m_pText_Disturb_Dish_Add.color = new Color(0, 0, 0);
        m_pText_Disturb_Dish_Remove.color = new Color(0, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMission;
        Tool_Info.Instance.m_eUnitMachine = eMachine;

        EventDelegateManager_ForTool.Instance.OnMission_UnCheck();
    }

    public void OnInputField_EndEdit_MachinePercent(int nUnitMachine)
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

            bool IsSame = false;
            string strPercent = m_pInputField_MachinePercent[nUnitMachine].text;
            int nPercent = Helper.ConvertStringToInt(strPercent);

            if (pMapDataMissionBaseInfo.m_UnitMachinePercentTable[(eUnitMachine)nUnitMachine] == nPercent)
            {
                IsSame = true;
            }

            pMapDataMissionBaseInfo.m_UnitMachinePercentTable[(eUnitMachine)nUnitMachine] = nPercent;

            if (IsSame == false)
            {
                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
            }
        }
    }

    public void OnInputField_EndEdit_TimeBomb_Machine_Number(string strData)
    {
		if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
		{
			MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

			string strNumber = m_pInputField_TimeBomb_Machine_Number.text;
            pMapDataMissionBaseInfo.m_nUnitMachine_TimeBomb_Number = Helper.ConvertStringToInt(strNumber);
		}
	}

    public void OnButtonClick_Dropdown_Disturb(int nIndex)
    {
        Tool_Info.Instance.m_eCurrMissionEditDisturb = (eDisturb)m_pDropdown_Disturb.value;

        OnButtonClick_Disturb();
    }

    public void OnButtonClick_Disturb()
    {
        if (m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine] != null)
        {
            m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eUnitMachine = eUnitMachine.None;

        m_pText_UnitType_Magician.color = new Color(0, 0, 0);
        m_pText_UnitType_Block.color = new Color(0, 0, 0);
        m_pText_UnitShape_Horizontal.color = new Color(0, 0, 0);
        m_pText_UnitShape_Vertical.color = new Color(0, 0, 0);
        m_pText_UnitShape_Cross.color = new Color(0, 0, 0);
        m_pText_UnitShape_Square.color = new Color(0, 0, 0);
        m_pText_UnitShape_TimeBomb.color = new Color(0, 0, 0);

        m_pText_ChnageFixedUnit.color = new Color(0, 0, 0);

        m_pText_DisturbCreate.color = new Color(1, 0, 0);

        m_pText_Disturb_Dish_Add.color = new Color(0, 0, 0);
        m_pText_Disturb_Dish_Remove.color = new Color(0, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMissionAtDisturb;

        EventDelegateManager_ForTool.Instance.OnMission_UnCheck();
    }

    public void OnButtonClick_Disturb_DeleteAll()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

            pMapDataMissionBaseInfo.m_DisturbTable.Clear();

            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
        }
    }

    public void OnButtonClick_UnitType_Magician()
    {
        if (m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine] != null)
        {
            m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eUnitMachine = eUnitMachine.None;

        m_pText_DisturbCreate.color = new Color(0, 0, 0);

        m_pText_UnitType_Magician.color = new Color(1, 0, 0);
        m_pText_UnitType_Block.color = new Color(0, 0, 0);
        m_pText_UnitShape_Horizontal.color = new Color(0, 0, 0);
        m_pText_UnitShape_Vertical.color = new Color(0, 0, 0);
        m_pText_UnitShape_Cross.color = new Color(0, 0, 0);
        m_pText_UnitShape_Square.color = new Color(0, 0, 0);
        m_pText_UnitShape_TimeBomb.color = new Color(0, 0, 0);

        m_pText_ChnageFixedUnit.color = new Color(0, 0, 0);

        m_pText_Disturb_Dish_Add.color = new Color(0, 0, 0);
        m_pText_Disturb_Dish_Remove.color = new Color(0, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMissionAtUnitType_Magician;

        EventDelegateManager_ForTool.Instance.OnMission_UnCheck();
    }

    public void OnButtonClick_UnitType_Block()
    {
        if (m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine] != null)
        {
            m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eUnitMachine = eUnitMachine.None;

        m_pText_DisturbCreate.color = new Color(0, 0, 0);

        m_pText_UnitType_Magician.color = new Color(0, 0, 0);
        m_pText_UnitType_Block.color =new Color(1, 0, 0);
        m_pText_UnitShape_Horizontal.color = new Color(0, 0, 0);
        m_pText_UnitShape_Vertical.color = new Color(0, 0, 0);
        m_pText_UnitShape_Cross.color = new Color(0, 0, 0);
        m_pText_UnitShape_Square.color = new Color(0, 0, 0);
        m_pText_UnitShape_TimeBomb.color = new Color(0, 0, 0);

        m_pText_ChnageFixedUnit.color = new Color(0, 0, 0);

        m_pText_Disturb_Dish_Add.color = new Color(0, 0, 0);
        m_pText_Disturb_Dish_Remove.color = new Color(0, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMissionAtUnitType_Block;

        EventDelegateManager_ForTool.Instance.OnMission_UnCheck();
    }

    public void OnButtonClick_UnitShape_Horizontal()
    {
        if (m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine] != null)
        {
            m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eUnitMachine = eUnitMachine.None;

        m_pText_DisturbCreate.color = new Color(0, 0, 0);

        m_pText_UnitType_Magician.color = new Color(0, 0, 0);
        m_pText_UnitType_Block.color = new Color(0, 0, 0);
        m_pText_UnitShape_Horizontal.color = new Color(1, 0, 0);
        m_pText_UnitShape_Vertical.color = new Color(0, 0, 0);
        m_pText_UnitShape_Cross.color = new Color(0, 0, 0);
        m_pText_UnitShape_Square.color = new Color(0, 0, 0);
        m_pText_UnitShape_TimeBomb.color = new Color(0, 0, 0);

        m_pText_ChnageFixedUnit.color = new Color(0, 0, 0);

        m_pText_Disturb_Dish_Add.color = new Color(0, 0, 0);
        m_pText_Disturb_Dish_Remove.color = new Color(0, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMissionAtUnitShape_Horizontal;

        EventDelegateManager_ForTool.Instance.OnMission_UnCheck();
    }

    public void OnButtonClick_UnitShape_Vertical()
    {
        if (m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine] != null)
        {
            m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eUnitMachine = eUnitMachine.None;

        m_pText_DisturbCreate.color = new Color(0, 0, 0);

        m_pText_UnitType_Magician.color = new Color(0, 0, 0);
        m_pText_UnitType_Block.color = new Color(0, 0, 0);
        m_pText_UnitShape_Horizontal.color = new Color(0, 0, 0);
        m_pText_UnitShape_Vertical.color = new Color(1, 0, 0);
        m_pText_UnitShape_Cross.color = new Color(0, 0, 0);
        m_pText_UnitShape_Square.color = new Color(0, 0, 0);
        m_pText_UnitShape_TimeBomb.color = new Color(0, 0, 0);

        m_pText_ChnageFixedUnit.color = new Color(0, 0, 0);

        m_pText_Disturb_Dish_Add.color = new Color(0, 0, 0);
        m_pText_Disturb_Dish_Remove.color = new Color(0, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMissionAtUnitShape_Vertical;

        EventDelegateManager_ForTool.Instance.OnMission_UnCheck();
    }

    public void OnButtonClick_UnitShape_Cross()
    {
        if (m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine] != null)
        {
            m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eUnitMachine = eUnitMachine.None;

        m_pText_DisturbCreate.color = new Color(0, 0, 0);

        m_pText_UnitType_Magician.color = new Color(0, 0, 0);
        m_pText_UnitType_Block.color = new Color(0, 0, 0);
        m_pText_UnitShape_Horizontal.color = new Color(0, 0, 0);
        m_pText_UnitShape_Vertical.color = new Color(0, 0, 0);
        m_pText_UnitShape_Cross.color = new Color(1, 0, 0);
        m_pText_UnitShape_Square.color = new Color(0, 0, 0);
        m_pText_UnitShape_TimeBomb.color = new Color(0, 0, 0);

        m_pText_ChnageFixedUnit.color = new Color(0, 0, 0);

        m_pText_Disturb_Dish_Add.color = new Color(0, 0, 0);
        m_pText_Disturb_Dish_Remove.color = new Color(0, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMissionAtUnitShape_Cross;

        EventDelegateManager_ForTool.Instance.OnMission_UnCheck();
    }

    public void OnButtonClick_UnitShape_Square()
    {
        if (m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine] != null)
        {
            m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eUnitMachine = eUnitMachine.None;

        m_pText_DisturbCreate.color = new Color(0, 0, 0);

        m_pText_UnitType_Magician.color = new Color(0, 0, 0);
        m_pText_UnitType_Block.color = new Color(0, 0, 0);
        m_pText_UnitShape_Horizontal.color = new Color(0, 0, 0);
        m_pText_UnitShape_Vertical.color = new Color(0, 0, 0);
        m_pText_UnitShape_Cross.color = new Color(0, 0, 0);
        m_pText_UnitShape_Square.color = new Color(1, 0, 0);
        m_pText_UnitShape_TimeBomb.color = new Color(0, 0, 0);

        m_pText_ChnageFixedUnit.color = new Color(0, 0, 0);

        m_pText_Disturb_Dish_Add.color = new Color(0, 0, 0);
        m_pText_Disturb_Dish_Remove.color = new Color(0, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMissionAtUnitShape_Square;

        EventDelegateManager_ForTool.Instance.OnMission_UnCheck();
    }

    public void OnButtonClick_UnitShape_TimeBomb()
    {
        if (m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine] != null)
        {
            m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eUnitMachine = eUnitMachine.None;

        m_pText_DisturbCreate.color = new Color(0, 0, 0);

        m_pText_UnitType_Magician.color = new Color(0, 0, 0);
        m_pText_UnitType_Block.color = new Color(0, 0, 0);
        m_pText_UnitShape_Horizontal.color = new Color(0, 0, 0);
        m_pText_UnitShape_Vertical.color = new Color(0, 0, 0);
        m_pText_UnitShape_Cross.color = new Color(0, 0, 0);
        m_pText_UnitShape_Square.color = new Color(0, 0, 0);
        m_pText_UnitShape_TimeBomb.color = new Color(1, 0, 0);

        m_pText_ChnageFixedUnit.color = new Color(0, 0, 0);

        m_pText_Disturb_Dish_Add.color = new Color(0, 0, 0);
        m_pText_Disturb_Dish_Remove.color = new Color(0, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eMissionAtUnitShape_TimeBomb;

        EventDelegateManager_ForTool.Instance.OnMission_UnCheck();
    }

    public void OnInputField_EndEdit_TimeBomb_Number(string strData)
    {
        string strNumber = m_pInputField_TimeBomb_Number.text;
        Tool_Info.Instance.m_nTimeBombUnit_Number = Helper.ConvertStringToInt(strNumber);
    }

    public void OnButtonClick_Dropdown_Disturb_Dish(int nIndex)
    {
        Tool_Info.Instance.m_nDisturb_Dish_Add_Index = m_pDropdown_Disturb_Dish.value;

        OnButtonClick_Disturb_Dish_Add();
    }

    public void OnButtonClick_Disturb_Dish_Add()
    {
        if (m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine] != null)
        {
            m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eUnitMachine = eUnitMachine.None;

        m_pText_DisturbCreate.color = new Color(0, 0, 0);
        m_pText_UnitType_Magician.color = new Color(0, 0, 0);
        m_pText_UnitType_Block.color = new Color(0, 0, 0);
        m_pText_UnitShape_Horizontal.color = new Color(0, 0, 0);
        m_pText_UnitShape_Vertical.color = new Color(0, 0, 0);
        m_pText_UnitShape_Cross.color = new Color(0, 0, 0);
        m_pText_UnitShape_Square.color = new Color(0, 0, 0);
        m_pText_UnitShape_TimeBomb.color = new Color(0, 0, 0);

        m_pText_ChnageFixedUnit.color = new Color(0, 0, 0);

        m_pText_Disturb_Dish_Add.color = new Color(1, 0, 0);
        m_pText_Disturb_Dish_Remove.color = new Color(0, 0, 0);

        Tool_Info.Instance.m_eEditMode = eTool_EditMode.eDisturb_Dish;

        EventDelegateManager_ForTool.Instance.OnMission_UnCheck();
    }

    public void OnButtonClick_Disturb_Dish_Remove()
    {
		if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
		{
            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

            int nIndex = m_pDropdown_Disturb_Dish.value;

            if (pMapDataMissionBaseInfo.m_DishGroupTable.ContainsKey(nIndex) == true)
            {
                pMapDataMissionBaseInfo.m_DishGroupTable.Remove(nIndex);

                EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
                EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
            }
        }
	}

    public void OnMission_Common_UnCheck()
    {
        if (m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine] != null)
        {
            m_pText_Machine[(int)Tool_Info.Instance.m_eUnitMachine].color = new Color(0, 0, 0);
        }

        Tool_Info.Instance.m_eUnitMachine = eUnitMachine.None;

        m_pText_DisturbCreate.color = new Color(0, 0, 0);
        m_pText_UnitType_Magician.color = new Color(0, 0, 0);
        m_pText_UnitType_Block.color = new Color(0, 0, 0);
        m_pText_UnitShape_Horizontal.color = new Color(0, 0, 0);
        m_pText_UnitShape_Vertical.color = new Color(0, 0, 0);
        m_pText_UnitShape_Cross.color = new Color(0, 0, 0);
        m_pText_UnitShape_Square.color = new Color(0, 0, 0);
        m_pText_UnitShape_TimeBomb.color = new Color(0, 0, 0);
        m_pText_ChnageFixedUnit.color = new Color(0, 0, 0);
        m_pText_Disturb_Dish_Add.color = new Color(0, 0, 0);
        m_pText_Disturb_Dish_Remove.color = new Color(0, 0, 0);
    }

    public void OnButtonClick_Unit_TimeBomb_AllDelete()
    {
        if (SavedGameDataInfo.Instance.m_pMapDataInfo != null)
        {
            MapDataMissionBaseInfo pMapDataMissionBaseInfo = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo;

            List<int> removeList = new List<int>();
            foreach (KeyValuePair<int, eUnitShape> item in pMapDataMissionBaseInfo.m_SlotUnitShapeTable)
            {
                if (item.Value == eUnitShape.TimeBomb)
                {
                    removeList.Add(item.Key);
                }
            }

            foreach (int nRemoveID in removeList)
            {
                pMapDataMissionBaseInfo.m_SlotUnitShapeTable.Remove(nRemoveID);
            }

            pMapDataMissionBaseInfo.m_SlotUnitTimeBomb_NumberTable.Clear();

            EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
            EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
        }
    }

    public void OnButtonClick_AllMissionRemove()
    {
        EventDelegateManager_ForTool.Instance.OnAllMissionRemove();

        EventDelegateManager_ForTool.Instance.OnUpdateMap(false);
        EventDelegateManager_ForTool.Instance.OnPostUpdateMap(false);
    }
}

#endif