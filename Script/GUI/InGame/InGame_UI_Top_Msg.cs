using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI_Top_Msg : MonoBehaviour
{
    private Queue<string>           m_MsgQueue      = new Queue<string>();
    private string                  m_strCurrMsg    = "";
    private Image                   m_pImage        = null;
    private Text                    m_pText         = null;

    private Transformer_Scalar      m_pAlpha        = new Transformer_Scalar(0);
    private Transformer_Vector3     m_pPos          = new Transformer_Vector3(new Vector3(720,0,0));

    private enum eState
    {
        None,
        Begin,
        Play,
        Ending,
    }

    private eState                  m_eState        = eState.None;

    void Start()
    {
        GameObject ob;

        ob = Helper.FindChildGameObject(gameObject, "Image");
        m_pImage = ob.GetComponent<Image>();

        ob = Helper.FindChildGameObject(gameObject, "Text_Msg");
        m_pText = ob.GetComponent<Text>();
    }

    void Update()
    {
        m_pAlpha.Update(Time.deltaTime);
        float fAlpha = m_pAlpha.GetCurScalar();
        m_pImage.color = new Color(1,1,1,fAlpha);

        m_pPos.Update(Time.deltaTime);
        Vector3 vPos = m_pPos.GetCurVector3();
        m_pText.gameObject.transform.localPosition = vPos;
    }

    public void SetScoreRankingMsg(string strMsg)
    {
        if (m_eState == eState.None || m_eState == eState.Ending)
        {
            m_strCurrMsg = strMsg;
            m_eState = eState.Begin;

            TransformerEvent eventValue;

            float fAlpha = m_pAlpha.GetCurScalar();
            m_pAlpha.OnReset();
            eventValue = new TransformerEvent_Scalar(0, fAlpha);
            m_pAlpha.AddEvent(eventValue);
            eventValue = new TransformerEvent_Scalar(0.5f, 1);
            m_pAlpha.AddEvent(eventValue);
            m_pAlpha.SetCallback(null, OnDone_Alpha_FadeIn);
            m_pAlpha.OnPlay();

            m_pPos.OnReset();
        }
        else if(strMsg != m_strCurrMsg)
        {
            m_MsgQueue.Clear();
            m_MsgQueue.Enqueue(strMsg);
        }
    }

    public void SetMsg(string strMsg)
    {
        if (m_eState == eState.None || m_eState == eState.Ending)
        {
            m_strCurrMsg = strMsg;
            m_eState = eState.Begin;

            TransformerEvent eventValue;

            float fAlpha = m_pAlpha.GetCurScalar();
            m_pAlpha.OnReset();
            eventValue = new TransformerEvent_Scalar(0, fAlpha);
            m_pAlpha.AddEvent(eventValue);
            eventValue = new TransformerEvent_Scalar(0.5f, 1);
            m_pAlpha.AddEvent(eventValue);
            m_pAlpha.SetCallback(null, OnDone_Alpha_FadeIn);
            m_pAlpha.OnPlay();

            m_pPos.OnReset();
        }
        else
        {
            m_MsgQueue.Enqueue(strMsg);
        }
    }

    private void OnDone_Alpha_FadeIn(TransformerEvent eventValue)
    {
        m_eState = eState.Play;

        m_pText.text = m_strCurrMsg;
        float fLength = m_pText.preferredWidth;
        float fTime = (fLength + 720) * 0.01f;

        m_pPos.OnReset();
        eventValue = new TransformerEvent_Vector3(fTime, new Vector3(-360 - fLength, 0, 0));
        m_pPos.AddEvent(eventValue);
        m_pPos.SetCallback(null, OnDone_Pos_Text);
        m_pPos.OnPlay();
    }

    private void OnDone_Pos_Text(TransformerEvent eventValue)
    {
        if (m_MsgQueue.Count != 0)
        {
            string strMsg = m_MsgQueue.Dequeue();
            m_pText.text = strMsg;
            float fLength = m_pText.preferredWidth;
            float fTime = (fLength + 720) * 0.01f;

            m_pPos.OnReset();
            eventValue = new TransformerEvent_Vector3(fTime, new Vector3(-360 - fLength, 0, 0));
            m_pPos.AddEvent(eventValue);
            m_pPos.SetCallback(null, OnDone_Pos_Text);
            m_pPos.OnPlay();

            m_pText.gameObject.transform.localPosition = new Vector3(720, 0, 0);
        }
        else
        {
            m_eState = eState.Ending;

            m_pAlpha.OnReset();
            eventValue = new TransformerEvent_Scalar(0, 1);
            m_pAlpha.AddEvent(eventValue);
            eventValue = new TransformerEvent_Scalar(0.5f, 0);
            m_pAlpha.AddEvent(eventValue);
            m_pAlpha.SetCallback(null, OnDone_Alpha_FadeOut);
            m_pAlpha.OnPlay();
        }
    }

    private void OnDone_Alpha_FadeOut(TransformerEvent eventValue)
    {
        m_eState = eState.None;
    }
}
