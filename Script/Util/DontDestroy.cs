using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
	private static DontDestroy m_pInstance = null;
	public static DontDestroy Instance { get { return m_pInstance; } }

	private void Awake()
	{
		if (m_pInstance != null)
		{
			Destroy(gameObject);
			return;
		}

		m_pInstance = this;
		DontDestroyOnLoad(gameObject);
	}
	void Start()
    {
        
    }
	
    void Update()
    {
        
    }
}
