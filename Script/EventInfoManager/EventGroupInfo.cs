using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;


public class EventGroupInfo
{
	string 				m_strCharacterName		= "";
	string 				m_strAnimName 			= "";
	string 				m_strFaceTexName 	    = "None";
	List<EventInfo>		m_EventInfoList 		= new List<EventInfo>();
	
	public EventGroupInfo()
	{
	}
	
	public EventGroupInfo(string CharacterName, string AnimName)
	{
		m_strCharacterName = CharacterName;
		m_strAnimName = AnimName;
	}
	
	public void 		SetCharacterName(string CharacterName)
	{
		m_strCharacterName = CharacterName;
	}
	
	public string 		GetCharacterName()
	{
		return m_strCharacterName;
	}
	
	public void 		SetAnimName(string AnimName)
	{
		m_strAnimName = AnimName;	
	}
	
	public string 		GetAnimName()
	{
		return m_strAnimName;
	}
	
	public void 		SetFaceTexName(string MaterialName)
	{
        m_strFaceTexName = MaterialName;
	}
	
	public string 		GetFaceTexName()
	{
        return m_strFaceTexName;
	}	
	
	public void 		AddEventInfo(EventInfo eventInfo)
	{
		m_EventInfoList.Add (eventInfo);
	}
	
	public void 		DeleteEventInfo(EventInfo eventInfo)
	{
		m_EventInfoList.Remove(eventInfo);
	}
	
	public int 			GetNumEventInfo()
	{
		return m_EventInfoList.Count;
	}
	
	public EventInfo 	GetEventInfo_byIndex(int Index)
	{
		if( Index < 0 || Index > m_EventInfoList.Count - 1 )
			return null;
		
		return m_EventInfoList[Index];
	}
	
	public void 		LoadFile(BinaryReader br)
	{
		m_strCharacterName 		= br.ReadString ();
		m_strAnimName 			= br.ReadString ();
        m_strFaceTexName        = br.ReadString();
		
		int NumEventInfo = br.ReadInt32();
		
		for( int i = 0; i < NumEventInfo; ++i )
		{
			EventInfo eventInfo = new EventInfo();
			eventInfo.LoadFile(br);
			m_EventInfoList.Add (eventInfo);
		}
	}
	
	public void 		SaveFile(BinaryWriter bw)
	{
		bw.Write(m_strCharacterName);
		bw.Write(m_strAnimName);
        bw.Write(m_strFaceTexName);
		
		bw.Write(m_EventInfoList.Count);
		
		foreach(EventInfo eventInfo in m_EventInfoList)
		{
			eventInfo.SaveFile (bw);
		}
	}
}
