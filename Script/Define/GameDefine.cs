using UnityEngine;
using System.Collections;

public enum eSceneType
{
	None,
    
	Scene_Logo,
    Scene_Middle,
	Scene_Lobby,
	Scene_InGame,
	Scene_Tool,

	Max,
}

public enum eScreenAnchorPosition
{
	UpperLeft,
	UpperCenter,
	UpperRight,
	MiddleLeft,
	MiddleCenter,
	MiddleRight,
	LowerLeft,
	LowerCenter,
	LowerRight,
}

public enum eGameEventType
{
	None,
	GetMission_BreadAction,
	GetMission_BreadBackAction,
	GetMission_FishAction,
	GetMission_JellyBackAction,
	Projectile_Octopus_Ink,
	Max,
}

public enum ePlayingMode
{
	Stop,
	Play,
	Pause,
}

public enum eSoundKind
{
	eBKG,
	eEffect
}

public enum eSoundType
{
	Lobby,
	Button,
	Popup,
	IngamePopup,
	InGame,
	InGame1,
	Unit_Miss,
	Unit_Delete,
	Unit_Delete2,
	Unit_Delete3,
	Unit_Delete4,
	MagicianPlus,
	StripePlus,
	BombPlus,
	PlanePlus,
	MagicianUse,
	StripeUse,
	BombUse,
	PlaneUse,
	SwapUse,
	PunchUse,
	RuncatUse,
	Mission,
	OutofMove5,
	OutofMove0,
	Failed,
	Continue,
	Bonus,
	Clear,
	Clear_Cat,
	Result,
	Ice,
	Block,
	Cotton,
	Box,
	Fixing,
	Hide,
	Bell,
	Apple,
	Rock,
	Grass,
	Bread,
	Jelly,
	Fish,
	Number,
	Octopus,
	Can,
	Knit,
	TimeBomb_1,
	TimeBomb_2,
    Achievement,
    Drop,
    Roulette, 
    Attendance, 
    Reward, 
    Message_Ad,
    Milk,

    Max,
}

public enum eItemType
{
	None, 

	Move3,
	RainbowCat,
	StripeToeJelly,

	Swap,
	CatPunch,
	RunningCat,

	MissionChange,
	Continue,
	HeartCharge,
	Heart,
	Coin,

	Max,
}

public enum eAchievementType
{
	None, 

	RedUnit_Remove,

	StripeUnit_Remove,
	BombUnit_Remove,
	RainbowUnit_Remove,
	PlaneUnit_Remove,

	CombinationStripeStripe,
	CombinationStripeBomb,
	CombinationStripeRainbow,
	CombinationStripePlain,
	CombinationBombBomb,
	CombinationBombRainbow,
	CombinationBombPlain,
	CombinationRainbowRainbow,
	CombinationRainbowPlain,
	CombinationPlainPlain,

	InGame_UseItem,
	InGame_UseItem_Swap,
	InGame_UseItem_StripeHV,
	InGame_UseItem_SlotAttack,

	Mission_Bell,
	Mission_Mouse,
	Mission_Apple,
	Mission_Rock,
	Mission_Grass,
	Mission_Bread,
	Mission_Jelly,
	Mission_Fish,
	Mission_Number,
	Mission_OctopusInk,
	Mission_Knit,
	Mission_Can,
	Mission_Butterfly,

	Goal_Level,

	Max,
}

public enum eIAPType
{
	None,
	Package_Beginner,
	Single_Free,
	Single_Ads,
	Single,
	Package,

	Max,
}

public enum eRewardSubject
{
	eAttendance,
	eRoulette,
	eArchievement,
	eShop,
	eNewUser_Present,
}

public enum eLanguage
{
    kr,
    us,
    jp,

    Max,
}

public partial class GameDefine
{
	// 버전 변화 데이터
	// 1 > 2 : 세이브 아이디 넣기
	// 2 > 3 : 유저 레벨 스코어 기록
	static public byte					ms_byClient_Ver						= 3;	

    static public eScreenAnchorPosition ms_eScreenAnchorPosition			= eScreenAnchorPosition.LowerCenter;

	static public int					ms_nMaxLevel						= 286;		// 꼭 하나씩 더 만들어 놓아야 함.

	static public int					ms_nNewGameAdsShowLevel				= 16;

	static public int					ms_nShopItemFree_Max				= 7;

	static public int					ms_nInGameFailCountToMissionChange	= 10;

	static public int					ms_nMaxAutoChargeHeart				= 5;
	static public double				ms_dwAutoChargeHeartTimeSecond		= 60 * 30;  // 30분당 하나 충전
	static public int					ms_nHeartFree_Max					= 5;

	static public int					ms_nMaxItemRoulette					= 8;
	static public int					ms_nRouletteAdsShowLevel			= 16;

	static public bool					ms_IsVersionCheck					= true;
    static public bool                  ms_IsGoogleLogin                    = true;
    static public bool                  ms_IsUseAds                         = true;
    static public bool                  ms_IsUseSavedGame                   = true;
    static public bool                  ms_IsGoogleLeaderboard              = true;

    static public string                ms_UnityCountryCode                 = "us";

	static public int					ms_nFiveLevelClear_Present_Heart	= 10;
	static public int					ms_nFiveLevelClear_Present_Coin		= 100;

	public GameDefine()
	{
		AppInstance.Instance.m_vBaseResolution = new Vector2(720, 1280);
		ms_eScreenAnchorPosition = eScreenAnchorPosition.LowerCenter;
	}
}
