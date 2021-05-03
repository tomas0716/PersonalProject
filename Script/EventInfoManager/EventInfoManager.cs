using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

public class EventInfoManager
{
	Dictionary<string, List<EventGroupInfo>>	m_EventGroupInfoTable = new Dictionary<string, List<EventGroupInfo>>();
	
	public EventInfoManager()
    {
    
    }
	
	public void 					AddEvent(string CharacterName, string AnimName, EventInfo eventInfo)
	{
		EventGroupInfo eventGroupInfo = CreateEventGroupInfo(CharacterName, AnimName);
		eventGroupInfo.AddEventInfo(eventInfo);
	}
	
	public void 					DeleteEventGroupInfo(string CharacterName, string AnimName)
	{
		foreach(KeyValuePair<string, List<EventGroupInfo>> item in m_EventGroupInfoTable)
		{
			if( item.Key == CharacterName )
			{
				foreach(EventGroupInfo GroupInfo in item.Value)
				{
					if( GroupInfo.GetAnimName() == AnimName )
					{
						item.Value.Remove (GroupInfo);
						
						return;
					}
				}
			}
		}			
	}
	
	public EventGroupInfo 			FindEventGroupInfo(string CharacterName, string AnimName)
	{
		foreach(KeyValuePair<string, List<EventGroupInfo>> item in m_EventGroupInfoTable)
		{
			if( item.Key == CharacterName )
			{
				foreach(EventGroupInfo GroupInfo in item.Value)
				{
					if( GroupInfo.GetAnimName() == AnimName )
					{
						return GroupInfo;
					}
				}
			}
		}		
		
		return null;
	}	
	
	public EventGroupInfo 			CreateEventGroupInfo(string CharacterName, string AnimName)
	{
		EventGroupInfo newGroup  = null;
		
		foreach(KeyValuePair<string, List<EventGroupInfo>> item in m_EventGroupInfoTable)
		{
			if( item.Key == CharacterName )
			{
				foreach(EventGroupInfo GroupInfo in item.Value)
				{
					if( GroupInfo.GetAnimName() == AnimName )
					{
						return GroupInfo;
					}
				}
				
				newGroup = new EventGroupInfo(CharacterName, AnimName);
				item.Value.Add(newGroup);
				
				return newGroup;
			}
		}		
		
		List<EventGroupInfo> 	EventGroupInfoList = new List<EventGroupInfo>();
		m_EventGroupInfoTable.Add (CharacterName, EventGroupInfoList);	
		
		newGroup = new EventGroupInfo(CharacterName, AnimName);
		EventGroupInfoList.Add(newGroup);	
		
		return newGroup;
	}
	
	private bool 					IsCharacter(string CharacterName)
	{
		foreach(KeyValuePair<string, List<EventGroupInfo>> item in m_EventGroupInfoTable)
		{
			if( item.Key == CharacterName )
			{
				return true;
			}
		}
		
		return false;
	}

	public void 					LoadFile(string CharacterName, TextAsset textAsset)
	{
        if (IsCharacter(CharacterName) == true || textAsset == null)
		{
			return;
		}
		
		List<EventGroupInfo> 	EventGroupInfoList = new List<EventGroupInfo>();
		m_EventGroupInfoTable.Add (CharacterName, EventGroupInfoList);			
		
		if( textAsset != null )
		{
			MemoryStream stream = new MemoryStream(textAsset.bytes);
			
			BinaryReader br = new BinaryReader(stream);
			
			int NumGroupInfo = br.ReadInt32 ();
			
			for( int i = 0; i < NumGroupInfo; ++i )
			{
				EventGroupInfo Group = new EventGroupInfo();
				Group.LoadFile (br);
				
				EventGroupInfoList.Add (Group);
			}
						
			br.Close ();
		}
	}
}
