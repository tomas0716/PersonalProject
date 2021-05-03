using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ParticleManager
{
	private List<ParticleInfo>		m_ParticleInfoList          = new List<ParticleInfo>();
	private List<ParticleInfo>		m_RemoveParticleInfoList    = new List<ParticleInfo>();
	
    public ParticleManager()
    {
    
    }

	private void UpdateRemove()
	{
		foreach(ParticleInfo Remove in m_RemoveParticleInfoList)
		{
			foreach(ParticleInfo Info in m_ParticleInfoList)
			{
				if( Info == Remove )
				{
					Info.Destroy ();
					m_ParticleInfoList.Remove (Info);
					break;
				}
			}
		}
		
		m_RemoveParticleInfoList.Clear ();
	}
	
	public void Update () 
	{
		UpdateRemove();
		
		foreach(ParticleInfo Info in m_ParticleInfoList)
		{
			Info.Update ();

			if (Info.GetGameObject() == null)
			{
				m_RemoveParticleInfoList.Add(Info);
			}
		}		
	}
	
	public ParticleInfo  	LoadParticleSystem(string FileName, Vector3 vPos, SlotUnit pUnit = null)
	{
		ParticleInfo Info = new ParticleInfo();

		Info.LoadParticleSystem(FileName, vPos, pUnit);
		m_ParticleInfoList.Add(Info);

		return Info;
	}

    public ParticleInfo     LoadParticleSystem(string FileName, Vector3 vPos, Quaternion QuatRot, SlotUnit pUnit = null)
	{
        ParticleInfo Info = new ParticleInfo();
		Info.LoadParticleSystem(FileName, vPos, QuatRot, pUnit);
		
		m_ParticleInfoList.Add(Info);
		
		return Info;
	}
	
	public void				RemoveParticleInfo(ParticleInfo Info)
	{
		if( Info != null )
		{
			m_RemoveParticleInfoList.Add (Info);
		}
	}
}
