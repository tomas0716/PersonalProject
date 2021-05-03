using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

public class Tool_Info
{
	private static Tool_Info ms_pInstance = null;
	public static Tool_Info Instance { get { return ms_pInstance; } }

	public eTool_MainMenu		m_eCurrMainMenu				= eTool_MainMenu.eMapShape;

	public eTool_EditMode		m_eEditMode					= eTool_EditMode.eNone;
	public eSlotFillWay			m_eCurrEditSlotFillWay		= eSlotFillWay.Close;
	public eDisturb				m_eCurrEditDisturb			= eDisturb.None;
	public eBarrigate			m_eCurrEditBarrigate		= eBarrigate.None;
	public int					m_nCurrEditSlotMoveIndex	= -1;

	public eMissionType			m_eCurrMissionTabType		= eMissionType.Unit;
	public eMissionType			m_eCurrEditMissionType		= eMissionType.None;
	public eDisturb				m_eCurrMissionEditDisturb	= eDisturb.None;
	public eUnitType			m_eCurrEditFixedUnitType	= eUnitType.Empty;

	public bool[]				m_IsActiveMission			= new bool[(int)eMissionType.Max];
	public bool[]				m_IsActiveDisturb			= new bool[(int)eDisturb.Max];

	public Slot					m_pSlotLink_In				= null;

	public bool					m_IsEditing					= false;
	public bool					m_IsDisturbPoint_Visible	= true;

	public int					m_nDisturb_Dish_Add_Index	= 0;

	public int					m_nTimeBombUnit_Number		= 10;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Mission Sub
	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	// Bell
	public eTool_Bell_EditMode	m_eBell_EditMode			= eTool_Bell_EditMode.eNone;
	// Mouse
	public int					m_nMouseDisturb				= -1;
	public eDisturb				m_eDisturb_Mouse			= eDisturb.None;
	// Apple
	public int					m_nAppleCount				= -1;
	// Rock
	public int					m_nRockCount				= -1;
	// Jelly
	public int					m_nJelly_Pattern			= -1;
	// Fish
	public eTool_Fish_EditMode	m_eFish_EditMode			= eTool_Fish_EditMode.eNone;
	// Number
	public int					m_nCurrNumber				= -1;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Machine
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	///
	public eUnitMachine			m_eUnitMachine				= eUnitMachine.None;

	public bool					m_IsMapReflesh				= false;

	public Tool_Info()
	{
		ms_pInstance = this;
	}

	public void InActiveMission()
	{
		for (int i = 0; i < (int)eMissionType.Max; ++i)
		{
			m_IsActiveMission[i] = false;
		}
	}

	public void InActiveDisturb()
	{
		for (int i = 0; i < (int)eDisturb.Max; ++i)
		{
			m_IsActiveDisturb[i] = false;
		}
	}
}

#endif