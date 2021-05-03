using UnityEngine;
using System.Collections;

public class GameEvent
{
	protected eGameEventType	m_eGameEventType = eGameEventType.None;
		
	public GameEvent()
	{
	}
	
	public virtual void 			Update()
	{
	}
	
	public virtual void 			OnDone()
	{
		AppInstance.Instance.m_pGameEventManager.RemoveGameEvent(this);
	}
	
	public virtual eGameEventType 	GetGameEventType()
	{
		return m_eGameEventType;
	}
	
	public virtual void 			OnDestroy()
	{
	}
}
