using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

public class Tool_WindowCollect : MonoBehaviour
{
	private static Tool_WindowCollect ms_pInstance = null;
	public static Tool_WindowCollect Instance { get { return ms_pInstance; } }

	private void Awake()
	{
		ms_pInstance = this;
	}

	public GameObject	m_GameObject_CreateMap		= null;
	public GameObject	m_GameObject_MapShape		= null;
	public GameObject	m_GameObject_Mission		= null;

	public void Init()
	{
		if (m_GameObject_CreateMap != null)
			m_GameObject_CreateMap.SetActive(true);

		if (m_GameObject_MapShape != null)
			m_GameObject_MapShape.SetActive(true);

		if (m_GameObject_Mission != null)
			m_GameObject_Mission.SetActive(false);
	}
}

#endif