using System;
using UnityEngine;
using System.Collections;

public enum eTransformerState
{
	eTransformerState_Playing,
	eTransformerState_Pause,
	eTransformerState_Stop,
	eTransformerState_Complete,
}

public class Transformer
{
    public delegate void OnOneEventDone( int key, TransformerEvent eventValue );
    public delegate void OnCompleteEventDone(TransformerEvent eventValue);

	protected ArrayList 			m_EventList         = null;
	
	protected eTransformerState		m_eTransformerState = eTransformerState.eTransformerState_Stop;
	
	protected float 				m_fTime             = 0.0f;

	protected TransformerEvent		m_pCurEvent         = null;
	protected TransformerEvent		m_pPrevEvent        = null;
	protected int 					m_nEventIndex       = 0;

	protected bool					m_IsLoop			= false;
	
    protected OnOneEventDone        m_OnOneEventDone;
	protected OnCompleteEventDone   m_OnCompleteEventDone;
	
	public Transformer ()
	{
		m_EventList = new ArrayList();
	}
	
	public virtual void Update(float deltaTime) 
	{ 
	}
	
	public void 	Update_Inner(float deltaTime)
	{
		if( m_eTransformerState == eTransformerState.eTransformerState_Playing )
		{
			m_fTime += deltaTime;

            if( m_fTime >= m_pCurEvent.m_fTime )
			{
				m_pPrevEvent = m_pCurEvent;
				
				if( m_nEventIndex + 1 < m_EventList.Count )
				{	
					OneEventDone();
					m_pCurEvent = (TransformerEvent)(m_EventList.ToArray())[++m_nEventIndex];
					
                    if( m_OnOneEventDone != null )
                    {
                        m_OnOneEventDone( m_nEventIndex - 1, m_pPrevEvent );
                    }

					Update_Inner(0);
				}
				else
				{
					m_eTransformerState = eTransformerState.eTransformerState_Complete;
					
					ComplteEventDone();
					
                    if( m_OnCompleteEventDone != null )
                    {
                        m_OnCompleteEventDone( m_pCurEvent );
                    }

					if (m_IsLoop == true)
					{
						OnPlay();
					}
				}
			}		
		}
	}
	
	public virtual void AddEvent(TransformerEvent eventValue)
	{
		m_EventList.Add (eventValue);
	}
	
	protected virtual void PrePlay()
	{
	}
	
	public void 	OnPlay()
	{
		if( m_eTransformerState == eTransformerState.eTransformerState_Complete )
		{
			OnStop ();
		}
		
		if( m_eTransformerState == eTransformerState.eTransformerState_Stop ||
			m_eTransformerState == eTransformerState.eTransformerState_Complete )
		{
			PrePlay();
		}
		
		m_eTransformerState = eTransformerState.eTransformerState_Playing;

		Update(0);
	}
	
	public void 	OnPause()
	{
		if( m_eTransformerState == eTransformerState.eTransformerState_Playing)
		{
			m_eTransformerState = eTransformerState.eTransformerState_Pause;
		}
	}
	
	protected virtual void PreStop()
	{
	}
	
	public void 	OnStop()
	{
		PreStop();
		
		m_eTransformerState = eTransformerState.eTransformerState_Stop;
		m_fTime = 0.0f;
		
		m_nEventIndex = 0;
		m_pCurEvent = m_pPrevEvent = (TransformerEvent)(m_EventList.ToArray())[0];
	}
	
	public void 	OnResetEvent()
	{
		TransformerEvent firstEvent = (TransformerEvent)(m_EventList.ToArray())[0];
		m_EventList.Clear ();
		
		m_EventList.Add (firstEvent);
		
		OnStop ();
	}
	
	public void 	OnResetCallback()
	{
        m_OnOneEventDone = null;
        m_OnCompleteEventDone = null;
	}

	public void		OnReset()
	{
		OnStop(); 
		OnResetEvent();
		OnResetCallback();
	}

