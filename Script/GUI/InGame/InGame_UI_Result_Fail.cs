using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI_Result_Fail : MonoBehaviour
{
    private GameObject              m_pGameObject_Result    = null;
    private Transformer_Vector3     m_pPos                  = new Transformer_Vector3(new Vector3(0, -730, 0));

    void Start()
    {
        Helper.OnSoundPlay(eSoundType.Failed, false);

        m_pGameObject_Result = Helper.FindChildGameObject(gameObject, "Result");

        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(m_pGameObject_Result, "Text_1");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.InGame_Result_Fail_01);

        ob = Helper.FindChildGameObject(m_pGameObject_Result, "Text_2");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.InGame_Result_Fail_02);

        TransformerEvent_Vector3 eventValue;
        eventValue = new TransformerEvent_Vector3(0.15f, new Vector3(0, 30, 0));
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(0.2f, Vector3.zero);
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(2.35f, Vector3.zero);
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(2.4f, new Vector3(0, 30, 0));
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(2.6f, new Vector3(0, -730, 0));
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(3.2f, new Vector3(0, -730, 0));
        m_pPos.AddEvent(eventValue);
        m_pPos.SetCallback(null, OnDone_Pos);
        m_pPos.OnPlay();

        int nLevel = SavedGameDataInfo.Instance.m_nLevel;
        if (nLevel <= 150)
        {
            Helper.FirebaseLogEvent("InGame_Fail_" + nLevel.ToString());
        }
        else
        {
            Helper.FirebaseLogEvent("InGame_Fail", "Level", nLevel);
        }
    }

    void Update()
    {
        m_pPos.Update(Time.deltaTime);
        Vector3 vPos = m_pPos.GetCurVector3();
        m_pGameObject_Result.transform.localPosition = vPos;
    }

    private void OnDone_Pos(TransformerEvent eventValue)
    {
        AppInstance.Instance.m_pEventDelegateManager.OnInGame_Result_FailActionDone();
        GameObject.Destroy(gameObject);
    }
}
