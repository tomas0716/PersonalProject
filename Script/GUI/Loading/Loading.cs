using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private int                 m_nIndex                = 0;
    private GameObject []       m_pGameObject_Image     = new GameObject[6];
    private Transformer_Timer   m_pTimer_ImageChange    = new Transformer_Timer();

	private void Awake()
	{
        for (int i = 0; i < 6; ++i)
        {
            string strName = "Loading_" + (i + 1).ToString();
            m_pGameObject_Image[i] = Helper.FindChildGameObject(gameObject, strName);
            m_pGameObject_Image[i].SetActive(false);
        }

        TransformerEvent_Timer eventValue = null;
        eventValue = new TransformerEvent_Timer(0.2f);
        m_pTimer_ImageChange.AddEvent(eventValue);
        m_pTimer_ImageChange.SetCallback(null, OnDone_Timer_ImageChange);
        m_pTimer_ImageChange.OnPlay();

        m_pGameObject_Image[0].SetActive(true);

        AppInstance.Instance.m_pEventDelegateManager.OnEventDeleteLoading += OnDeleteLoading;
    }

    private void OnDestroy()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventDeleteLoading -= OnDeleteLoading;
    }

    void Update()
    {
        m_pTimer_ImageChange.Update(Time.deltaTime);
    }

    private void OnDone_Timer_ImageChange(TransformerEvent eventValue)
    {
        int nPrevIndex = m_nIndex;
        m_nIndex = m_nIndex == 5 ? 0 : ++m_nIndex;

        m_pGameObject_Image[nPrevIndex].SetActive(false);
        m_pGameObject_Image[m_nIndex].SetActive(true);

        m_pTimer_ImageChange.OnStop();
        m_pTimer_ImageChange.OnPlay();
    }

    public void OnDeleteLoading()
    {
        Destroy(gameObject);
    }
}