	public void SetLoop(bool IsLoop)
	{
		m_IsLoop = IsLoop;
	}
	
	public void 	SetCallback(OnOneEventDone onOneEventDone, OnCompleteEventDone onCompleteEventDone )
	{
        m_OnOneEventDone = onOneEventDone;
        m_OnCompleteEventDone = onCompleteEventDone;
	}
	
	public float 	GetCurTime()
	{
		return m_fTime;
	}
	
	public virtual float 	GetCurScalar() 	
	{
		return 0.0f; 
	}

	public virtual Vector2	GetCurVector2()
	{
		return new Vector2(0, 0);
	}

	public virtual Vector3	GetCurVector3() 
	{ 
		return new Vector3(0,0,0);
	}

	public virtual Color	GetCurColor()
	{
		return new Color(0, 0, 0);
	}

	public virtual eTransformerState GetCurrTransformerState()
	{
		return m_eTransformerState;
	}

	protected virtual void 	OneEventDone()
	{
	}
	
	protected virtual void 	ComplteEventDone()
	{
	}
	
	protected float CalcuLinear(float PrevScalar, float PrevEventTime, float CurScalar, float CurEventTime)
	{
		if( CurEventTime == PrevEventTime )
			return CurScalar;

		float fRatio = ((CurScalar - PrevScalar) / (CurEventTime - PrevEventTime));

		float fTime = m_fTime;
		
		if( m_fTime > CurEventTime )
			fTime = CurEventTime;

		return (((fTime - PrevEventTime) * fRatio) + PrevScalar);	
	}

	protected Vector2 CalcuLinear(Vector2 PrevScalar, float PrevEventTime, Vector2 CurScalar, float CurEventTime)
	{
		if (CurEventTime == PrevEventTime)
			return CurScalar;

		Vector2 Ratio = ((CurScalar - PrevScalar) / (CurEventTime - PrevEventTime));

		float fTime = m_fTime;

		if (m_fTime > CurEventTime)
			fTime = CurEventTime;

		return (((fTime - PrevEventTime) * Ratio) + PrevScalar);
	}

	protected Vector3 CalcuLinear(Vector3 PrevScalar, float PrevEventTime, Vector3 CurScalar, float CurEventTime)
	{
		if( CurEventTime == PrevEventTime )
			return CurScalar;

		Vector3 Ratio = ((CurScalar - PrevScalar) / (CurEventTime - PrevEventTime));

		float fTime = m_fTime;
		
		if( m_fTime > CurEventTime )
			fTime = CurEventTime;

		return (((fTime - PrevEventTime) * Ratio) + PrevScalar);	
	}

	protected Color CalcuLinear(Color PrevColor, float PrevEventTime, Color CurColor, float CurEventTime)
	{
		if (CurEventTime == PrevEventTime)
			return CurColor;

		Color Ratio = ((CurColor - PrevColor) / (CurEventTime - PrevEventTime));

		float fTime = m_fTime;

		if (m_fTime > CurEventTime)
			fTime = CurEventTime;

		return (((fTime - PrevEventTime) * Ratio) + PrevColor);
	}

	protected Vector3 CalcuBazier4(Vector3 FirstValue, Vector3 SecondValue, Vector3 ThirdValue, Vector3 FourthValue, float PrevEventTime, float CurEventTime)
	{
		float time = m_fTime;
		if( m_fTime > CurEventTime )
			time = CurEventTime;
		
		float TimeInterval = CurEventTime - PrevEventTime;
		float ElapsedTime = time - PrevEventTime;
		
		float Ratio = ElapsedTime / TimeInterval;
		
		float mum1, mum13, mu3;
		
		mum1 = 1 - Ratio;
		mum13 = mum1 * mum1 * mum1;
		mu3 = Ratio * Ratio * Ratio;
		Vector3 CurrPos = mum13 * FirstValue + 3 * Ratio * mum1 * mum1 * SecondValue + 3 * Ratio * Ratio * mum1 * ThirdValue + mu3 * FourthValue;

		return CurrPos;
	}
}
