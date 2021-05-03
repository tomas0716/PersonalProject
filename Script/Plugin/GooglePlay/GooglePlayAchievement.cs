using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayAchievement : MonoBehaviour
{
    private static GooglePlayAchievement m_pInstance = null;
    public static GooglePlayAchievement Instance { get { return m_pInstance; } }

    private int                         m_nRewardCoin           = 0;
    private Dictionary<eItemType, int>  m_RewardItemTable       = new Dictionary<eItemType, int>();

    private Dictionary<ExcelData_AchievementDataInfo, bool>  m_AchievementStateTable = new Dictionary<ExcelData_AchievementDataInfo, bool>();
    private List<ExcelData_AchievementDataInfo> m_CurrRewardAchievementDataInfoList = new List<ExcelData_AchievementDataInfo>();

    void Awake()
    {
        m_pInstance = this;
    }

	private void OnDestroy()
	{
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;
    }

	void Update()
    {
        
    }

    public void LoadAchievements()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (GameDefine.ms_IsGoogleLogin == true)
        {
            if (Social.localUser.authenticated == true)
            {
                PlayGamesPlatform.Instance.LoadAchievements((callback) =>
                {
                    foreach (UnityEngine.SocialPlatforms.IAchievement ach in callback)
                    {
                        PlayGamesAchievement pAchv = ach as PlayGamesAchievement;
                        if (pAchv != null)
                        {
                            string strLog = string.Format("Id : {0}, CurrStep : {1}, TotalStep : {2}, PercentCompleted : {3}, Completed : {4}, Hidden : {5}, IsIncremental : {6}",
                                                            pAchv.id,
                                                            pAchv.currentSteps,
                                                            pAchv.totalSteps,
                                                            pAchv.percentCompleted,
                                                            pAchv.completed,
                                                            pAchv.hidden,
                                                            pAchv.isIncremental);

                            ExcelData_AchievementDataInfo pAchievementDataInfo = ExcelDataManager.Instance.m_pExcelData_AchievementData.FindAchievementDataInfo_byAchievementID(pAchv.id);
                            if (pAchievementDataInfo != null)
                            {
                                pAchievementDataInfo.m_strTitle = pAchv.title;
                                pAchievementDataInfo.m_nCurrCount = pAchv.currentSteps;
                                pAchievementDataInfo.m_nPercentCompleted = pAchv.percentCompleted;
                                pAchievementDataInfo.m_IsCompleted = pAchv.completed;
                                pAchievementDataInfo.m_IsHidden = pAchv.hidden;
                                pAchievementDataInfo.m_IsIncremental = pAchv.isIncremental;
                            }
                        }
                    }

                    CheckUnlockAchievement();

                    AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayAchievementLoaded();
                });
            }
            else
            {
                AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayAchievementLoaded();
            }
        }
        else
        {
            AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayAchievementLoaded();
        }
#else
        AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayAchievementLoaded();
#endif
    }

    public void ShowAchievement()
    {
        if (Social.localUser.authenticated == true)
        {
            ((PlayGamesPlatform)Social.Active).ShowAchievementsUI();
        }
    }

    public void OnAchievement(Dictionary<ExcelData_AchievementDataInfo,int> AchievementDataInfoTable)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (GameDefine.ms_IsGoogleLogin == true)
        {
            if (Social.localUser.authenticated == true)
            {
                if (AchievementDataInfoTable.Count == 0)
                {
                    return;
                }

                m_nRewardCoin = 0;
                m_RewardItemTable.Clear();
                m_AchievementStateTable.Clear();
                m_CurrRewardAchievementDataInfoList.Clear();

                foreach (KeyValuePair<ExcelData_AchievementDataInfo, int> item in AchievementDataInfoTable)
                {
                    m_AchievementStateTable.Add(item.Key, false);
                }

                foreach (KeyValuePair<ExcelData_AchievementDataInfo, int> item in AchievementDataInfoTable)
                {
                    OnAchievement(item.Key, item.Value);
                }
            }
            else
            {
                m_CurrRewardAchievementDataInfoList.Clear();
                AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayAchievementSendComplete(m_CurrRewardAchievementDataInfoList);
            }
        }
        else
        {
            m_CurrRewardAchievementDataInfoList.Clear();
            AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayAchievementSendComplete(m_CurrRewardAchievementDataInfoList);
        }
