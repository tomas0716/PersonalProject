using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
	public delegate void Callback_OK();
	public delegate void Callback_Cancel();

	public event Callback_OK		m_Callback_OK;
	public event Callback_Cancel	m_Callback_Cancel;

	private GameObject				m_pGameObject_Parent	= null;
	private Transformer_Scalar		m_pScale				= new Transformer_Scalar(0);

	public enum eMessageBoxType
	{
		OK,
		OKCancel,
	}

	private eMessageBoxType m_eMessageBoxType = eMessageBoxType.OK;

    private int m_nIndex = 0;

	public void Initialize(eMessageBoxType eType, string strTitle, string strDesc, string strOKText, string strCancelText)
    {
        ++GameInfo.Instance.m_nMessageBoxOpenCount;
        m_nIndex = GameInfo.Instance.m_nMessageBoxOpenCount;

        m_pGameObject_Parent = Helper.FindChildGameObject(gameObject, "Parent");
		m_pGameObject_Parent.transform.localScale = new Vector3(0, 0, 1);

		m_eMessageBoxType = eType;

		GameObject ob;
		Text pText;

		ob = Helper.FindChildGameObject(gameObject, "Text_Title");
		pText = ob.GetComponent<Text>();
		pText.text = strTitle;

		string strReplace = strDesc.Replace("\\n", "\n");
		ob = Helper.FindChildGameObject(gameObject, "Text_Desc");
		pText = ob.GetComponent<Text>();
		pText.text = strReplace;

		if (strTitle == "")
		{
			ob.transform.localPosition = new Vector3(0,80,0);
		}

		switch (eType)
		{
			case eMessageBoxType.OK:
				{
					ob = Helper.FindChildGameObject(gameObject, "OKCancel");
					ob.SetActive(false);

					ob = Helper.FindChildGameObject(gameObject, "OK");
					ob = Helper.FindChildGameObject(ob, "OK_Button");
					ob = Helper.FindChildGameObject(ob, "Text");
					pText = ob.GetComponent<Text>();
					pText.text = strOKText;
				}
				break;

			case eMessageBoxType.OKCancel:
				{
					ob = Helper.FindChildGameObject(gameObject, "OK");
					ob.SetActive(false);

					GameObject ob_OKCancel = Helper.FindChildGameObject(gameObject, "OKCancel");
					ob = Helper.FindChildGameObject(ob_OKCancel, "OK_Button");
					ob = Helper.FindChildGameObject(ob, "Text");
					pText = ob.GetComponent<Text>();
					pText.text = strOKText;

					ob = Helper.FindChildGameObject(ob_OKCancel, "Cancel_Button");
					ob = Helper.FindChildGameObject(ob, "Text");
					pText = ob.GetComponent<Text>();
					pText.text = strCancelText;
				}
				break;
		}

		Helper.OnSoundPlay(eSoundType.Popup, false);

		TransformerEvent_Scalar eventValue;
		eventValue = new TransformerEvent_Scalar(0.2f, 1.0f);
		m_pScale.AddEvent(eventValue);
		m_pScale.OnPlay();

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

    private void OnDestroy()
    {
        --GameInfo.Instance.m_nMessageBoxOpenCount;

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
    }

    private void Update()
	{
		m_pScale.Update(Time.deltaTime);
		float fScale = m_pScale.GetCurScalar();
		m_pGameObject_Parent.transform.localScale = new Vector3(fScale, fScale, 1);
	}

	public void OnButtonClick_Close()
	{
		if (m_eMessageBoxType == eMessageBoxType.OK)
		{
			Helper.OnSoundPlay(eSoundType.Popup, false);

			m_Callback_OK?.Invoke();
			Destroy(gameObject);
		}
	}

	public void OnButtonClick_OK()
	{
		Helper.OnSoundPlay(eSoundType.Popup, false);

		m_Callback_OK?.Invoke();
		Destroy(gameObject);
	}

	public void OnButtonClick_Cancel()
	{
		Helper.OnSoundPlay(eSoundType.Popup, false);

		m_Callback_Cancel?.Invoke();
		Destroy(gameObject);
	}

    public void OnHardwareBackButtonClick()
    {
        if (GameInfo.Instance.m_IsHardwareBackButtonProcess == false &&
            m_nIndex == GameInfo.Instance.m_nMessageBoxOpenCount)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = true;

            if (m_eMessageBoxType == eMessageBoxType.OK)
            {
                OnButtonClick_OK();
            }
        }
    }
}
