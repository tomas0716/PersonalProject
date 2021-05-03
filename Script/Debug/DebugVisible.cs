using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVisible: MonoBehaviour
{
    void Awake()
    {
#if !_DEBUG
		gameObject.SetActive(false);
#endif
	}
}
