using UnityEngine;
using System.Collections;

public class PerspectiveCamera 
{
	private Vector2 	m_vPrevMousePos = new Vector2(0,0);
	private Vector2 	m_vMousePos		= new Vector2(0,0);
	
	enum	eMovementType
	{
		eMovement_Yaw = 0,
		eMovement_Pitch,

		eMovement_Wheel,

		eMovement_MovingX,
		eMovement_MovingY,
		eMovement_MovingZ,

		eMovement_Max,
	}

	private float	[]	m_fMovement = new float[(int)eMovementType.eMovement_Max];

	enum	eMouseEventType
	{
		eMouseEvent_None = 0,
		eMouseEvent_LButtonDown,
		eMouseEvent_RButtonDown,
		eMouseEvent_MButtonDown,
	}

	eMouseEventType	    m_eMouseEventType   = eMouseEventType.eMouseEvent_None;
	
	private Vector3		m_vPosition			= new Vector3(0,0,0);
	private Vector3		m_vTarget			= new Vector3(0,0,0);
	
	private Vector3		m_vDirection		= new Vector3(0,0,0);
	private Vector3		m_vRightDirection	= new Vector3(0,0,0);
	
	private bool 		m_IsCtrlKeyDown 	= true;
	
	public bool			m_IsActive 			= true;

	public PerspectiveCamera()
	{
		Reset();
	}
	
	public void 	Reset()
	{
		m_vMousePos.x = m_vPrevMousePos.x = 0;
		m_vMousePos.y = m_vPrevMousePos.y = 0;
		
		m_fMovement[(int)eMovementType.eMovement_Wheel] 	= 600.0f;
		m_fMovement[(int)eMovementType.eMovement_Pitch] 	= 45.0f;
		m_fMovement[(int)eMovementType.eMovement_Yaw] 	= 180.0f;
		
		m_fMovement[(int)eMovementType.eMovement_MovingX] = 0.0f;
		m_fMovement[(int)eMovementType.eMovement_MovingY] = 0.0f;
		m_fMovement[(int)eMovementType.eMovement_MovingZ] = 0.0f;	
		
		UpdateCamera();
	}
	
	public void 	Update () 
	{
		if( m_IsActive == true && Camera.main != null )
		{
			Camera.main.transform.position = m_vPosition;
			Camera.main.transform.rotation = Quaternion.LookRotation(m_vDirection);;	
		}
		
		OnMouseProcess();
	}
	
	void 	UpdateCamera()
	{
		Check();
		
		float fDeltaX, fDeltaY, fDeltaZ;
		
		fDeltaX = m_fMovement[(int)eMovementType.eMovement_Wheel] * Mathf.Sin(Mathf.Deg2Rad * m_fMovement[(int)eMovementType.eMovement_Yaw]) * Mathf.Cos(Mathf.Deg2Rad * m_fMovement[(int)eMovementType.eMovement_Pitch]);
		fDeltaY = m_fMovement[(int)eMovementType.eMovement_Wheel] * Mathf.Sin(Mathf.Deg2Rad * m_fMovement[(int)eMovementType.eMovement_Pitch]);
		fDeltaZ = m_fMovement[(int)eMovementType.eMovement_Wheel] * Mathf.Cos(Mathf.Deg2Rad * m_fMovement[(int)eMovementType.eMovement_Yaw]) * Mathf.Cos(Mathf.Deg2Rad * m_fMovement[(int)eMovementType.eMovement_Pitch]);		

		m_vTarget 	= new Vector3(m_fMovement[(int)eMovementType.eMovement_MovingX], m_fMovement[(int)eMovementType.eMovement_MovingY], m_fMovement[(int)eMovementType.eMovement_MovingZ]);
		m_vPosition 	= new Vector3(fDeltaX + m_fMovement[(int)eMovementType.eMovement_MovingX], fDeltaY + m_fMovement[(int)eMovementType.eMovement_MovingY], fDeltaZ + m_fMovement[(int)eMovementType.eMovement_MovingZ]);

		m_vDirection = m_vTarget - m_vPosition;
		m_vRightDirection = Vector3.Cross (m_vDirection, new Vector3(0,1,0));
	}
		
