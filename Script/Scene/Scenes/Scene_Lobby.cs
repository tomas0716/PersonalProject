using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene_Lobby : CScene
{
    private int m_nPresentRewardPopupOpenCount = 0;

    static private bool ms_IsFirstEnterLobby = true;

    protected override void Initialize()
	{
        Application.targetFrameRate = 60;

        AppInstance.Instance.m_pEventDelegateManager.OnDeleteInGameLoading();

        if (ms_IsFirstEnterLobby == true)
        {
            ms_IsFirstEnterLobby = false;

            int nScoreRanking = GooglePlayLeaderboard.Instance.GetScoreRanking();
            int nScore = (int)SavedGameDataInfo.Instance.m_nScore;

            if (nScore > 0 && nScoreRanking == -1 && Social.localUser.authenticated == true)
            {
                // Leaderboard Not Open
            }
        }

        AppInstance.Instance.m_pEventDelegateManager.OnEventDestroyInGameLoading += OnDestroyInGameLoading;
    }

    protected override void Destroy()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventDestroyInGameLoading -= OnDestroyInGameLoading;
        AppInstance.Instance.m_pEventDelegateManager.OnEventRewardPopupClosed -= OnRewardPopupClosed;
    }

    protected override void Inner_Update()
	{
    }

    protected override void Inner_FixedUpdate()
    {
    }

    private void OnDestroyInGameLoading()
    {
        Helper.OnSoundPlay(eSoundType.Lobby, true);

        bool IsOpenAttendance = true;
        if (GameInfo.Instance.m_IsInGameSuccess == true)
        {
            if (GameInfo.Instance.m_IsCurrLevelPlay == true && SavedGameDataInfo.Instance.m_nLevel == 6)
            {
                IsOpenAttendance = false;
                m_nPresentRewardPopupOpenCount = 2;

                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/RewardPopup");

                ob = GameObject.Instantiate(ob);
                RewardPopup pRewardPopup = ob.GetComponent<RewardPopup>();
                pRewardPopup.SetRewardInfo(eRewardSubject.eNewUser_Present, eItemType.Coin, GameDefine.ms_nFiveLevelClear_Present_Coin);

                ob = GameObject.Instantiate(ob);
                pRewardPopup = ob.GetComponent<RewardPopup>();
                pRewardPopup.SetRewardInfo(eRewardSubject.eNewUser_Present, eItemType.Heart, GameDefine.ms_nFiveLevelClear_Present_Heart);

                AppInstance.Instance.m_pEventDelegateManager.OnEventRewardPopupClosed += OnRewardPopupClosed;
            }
        }

        GameInfo.Instance.m_IsInGameSuccess = false;

        if (IsOpenAttendance == true)
        {
            if (SavedGameDataInfo.Instance.m_nLevel >= 6)
            {
                if (SavedGameDataInfo.Instance.m_IsGetAttendance == false)
                {
                    GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/Attendance");
                    ob = GameObject.Instantiate(ob);
                    Attendance pAttendance = ob.GetComponent<Attendance>();
                    pAttendance.SetFirstOpen();
                }
            }
        }
    }

    public void OnRewardPopupClosed()
    {
        --m_nPresentRewardPopupOpenCount;

        if (m_nPresentRewardPopupOpenCount == 0)
        {
            if (SavedGameDataInfo.Instance.m_nLevel >= 6)
            {
                if (SavedGameDataInfo.Instance.m_IsGetAttendance == false)
                {
                    GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/Attendance");
                    ob = GameObject.Instantiate(ob);
                    Attendance pAttendance = ob.GetComponent<Attendance>();
                    pAttendance.SetFirstOpen();
                }
            }
        }
    }
}