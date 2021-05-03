using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EventDelegateManager
{
	public delegate void OnEvent_InGameIntroStart();
	public event OnEvent_InGameIntroStart OnEventInGameIntroStart;
	public void OnInGameIntroStart()
	{
		OnEventInGameIntroStart?.Invoke();
	}

	public delegate void OnEvent_InGamePlay();
	public event OnEvent_InGamePlay OnEventInGamePlay;
	public void OnInGamePlay()
	{
		OnEventInGamePlay?.Invoke();
	}

	public delegate void OnEvent_GamePause();
	public event OnEvent_GamePause OnEventGamePause;
	public void OnGamePause()
	{
		OnEventGamePause?.Invoke();
	}

	public delegate void OnEvent_GameStop();
	public event OnEvent_GameStop OnEventGameStop;
	public void OnGameStop()
	{
		OnEventGameStop?.Invoke();
	}

	public delegate void OnEvent_CreateInGameLoading();
	public event OnEvent_CreateInGameLoading OnEventCreateInGameLoading;
	public void OnCreateInGameLoading()
	{
		OnEventCreateInGameLoading?.Invoke();
	}

	public delegate void OnEvent_DeleteInGameLoading();
	public event OnEvent_DeleteInGameLoading OnEventDeleteInGameLoading;
	public void OnDeleteInGameLoading()
	{
		OnEventDeleteInGameLoading?.Invoke();
	}

	public delegate void OnEvent_DestroyInGameLoading();
	public event OnEvent_DestroyInGameLoading OnEventDestroyInGameLoading;
	public void OnDestroyInGameLoading()
	{
		OnEventDestroyInGameLoading?.Invoke();
	}

	public delegate void OnEvent_InGame_NewGameItemApplyDone();
	public event OnEvent_InGame_NewGameItemApplyDone OnEventInGame_NewGameItemApplyDone;
	public void OnInGame_NewGameItemApplyDone()
	{
		OnEventInGame_NewGameItemApplyDone?.Invoke();
	}

	public delegate void OnEvent_InGame_CombinationMagicianMagicianActionDone(Slot pSlot_01, Slot pSlot_02, Vector2 vMiddlePos);
	public event OnEvent_InGame_CombinationMagicianMagicianActionDone OnEventInGame_CombinationMagicianMagicianActionDone;
	public void OnInGame_CombinationMagicianMagicianActionDone(Slot pSlot_01, Slot pSlot_02, Vector2 vMiddlePos)
	{
		OnEventInGame_CombinationMagicianMagicianActionDone?.Invoke(pSlot_01, pSlot_02, vMiddlePos);
	}

	public delegate void OnEvent_InGame_UpdataMissionUI();
	public event OnEvent_InGame_UpdataMissionUI OnEventInGame_UpdataMissionUI;
	public void OnInGame_UpdataMissionUI()
	{
		OnEventInGame_UpdataMissionUI?.Invoke();
	}

	public delegate void OnEvent_InGame_UpdataStarUI();
	public event OnEvent_InGame_UpdataStarUI OnEventInGame_UpdataStarUI;
	public void OnInGame_UpdataStarUI()
	{
		OnEventInGame_UpdataStarUI?.Invoke();
	}

	public delegate void OnEvent_InGame_UpdateMoveCount(int nMoveCount);
	public event OnEvent_InGame_UpdateMoveCount OnEventInGame_UpdateMoveCount;
	public void OnInGame_UpdateMoveCount(int nMoveCount)
	{
		OnEventInGame_UpdateMoveCount?.Invoke(nMoveCount);
	}

	public delegate void OnEvent_InGame_UseItem_PlusMoveCount();
	public event OnEvent_InGame_UseItem_PlusMoveCount OnEventInGame_UseItem_PlusMoveCount;
	public void OnInGame_UseItem_PlusMoveCount()
	{
		OnEventInGame_UseItem_PlusMoveCount?.Invoke();
	}

	public delegate void OnEvent_InGame_UseItem_PlusMoveParticleStart();
	public event OnEvent_InGame_UseItem_PlusMoveParticleStart OnEventInGame_UseItem_PlusMoveParticleStart;
	public void OnInGame_UseItem_PlusMoveParticleStart()
	{
		OnEventInGame_UseItem_PlusMoveParticleStart?.Invoke();
	}

	public delegate void OnEvent_InGame_UseItem_UpdateMoveCountDone();
	public event OnEvent_InGame_UseItem_UpdateMoveCountDone OnEventInGame_UseItem_UpdateMoveCountDone;
	public void OnInGame_UseItem_UpdateMoveCountDone()
	{
		OnEventInGame_UseItem_UpdateMoveCountDone?.Invoke();
	}

	public delegate void OnEvent_InGame_MoveCountZeroActionDone();
	public event OnEvent_InGame_MoveCountZeroActionDone OnEventInGame_MoveCountZeroActionDone;
	public void OnInGame_MoveCountZeroActionDone()
	{
		OnEventInGame_MoveCountZeroActionDone?.Invoke();
	}

	public delegate void OnEvent_InGame_ItemBuy_Continue_Cancel();
	public event OnEvent_InGame_ItemBuy_Continue_Cancel OnEventInGame_ItemBuy_Continue_Cancel;
	public void OnInGame_ItemBuy_Continue_Cancel()
	{
		OnEventInGame_ItemBuy_Continue_Cancel?.Invoke();
	}

	public delegate void OnEvent_InGame_Result_FailActionDone();
	public event OnEvent_InGame_Result_FailActionDone OnEventInGame_Result_FailActionDone;
	public void OnInGame_Result_FailActionDone()
	{
		OnEventInGame_Result_FailActionDone?.Invoke();
	}

	public delegate void OnEvent_InGame_ShowCombo(SlotUnit pUnit, int nCombo);
	public event OnEvent_InGame_ShowCombo OnEventInGame_ShowCombo;
	public void OnInGame_ShowCombo(SlotUnit pUnit, int nCombo)
	{
		OnEventInGame_ShowCombo?.Invoke(pUnit, nCombo);
	}

	public delegate void OnEvent_InGame_ChangeScore(int nScore);
	public event OnEvent_InGame_ChangeScore OnEventInGame_ChangeScore;
	public void OnInGame_ChangeScore(int nScore)
	{
		OnEventInGame_ChangeScore?.Invoke(nScore);
	}

	public delegate void OnEvent_InGame_CheckRemoveSlotComplete();
	public event OnEvent_InGame_CheckRemoveSlotComplete OnEventInGame_CheckRemoveSlotComplete;
	public void OnInGame_CheckRemoveSlotComplete()
	{
		OnEventInGame_CheckRemoveSlotComplete?.Invoke();
	}

	public delegate void OnEvent_InGame_CheckTimeBomb();
	public event OnEvent_InGame_CheckTimeBomb OnEventInGame_CheckTimeBomb;
	public void OnInGame_CheckTimeBomb()
	{
		OnEventInGame_CheckTimeBomb?.Invoke();
	}

	public delegate void OnEvent_InGame_TimeBombExplosion();
	public event OnEvent_InGame_TimeBombExplosion OnEventInGame_TimeBombExplosion;
	public void OnInGame_TimeBombExplosion()
	{
		OnEventInGame_TimeBombExplosion?.Invoke();
	}

    public delegate void OnEvent_InGame_Continue_TimeBombIncrease();
    public event OnEvent_InGame_Continue_TimeBombIncrease OnEventInGame_Continue_TimeBombIncrease;
    public void OnInGame_Continue_TimeBombIncrease()
    {
        OnEventInGame_Continue_TimeBombIncrease?.Invoke();
    }

    public delegate void OnEvent_InGame_UpdateMissionCount(eMissionType eType);
	public event OnEvent_InGame_UpdateMissionCount OnEventInGame_UpdateMissionCount;
	public void OnInGame_UpdateMissionCount(eMissionType eType)
	{
		OnEventInGame_UpdateMissionCount?.Invoke(eType);
	}

	public delegate void OnEvent_InGame_SlotMoveAndCreate();
	public event OnEvent_InGame_SlotMoveAndCreate OnEventInGame_SlotMoveAndCreate;
	public void OnInGame_SlotMoveAndCreate()
	{
		OnEventInGame_SlotMoveAndCreate?.Invoke();
	}

	public delegate void OnEvent_InGame_DishBreak(Slot pSlot);
	public event OnEvent_InGame_DishBreak OnEventInGame_DishBreak;
	public void OnInGame_DishBreak(Slot pSlot)
	{
		OnEventInGame_DishBreak?.Invoke(pSlot);
	}

	public delegate void OnEvent_InGame_RemoveDishGroup(Disturb_DishGroup pDishGroup, List<Slot> slotList);
	public event OnEvent_InGame_RemoveDishGroup OnEventInGame_RemoveDishGroup;
	public void OnInGame_RemoveDishGroup(Disturb_DishGroup pDishGroup, List<Slot> slotList)
	{
		OnEventInGame_RemoveDishGroup?.Invoke(pDishGroup, slotList);
	}

	public delegate void OnEvent_InGame_GetMission(eMissionType eMissionType);
	public event OnEvent_InGame_GetMission OnEventInGame_GetMission;
	public void OnInGame_GetMission(eMissionType eMissionType)
	{
		OnEventInGame_GetMission?.Invoke(eMissionType);
	}

	public delegate void OnEvent_InGame_CreateGetMissionMoveItem_Bell();
	public event OnEvent_InGame_CreateGetMissionMoveItem_Bell OnEventInGame_CreateGetMissionMoveItem_Bell;
	public void OnInGame_CreateGetMissionMoveItem_Bell()
	{
		OnEventInGame_CreateGetMissionMoveItem_Bell?.Invoke();
	}

	public delegate void OnEvent_InGame_SquareUntiProjectileDone(eUnitType eType, Slot pSlot, Slot pTargetSlot, eUnitShape eShape, eUnitType eSubUnitType);
	public event OnEvent_InGame_SquareUntiProjectileDone OnEventInGame_SquareUntiProjectileDone;
	public void OnInGame_SquareUntiProjectileDone(eUnitType eType, Slot pSlot, Slot pTargetSlot, eUnitShape eShape, eUnitType eSubUnitType)
	{
		OnEventInGame_SquareUntiProjectileDone?.Invoke(eType, pSlot, pTargetSlot, eShape, eSubUnitType);
	}

	public delegate void OnEvent_InGame_GetMissionMoveItem_Done(eMissionType eMissionType);
	public event OnEvent_InGame_GetMissionMoveItem_Done OnEventInGame_GetMissionMoveItem_Done;
	public void OnInGame_GetMissionMoveItem_Done(eMissionType eMissionType)
	{
		OnEventInGame_GetMissionMoveItem_Done?.Invoke(eMissionType);
	}

	public delegate void OnEvent_InGame_MissionNumber_Open(int nOpenNumber);
	public event OnEvent_InGame_MissionNumber_Open OnEventInGame_MissionNumber_Open;
	public void OnInGame_MissionNumber_Open(int nOpenNumber)
	{
		OnEventInGame_MissionNumber_Open?.Invoke(nOpenNumber);
	}

	public delegate void OnEvent_InGame_MissionNumber_Open_DirectionEnd();
	public event OnEvent_InGame_MissionNumber_Open_DirectionEnd OnEventInGame_MissionNumber_Open_DirectionEnd;
	public void OnInGame_MissionNumber_Open_DirectionEnd()
	{
		OnEventInGame_MissionNumber_Open_DirectionEnd?.Invoke();
	}

	public delegate void OnEvent_InGame_CheckMissionComplete();
	public event OnEvent_InGame_CheckMissionComplete OnEventInGame_CheckMissionComplete;
	public void OnInGame_CheckMissionComplete()
	{
		OnEventInGame_CheckMissionComplete?.Invoke();
	}

	public delegate void OnEvent_InGame_UnitShuffle();
	public event OnEvent_InGame_UnitShuffle OnEventInGame_UnitShuffle;
	public void OnInGame_UnitShuffle()
	{
		OnEventInGame_UnitShuffle?.Invoke();
	}

	public delegate void OnEvent_InGame_MissionCompleteActionDone();
	public event OnEvent_InGame_MissionCompleteActionDone OnEventInGame_MissionCompleteActionDone;
	public void OnInGame_MissionCompleteActionDone()
	{
		OnEventInGame_MissionCompleteActionDone?.Invoke();
	}

	public delegate void OnEvent_InGame_MissionCompleteDestroy();
	public event OnEvent_InGame_MissionCompleteDestroy OnEventInGame_MissionCompleteDestroy;
	public void OnInGame_MissionCompleteDestroy()
	{
		OnEventInGame_MissionCompleteDestroy?.Invoke();
	}

	public delegate void OnEvent_InGame_BonusPang_BubbleActionDone();
	public event OnEvent_InGame_BonusPang_BubbleActionDone OnEventInGame_BonusPang_BubbleActionDone;
	public void OnInGame_BonusPang_BubbleActionDone()
	{
		OnEventInGame_BonusPang_BubbleActionDone?.Invoke();
	}

	public delegate void OnEvent_InGame_ItemBuy_Complete(eItemType eType);
	public event OnEvent_InGame_ItemBuy_Complete OnEventInGame_ItemBuy_Complete;
	public void OnInGame_ItemBuy_Complete(eItemType eType)
	{
		OnEventInGame_ItemBuy_Complete?.Invoke(eType);
	}

	public delegate void OnEvent_InGame_Leaderboard_SendScore_Success();
	public event OnEvent_InGame_Leaderboard_SendScore_Success OnEventInGame_Leaderboard_SendScore_Success;
	public void OnInGame_Leaderboard_SendScore_Success()
	{
		OnEventInGame_Leaderboard_SendScore_Success?.Invoke();
	}

	public delegate void OnEvent_InGame_Leaderboard_SendScore_Failed();
	public event OnEvent_InGame_Leaderboard_SendScore_Failed OnEventInGame_Leaderboard_SendScore_Failed;
	public void OnInGame_Leaderboard_SendScore_Failed()
	{
		OnEventInGame_Leaderboard_SendScore_Failed?.Invoke();
	}

	public delegate void OnEvent_InGame_Leaderboard_RequestMyScoreRankingDone();
	public event OnEvent_InGame_Leaderboard_RequestMyScoreRankingDone OnEventInGame_Leaderboard_RequestMyScoreRankingDone;
	public void OnInGame_Leaderboard_RequestMyScoreRankingDone()
	{
		OnEventInGame_Leaderboard_RequestMyScoreRankingDone?.Invoke();
	}

	public delegate void OnEvent_InGame_Leaderboard_SendLevel_Success();
	public event OnEvent_InGame_Leaderboard_SendLevel_Success OnEventInGame_Leaderboard_SendLevel_Success;
	public void OnInGame_Leaderboard_SendLevel_Success()
	{
		OnEventInGame_Leaderboard_SendLevel_Success?.Invoke();
	}

	public delegate void OnEvent_InGame_Leaderboard_SendLevel_Failed();
	public event OnEvent_InGame_Leaderboard_SendLevel_Failed OnEventInGame_Leaderboard_SendLevel_Failed;
	public void OnInGame_Leaderboard_SendLevel_Failed()
	{
		OnEventInGame_Leaderboard_SendLevel_Failed?.Invoke();
	}

	public delegate void OnEvent_InGame_ItemUse(eItemType eType);
	public event OnEvent_InGame_ItemUse OnEventInGame_ItemUse;
	public void OnInGame_ItemUse(eItemType eType)
	{
		OnEventInGame_ItemUse?.Invoke(eType);
	}

	public delegate void OnEvent_InGame_UsedItem(eItemType eType);
	public event OnEvent_InGame_UsedItem OnEventInGame_UsedItem;
	public void OnInGame_UsedItem(eItemType eType)
	{
		OnEventInGame_UsedItem?.Invoke(eType);
	}

	public delegate void OnEvent_InGame_ItemUseCancel();
	public event OnEvent_InGame_ItemUseCancel OnEventInGame_ItemUseCancel;
	public void OnInGame_ItemUseCancel()
	{
		OnEventInGame_ItemUseCancel?.Invoke();
	}

	public delegate void OnEvent_InGame_ItemUse_Tooltip_Destroy();
	public event OnEvent_InGame_ItemUse_Tooltip_Destroy OnEventInGame_ItemUse_Tooltip_Destroy;
	public void OnInGame_ItemUse_Tooltip_Destroy()
	{
		OnEventInGame_ItemUse_Tooltip_Destroy?.Invoke();
	}

	public delegate void OnEvent_InGame_UI_Bottom_Visible(bool IsVisible);
	public event OnEvent_InGame_UI_Bottom_Visible OnEventInGame_UI_Bottom_Visible;
	public void OnInGame_UI_Bottom_Visible(bool IsVisible)
	{
		OnEventInGame_UI_Bottom_Visible?.Invoke(IsVisible);
	}

	public delegate void OnEvent_InGame_Darkly_Start(float fFadeTime, float fDarklyColor);
	public event OnEvent_InGame_Darkly_Start OnEventInGame_Darkly_Start;
	public void OnInGame_Darkly_Start(float fFadeTime, float fDarklyColor)
	{
		OnEventInGame_Darkly_Start?.Invoke(fFadeTime, fDarklyColor);
	}

	public delegate void OnEvent_InGame_Darkly_End(float fFadeTime);
	public event OnEvent_InGame_Darkly_End OnEventInGame_Darkly_End;
	public void OnInGame_Darkly_End(float fFadeTime)
	{
		OnEventInGame_Darkly_End?.Invoke(fFadeTime);
	}
}
