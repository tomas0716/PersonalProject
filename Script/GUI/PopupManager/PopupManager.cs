using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
	private static PopupManager m_pInstance = null;
	public static PopupManager Instance { get { return m_pInstance; } }

    private List<GameObject>    m_PopupList             = new List<GameObject>();
    private GameObject          m_pLastGameObject       = null;

    void Awake()
    {
        m_pInstance = this;
    }

    public void ClearAll()
    {
        m_pLastGameObject = null;
        m_PopupList.Clear();
    }

    public void AddPopup(GameObject ob)
    {
        if (m_pLastGameObject != null)
        {
            m_pLastGameObject.SetActive(false);
        }

        m_PopupList.Add(ob);
        m_pLastGameObject = ob;
    }

    public void ShowLastPopup()
    {
        if (m_pLastGameObject != null)
        {
            m_pLastGameObject.SetActive(true);
        }
    }

    public void HideLastPopup()
    {
        if (m_pLastGameObject != null)
        {
            m_pLastGameObject.SetActive(false);
        }
    }

    public void RemovePopup(GameObject ob)
    {
        if (m_pLastGameObject == ob)
        {
            if (m_PopupList.Count == 1)
            {
                m_pLastGameObject = null;
            }
            else if (m_PopupList.Count > 1)
            {
                m_pLastGameObject = m_PopupList[m_PopupList.Count - 2];
            }
        }

        GameObject.Destroy(ob);

        if (m_PopupList.Contains(ob) == true)
        {
            m_PopupList.Remove(ob);
        }
    }

    public void RemoveLastPopup()
    {
        if (m_pLastGameObject != null)
        {
            if (m_PopupList.Contains(m_pLastGameObject) == true)
            {
                GameObject.Destroy(m_pLastGameObject);
                m_PopupList.Remove(m_pLastGameObject);
            }

            if (m_PopupList.Count == 0)
            {
                m_pLastGameObject = null;
            }
            else
            {
                m_pLastGameObject = m_PopupList[m_PopupList.Count-1];
            }
        }
    }
}