	void 	OnMouseProcess()
	{
		if (Input.GetMouseButtonDown(0))        // Left Button Down
		{
			OnLButtonDown(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		}
		else if (Input.GetMouseButtonUp(0)) // Left Button Up
		{
			OnLButtonUp(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		}
		else if (Input.GetMouseButtonDown(1))   // Right Button Down
		{
			OnRButtonDown(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		}
		else if (Input.GetMouseButtonUp(1)) // Right Button Up
		{
			OnRButtonUp(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0 && m_IsActive == true)
		{
			OnMouseWheel(-10);
		}
		else if (Input.GetAxis("Mouse ScrollWheel") > 0 && m_IsActive == true)
		{
			OnMouseWheel(10);
		}
		else if (m_IsActive == true && m_IsCtrlKeyDown == true)
		{
			OnMouseMove(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		}
	}

	void	Check()
	{
		if( m_fMovement[(int)eMovementType.eMovement_Pitch] > (180 * 0.4999f) )
			m_fMovement[(int)eMovementType.eMovement_Pitch] = 180 * 0.4999f;
		else if( m_fMovement[(int)eMovementType.eMovement_Pitch] < -(180 * 0.4999f) )
			m_fMovement[(int)eMovementType.eMovement_Pitch] = -(180 * 0.4999f);
	}

	void 	OnLButtonDown(Vector2 point)
	{
		m_vMousePos = point;
		m_eMouseEventType = eMouseEventType.eMouseEvent_LButtonDown;
	}
	
	void 	OnLButtonUp(Vector2 point)
	{
		m_eMouseEventType = eMouseEventType.eMouseEvent_None;
	}
	
	void 	OnRButtonDown(Vector2 point)
	{
		m_vMousePos = point;
		m_eMouseEventType = eMouseEventType.eMouseEvent_RButtonDown;
	}
	
	void 	OnRButtonUp(Vector2 point)
	{
		m_eMouseEventType = eMouseEventType.eMouseEvent_None;
	}
	
	void 	OnMouseMove(Vector2 point)
	{
		if( m_eMouseEventType == eMouseEventType.eMouseEvent_LButtonDown )
		{
			m_vPrevMousePos = m_vMousePos;
			m_vMousePos = point;
	
			Vector3 vRightDirection = m_vRightDirection;
			Vector3 vDirection = m_vDirection;
	
			vRightDirection.Normalize();
			vDirection.Normalize();
	
			Vector3 vDeltaX = vRightDirection * (m_vMousePos.x - m_vPrevMousePos.x);
			Vector3 vDeltaY = vDirection * (m_vPrevMousePos.y - m_vMousePos.y);
	
			m_fMovement[(int)eMovementType.eMovement_MovingX] += (vDeltaX.x + vDeltaY.x) * 0.6f;
			m_fMovement[(int)eMovementType.eMovement_MovingZ] += (vDeltaX.z + vDeltaY.z) * 0.6f;
	
			UpdateCamera();
		}
	
		if( m_eMouseEventType == eMouseEventType.eMouseEvent_RButtonDown )
		{
			m_vPrevMousePos = m_vMousePos;
			m_vMousePos = point;
	
			m_fMovement[(int)eMovementType.eMovement_Yaw]	+= (m_vMousePos.x - m_vPrevMousePos.x) * 0.3f;
			m_fMovement[(int)eMovementType.eMovement_Pitch]	+= (m_vPrevMousePos.y - m_vMousePos.y) * 0.3f;
	
			UpdateCamera();
		}
	}
	
	void	OnMouseWheel(short sDelta)
	{
		float fDelta = (float)(sDelta * 3.0f);
		Vector3 vDirection = m_vDirection;

		float fLength = Vector3.Distance(new Vector3(0, 0, 0), vDirection);

		if (sDelta < 0 && fLength > 100000)
			return;
		else if (sDelta > 0 && fLength < 0.1f)
			return;

		m_fMovement[(int)eMovementType.eMovement_Wheel] += ((float)-fDelta);

		UpdateCamera();
	}	
}
