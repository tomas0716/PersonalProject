using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayLeaderboard : MonoBehaviour
{
    private static GooglePlayLeaderboard m_pInstance = null;
    public static GooglePlayLeaderboard Instance { get { return m_pInstance; } }

    private int m_nPrevScoreRanking     = -1;
    private int m_nScoreRanking         = -1;

    private Dictionary<int,KeyValuePair<string, long>>  m_RankerTable       = new Dictionary<int, KeyValuePair<string, long>>();        // Key : rank, Value : ( Key : UserID, Value : Score )
    private Dictionary<string, string>                  m_RankerNameTable   = new Dictionary<string, string>();                         // Key : UserID, Value : PlayerName

    private int m_nMaxRanker            = 1000;

    void Awake()
    {
        m_pInstance = this;
    }

    public void ShowLeaderboard()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (GameDefine.ms_IsGoogleLogin == true)
        {
            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI();
        }
#endif
    }

    public void ShowLeaderboard_Score()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (GameDefine.ms_IsGoogleLogin == true)
        {
            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard_score_leaderboard);
        }
#endif
    }

    public void ShowLeaderboard_Level()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (GameDefine.ms_IsGoogleLogin == true)
        {
            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard_level_leaderboard);
        }
#endif
    }

    public void SendLeaderboard_Score(long nSocre)
    {
        if (GameDefine.ms_IsGoogleLeaderboard == false)
        {
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_Leaderboard_SendScore_Success();
            return;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        if (GameDefine.ms_IsGoogleLogin == true)
        {
            if (Social.localUser.authenticated == true)
            {
                Social.ReportScore(nSocre, GPGSIds.leaderboard_score_leaderboard, (bool IsSuccess) =>
                {
                    if (IsSuccess == true)
                    {
                        PlayGamesPlatform.Instance.LoadScores(GPGSIds.leaderboard_score_leaderboard, LeaderboardStart.PlayerCentered, 1, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, (data) =>
                        {
                            LeaderboardScoreData pData = data;

                            if (pData.Valid == true)
                            {
                                if (pData.Scores.Length != 0)
                                {
                                    foreach (UnityEngine.SocialPlatforms.IScore pScore in pData.Scores)
                                    {
                                        PlayGamesScore pPlayGamesScore = pScore as PlayGamesScore;

                                        if (pPlayGamesScore != null && pPlayGamesScore.userID == GooglePlayGamesClient.Instance.UserID)
                                        {
                                            m_nPrevScoreRanking = m_nScoreRanking;
                                            m_nScoreRanking = pPlayGamesScore.rank;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // 실패여도 그냥 넘어간다.
                            }

                            AppInstance.Instance.m_pEventDelegateManager.OnInGame_Leaderboard_SendScore_Success();
                        });
                    }
                    else
                    {
                        AppInstance.Instance.m_pEventDelegateManager.OnInGame_Leaderboard_SendScore_Failed();
                    }
                });
            }
            else
            {
                AppInstance.Instance.m_pEventDelegateManager.OnInGame_Leaderboard_SendScore_Failed();
            }
        }
        else
        {
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_Leaderboard_SendScore_Success();
        }
#else
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_Leaderboard_SendScore_Success();
#endif
	}

	public void SendLeaderboard_Level(int nLevel)
    {
        if (GameDefine.ms_IsGoogleLeaderboard == false)
        {
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_Leaderboard_SendLevel_Success();
            return;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        if (GameDefine.ms_IsGoogleLogin == true)
        {
            if (Social.localUser.authenticated == true)
            {
                Social.ReportScore(nLevel, GPGSIds.leaderboard_level_leaderboard, (bool IsSuccess) =>
                {
                    if (IsSuccess == true)
                    {
                        AppInstance.Instance.m_pEventDelegateManager.OnInGame_Leaderboard_SendLevel_Success();
                    }
                    else
                    {
                        AppInstance.Instance.m_pEventDelegateManager.OnInGame_Leaderboard_SendLevel_Failed();
                    }
                });
            }
            else
            {
                AppInstance.Instance.m_pEventDelegateManager.OnInGame_Leaderboard_SendLevel_Failed();
            }
        }
        else 
        {
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_Leaderboard_SendLevel_Success();
        }
#else
        AppInstance.Instance.m_pEventDelegateManager.OnInGame_Leaderboard_SendLevel_Success();
#endif
    }

	public void GetMyRank()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		if (GameDefine.ms_IsGoogleLogin == true)
		{
			if (Social.localUser.authenticated == true)
			{
				PlayGamesPlatform.Instance.LoadScores(GPGSIds.leaderboard_score_leaderboard, LeaderboardStart.PlayerCentered, 1, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, (data) =>
				{
					LeaderboardScoreData pData = data;

					if (pData.Valid == true)
					{
                        bool IsFind = false;
						if (pData.Scores.Length != 0)
						{
							foreach (UnityEngine.SocialPlatforms.IScore pScore in pData.Scores)
							{
								PlayGamesScore pPlayGamesScore = pScore as PlayGamesScore;

								if (pPlayGamesScore != null && pPlayGamesScore.userID == GooglePlayGamesClient.Instance.UserID)
								{
                                    IsFind = true;
									m_nPrevScoreRanking = m_nScoreRanking;
									m_nScoreRanking = pPlayGamesScore.rank;
								}
							}
						}

                        if(IsFind == false)
                        {
                            Helper.FirebaseLogEvent("Rank_No");
                        }
					}
                    else 
                    {
                        Helper.FirebaseLogEvent("Rank_No");
                    }

					AppInstance.Instance.m_pEventDelegateManager.OnLeaderboard_GetMyRank();
				});
			}
			else
			{
                Helper.FirebaseLogEvent("Rank_NotLogin");
                AppInstance.Instance.m_pEventDelegateManager.OnLeaderboard_GetMyRank();
            }
		}
		else
		{
            AppInstance.Instance.m_pEventDelegateManager.OnLeaderboard_GetMyRank();
        }
#else
        AppInstance.Instance.m_pEventDelegateManager.OnLeaderboard_GetMyRank();
#endif
    }

	public void GetRanker()
    {
        if (Social.localUser.authenticated == true)
        {
            List<string> UserIDList = new List<string>();

            PlayGamesPlatform.Instance.LoadScores(GPGSIds.leaderboard_score_leaderboard, LeaderboardStart.TopScores, 25, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, (data) =>
            {
                bool IsNextRequest = true;
                LeaderboardScoreData pData = data;

                if (pData.Valid == true)
                {
                    m_nScoreRanking = pData.PlayerScore.rank;

                    if (pData.Scores.Length != 0)
                    {
                        foreach (UnityEngine.SocialPlatforms.IScore pScore in pData.Scores)
                        {
                            PlayGamesScore pPlayGamesScore = pScore as PlayGamesScore;

                            if (pPlayGamesScore != null && m_RankerTable.ContainsKey(pPlayGamesScore.rank) == false)
                            {
                                m_RankerTable.Add(pPlayGamesScore.rank, new KeyValuePair<string, long>(pPlayGamesScore.userID, pPlayGamesScore.value));
                                UserIDList.Add(pPlayGamesScore.userID);

                                if (pPlayGamesScore.userID == GooglePlayGamesClient.Instance.UserID)
                                {
                                    m_nScoreRanking = pPlayGamesScore.rank;
                                    IsNextRequest = false;
                                }
                            }
                        }
                    }

                    RequestLoadUser(UserIDList.ToArray());

                    if (IsNextRequest == true && pData.NextPageToken != null)
                    {
                        LoadMoreScores(pData.NextPageToken);
                    }
                }
            });
        }
    }
    
    private void LoadMoreScores(ScorePageToken pScorePageToken)
    {
        List<string> UserIDList = new List<string>();

        PlayGamesPlatform.Instance.LoadMoreScores(pScorePageToken, 25, (data) =>
        {
            bool IsNextRequest = true;
            LeaderboardScoreData pData = data;

            if (pData.Valid == true)
            {
                if (pData.Scores.Length != 0)
                {
                    foreach (UnityEngine.SocialPlatforms.IScore pScore in pData.Scores)
                    {
                        PlayGamesScore pPlayGamesScore = pScore as PlayGamesScore;
                        if (pPlayGamesScore != null && m_RankerTable.ContainsKey(pPlayGamesScore.rank) == false)
                        {
                            m_RankerTable.Add(pPlayGamesScore.rank, new KeyValuePair<string, long>(pPlayGamesScore.userID, pPlayGamesScore.value));
                            UserIDList.Add(pPlayGamesScore.userID);

                            if (m_nMaxRanker <= pPlayGamesScore.rank)
                            {
                                IsNextRequest = false;
                            }

                            if (pPlayGamesScore.userID == GooglePlayGamesClient.Instance.UserID)
                            {
                                IsNextRequest = false;
                            }
                        }
                    }
                }

                RequestLoadUser(UserIDList.ToArray());

                if (IsNextRequest == true && pData.NextPageToken != null)
                {
                    LoadMoreScores(pData.NextPageToken);
                }
            }
        });
    }

    private void RequestLoadUser(string [] userIDs)
    {
        PlayGamesPlatform.Instance.LoadUsers(userIDs, (Data) =>
        {
            UnityEngine.SocialPlatforms.IUserProfile[] userProfiles = Data;

            if (userProfiles != null)
            {
                foreach (UnityEngine.SocialPlatforms.IUserProfile pUserProfile in userProfiles)
                {
                    if (m_RankerNameTable.ContainsKey(pUserProfile.id) == false)
                    {
                        m_RankerNameTable.Add(pUserProfile.id, pUserProfile.userName);
                    }
                }

                foreach (KeyValuePair<int, KeyValuePair<string, long>> item in m_RankerTable)
                {
                    OutputLog.Log("Ranker rank : " + item.Key.ToString() + ", name : " + m_RankerNameTable[item.Value.Key] + ", score : " + item.Value.Value);
                }
            }
        });
    }

    public Dictionary<int, KeyValuePair<string, long>> GetRankerTable()
    {
        return m_RankerTable;
    }

    public string GetUserName(string strUserID)
    {
        if (m_RankerNameTable.ContainsKey(strUserID) == true)
        {
            return m_RankerNameTable[strUserID];
        }

        return "";
    }

    public int GetPrevScoreRanking()
    {
        return m_nPrevScoreRanking;
    }

    public int GetScoreRanking()
    {
        return m_nScoreRanking;
    }

    public int GetMyRanking(int nScore, int nPrevRanking)
    {
        for (int i = nPrevRanking; i >= 1; --i)
        {
            if (m_RankerTable.ContainsKey(i) == true && m_RankerTable[i].Value < nScore)
            {
                return i;
            }
        }

        return m_nScoreRanking;
    }
}
