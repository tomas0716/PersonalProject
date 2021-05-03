using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Middle : CScene
{
    private Transformer_Scalar m_pTimer = new Transformer_Scalar(0);
    
    protected override void Initialize()
    {
        if (GameInfo.Instance.m_IsChangeLanguage == false)
        {
            m_pTimer.OnReset();
            TransformerEvent_Scalar eventValue = new TransformerEvent_Scalar(0.2f, 0);
            m_pTimer.AddEvent(eventValue);
            m_pTimer.SetCallback(null, OnTimerDone_Normal);
            m_pTimer.OnPlay();
        }
        else
        {
            GameInfo.Instance.m_IsChangeLanguage = false;

            m_pTimer.OnReset();
            TransformerEvent_Scalar eventValue = new TransformerEvent_Scalar(3, 0);
            m_pTimer.AddEvent(eventValue);
            m_pTimer.SetCallback(null, OnTimerDone_ChangeLanguage);
            m_pTimer.OnPlay();
        }
    }

    protected override void Destroy()
    {

    }

    protected override void Inner_Update()
    {
        m_pTimer.Update(Time.deltaTime);
    }

    protected override void Inner_FixedUpdate()
    {
    }
  
    public void OnTimerDone_Normal(TransformerEvent eventValue)
    {
        AppInstance.Instance.m_pSceneManager.ChangeScene(eSceneType.Scene_Lobby, false);
    }

    public void OnTimerDone_ChangeLanguage(TransformerEvent eventValue)
    {
        AppInstance.Instance.m_pSceneManager.ChangeScene(eSceneType.Scene_Lobby, false);
    }
}