#else
        AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayAchievementSendComplete(m_CurrRewardAchievementDataInfoList);
#endif
    }

    private void OnAchievement(ExcelData_AchievementDataInfo pAchievementDataInfo, int nAddCount)
    {
        if (pAchievementDataInfo.m_IsCompleted == false && pAchievementDataInfo.m_IsActive == true && pAchievementDataInfo.m_IsHidden == false)
        {
            switch (pAchievementDataInfo.m_eAchievementType)
            {
                case eAchievementType.Goal_Level:
                    {
                        if (pAchievementDataInfo.m_nGoalCount <= SavedGameDataInfo.Instance.m_nLevel)
                        {
                            Social.ReportProgress(pAchievementDataInfo.m_strAchievementID, 100.0f, (success) =>
                            {
                                if (success == true)
                                {
                                    pAchievementDataInfo.m_IsCompleted = true;

									m_nRewardCoin += pAchievementDataInfo.m_nRewardCoin;
									if (pAchievementDataInfo.m_eRewardItemType != eItemType.None)
									{
                                        if (m_RewardItemTable.ContainsKey(pAchievementDataInfo.m_eRewardItemType) == true)
                                        {
                                            m_RewardItemTable[pAchievementDataInfo.m_eRewardItemType] += pAchievementDataInfo.m_nRewardItemCount;
                                        }
                                        else
                                        {
                                            m_RewardItemTable.Add(pAchievementDataInfo.m_eRewardItemType, pAchievementDataInfo.m_nRewardItemCount);
                                        }
									}

									m_AchievementStateTable[pAchievementDataInfo] = true;

                                    m_CurrRewardAchievementDataInfoList.Add(pAchievementDataInfo);

                                    CheckAchievementProcessComplete();

								}
                                else if (success == false)
                                {
                                    m_AchievementStateTable[pAchievementDataInfo] = true;
                                    CheckAchievementProcessComplete();
                                }
                            });
                        }
                    }
                    break;
                default:
                    {
                        bool IsComplete = pAchievementDataInfo.m_nGoalCount <= pAchievementDataInfo.m_nCurrCount + nAddCount ? true : false;
                        nAddCount = pAchievementDataInfo.m_nGoalCount < pAchievementDataInfo.m_nCurrCount + nAddCount ? pAchievementDataInfo.m_nGoalCount - pAchievementDataInfo.m_nCurrCount : nAddCount;
                     
                        PlayGamesPlatform.Instance.IncrementAchievement(pAchievementDataInfo.m_strAchievementID, nAddCount, (success) =>
                        {
                            if (success == true)
                            {
                                pAchievementDataInfo.m_nCurrCount += nAddCount;

                                if (IsComplete == true)
                                {
                                    // 보상 획득

                                    pAchievementDataInfo.m_IsCompleted = true;

                                    m_nRewardCoin += pAchievementDataInfo.m_nRewardCoin;
                                    if (pAchievementDataInfo.m_eRewardItemType != eItemType.None)
                                    {
                                        if (m_RewardItemTable.ContainsKey(pAchievementDataInfo.m_eRewardItemType) == true)
                                        {
                                            m_RewardItemTable[pAchievementDataInfo.m_eRewardItemType] += pAchievementDataInfo.m_nRewardItemCount;
                                        }
                                        else
                                        {
                                            m_RewardItemTable.Add(pAchievementDataInfo.m_eRewardItemType, pAchievementDataInfo.m_nRewardItemCount);
                                        }
                                    }

                                    m_AchievementStateTable[pAchievementDataInfo] = true;
                                    m_CurrRewardAchievementDataInfoList.Add(pAchievementDataInfo);

                                    CheckAchievementProcessComplete();
                                }
                                else
                                {
                                    m_AchievementStateTable[pAchievementDataInfo] = true;
                                    CheckAchievementProcessComplete();
                                }
                            }
                            else
                            {
                                m_AchievementStateTable[pAchievementDataInfo] = true;
                                CheckAchievementProcessComplete();
                            }
                        });
                    }
                    break;
            }
        }
    }

    private void CheckAchievementProcessComplete()
    {
        bool IsComplete = true;
        foreach (KeyValuePair<ExcelData_AchievementDataInfo, bool> item in m_AchievementStateTable)
        {
            if (item.Value == false)
            {
                IsComplete = false;
            }
        }

        if (IsComplete == true)
        {
            StartCoroutine(EndAchievement());
        }
    }

    IEnumerator EndAchievement()
    {
        yield return new WaitForEndOfFrame();

        if (m_nRewardCoin != 0 || m_RewardItemTable.Count != 0)
        {
            SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Coin] += m_nRewardCoin;
            
            foreach (KeyValuePair<eItemType, int> item in m_RewardItemTable)
            {
                SavedGameDataInfo.Instance.m_nItemCounts[(int)item.Key] += item.Value;
            }

            AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save += OnGooglePlaySavedGameDone_Save;

            SavedGameDataInfo.Instance.Save();

            CheckUnlockAchievement();

            foreach (ExcelData_AchievementDataInfo pAchievementDataInfo in m_CurrRewardAchievementDataInfoList)
            {
				if (pAchievementDataInfo.m_pAchievementDataInfo_NextLink != null && pAchievementDataInfo.m_pAchievementDataInfo_NextLink.m_nUnlockLevel <= SavedGameDataInfo.Instance.m_nLevel)
				{
                    PlayGamesPlatform.Instance.ReportProgress(pAchievementDataInfo.m_pAchievementDataInfo_NextLink.m_strAchievementID, 0.0f, (success) =>
					{
                        if (success == true)
                        {
                            pAchievementDataInfo.m_pAchievementDataInfo_NextLink.m_IsHidden = false;
                        }
                        else
                        {
                            OutputLog.Log("Unlock Achievement fail : " + pAchievementDataInfo.m_pAchievementDataInfo_NextLink.m_nID.ToString() + ", " + pAchievementDataInfo.m_pAchievementDataInfo_NextLink.m_strAchievementID);
                        }
					});
				}
			}
        }
        else
        {
            AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayAchievementSendComplete(m_CurrRewardAchievementDataInfoList);
            m_CurrRewardAchievementDataInfoList.Clear();
        }
    }

    public void OnGooglePlaySavedGameDone_Save()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlaySavedGameDone_Save -= OnGooglePlaySavedGameDone_Save;

        AppInstance.Instance.m_pEventDelegateManager.OnUpdateItemState();
        AppInstance.Instance.m_pEventDelegateManager.OnOnUpdateShopItem();
        AppInstance.Instance.m_pEventDelegateManager.OnUpdateCoinInfo();

        AppInstance.Instance.m_pEventDelegateManager.OnGooglePlayAchievementSendComplete(m_CurrRewardAchievementDataInfoList);
        m_CurrRewardAchievementDataInfoList.Clear();
    }

    public void CheckUnlockAchievement()
    {
        int nNumData = ExcelDataManager.Instance.m_pExcelData_AchievementData.GetNumData();

        for (int i = 0; i < nNumData; ++i)
        {
            ExcelData_AchievementDataInfo pAchievementDataInfo = ExcelDataManager.Instance.m_pExcelData_AchievementData.GetData_byIndex(i);

            if (pAchievementDataInfo != null)
            {
                if (pAchievementDataInfo.m_IsHidden == true &&
                    pAchievementDataInfo.m_nUnlockLevel <= SavedGameDataInfo.Instance.m_nLevel &&
                    (pAchievementDataInfo.m_pAchievementDataInfo_PrevLink == null ||
                    (pAchievementDataInfo.m_pAchievementDataInfo_PrevLink != null &&
                    pAchievementDataInfo.m_pAchievementDataInfo_PrevLink.m_IsCompleted == true)) &&
                    m_CurrRewardAchievementDataInfoList.Contains(pAchievementDataInfo) == false)
                {
                    PlayGamesPlatform.Instance.ReportProgress(pAchievementDataInfo.m_strAchievementID, 0.0f, (success) =>
                    {
                        if (success == true)
                        {
                            pAchievementDataInfo.m_IsHidden = false;
                        }
                        else
                        {
                            OutputLog.Log("Unlock Achievement fail : " + pAchievementDataInfo.m_nID.ToString() + ", " + pAchievementDataInfo.m_strAchievementID);
                        }
                    });
                }
            }
        }
    }
}
