using System;
using UnityEngine;
using System.Collections;

public class Transformer_Vector3 : Transformer
{
	Vector3 m_CurScalar3;
	
	public Transformer_Vector3 (Vector3 scalar3)
	{
		m_CurScalar3 = scalar3;
			
		TransformerEvent_Vector3 eventScalar3 = null;
		eventScalar3 = new TransformerEvent_Vector3(0, scalar3);
		
		AddEvent (eventScalar3);
		
		m_pCurEvent = m_pPrevEvent = (TransformerEvent)(m_EventList.ToArray())[0];
	}
	
	public Transformer_Vector3 (Vector3 scalar3, object parameta)
	{
		m_CurScalar3 = scalar3;
			
		TransformerEvent_Vector3 eventScalar3 = null;
		eventScalar3 = new TransformerEvent_Vector3(0, scalar3, parameta);
		
		AddEvent (eventScalar3);
		
		m_pCurEvent = m_pPrevEvent = (TransformerEvent)(m_EventList.ToArray())[0];
	}	
	
	public override void Update(float deltaTime)
	{
		Update_Inner(deltaTime);
		
		if( m_eTransformerState == eTransformerState.eTransformerState_Playing )
		{
			TransformerEvent_Vector3 CurEvent = (TransformerEvent_Vector3)(m_pCurEvent);
			TransformerEvent_Vector3 PrevEvent = (TransformerEvent_Vector3)(m_pPrevEvent);
			
			m_CurScalar3 = CalcuLinear(PrevEvent.m_Vector3, PrevEvent.m_fTime, CurEvent.m_Vector3, CurEvent.m_fTime);
		}	
	}
	
	protected override void OneEventDone()
	{
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
	}
	
	protected override void PreStop()
	{
		TransformerEvent_Vector3 eventValue = (TransformerEvent_Vector3)m_EventList[0];
		m_CurScalar3 = eventValue.m_Vector3;		
	}		
}

