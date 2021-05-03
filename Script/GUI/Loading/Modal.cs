using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modal : MonoBehaviour
{
    void Start()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventDeleteModal += OnDeleteModal;
    }

    private void OnDestroy()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventDeleteModal -= OnDeleteModal;
    }

    public void OnDeleteModal()
    {
        Destroy(gameObject);
    }
}
