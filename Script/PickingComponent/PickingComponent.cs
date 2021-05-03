using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PickingComponent : MonoBehaviour
{
	public delegate void Callback_LButtonDown(GameObject gameObject, Vector3 vPos, object ob, int nFingerID);
	public delegate void Callback_LButtonUp(GameObject gameObject, Vector3 vPos, object ob, int nFingerID, bool IsDown);
	public delegate void Callback_RButtonDown(GameObject gameObject, Vector3 vPos, object ob, int nFingerID);
	public delegate void Callback_RButtonUp(GameObject gameObject, Vector3 vPos, object ob, int nFingerID);
	public delegate void Callback_Touch(GameObject gameObject, Vector3 vPos, object ob);
	public delegate void Callback_Move(GameObject gameObject, Vector3 vPos, object ob, int nFingerID);

	public event Callback_LButtonDown	m_Callback_LButtonDown;
	public event Callback_LButtonUp		m_Callback_LButtonUp;
	public event Callback_RButtonDown	m_Callback_RButtonDown;
	public event Callback_RButtonUp		m_Callback_RButtonUp;
	public event Callback_Move			m_Callback_Move;
	public event Callback_Touch			m_Callback_Touch;

	private bool						m_IsPickingCheck	= false;
	private object						m_pParameta			= null;

	private Dictionary<int,bool>		m_FingerIDTable = new Dictionary<int, bool>();

	static private int					m_nFirstTouchID		= -1;

#if UNITY_EDITOR
	private bool						m_IsLButtonDown		= false;
	private bool						m_IsRButtonDown		= false;
#endif

	void Update()
	{
#if UNITY_EDITOR
		if (m_IsPickingCheck == true)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			int layerMask = 1 << gameObject.layer;
			if (Physics.Raycast(ray, out hit, 10000000, layerMask))
			{
				if (hit.collider.gameObject == gameObject)
				{
					if (Input.GetMouseButtonDown(0) == true)
					{
						if (m_IsLButtonDown == false)
						{
							m_IsLButtonDown = true;

							m_Callback_LButtonDown?.Invoke(gameObject, hit.point, m_pParameta, 0);
						}
					}
					else if (Input.GetMouseButtonUp(0) == true)
					{
						if (m_IsLButtonDown == true)
						{
							m_Callback_LButtonUp?.Invoke(gameObject, hit.point, m_pParameta, 0, m_IsLButtonDown);

							m_IsLButtonDown = false;
						}
					}
					else if (Input.GetMouseButtonDown(1) == true)
					{
						if (m_IsRButtonDown == false)
						{
							m_IsRButtonDown = true;

							m_Callback_RButtonDown?.Invoke(gameObject, hit.point, m_pParameta, 0);
						}
					}
					else if (Input.GetMouseButtonUp(1) == true)
					{
						if (m_IsRButtonDown == true)
						{
							m_Callback_RButtonUp?.Invoke(gameObject, hit.point, m_pParameta, 0);

							m_IsRButtonDown = false;
						}
					}
					else
					{
						m_Callback_Move?.Invoke(gameObject, hit.point, m_pParameta, 0);
					}
				}
				else
				{
					if (Input.GetMouseButtonUp(0) == true)
					{
						m_IsLButtonDown = false;
					}

					if (Input.GetMouseButtonUp(1) == true)
					{
						m_IsRButtonDown = false;
					}
				}
			}
			else
			{
				if (Input.GetMouseButtonUp(0) == true)
				{
					m_IsLButtonDown = false;
				}

				if (Input.GetMouseButtonUp(1) == true)
				{
					m_IsRButtonDown = false;
				}
			}
		}
#endif
		if (m_IsPickingCheck == true)
		{
			if (Input.touchCount > 0)
			{
				for (int i = 0; i < Input.touchCount; ++i)
				{
					Touch touch = Input.GetTouch(i);

					Vector3 touchedPos = Camera.main.ScreenToWorldPoint(touch.position);

					Ray ray = Camera.main.ScreenPointToRay(touch.position);
					RaycastHit hit;

					int layerMask = 1 << gameObject.layer;
					if (Physics.Raycast(ray, out hit, 10000000, layerMask))
					{
						if (hit.collider.gameObject == gameObject)
						{
							if (touch.phase == TouchPhase.Began)
							{
								if (m_nFirstTouchID == -1 || GameDefine.ms_IsMultiTouchActive == true)
								{
									m_nFirstTouchID = touch.fingerId;
									m_Callback_LButtonDown?.Invoke(gameObject, hit.point, m_pParameta, touch.fingerId);

									m_FingerIDTable.Add(touch.fingerId, true);
								}
							}
							else if (touch.phase == TouchPhase.Ended)
							{
								if (m_FingerIDTable.ContainsKey(touch.fingerId) == true)
								{
									m_Callback_LButtonUp?.Invoke(gameObject, hit.point, m_pParameta, touch.fingerId, true);

									if (m_Callback_Touch != null)
									{
										m_Callback_Touch(gameObject, hit.point, m_pParameta);
										m_FingerIDTable.Remove(touch.fingerId);
									}
								}
								else
								{
									m_Callback_LButtonUp?.Invoke(gameObject, hit.point, m_pParameta, touch.fingerId, false);
								}

								if (m_nFirstTouchID == touch.fingerId)
								{
									m_nFirstTouchID = -1;
								}
							}
							else if (touch.phase == TouchPhase.Moved)
							{
								m_Callback_Move?.Invoke(gameObject, hit.point, m_pParameta, touch.fingerId);
							}
						}
					}
					else
					{
						if (touch.phase == TouchPhase.Ended)
						{
							if (m_FingerIDTable.ContainsKey(touch.fingerId) == true)
							{
								m_FingerIDTable.Remove(touch.fingerId);
							}

							if (m_nFirstTouchID == touch.fingerId)
							{
								m_nFirstTouchID = -1;
							}
						}
					}
				}
			}
		}
	}

	public void AddCallback_LButtonDown(Callback_LButtonDown function)
	{
		m_Callback_LButtonDown += function;
		m_IsPickingCheck = true;
	}

	public void AddCallback_LButtonUp(Callback_LButtonUp function)
	{
		m_Callback_LButtonUp += function;
		m_IsPickingCheck = true;
	}

	public void AddCallback_RButtonDown(Callback_RButtonDown function)
	{
		m_Callback_RButtonDown += function;
		m_IsPickingCheck = true;
	}

	public void AddCallback_RButtonUp(Callback_RButtonUp function)
	{
		m_Callback_RButtonUp += function;
		m_IsPickingCheck = true;
	}

	public void AddCallback_Move(Callback_Move function)
	{
		m_Callback_Move += function;
		m_IsPickingCheck = true;
	}

	public void AddCallback_Touch(Callback_Touch function)
	{
		m_Callback_Touch += function;
		m_IsPickingCheck = true;
	}

	public void RemoveCallback_LButtonDown(Callback_LButtonDown function)
	{
		m_Callback_LButtonDown -= function;
		m_IsPickingCheck = false;
	}

	public void RemoveCallback_LButtonUp(Callback_LButtonUp function)
	{
		m_Callback_LButtonUp -= function;
		m_IsPickingCheck = false;
	}

	public void RemoveCallback_RButtonDown(Callback_RButtonDown function)
	{
		m_Callback_RButtonDown -= function;
		m_IsPickingCheck = false;
	}

	public void RemoveCallback_RButtonClick(Callback_RButtonUp function)
	{
		m_Callback_RButtonUp -= function;
		m_IsPickingCheck = false;
	}

	public void RemoveCallback_Move(Callback_Move function)
	{
		m_Callback_Move -= function;
		m_IsPickingCheck = false;
	}

	public void RemoveCallback_Touch(Callback_Touch function)
	{
		m_Callback_Touch -= function;
		m_IsPickingCheck = false;
	}

	public void SetParameta(object ob)
	{
		m_pParameta = ob;
	}

	static public Vector3 GetCurrPickingPos(string LayerName)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		int layerMask = 1 << LayerMask.NameToLayer(LayerName);
		if (Physics.Raycast(ray, out hit, 100000, layerMask))
		{
			return hit.point;
		}

		return Vector3.zero;
	}
}
