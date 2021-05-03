using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_UI_ItemUse_SlotAttack_Direction : MonoBehaviour
{
    private GameObject          m_pGameObject_Image_01       = null;
    private GameObject          m_pGameObject_Image_02       = null;
    private GameObject          m_pGameObject_Image_Effect   = null;

    private Transformer_Timer   m_pTimer                    = new Transformer_Timer();
    private Transformer_Scalar  m_pScale_Effect             = new Transformer_Scalar(1);

    void Awake()
    {
        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Parent");
        Vector3 vUIPos = Helper.Get3DPosToUIPos(gameObject, Camera.main, InGameInfo.Instance.m_pSlot_InGameItemUse_SlotAttack.GetPlane_Slot().GetGameObject().transform.position);
        ob.transform.localPosition = vUIPos;

        m_pGameObject_Image_01 = Helper.FindChildGameObject(ob, "Image_01");
        m_pGameObject_Image_02 = Helper.FindChildGameObject(ob, "Image_02");
        m_pGameObject_Image_Effect = Helper.FindChildGameObject(ob, "Image_Effect");

        TransformerEvent_Timer eventValue;
        eventValue = new TransformerEvent_Timer(GameDefine.ms_fInGameItemUse_SlotAttack_DirectionTime);
        m_pTimer.AddEvent(eventValue);
        m_pTimer.SetCallback(null, OnDone_Timer_SlotAttack);
        m_pTimer.OnPlay();
    }

    void Update()
    {
        m_pTimer.Update(Time.deltaTime);
        m_pScale_Effect.Update(Time.deltaTime);
        float fScale = m_pScale_Effect.GetCurScalar();
        m_pGameObject_Image_Effect.transform.localScale = new Vector3(fScale, fScale, 1);
    }

    private void OnDone_Timer_SlotAttack(TransformerEvent eventValue)
    {
        m_pGameObject_Image_01.SetActive(false);
        m_pGameObject_Image_02.SetActive(true);
        m_pGameObject_Image_Effect.SetActive(true);

        eventValue = new TransformerEvent_Scalar(0.1f, 2.2f);
        m_pScale_Effect.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.2f, 1);
        m_pScale_Effect.AddEvent(eventValue);
        m_pScale_Effect.OnPlay();

        m_pTimer.OnReset();
        eventValue = new TransformerEvent_Timer(GameDefine.ms_fInGameItemUse_SlotAttack_Delay - GameDefine.ms_fInGameItemUse_SlotAttack_DirectionTime);
        m_pTimer.AddEvent(eventValue);
        m_pTimer.SetCallback(null, OnDone_Timer_Done);
        m_pTimer.OnPlay();
    }

    private void OnDone_Timer_Done(TransformerEvent eventValue)
    {
        GameObject.Destroy(gameObject);
    }
}
