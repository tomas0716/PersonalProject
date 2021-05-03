using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

public class EventInfo
{
	string 		m_strEventName 			= "";
	float 		m_fEventTime 			= 0;
	
	string 		m_strEffectFileName 	= "";
	string 		m_strBoneName 			= "";
	Vector3 	m_vBonePosInterval 	    = Vector3.zero;
	Vector3 	m_vEffectRotation 	    = Vector3.zero;
	bool 		m_IsEffectBoneFollow	= false;
	
	public EventInfo()
	{
	}
	
	public EventInfo(string EventName, float EventTime)
	{
		m_strEventName = EventName;
		m_fEventTime = EventTime;
	}
	
	public void 		SetEventName(string EventName)
	{
		m_strEventName = EventName;
	}
	
	public string 		GetEventName()
	{
		return m_strEventName;
	}
	
	public void 		SetEventTime(float time)
	{
		m_fEventTime = time;
	}
	
	public float 		GetEventTime()
	{
		return m_fEventTime;
	}
	
	public void 		SetEffectFileName(string FileName)
	{
		m_strEffectFileName = FileName;
	}
	
	public string 		GetEffectFileName()
	{
		return m_strEffectFileName;
	}
	
	public void 		SetBoneName(string BoneName)
	{
		m_strBoneName = BoneName;
	}
	
	public string 		GetBoneName()
	{
		return m_strBoneName;
	}
	
	public void 		SetBonePosInterval(Vector3 vPos)
	{
		m_vBonePosInterval = vPos;
	}
	
	public Vector3 		GetBonePosInterval()
	{
		return m_vBonePosInterval;
	}
	
	public void 		SetEffectRotation(Vector3 vRot)
	{
		m_vEffectRotation = vRot;
	}
	
	public Vector3 		GetEffectRotation()
	{
		return m_vEffectRotation;
	}
	
	public void 		SetEffectBoneFollow(bool IsFollow)
	{
		m_IsEffectBoneFollow = IsFollow;
	}
	
	public bool 		IsEffectBoneFollow()
	{
		return m_IsEffectBoneFollow;
	}
	
	public void 		LoadFile(BinaryReader br)
	{
		m_strEventName 			= br.ReadString ();
		m_fEventTime 			= br.ReadSingle();
		
		m_strEffectFileName 	= br.ReadString ();
		m_strBoneName 			= br.ReadString ();
		
		m_vBonePosInterval.x 	= br.ReadSingle();
		m_vBonePosInterval.y 	= br.ReadSingle();
		m_vBonePosInterval.z 	= br.ReadSingle();
		
		m_vEffectRotation.x 	= br.ReadSingle();
		m_vEffectRotation.y 	= br.ReadSingle();
		m_vEffectRotation.z 	= br.ReadSingle();
		
		m_IsEffectBoneFollow	= br.ReadBoolean ();
	}
	
	public void 		SaveFile(BinaryWriter bw)
	{
		bw.Write(m_strEventName);
		bw.Write(m_fEventTime);
		
		bw.Write(m_strEffectFileName);
		bw.Write(m_strBoneName);
		
		bw.Write(m_vBonePosInterval.x);
		bw.Write(m_vBonePosInterval.y);
		bw.Write(m_vBonePosInterval.z);
		
		bw.Write(m_vEffectRotation.x);
		bw.Write(m_vEffectRotation.y);
		bw.Write(m_vEffectRotation.z);
		
		bw.Write (m_IsEffectBoneFollow);
	}	
}
