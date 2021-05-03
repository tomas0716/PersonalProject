using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI_MoveWarning : MonoBehaviour
{
    private Image               m_pImage        = null;
    private Transformer_Scalar  m_pAlpha        = new Transformer_Scalar(0);

    void Start()
    {
        GameObject ob = Helper.FindChildGameObject(gameObject, "Image");
        m_pImage = ob.GetComponent<Image>();

        float fInterval = 0.4f;

        TransformerEvent_Scalar eventValue;
        eventValue = new TransformerEvent_Scalar(0, 0);
        m_pAlpha.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(fInterval * 1, 1);
        m_pAlpha.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(fInterval * 2, 0);
        m_pAlpha.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(fInterval * 3, 1);
        m_pAlpha.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(fInterval * 4, 0);
        m_pAlpha.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(fInterval * 5, 1);
        m_pAlpha.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(fInterval * 6, 0);
        m_pAlpha.AddEvent(eventValue);
        m_pAlpha.SetCallback(null, OnDone_Alpha);
        m_pAlpha.OnPlay();

        Helper.OnSoundPlay(eSoundType.OutofMove5, false);
    }

    void Update()
    {
        m_pAlpha.Update(Time.deltaTime);
        float fAlpha = m_pAlpha.GetCurScalar();
        m_pImage.color = new Color(1,1,1,fAlpha);
    }

    private void OnDone_Alpha(TransformerEvent eventValue)
    {
        GameObject.Destroy(gameObject);
    }
}
