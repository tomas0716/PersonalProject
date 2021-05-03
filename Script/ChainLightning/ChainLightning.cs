using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainLightning : MonoBehaviour
{
	public GameObject			lineRendererPrefab;
	public GameObject			lightRendererPrefab;

	private LightningBolt		m_pLightningBolt	= null;
	private Vector3				m_vPos_01			= Vector3.zero;
	private Vector3				m_vPos_02			= Vector3.zero;

	private float				m_fSegmentLength	= 0;
	private float				m_fNextRefresh		= 0;

	private Transformer_Timer	m_pTimer_Destroy	= new Transformer_Timer();

	void Start()
	{
		m_fSegmentLength = GameDefine.ms_fBaseSlotSize * InGameInfo.Instance.m_fSlotScale;
		m_pLightningBolt = new LightningBolt(m_fSegmentLength);
		m_pLightningBolt.Init(2, lineRendererPrefab, lightRendererPrefab);
		m_pLightningBolt.Activate();
	}

	private void OnDestroy()
	{
		m_pLightningBolt.OnDestroy();
	}

	void Update()
	{
		m_pTimer_Destroy.Update(Time.deltaTime);

		if (Time.time > m_fNextRefresh)
		{
			m_pLightningBolt.DrawLightning(m_vPos_01, m_vPos_02);
			m_fNextRefresh = Time.time + 0.05f;
		}
	}

	public void Init(Vector3 vPos_01, Vector3 vPos_02, float fDestroyTime)
	{
		m_vPos_01 = vPos_01;
		m_vPos_02 = vPos_02;

		TransformerEvent eventValue;
		eventValue = new TransformerEvent_Timer(fDestroyTime);
		m_pTimer_Destroy.AddEvent(eventValue);
		m_pTimer_Destroy.SetCallback(null, OnDone_Timer_Destroy);
		m_pTimer_Destroy.OnPlay();
	}

	private void OnDone_Timer_Destroy(TransformerEvent eventValeu)
	{
		GameObject.Destroy(gameObject);
	}
}
