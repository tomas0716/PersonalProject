using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_BonusPang_Bubble : MonoBehaviour
{
    private Slot                m_pTargetSlot       = null;
    private float               m_fDelay            = 0;
    private Transformer_Timer   m_pTimer_Delay      = new Transformer_Timer();
    private Transformer_Vector3 m_pPos              = null;

    private Image               m_pImage            = null;
    private ParticleInfo        m_pParticleInfo     = null;

    void Start()
    {
        ++InGameInfo.Instance.m_nBonusPang_BubbleCount;

        GameObject ob;

        ob = Helper.FindChildGameObject(gameObject, "Image");
        m_pImage = ob.GetComponent<Image>();
        m_pImage.gameObject.SetActive(false);

        Vector3 vStartPos = Vector3.zero;

        switch (AppInstance.Instance.m_pSceneManager.GetCurrSceneType())
        {
            case eSceneType.Scene_InGame:
                {
                    Scene_InGame pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_InGame;

                    if (pScene != null)
                    {
                        vStartPos = pScene.m_pGameController.m_pMainUI.GetMoveCountPos();
                        vStartPos.z = -(float)(ePlaneOrder.Fx_BonusPang);
                    }
                }
                break;

            case eSceneType.Scene_Tool:
                {
                    Scene_Tool pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_Tool;

                    if (pScene != null)
                    {
                        vStartPos = pScene.m_pGameController.m_pMainUI.GetMoveCountPos();
                        vStartPos.z = -(float)(ePlaneOrder.Fx_BonusPang);
                    }
                }
                break;
        }

        //Vector3 vDestPos = Helper.Get3DPosToUIPos(gameObject, Camera.main, m_pTargetSlot.GetPlane_Slot().GetGameObject().transform.position);
        Vector3 vDestPos = m_pTargetSlot.GetPlane_Slot().GetGameObject().transform.position;
        vDestPos.z = -(float)(ePlaneOrder.Fx_BonusPang);

        float fDistance = Vector3.Distance(vStartPos, vDestPos);
        //float fTime = (fDistance / 160.0f) * 0.042f * (1.0f / AppInstance.Instance.m_fMainScale);
        float fTime = (fDistance / 160.0f) * 0.042f;

        TransformerEvent eventValue;
        m_pPos = new Transformer_Vector3(vStartPos);
        eventValue = new TransformerEvent_Vector3(fTime, vDestPos);
        m_pPos.AddEvent(eventValue);
        m_pPos.SetCallback(null, OnDone_Pos);

        eventValue = new TransformerEvent_Timer(m_fDelay);
        m_pTimer_Delay.AddEvent(eventValue);
        m_pTimer_Delay.SetCallback(null, OnDone_Delay);
        m_pTimer_Delay.OnPlay();
    }

    public void Init(Slot pSlot, float fDelay)
    {
        m_pTargetSlot = pSlot;
        m_fDelay = fDelay;
    }

    void Update()
    {
        m_pTimer_Delay.Update(Time.deltaTime);
        m_pPos.Update(Time.deltaTime);
        Vector3 vPos = m_pPos.GetCurVector3();
        //m_pImage.gameObject.transform.localPosition = vPos;
        if (m_pParticleInfo != null)
        {
            m_pParticleInfo.SetPosition(vPos);
        }
    }

    private void OnDone_Delay(TransformerEvent eventValue)
    {
        //m_pImage.gameObject.SetActive(true);
        m_pPos.OnPlay();

        m_pParticleInfo = AppInstance.Instance.m_pParticleManager.LoadParticleSystem("FX_BonusPang", Vector3.zero);
        m_pParticleInfo.SetScale(AppInstance.Instance.m_fMainScale * InGameInfo.Instance.m_fSlotScale * 0.7f);
        Update();

        InGameInfo.Instance.m_nCurrMoveCount -= 1;
        AppInstance.Instance.m_pEventDelegateManager.OnInGame_UpdateMoveCount(InGameInfo.Instance.m_nCurrMoveCount);
        SlotManager.GeneratorBonusPangScore();
    }

    private void OnDone_Pos(TransformerEvent eventValue)
    {
        Helper.OnSoundPlay(eSoundType.Bonus, false);

        AppInstance.Instance.m_pParticleManager.RemoveParticleInfo(m_pParticleInfo);
        
        --InGameInfo.Instance.m_nBonusPang_BubbleCount;

        m_pTargetSlot.GetSlotUnit().CreateBonusPang_Bubble();
        m_pTargetSlot.SetSlotFocus();
        m_pTargetSlot.SetSlotDying(true);
        m_pTargetSlot.SetRemoveSchedule(true);

        if (InGameInfo.Instance.m_nBonusPang_BubbleCount == 0)
        {
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_UpdateMoveCount(0);
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_BonusPang_BubbleActionDone();
        }

        GameObject.Destroy(gameObject);
    }
}
