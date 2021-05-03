using System;
using UnityEngine;
using System.Collections;

public class Transformer_Color : Transformer
{
	Color m_CurColor;
	
	public Transformer_Color(Color color)
	{
		m_CurColor = color;
			
		TransformerEvent_Color eventColor = null;
		eventColor = new TransformerEvent_Color(0, color);
		
		AddEvent (eventColor);
		
		m_pCurEvent = m_pPrevEvent = (TransformerEvent)(m_EventList.ToArray())[0];
	}
	
	public Transformer_Color(Color color, object parameta)
	{
		m_CurColor = color;

		TransformerEvent_Color eventColor = null;
		eventColor = new TransformerEvent_Color(0, color, parameta);
		
		AddEvent (eventColor);
		
		m_pCurEvent = m_pPrevEvent = (TransformerEvent)(m_EventList.ToArray())[0];
	}	
	
	public override void Update(float deltaTime)
	{
		Update_Inner(deltaTime);
		
		if( m_eTransformerState == eTransformerState.eTransformerState_Playing )
		{
			TransformerEvent_Color CurEvent = (TransformerEvent_Color)(m_pCurEvent);
			TransformerEvent_Color PrevEvent = (TransformerEvent_Color)(m_pPrevEvent);

			m_CurColor = CalcuLinear(PrevEvent.m_Color, PrevEvent.m_fTime, CurEvent.m_Color, CurEvent.m_fTime);
		}	
	}
	
	protected override void OneEventDone()
	{
	}	
	
	protected override void ComplteEventDone()
	{
		TransformerEvent_Color eventValue = (TransformerEvent_Color)(m_pCurEvent);
		m_CurColor = eventValue.m_Color;
	}	
	
	public override Color GetCurColor() 
	{ 
		return m_CurColor;
	}	
	
	protected override void PrePlay()
	{
	}
	
	protected override void PreStop()
	{
		TransformerEvent_Color eventValue = (TransformerEvent_Color)m_EventList[0];
		m_CurColor = eventValue.m_Color;		
	}		
}

