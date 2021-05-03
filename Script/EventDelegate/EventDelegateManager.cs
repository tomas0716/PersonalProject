using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EventDelegateManager
{

	public delegate void OnEvent_StoreVersionCheckComplete(bool IsSameVersion);
	public event OnEvent_StoreVersionCheckComplete OnEventStoreVersionCheckComplete;
	public void OnStoreVersionCheckComplete(bool IsSameVersion)
	{
		OnEventStoreVersionCheckComplete?.Invoke(IsSameVersion);
	}

	// 구글 플레이 로그인 결과
	public delegate void OnEvent_GooglePlayLoginResult(bool IsResult, string strUserID, string strEmail);
	public event OnEvent_GooglePlayLoginResult OnEventGooglePlayLoginResult;
	public void OnGooglePlayLoginResult(bool IsResult, string strUserID, string strEmail)
	{
		OnEventGooglePlayLoginResult?.Invoke(IsResult, strUserID, strEmail);
	}

	public delegate void OnEvent_AlreadyGooglePlayLoginResult(bool IsResult);
	public event OnEvent_AlreadyGooglePlayLoginResult OnEventAlreadyGooglePlayLoginResult;
	public void OnAlreadyGooglePlayLoginResult(bool IsResult)
	{
		OnEventAlreadyGooglePlayLoginResult?.Invoke(IsResult);
	}

	public delegate void OnEvent_Leaderboard_GetMyRank();
	public event OnEvent_Leaderboard_GetMyRank OnEventLeaderboard_GetMyRank;
	public void OnLeaderboard_GetMyRank()
	{
		OnEventLeaderboard_GetMyRank?.Invoke();
	}

	public delegate void OnEvent_GooglePlaySavedGameDone_Save();
	public event OnEvent_GooglePlaySavedGameDone_Save OnEventGooglePlaySavedGameDone_Save;
	public void OnGooglePlaySavedGameDone_Save()
	{
		OnEventGooglePlaySavedGameDone_Save?.Invoke();
	}

	public delegate void OnEvent_GooglePlaySavedGameDone_Load();
	public event OnEvent_GooglePlaySavedGameDone_Load OnEventGooglePlaySavedGameDone_Load;
	public void OnGooglePlaySavedGameDone_Load()
	{
		OnEventGooglePlaySavedGameDone_Load?.Invoke();
	}

	public delegate void OnEvent_GooglePlayAchievementLoaded();
	public event OnEvent_GooglePlayAchievementLoaded OnEventGooglePlayAchievementLoaded;
	public void OnGooglePlayAchievementLoaded()
	{
		OnEventGooglePlayAchievementLoaded?.Invoke();
	}

	public delegate void OnEvent_GooglePlayAchievementSendComplete(List<ExcelData_AchievementDataInfo> currRewardAchievementDataInfoList);
	public event OnEvent_GooglePlayAchievementSendComplete OnEventGooglePlayAchievementSendComplete;
	public void OnGooglePlayAchievementSendComplete(List<ExcelData_AchievementDataInfo> currRewardAchievementDataInfoList)
	{
		OnEventGooglePlayAchievementSendComplete?.Invoke(currRewardAchievementDataInfoList);
	}

	public delegate void OnEvent_CreateLoading();
	public event OnEvent_CreateLoading OnEventCreateLoading;
	public void OnCreateLoading()
	{
		OnEventCreateLoading?.Invoke();
	}

	public delegate void OnEvent_DeleteLoading();
	public event OnEvent_DeleteLoading OnEventDeleteLoading;
	public void OnDeleteLoading()
	{
		OnEventDeleteLoading?.Invoke();
	}

	public delegate void OnEvent_DeleteModal();
	public event OnEvent_DeleteModal OnEventDeleteModal;
	public void OnDeleteModal()
	{
		OnEventDeleteModal?.Invoke();
	}

	public delegate void OnEvent_UpdateItemState();
	public event OnEvent_UpdateItemState OnEventUpdataItemState;
	public void OnUpdateItemState()
	{
		OnEventUpdataItemState?.Invoke();
	}

	public delegate void OnEvent_ItemBuy(eItemType eType);
	public event OnEvent_ItemBuy OnEventItemBuy;
	public void OnItemBuy(eItemType eType)
	{
		OnEventItemBuy?.Invoke(eType);
	}

	public delegate void OnEvent_ShopOpen();
	public event OnEvent_ShopOpen OnEventShopOpen;
	public void OnShopOpen()
	{
		OnEventShopOpen?.Invoke();
	}

	public delegate void OnEvent_UpdateShopItem();
	public event OnEvent_UpdateShopItem OnEventUpdateShopItem;
	public void OnOnUpdateShopItem()
	{
		OnEventUpdateShopItem?.Invoke();
	}

	public delegate void OnEvent_UpdateHeartInfo();
	public event OnEvent_UpdateHeartInfo OnEventUpdateHeartInfo;
	public void OnUpdateHeartInfo()
	{
		OnEventUpdateHeartInfo?.Invoke();
	}

	public delegate void OnEvent_UpdateCoinInfo();
	public event OnEvent_UpdateCoinInfo OnEventUpdateCoinInfo;
	public void OnUpdateCoinInfo()
	{
		OnEventUpdateCoinInfo?.Invoke();
	}

	public delegate void OnEvent_GoogleRewardsAdsLoadFailed();
	public event OnEvent_GoogleRewardsAdsLoadFailed OnEventGoogleRewardsAdsLoadFailed;
	public void OnGoogleRewardsAdsLoadFailed()
	{
		OnEventGoogleRewardsAdsLoadFailed?.Invoke();
	}

	public delegate void OnEvent_GoogleRewardsAdsComplete();
	public event OnEvent_GoogleRewardsAdsComplete OnEventGoogleRewardsAdsComplete;
	public void OnGoogleRewardsAdsComplete()
	{
		OnEventGoogleRewardsAdsComplete?.Invoke();
	}

	public delegate void OnEvent_GoogleProcessPurchaseFailed(ExcelData_Google_IAPDataInfo pIAPDataInfo);
	public event OnEvent_GoogleProcessPurchaseFailed OnEventGoogleProcessPurchaseFailed;
	public void OnGoogleProcessPurchaseFailed(ExcelData_Google_IAPDataInfo pIAPDataInfo)
	{
		OnEventGoogleProcessPurchaseFailed?.Invoke(pIAPDataInfo);
	}

	public delegate void OnEvent_GoogleProcessPurchaseDone(ExcelData_Google_IAPDataInfo pIAPDataInfo);
	public event OnEvent_GoogleProcessPurchaseDone OnEventGoogleProcessPurchaseDone;
	public void OnGoogleProcessPurchaseDone(ExcelData_Google_IAPDataInfo pIAPDataInfo)
	{
		OnEventGoogleProcessPurchaseDone?.Invoke(pIAPDataInfo);
	}

	public delegate void OnEvent_BKGSoundDone();
	public event OnEvent_BKGSoundDone OnEventBKGSoundDone;
	public void OnBKGSoundDone()
	{
		OnEventBKGSoundDone?.Invoke();
	}

	public delegate void OnEvent_ScreenShot(Texture2D pTex);
	public event OnEvent_ScreenShot OnEventScreenShot;
	public void OnScreenShot(Texture2D pTex)
	{
		OnEventScreenShot?.Invoke(pTex);
	}

	public delegate void OnEvent_DestroyScreenShot();
	public event OnEvent_DestroyScreenShot OnEventDestroyScreenShot;
	public void OnDestroyScreenShot()
	{
		OnEventDestroyScreenShot?.Invoke();
	}

	public delegate void OnEvent_InGameStartHeartMinus();
	public event OnEvent_InGameStartHeartMinus OnEventInGameStartHeartMinus;
	public void OnInGameStartHeartMinus()
	{
		OnEventInGameStartHeartMinus?.Invoke();
	}

	public delegate void OnEvent_VideoRewardHeartPlus();
	public event OnEvent_VideoRewardHeartPlus OnEventVideoRewardHeartPlus;
	public void OnVideoRewardHeartPlus()
	{
		OnEventVideoRewardHeartPlus?.Invoke();
	}

	public delegate void OnEvent_VideoRewardGetItemRandom();
	public event OnEvent_VideoRewardGetItemRandom OnEventVideoRewardGetItemRandom;
	public void OnVideoRewardGetItemRandom()
	{
		OnEventVideoRewardGetItemRandom?.Invoke();
	}

	public delegate void OnEvent_LevelClearHeartPlus();
	public event OnEvent_LevelClearHeartPlus OnEventLevelClearHeartPlus;
	public void OnLevelClearHeartPlus()
	{
		OnEventLevelClearHeartPlus?.Invoke();
	}

	public delegate void OnEvent_CoinToHeartCharge();
	public event OnEvent_CoinToHeartCharge OnEventCoinToHeartCharge;
	public void OnCoinToHeartCharge()
	{
		OnEventCoinToHeartCharge?.Invoke();
	}

	public delegate void OnEvent_RewardPopupClosed();
	public event OnEvent_RewardPopupClosed OnEventRewardPopupClosed;
	public void OnRewardPopupClosed()
	{
		OnEventRewardPopupClosed?.Invoke();
	}

	public delegate void OnEvent_ShopItemAdsComplete();
	public event OnEvent_ShopItemAdsComplete OnEventShopItemAdsComplete;
	public void OnShopItemAdsComplete()
	{
		OnEventShopItemAdsComplete?.Invoke();
	}


	public delegate void OnEvent_RouletteComplete();
	public event OnEvent_RouletteComplete OnEventRouletteComplete;
	public void OnRouletteComplete()
	{
		OnEventRouletteComplete?.Invoke();
	}


	public delegate void OnEvent_AttendaceComplete();
	public event OnEvent_AttendaceComplete OnEventAttendaceComplete;
	public void OnAttendaceComplete()
	{
		OnEventAttendaceComplete?.Invoke();
	}

	public delegate void OnEvent_NextDay();
	public event OnEvent_NextDay OnEventNextDay;
	public void OnNextDay()
	{
		OnEventNextDay?.Invoke();
	}

    public delegate void OnEvent_HardwareBackButtonClick();
    public event OnEvent_HardwareBackButtonClick OnEventHardwareBackButtonClick;
    public void OnHardwareBackButtonClick()
    {
        OnEventHardwareBackButtonClick?.Invoke();
    }
}
