using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequenceAnimComponent : MonoBehaviour
{
	public delegate void OnDone_Anim();
	private OnDone_Anim m_Callback = null;

	private List<AtlasInfo> m_AtlasInfoList = new List<AtlasInfo>();

	private Plane2D				m_pPlane	= null;
	private float				m_fTime		= 0;
	private bool				m_IsLoop	= false;

	private Transformer_Scalar	m_pTimer	= new Transformer_Scalar(0);

	public void SetAnimInfo(Plane2D Plane, List<AtlasInfo> AtlasInfoList, float Time, bool IsLoop)
	{
		m_AtlasInfoList.Clear();
		m_pPlane = Plane;
		m_AtlasInfoList = AtlasInfoList;
		m_fTime = Time;
		m_IsLoop = IsLoop;

		m_pTimer.OnStop();
		m_pPlane.SetTextureInfo(AtlasInfoList[0]);
	}

	void		Update()
	{
		m_pTimer.Update(Time.deltaTime);	
	}

	public void OnPlay()
	{
		if (m_AtlasInfoList.Count != 0 && m_pPlane != null)
		{
			AtlasInfo atlasInfo = m_AtlasInfoList[0];
			m_pPlane.SetTextureInfo(atlasInfo);
		}

		m_pTimer.OnResetEvent();
		m_pTimer.OnResetCallback();
		m_pTimer.OnStop();

		float fSpliteTime = m_fTime / m_AtlasInfoList.Count;

		TransformerEvent_Scalar eventValue = null;

		for (int i = 1; i < m_AtlasInfoList.Count; ++i)
		{
			eventValue = new TransformerEvent_Scalar(fSpliteTime * i, i);
			m_pTimer.AddEvent(eventValue);
		}

		eventValue = new TransformerEvent_Scalar(m_fTime, 0);
		m_pTimer.AddEvent(eventValue);

		m_pTimer.SetCallback(OnDone_SpriteAnimOneEvent, OnDone_SpriteAnim);

		m_pTimer.OnPlay();
	}

	public void OnPause()
	{
		m_pTimer.OnPause();
	}

	public void OnStop()
	{
		m_pTimer.OnStop();
	}

	public void OnDone_SpriteAnimOneEvent(int key, TransformerEvent eventValue)
	{
		AtlasInfo atlasInfo = m_AtlasInfoList[key];

		if (m_pPlane != null)
		{
			m_pPlane.SetTextureInfo(atlasInfo);
		}
	}

	public void OnDone_SpriteAnim(TransformerEvent eventValue)
	{
		if (m_IsLoop)
		{
			m_pTimer.OnStop();
			m_pTimer.OnPlay();
		}
		else
		{
			if (m_Callback != null)
			{
				m_Callback();
				m_Callback = null;
			}
		}

		if (m_AtlasInfoList.Count != 0 && m_pPlane != null)
		{
			AtlasInfo atlasInfo = m_AtlasInfoList[0];
			m_pPlane.SetTextureInfo(atlasInfo);
		}
	}

	public void SetCallback(OnDone_Anim Callback)
	{
		m_Callback = Callback;
	}
}
