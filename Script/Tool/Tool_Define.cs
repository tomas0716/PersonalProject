using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

public enum eTool_MainMenu
{
	eFileCreate,
	eMapShape,
	eMission,

	eMax,
}

public enum eTool_EditMode
{
	eNone,
	eSlotLink,
	eInVisibleSlotLink,
	eBarrigate,
	eMission,
	eChangeFixedUnit,
	eMissionAtDisturb,
	eMissionAtUnitType_Magician,
	eMissionAtUnitType_Block,
	eMissionAtUnitShape_Horizontal,
	eMissionAtUnitShape_Vertical,
	eMissionAtUnitShape_Cross,
	eMissionAtUnitShape_Square,
	eMissionAtUnitShape_TimeBomb,
	eDisturb,
	eSlotMove,
	eDisturb_Dish,
}

public enum eTool_Bell_EditMode
{
	eNone,
	eBell_Create,
	eBell_Goal,
	eBell_Machine,
}

public enum eTool_Fish_EditMode
{
	eNone,
	eFish_Create,
	eFish_Machine,
}

public class Tool_Define
{
}

#endif