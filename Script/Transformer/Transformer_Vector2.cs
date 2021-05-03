using System;
using UnityEngine;
using System.Collections;

public class Transformer_Vector2 : Transformer
{
	Vector2 m_CurVector2;
	
	public Transformer_Vector2(Vector2 vector)
	{
		m_CurVector2 = vector;
			
		TransformerEvent_Vector2 eventVector2 = null;
		eventVector2 = new TransformerEvent_Vector2(0, vector);
		
		AddEvent (eventVector2);
		
		m_pCurEvent = m_pPrevEvent = (TransformerEvent)(m_EventList.ToArray())[0];
	}
	
	public Transformer_Vector2(Vector2 vector, object parameta)
	{
		m_CurVector2 = vector;

		TransformerEvent_Vector2 eventVector2 = null;
		eventVector2 = new TransformerEvent_Vector2(0, vector, parameta);
		
		AddEvent (eventVector2);
		
		m_pCurEvent = m_pPrevEvent = (TransformerEvent)(m_EventList.ToArray())[0];
	}	
	
	public override void Update(float deltaTime)
	{
		Update_Inner(deltaTime);
		
		if( m_eTransformerState == eTransformerState.eTransformerState_Playing )
		{
			TransformerEvent_Vector2 CurEvent = (TransformerEvent_Vector2)(m_pCurEvent);
			TransformerEvent_Vector2 PrevEvent = (TransformerEvent_Vector2)(m_pPrevEvent);

			m_CurVector2 = CalcuLinear(PrevEvent.m_Vector2, PrevEvent.m_fTime, CurEvent.m_Vector2, CurEvent.m_fTime);
		}	
	}
	
	protected override void OneEventDone()
	{
	}	
	
	protected override void ComplteEventDone()
	{
		TransformerEvent_Vector2 eventValue = (TransformerEvent_Vector2)(m_pCurEvent);
		m_CurVector2 = eventValue.m_Vector2;
	}	
	
	public override Vector2 GetCurVector2() 
	{ 
		return m_CurVector2;
	}	
	
	protected override void PrePlay()
	{
	}
	
	protected override void PreStop()
	{
		TransformerEvent_Vector2 eventValue = (TransformerEvent_Vector2)m_EventList[0];
		m_CurVector2 = eventValue.m_Vector2;		
	}		
}

