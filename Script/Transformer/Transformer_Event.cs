using System;
using UnityEngine;
using System.Collections;

public class TransformerEvent
{
	public float 		m_fTime         = 0.0f;
	public object 		m_pParameta     = null;
	public object []	m_pParametas	= null;
	
	public TransformerEvent ()
	{
	}	
}

public class TransformerEvent_Timer : TransformerEvent
{
	public TransformerEvent_Timer(float time)
	{
		m_fTime = time;
	}

	public TransformerEvent_Timer(float time, object parameta)
	{
		m_fTime = time;
		m_pParameta = parameta;
	}

	public TransformerEvent_Timer(float time, object [] parametas)
	{
		m_fTime = time;
		m_pParametas = parametas;
	}
}

public class TransformerEvent_Scalar : TransformerEvent
{
	public float 	m_fScalar = 0.0f;
	
	public TransformerEvent_Scalar(float time, float scalar)
	{
		m_fTime = time;
		m_fScalar = scalar;
	}
	
	public TransformerEvent_Scalar(float time, float scalar, object parameta)
	{
		m_fTime = time;
		m_fScalar = scalar;
		m_pParameta = parameta;
	}

	public TransformerEvent_Scalar(float time, float scalar, object[] parametas)
	{
		m_fTime = time;
		m_fScalar = scalar;
		m_pParametas = parametas;
	}
}

public class TransformerEvent_Vector2 : TransformerEvent
{
	public Vector2 m_Vector2;

	public TransformerEvent_Vector2(float time, Vector3 vector2)
	{
		m_fTime = time;
		m_Vector2 = vector2;
	}

	public TransformerEvent_Vector2(float time, Vector3 vector2, object parameta)
	{
		m_fTime = time;
		m_Vector2 = vector2;
		m_pParameta = parameta;
	}

	public TransformerEvent_Vector2(float time, Vector3 vector2, object[] parametas)
	{
		m_fTime = time;
		m_Vector2 = vector2;
		m_pParametas = parametas;
	}
}

public class TransformerEvent_Vector3 : TransformerEvent
{
	public Vector3 	m_Vector3;
	
	public TransformerEvent_Vector3(float time, Vector3 scalar3)
	{
		m_fTime = time;
		m_Vector3 = scalar3;
	}
	
	public TransformerEvent_Vector3(float time, Vector3 scalar3, object parameta)
	{
		m_fTime = time;
		m_Vector3 = scalar3;
		m_pParameta = parameta;
	}

	public TransformerEvent_Vector3(float time, Vector3 scalar3, object[] parametas)
	{
		m_fTime = time;
		m_Vector3 = scalar3;
		m_pParametas = parametas;
	}
}

public class TransformerEvent_Color : TransformerEvent
{
	public Color m_Color;

	public TransformerEvent_Color(float time, Color color)
	{
		m_fTime = time;
		m_Color = color;
	}

	public TransformerEvent_Color(float time, Color color, object parameta)
	{
		m_fTime = time;
		m_Color = color;
		m_pParameta = parameta;
	}

	public TransformerEvent_Color(float time, Color color, object[] parametas)
	{
		m_fTime = time;
		m_Color = color;
		m_pParametas = parametas;
	}
}