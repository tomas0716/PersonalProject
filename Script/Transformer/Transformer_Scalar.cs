using System;
using UnityEngine;
using System.Collections;

public class Transformer_Scalar : Transformer
{
	float 	m_fCurScalar = 0.0f;
	
	public Transformer_Scalar (float scalar)
	{
		m_fCurScalar = scalar;
		
		TransformerEvent_Scalar eventScalar = null;
		eventScalar = new TransformerEvent_Scalar(0, scalar);
		
		AddEvent(eventScalar);
		
		m_pCurEvent = m_pPrevEvent = (TransformerEvent)(m_EventList.ToArray())[0];
	}
	
	public Transformer_Scalar (float scalar, object parameta)
	{
		m_fCurScalar = scalar;
		
		TransformerEvent_Scalar eventScalar = null;
		eventScalar = new TransformerEvent_Scalar(0, scalar, parameta);
		
		AddEvent(eventScalar);
		
		m_pCurEvent = m_pPrevEvent = (TransformerEvent)(m_EventList.ToArray())[0];
	}	
	
	public override void Update(float deltaTime)
	{
		Update_Inner(deltaTime);
		
		if( m_eTransformerState == eTransformerState.eTransformerState_Playing )
		{
			TransformerEvent_Scalar CurEvent = (TransformerEvent_Scalar)(m_pCurEvent);
			TransformerEvent_Scalar PrevEvent = (TransformerEvent_Scalar)(m_pPrevEvent);
			m_fCurScalar = CalcuLinear(PrevEvent.m_fScalar, PrevEvent.m_fTime, CurEvent.m_fScalar, CurEvent.m_fTime);
		}
	}
	
	protected override void OneEventDone()
	{
	}
	
	protected override void ComplteEventDone()
	{
		TransformerEvent_Scalar eventValue = (TransformerEvent_Scalar)(m_pCurEvent);
		m_fCurScalar = eventValue.m_fScalar;
	}	
	
	public override float 	GetCurScalar() 	
	{
		return m_fCurScalar; 
	}	
	
	protected override void PrePlay()
	{
	}
	
	protected override void PreStop()
	{
		TransformerEvent_Scalar eventValue = (TransformerEvent_Scalar)m_EventList[0];
		m_fCurScalar = eventValue.m_fScalar;		
	}		
}

