using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class NumberPlane
{
	private List<AtlasInfo>		m_AtlasInfoList			= new List<AtlasInfo>();
	private List<Plane2D>		m_PlaneList				= new List<Plane2D>();
	private GameObject			m_pGameObject			= null;
	private int					m_nOriginNumber			= 0;
	private int 				m_nNumber 				= 0;
	private float				m_fHeight				= 0;
	private bool 				m_IsUsingSign 			= false;
	private bool 				m_IsUsingComma 			= false;
	private float 				m_fUnitInterval			= 0;
	private float				m_fNumberChangeTime		= 0;
	private float				m_fDepth				= 0;
	private Vector2				m_vPosition				= Vector2.zero;
	private Transformer_Scalar	m_pScalar				= new Transformer_Scalar(0);

	protected Vector3			m_vScale				= new Vector3(-1, -1, -1);
	protected bool				m_IsActiveColor			= false;
	protected Color				m_Color					= Color.white;

	// List Add Order : [0,1,2...9, (IsUsingSign == true : +,-), (IsUsingComma == true : ,)]
	public NumberPlane(List<AtlasInfo> AtlasInfoList, bool IsUsingSign, bool IsUsingComma, float UnitInterval, 
						float NumberChangeTime, float Depth)
	{
		m_AtlasInfoList		= AtlasInfoList;
		m_IsUsingSign		= IsUsingSign;
		m_IsUsingComma		= IsUsingComma;
		m_fUnitInterval		= UnitInterval * AppInstance.Instance.m_fMainScale;
		m_fNumberChangeTime	= NumberChangeTime;
		m_fDepth			= Depth;
		m_fHeight			= AtlasInfoList[0].GetHeight();

		m_pGameObject		= new GameObject("NumberPlane");

		UpdateNumber();
	}

	public void OnDestroy()
	{
		//foreach (Plane2D pPlane in m_PlaneList)
		//{
		//	pPlane.OnDestroy();
		//}

		//m_PlaneList.Clear();

		GameObject.Destroy(m_pGameObject);
		m_PlaneList.Clear();
	}

	public void 	Update(float fDeltaTime)
	{	
		m_pScalar.Update(fDeltaTime);
		int Number = (int)m_pScalar.GetCurScalar();
		
		if( Number != m_nNumber )
		{
			m_nNumber = Number;
			UpdateNumber();
		}
	}
	
	public void				SetNumber(int Number)
	{
		m_nOriginNumber = Number;
		
		m_pScalar.OnResetEvent();
		m_pScalar.OnResetCallback ();
		m_pScalar.OnStop ();
	
		if( m_fNumberChangeTime == 0 )
		{
			m_nNumber = Number;
			
			TransformerEvent_Scalar eventValue = null;
			eventValue = new TransformerEvent_Scalar(0, m_nNumber);
			m_pScalar.AddEvent(eventValue);
			
			m_pScalar.OnPlay ();			
		}
		else
		{		
			TransformerEvent_Scalar eventValue = null;
			eventValue = new TransformerEvent_Scalar(0, m_nNumber);
			m_pScalar.AddEvent(eventValue);
			eventValue = new TransformerEvent_Scalar(m_fNumberChangeTime, Number);
			m_pScalar.AddEvent(eventValue);			
			
			m_pScalar.OnPlay ();
		}
	
		UpdateNumber();
	}	
	
	public int 				GetNumber()
	{
		return m_nOriginNumber;
	}
	
	private void			UpdateNumber()
	{
		CalculationNumber();
	
		UpdateNumberInfo();
	}

	private void			RemoveNumberInfoAll()
	{
		foreach (Plane2D plane in m_PlaneList)
		{
			plane.OnDestroy();
		}

		m_PlaneList.Clear();
	}
	
	private void			CalculationNumber()
	{
		RemoveNumberInfoAll();

		Plane2D plane = null;
		int nNumber = m_nNumber;

		if( m_IsUsingSign == true )
		{
			if (nNumber > 0)
			{
				plane = new Plane2D(m_AtlasInfoList[10], m_fDepth);
				m_PlaneList.Add(plane);
			}
			else
			{
				plane = new Plane2D(m_AtlasInfoList[11], m_fDepth);
				m_PlaneList.Add(plane);
			}
		}

		string strNumber = nNumber.ToString();
		int nLen = strNumber.Length;

		bool IsComma = false;
		if (nLen > 3)
			IsComma = true;

		for (int i = 0; i < nLen; ++i)
		{
			if (i != 0 && ((nLen - i) % 3) == 0 && m_IsUsingComma == true && IsComma == true)
			{
				plane = new Plane2D(m_AtlasInfoList[12], m_fDepth);
				m_PlaneList.Add(plane);
			}

			char sNum = strNumber[i];
			int num = sNum - '0';

			plane = new Plane2D(m_AtlasInfoList[num], m_fDepth);
			m_PlaneList.Add(plane);
		}
	}

	private void			UpdateNumberInfo()
	{
		float EntireWidth = (m_PlaneList.Count - 1) * m_fUnitInterval;
		foreach (Plane2D plane in m_PlaneList)
		{
			AtlasInfo atlasInfo = plane.GetAtlasInfo();
			float Width = atlasInfo.GetWidth();
			float Height = atlasInfo.GetHeight();

			float Scale = m_fHeight / Height;
			plane.SetSize(new Vector2(Width * Scale, m_fHeight));

			EntireWidth += Width * Scale;
		}

		if (m_PlaneList.Count > 0)
		{
			Plane2D plane_first = m_PlaneList[0];

			float X = EntireWidth * -0.5f + plane_first.GetSize().x * 0.5f;
			foreach (Plane2D plane in m_PlaneList)
			{
				Vector2 vPos = new Vector2(X, 0);
				plane.GetGameObject().transform.SetParent(m_pGameObject.transform);
                plane.SetPosition(vPos);
				//plane.GetGameObject().transform.localPosition = vPos;
				plane.GetGameObject().transform.localScale = new Vector3(1,1,1);

				X += plane.GetSize().x + m_fUnitInterval;
			}
		}

		SetActiveColor(m_IsActiveColor);
		SetColor(m_Color);
	}

	public void				SetPosition(Vector2 vPos)
	{
		if (m_vPosition != vPos)
		{
			m_vPosition = vPos;
			m_pGameObject.transform.localPosition = m_vPosition;
		}
	}

	public Vector2			GetPosition()
	{
		return m_vPosition;
	}

	public void				SetHeight(float Height)
	{
		if (m_fHeight != Height)
		{
			m_fHeight = Height;
			UpdateNumberInfo();
		}
	}

	public float			GetHeight()
	{
		return m_fHeight;
	}

	public void SetScale(Vector3 Scale)
	{
		if (m_vScale != Scale)
		{
			m_vScale = Scale;
			Vector3 vNewScale = Scale * AppInstance.Instance.m_fMainScale;
			vNewScale.z = 1;

			if (m_pGameObject != null)
			{
				m_pGameObject.transform.localScale = vNewScale;
			}

			UpdateNumberInfo();
		}
	}

	public void SetScale(float fScale)
	{
		Vector3 vScale = new Vector3(fScale, fScale, 1);

		SetScale(vScale);
	}

	public void SetVisible(bool IsVisible)
	{
		m_pGameObject.SetActive(IsVisible);
	}

	public bool IsVisible()
	{
		return m_pGameObject.activeSelf;
	}

	public void 			SetNumberChangeTime(float time)
	{
		m_fNumberChangeTime = time;
	}
	
	public float 			GetNumberChangeTime()
	{
		return m_fNumberChangeTime;
	}

	public GameObject		GetGameObject()
	{
		return m_pGameObject;
	}

	public void				SetActiveColor(bool IsActive)
	{
		m_IsActiveColor = IsActive;

		foreach (Plane2D plane in m_PlaneList)
		{
			plane.SetActiveColor(m_IsActiveColor);
			plane.SetColor(m_Color);
		}
	}

	public void				SetColor(Color color)
	{
		m_Color = color;

		foreach (Plane2D plane in m_PlaneList)
		{
			plane.SetActiveColor(m_IsActiveColor);
			plane.SetColor(m_Color);
		}
	}

	public void				SetDepth_Backup()
	{
		foreach (Plane2D plane in m_PlaneList)
		{
			plane.SetDepth_Backup();
		}
	}

	public void				SetDepth(float fDepth)
	{
		foreach (Plane2D plane in m_PlaneList)
		{
			plane.SetDepth(fDepth);
		}
	}

	public void				ResetDepth_Backup()
	{
		foreach (Plane2D plane in m_PlaneList)
		{
			plane.ResetDepth_Backup();
		}
	}
}
