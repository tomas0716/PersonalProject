using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI_MoveCountZero : MonoBehaviour
{
    private GameObject m_pGameObject_MoveCountZero = null;
    private Transformer_Vector3 m_pPos = new Transformer_Vector3(new Vector3(0, 730, 0));
   
    void Start()
    {
        Helper.OnSoundPlay(eSoundType.OutofMove0, false);

        m_pGameObject_MoveCountZero = Helper.FindChildGameObject(gameObject, "MoveCountZero");

        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(m_pGameObject_MoveCountZero, "Text");
        pText = ob.GetComponent<Text>();

        switch (InGameInfo.Instance.m_eGameState)
        {
            case eGameState.Fail_TimeBombExploded:
                {
                    pText.text = Helper.GetTextDataString(eTextDataType.TimeBombExplosion);
                }
                break;

            default:
                {
                    pText.text = Helper.GetTextDataString(eTextDataType.InGame_MoveCountZero);
                }
                break;
        }
       
        TransformerEvent_Vector3 eventValue;
        eventValue = new TransformerEvent_Vector3(0.15f, new Vector3(0, -30, 0));
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(0.2f, Vector3.zero);
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(2.65f, Vector3.zero);
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(2.7f, new Vector3(0, 30, 0));
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(2.9f, new Vector3(0, -730, 0));
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(3.5f, new Vector3(0, -730, 0));
        m_pPos.AddEvent(eventValue);
        m_pPos.SetCallback(null, OnDone_Pos);
        m_pPos.OnPlay();
    }

    void Update()
    {
        m_pPos.Update(Time.deltaTime);
        Vector3 vPos = m_pPos.GetCurVector3();
        m_pGameObject_MoveCountZero.transform.localPosition = vPos;
    }

    private void OnDone_Pos(TransformerEvent eventValue)
    {
        AppInstance.Instance.m_pEventDelegateManager.OnInGame_MoveCountZeroActionDone();
        GameObject.Destroy(gameObject);
    }
}
