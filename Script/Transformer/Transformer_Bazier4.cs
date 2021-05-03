using UnityEngine;
using System.Collections;

public class Transformer_Bazier4 : Transformer 
{
	private TransformerEvent_Vector3 	m_pFirstEventValue 	= null,
										m_pSecondEventValue = null, 
										m_pThirdEventValue 	= null,
										m_pFourthEventValue = null;
	
	Vector3 m_CurScalar3 = new Vector3(0,0,0);
	
	public Transformer_Bazier4 (Vector3 BazierPosValue)
	{
		m_pCurEvent = new TransformerEvent_Vector3(0, BazierPosValue);
		AddEvent (m_pCurEvent);
	}
	
	public Transformer_Bazier4 (Vector3 BazierPosValue, object ob)
	{
		m_pCurEvent = new TransformerEvent_Vector3(0, BazierPosValue, ob);
		AddEvent (m_pCurEvent);	
	}	
	
	public override void Update(float deltaTime)
	{
		Update_Inner(deltaTime);
		
		if( m_eTransformerState == eTransformerState.eTransformerState_Playing )
		{			
			if( m_EventList.Count == 1 )
			{
				TransformerEvent_Vector3 eventValue = (TransformerEvent_Vector3)m_EventList[0];
				m_CurScalar3 = eventValue.m_Vector3;
			}
			else if( m_EventList.Count >= 4 )
			{
				m_CurScalar3 = CalcuBazier4(	m_pFirstEventValue.m_Vector3, m_pSecondEventValue.m_Vector3, m_pThirdEventValue.m_Vector3, m_pFourthEventValue.m_Vector3,
										  	m_pFirstEventValue.m_fTime, m_pFourthEventValue.m_fTime);				
			}
		}	
	}

	protected override void OneEventDone()
	{
		int eventCount = m_EventList.Count;
		
		if( m_nEventIndex < eventCount-1 && m_nEventIndex % 3 == 0 )
		{
			m_pFirstEventValue = (TransformerEvent_Vector3)m_EventList[m_nEventIndex];
			m_pSecondEventValue = (TransformerEvent_Vector3)m_EventList[m_nEventIndex+1];
			m_pThirdEventValue = (TransformerEvent_Vector3)m_EventList[m_nEventIndex+2];
			m_pFourthEventValue = (TransformerEvent_Vector3)m_EventList[m_nEventIndex+3];
		}
	}
	
	protected override void ComplteEventDone()
	{
		TransformerEvent_Vector3 eventValue = (TransformerEvent_Vector3)(m_pCurEvent);
		m_CurScalar3 = eventValue.m_Vector3;
	}		
	
	public override Vector3 GetCurVector3() 	
	{
		return new Vector3(m_CurScalar3.x,m_CurScalar3.y,m_CurScalar3.z); 
	}		
	
	protected override void PrePlay()
	{
		if( m_EventList.Count >= 4 )
		{
			m_pFirstEventValue = (TransformerEvent_Vector3)m_EventList[0];
			m_pSecondEventValue = (TransformerEvent_Vector3)m_EventList[1];
			m_pThirdEventValue = (TransformerEvent_Vector3)m_EventList[2];
			m_pFourthEventValue = (TransformerEvent_Vector3)m_EventList[3];
		}		
	}
	
	protected override void PreStop()
	{
		TransformerEvent_Vector3 eventValue = (TransformerEvent_Vector3)m_EventList[0];
		m_CurScalar3 = eventValue.m_Vector3;
	}
}
