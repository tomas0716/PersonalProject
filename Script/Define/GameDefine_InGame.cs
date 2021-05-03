using UnityEngine;
using System.Collections;

public enum ePlaneOrder
{
	BackGround_Base,
	BackGround_Cloud_Back,
	BackGround_Castle,
	BackGround_Island,
	BackGround_Cloud_Front,
	MainCharacter,

	SlotBack_Edge,
    SlotBack,
    SlotBack_1,
    SlotBack_2,

    Mission_Grass,
	Mission_Knit,
	Mission_OctopusInk,

	MoveSlot_Road_Deco,
	MoveSlot_Deco,

	Tool_Slot,
	Tool_Number,

	SlotUnit_Octopus_Ink,
	SlotUnit_Octopus_Spread,

	SlotUnit = 500,
	SlutUnit_Shuffle,
	SlotUnit_Hide,
	SlotUnit_Deco,
	SlotUnit_Deco_01,
	SlotUnit_Fish_Hit,

	SlotUnit_Number_ChangeEffect,
	SlotUnit_Number_Number,
	SlotUnit_Number_Number_In,


	Machine_Item,
	Machine_Back,

	Mission_Apple_Tree,
	Mission_Apple,
	Mission_Octopus,

	Mission_BreadBack,
	Mission_Bread_01,
	Mission_Bread_02,
	Mission_Bread_03,
	Mission_Bread_04,
	Mission_Bread_05,
	Mission_Bread_06,
	Mission_Bread_Dust,
	Mission_Bread_Hit,

	Mission_JellyBack,
	Mission_Jelly_01,
	Mission_Jelly_02,
	Mission_Jelly_03,
	Mission_Jelly_04,
	Mission_Jelly_05,
	Mission_Jelly_06,
	Mission_Jelly_07,
	Mission_Jelly_08,
	Mission_JellyBottle,

	Mission_Number_Back,
	Mission_Number_Number,
	Mission_Number_Number_In,

	Mission_Can_LeftBottom_Empty,
	Mission_Can_LeftBottom_Full,
	Mission_Can_RightBottom_Empty,
	Mission_Can_RightBottom_Full,
	Mission_Can_LeftTop_Empty,
	Mission_Can_LeftTop_Full,
	Mission_Can_RightTop_Empty,
	Mission_Can_RightTop_Full,

	Mission_Butterfly_NoColor,
	Mission_Butterfly,

	Disturb_Fixing_01,
	Disturb_Fixing_02,
	Disturb_Fixing_03,
	Disturb_Box_01,
	Disturb_Box_02,
	Disturb_Box_03,
	Disturb_Box_04,
	Disturb_Box_05,
	Disturb_Frozen_01,
	Disturb_Frozen_02,
	Disturb_Frozen_03,
	Disturb_Frozen_04,
	Disturb_Cotton,

	Disturb_Dish_Back,
	Disturb_Dish,
	Disturb_Dish_Empty,

	Barrigate,

	Mission_Deco_Bell_Goal,

	GoldenGaugeItem,

	Slot_Darkly,

	Slot_Backgound_Dying,
	Slot_Backgound_Focus,

	SlotBack_InGameItemUse_ChangeSlot,
	Slot_Dying_Back_Back_Back_Back,
	Slot_Dying_Back_Back_Back,
	Slot_Dying_Back_Back,
	Slot_Dying_Back,
	Slot_Dying,
	Slot_Dying_Front,
	Slot_Dying_Front_Front,
	Slot_Dying_Front_Front_Front,
	Slot_Dying_Front_Front_Front_Front,

	Slot_Notice = 900,
	Projectile_Octopus_Ink,
	Projectile_SlotUnit_Square,

	ApplyGameItem_StripeHV_Model,

	NewLine,

	SlotEmpty = 1000,

	Particle_Stripe_Unit	= 2000,

	Fx_Stripe_Line_01 = 3000,
	Fx_Stripe_Line_02,
	Fx_BonusPang,
	Fx_Item_ChangeUnitShape,
	Fx_Common,
}

public enum eSlotOverlap
{
	None,
	Left,
	Right,
	Down,
	Up,
}

public enum eUnitMachine
{
	None			= 0,
	Unit_Cross,
	Unit_Stripe,
	Unit_Magician,
	Unit_Square,
	Unit_TimeBomb,
	Disturb_Block,
	Mission_Bell,
	Mission_Fish,
	
	Max,
}

public enum eSlotUnitDeco
{
	eOctopus_Ink,
	eBonusPangBubble,

	Max,
}

public enum eGameState
{
	None,
	Playing,
	Success,
	Fail,
    Fail_TimeBombExploded,
}

public enum eClearStep
{
	None,
	SpecialUnit,
	RenameMove,
}

