using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEventManager
{
	private List<GameEvent>		m_GameEventList         = new List<GameEvent>();
	private List<GameEvent>		m_RemoveGameEventList   = new List<GameEvent>();
	
    public GameEventManager()
    {
    
    }

	private void 	UpdateRemove()
	{
		foreach(GameEvent Remove in m_RemoveGameEventList)
		{
			foreach(GameEvent gameEvent in m_GameEventList)
			{
				if( Remove == gameEvent )
				{
					gameEvent.OnDestroy();
					m_GameEventList.Remove (gameEvent);
					break;
				}
			}
		}
		
		m_RemoveGameEventList.Clear ();
	}	
	
	public void     Update () 
	{
		UpdateRemove();

        GameEvent[] gameEvents = m_GameEventList.ToArray();
        foreach (GameEvent gameEvent in gameEvents)
		{
			gameEvent.Update ();
		}
	}
	
	public void 	AddGameEvent(GameEvent gameEvent)
	{
		m_GameEventList.Add (gameEvent);
	}
	
	public void 	RemoveGameEvent(GameEvent gameEvent)
	{
		m_RemoveGameEventList.Add (gameEvent);
	}
	
	public void 	RemoveGameEvent(eGameEventType eType)
	{
		int Count = m_GameEventList.Count;
		for( int i = 0; i < Count; ++i )
		{
			GameEvent gameEvent = m_GameEventList[i];
			
			if( gameEvent.GetGameEventType() == eType )
			{
				m_RemoveGameEventList.Add (gameEvent);
			}
		}	
	}
	
	public void 	RemoveGameEventAll()
	{
		foreach (GameEvent gameEvent in m_GameEventList)
		{
			gameEvent.OnDestroy();
			m_GameEventList.Remove(gameEvent);
		}

		m_GameEventList.Clear();
		m_RemoveGameEventList.Clear();
	}
}
