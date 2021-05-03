using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eDisturb
{
	None						= 0,
	Fixing_01					= 1,
	Fixing_02					= 2,
	Frozen_01					= 3,
	Frozen_02					= 4,
	Frozen_03					= 5,
	Frozen_04					= 6,
	Box_01						= 7,
	Box_02						= 8,
	Box_03						= 9,
	Box_04						= 10,
	Box_05						= 11,
	Cotton						= 12,
	Hide_Show					= 13,
	Block						= 14,
	Dish						= 15,

	Max,
}

public enum eBarrigate
{
	None						= 0,
	Barrigate_Left				= 1,
	Barrigate_Right				= 2,
	Barrigate_Bottom			= 3,
	Barrigate_LeftRight			= 4,
	Barrigate_LeftBottom		= 5,
	Barrigate_RightBottom		= 6,
	Barrigate_LeftRightBottom	= 7,

	Max,
}
public enum eDisturb_DishRoad
{
	None,
	Starter,
	Middler,
	Ender,
}