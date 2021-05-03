using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_UI_ItemUse_StripeHV_Direction : MonoBehaviour
{
    private GameObject          m_pGameObject_Image         = null;

    private Transformer_Timer   m_pTimer                    = new Transformer_Timer();
    private Transformer_Scalar  m_pScale                    = new Transformer_Scalar(0);

    void Awake()
    {
        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Parent");
        Vector3 vUIPos = Helper.Get3DPosToUIPos(gameObject, Camera.main, InGameInfo.Instance.m_pSlot_InGameItemUse_StripeHV.GetPlane_Slot().GetGameObject().transform.position);
        ob.transform.localPosition = vUIPos;

        m_pGameObject_Image = Helper.FindChildGameObject(ob, "Image");

        TransformerEvent eventValue;
        eventValue = new TransformerEvent_Timer(GameDefine.ms_fInGameItemUse_Stripe_DirectionTime);
        m_pTimer.AddEvent(eventValue);
        m_pTimer.SetCallback(null, OnDone_Timer_StripeHV);
        m_pTimer.OnPlay();

        eventValue = new TransformerEvent_Scalar(0.3f, 1.0f);
        m_pScale.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(2.0f, 1.0f);
        m_pScale.AddEvent(eventValue);
        m_pScale.OnPlay();

        Update();
    }

    void Update()
    {
        m_pTimer.Update(Time.deltaTime);
        m_pScale.Update(Time.deltaTime);
        float fScale = m_pScale.GetCurScalar();
        m_pGameObject_Image.transform.localScale = new Vector3(fScale, fScale, 1);
    }

    private void OnDone_Timer_StripeHV(TransformerEvent eventValue)
    {
        GameObject.Destroy(gameObject);
    }
}
