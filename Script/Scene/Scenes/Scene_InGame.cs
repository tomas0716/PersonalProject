using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scene_InGame : CScene
{
    public  GameController          m_pGameController           = null;

    protected override void Initialize()
    {
        InGameInfo.Instance.m_IsInGameIntroing = true;

        m_pGameController = new GameController();
        StartCoroutine(InitializeGameController());

        AppInstance.Instance.m_pEventDelegateManager.OnEventDestroyInGameLoading += OnDestroyInGameLoading;
    }

    private IEnumerator InitializeGameController()
    {
        if (m_pGameController.m_pSlotManager.m_IsInitialize == false)
        {
            yield return null;
        }

        //SavedGameDataInfo.Instance.ResetMapDataInfo(SavedGameDataInfo.Instance.m_pMapDataInfo);
        m_pGameController.m_pSlotManager.CreateMap();
        m_pGameController.m_pSlotManager.CheckSlotMoveInfo();
        m_pGameController.m_pSlotManager.CreateSlotMove();
        m_pGameController.m_pSlotManager.CreateMission(SavedGameDataInfo.Instance.m_MapDataMissionList);
        m_pGameController.m_pSlotManager.CreateMissionAtUnitType();
        m_pGameController.m_pSlotManager.CreateMissionAtUnitShape();
        m_pGameController.m_pSlotManager.CreateMissionAtDisturb(SavedGameDataInfo.Instance.m_MapDataMissionList);

        m_pGameController.m_pSlotManager.Initialize_PossibleMoveSlotCheck();
    }

    protected override void Destroy()
    {
        m_pGameController.OnDestroy();

        AppInstance.Instance.m_pEventDelegateManager.OnEventDestroyInGameLoading -= OnDestroyInGameLoading;
    }

    protected override void Inner_Update()
    {
        m_pGameController.Update();
    }

    protected override void Inner_FixedUpdate()
    {
        m_pGameController.FixedUpdate();
    }

    public void OnDestroyInGameLoading()
    {
        // 일단 클릭 가능하게 한다.
        // 연출후에 다시 넣자
        AppInstance.Instance.m_pEventDelegateManager.OnInGameIntroStart();
    }
}