public enum eInGameClearSaveData
{
	SavedGame,
	Leaderboard_Score,
	Leaderboard_Level,

	Max,
}

public enum eInGameItemUseState
{
	None,
	Ready,
	Execute,
}

public partial class GameDefine
{
	static public float		ms_fSlotBaseImageSize						= 72;
	static public float		ms_fBaseSlotSize							= 63;

	static public int		ms_nInGameSlot_X							= 13;
	static public int		ms_nInGameSlot_Y							= 13;

	static public float		ms_fUnitMoveTime							= 0.083f;
	static public float		ms_fUnitMoveMutipleTime						= 0.9f;
	static public float		ms_fMoveAndCreateTime						= 0.224f;
	static public float		ms_fCheckRemoveTime							= 0.048f;
	static public float		ms_fSlotMoveTime							= 0.56f;

	static public int		ms_vnSlotCenter_Y							= 640;

	static public float		ms_fNextSpecialSlotRemoveDelayTime			= 0.126f;

	static public bool		ms_IsMultiTouchActive						= false;
	static public float		ms_fDarklyStartTime							= 0.05f;
	static public float		ms_fDarklyEndTime							= 0.02f;
	static public float		ms_fDarklyAlpha								= 0.65f;
	static public float		ms_fDarklyColor								= 1.0f;
	static public float		ms_fSpecialSlotStraight						= 0.02f;
	static public float		ms_fSpecialSlotSlotDyingTime				= 0.16f;

	static public float		ms_fApplyGameItem_StripeHV_StraightTime		= 0.2f;

	static public float		ms_fMagicianActiveLightingTime				= 0.48f;
	static public float		ms_fCombinationMagicianDelayTime			= 0.64f;

	static public float		ms_fDisturbBox_Scale						= 0.94f;

	static public int		ms_nLinkSlotMax								= 8;
	static public float		ms_fLinkSlotWarpInterval					= 44;
	static public int		ms_nLinkSlotMax_InVisible					= 18;

	static public int		ms_nSquareMagicianUnitProjectileCount		= 2;
	//static public int		ms_nSquareSquareUnitProjectileCount			= 4;
	//static public int		ms_nSquareUnitProjectileCount				= 3;
	static public int		ms_nSquareSquareUnitProjectileCount			= 2;
	static public int		ms_nSquareUnitProjectileCount				= 2;

	static public float		ms_fGetMissionDirectionTime_Bell			= 0.18f;		// 완료
	static public float		ms_fGetMissionDirectionTime_Mouse			= 0.2f;
	static public float		ms_fGetMissionDirectionTime_Apple			= 0.2f;			// 완료
	static public float		ms_fGetMissionDirectionTime_Rock			= 0.3f;	        // 완료
	static public float		ms_fGetMissionDirectionTime_Bread			= 0.2f;			// 완료
	static public float		ms_fGetMissionDirectionTime_Jelly			= 0.05f;	        // 완료
	static public float		ms_fGetMissionDirectionTime_Fish			= 0.4f;			// 완료
	static public float		ms_fGetMissionDirectionTime_Star			= 2.5f;
	static public float		ms_fGetMissionDirectionTime_Ufo				= 2.5f;
	static public float		ms_fGetMissionDirectionTime_Number			= 0.2f;           // 완료

	static public float		ms_fRemoveOctopuschainTime					= 0.22f;
	static public float		ms_fRemoveOctopusEffectTime					= 0.5f;

	static public Color[]	ms_clrNumberMission							= { new Color(49.0f/255.0f, 47.0f/255.0f, 57.0f/255.0f),		// None
																			new Color(246.0f/255.0f, 17.0f/255.0f, 17.0f/255.0f),		// Red
																			new Color(6.0f/255.0f, 60.0f/255.0f, 236.0f/255.0f),		// Blue
																			new Color(224.0f/255.0f, 166.0f/255.0f, 8.0f/255.0f),		// Yellow
																			new Color(85.0f/255.0f, 84.0f/255.0f, 84.0f/255.0f),		// White
																			new Color(112.0f/255.0f, 65.0f/255.0f, 179.0f/255.0f),		// Purple
																			new Color(54.0f/255.0f, 140.0f/255.0f, 2.0f/255.0f),};      // Brown

    static public float		ms_fNumberMissionChangeTime					= 0.3f;

	static public int		ms_nSlotMoveMaxCount						= 6;
	static public int		ms_nInGameMaxContinueCount					= 3;

	static public float		ms_fInGameItemUse_SlotAttack_DirectionTime	= 1.0f;
	static public float		ms_fInGameItemUse_SlotAttack_Delay			= 1.5f;

	static public float		ms_fInGameItemUse_Stripe_DirectionTime		= 1.2f;

	static public float		ms_fUnitMutipleScale						= 1.03f;
}