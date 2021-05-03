using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCombinationShape
{
	None			= 0,
	Square			= 1,
	Strike_3		= 2,
	Strike_4_H		= 3,
	Strike_4_V		= 4,
	Cross			= 5,
	Strike_5		= 6,
}

public enum eSlotFillWay
{
	None					= 0,
	Close					= 1,
	Normal					= 2,
	Create					= 3,
	Link_ForTool			= 4,
	InVisibleLink_ForTool	= 5,

	Max,
}

public enum eUnitType
{
	Empty			= 0,

	Red				= 1,
	Blue			= 2,
	Yellow			= 3,
	White			= 4,
	Purple			= 5,
	Brown			= 6,

	Magician		= 7,

	Bell			= 8,
	Fish			= 9,
	Block			= 10,
	Mouse			= 11,

	Max,
	Random			= Max,
}

public enum eNeighbor
{
	eNeighbor_None = -1,
	eNeighbor_00,
	eNeighbor_10,
	eNeighbor_20,
	eNeighbor_01,
	eNeighbor_21,
	eNeighbor_02,
	eNeighbor_12,
	eNeighbor_22,

	eMax,
}

public enum eUnitShape
{
	Normal			= 0,
	Square			= 1,
	Square_NoColor	= 2,
	Horizontal		= 3,
	Vertical		= 4,
	Cross			= 5,
	Magician		= 6,
	Number			= 7,
	TimeBomb		= 8,

	Max,
}

public enum eSlotLinkType
{
	None,
	In,
	Out,
}

public enum eSlotMoveRoad
{
	None,
	Starter,
	Middler,
	Ender,
	StarterAndEnder,
}

public enum eSlotUnitScaleMode
{
	Y,
	XY,
}