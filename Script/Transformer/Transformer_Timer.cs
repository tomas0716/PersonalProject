using System;
using UnityEngine;
using System.Collections;

public class Transformer_Timer : Transformer
{	
	public Transformer_Timer()
	{				
		TransformerEvent_Timer eventTimer = null;
		eventTimer = new TransformerEvent_Timer(0);
		
		AddEvent(eventTimer);
		
		m_pCurEvent = m_pPrevEvent = (TransformerEvent)(m_EventList.ToArray())[0];
	}
	
	public Transformer_Timer(object parameta)
	{		
		TransformerEvent_Timer eventTimer = null;
		eventTimer = new TransformerEvent_Timer(0, parameta);
		
		AddEvent(eventTimer);
		
		m_pCurEvent = m_pPrevEvent = (TransformerEvent)(m_EventList.ToArray())[0];
	}	
	
	public override void Update(float deltaTime)
	{
		Update_Inner(deltaTime);
	}	
}

