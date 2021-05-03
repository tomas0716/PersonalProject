using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI_UnitShuffle : MonoBehaviour
{
    private GameObject          m_pGameObject_Shuffle   = null;
    private Transformer_Vector3 m_pPos                  = new Transformer_Vector3(new Vector3(720, 0, 0));

    void Start()
    {
        m_pGameObject_Shuffle = Helper.FindChildGameObject(gameObject, "Shuffle");

        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(m_pGameObject_Shuffle, "Text_1");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.InGame_UnitShuffle_01);

        ob = Helper.FindChildGameObject(m_pGameObject_Shuffle, "Text_2");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.InGame_UnitShuffle_02);

        TransformerEvent_Vector3 eventValue;
        eventValue = new TransformerEvent_Vector3(0.3f, new Vector3(720, 0, 0));
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(0.5f, Vector3.zero);
        m_pPos.AddEvent(eventValue);
        m_pPos.SetCallback(null, OnDone_MoveCenter);
        m_pPos.OnPlay();
    }

    void Update()
    {
        m_pPos.Update(Time.deltaTime);
        Vector3 vPos = m_pPos.GetCurVector3();
        m_pGameObject_Shuffle.transform.localPosition = vPos;
    }

    public void OnDone_MoveCenter(TransformerEvent eventValue)
    {
        m_pPos.OnReset();
        eventValue = new TransformerEvent_Vector3(0.0f, Vector3.zero);
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(1.5f, Vector3.zero);
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(1.7f, new Vector3(-720, 0, 0));
        m_pPos.AddEvent(eventValue);
        m_pPos.SetCallback(null, OnDone_Pos);
        m_pPos.OnPlay();

        Update();

        AppInstance.Instance.m_pEventDelegateManager.OnInGame_UnitShuffle();
    }

    private void OnDone_Pos(TransformerEvent eventValue)
    {
        GameObject.Destroy(gameObject);
    }
}
