using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Tool : CScene
{
    public GameController m_pGameController = null;

    protected override void Initialize()
    {
#if UNITY_EDITOR
        new Tool_Info();

        AppInstance.Instance.m_pSceneManager.SetCurrScene(this);
        AppInstance.Instance.m_pSceneManager.SetCurrSceneType(eSceneType.Scene_Tool);

        AppInstance.Instance.m_pSoundPlayer.CreateSound();

        AppInstance.Instance.m_pSoundPlayer.StopBKG();

        InGameInfo.Instance.InGameStart_Reset();
        AppInstance.Instance.m_pInGameRandom.InGamePlay();

        m_pGameController = new GameController();
        m_pGameController.m_pGameObject_LastLayer.SetActive(false);

        Tool_WindowCollect.Instance.Init();
#endif
    }

    protected override void Destroy()
    {
        m_pGameController.OnDestroy();
    }

    protected override void Inner_Update()
    {
        m_pGameController.Update();
    }

    protected override void Inner_FixedUpdate()
    {
        m_pGameController.FixedUpdate();
    }
}
