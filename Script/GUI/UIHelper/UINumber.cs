using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINumber : MonoBehaviour
{
	private static GameObject	ms_GameObject_Image		= null;
	private GameObject			m_pGameObject			= null;
	private List<Sprite>		m_SpriteList			= new List<Sprite>();
	private List<GameObject>	m_ImageGameObjectList	= new List<GameObject>();
	private int					m_nOriginNumber			= 0;
	private int					m_nNumber				= 0;
	private int					m_nHeight				= 0;
	private bool				m_IsUsingSign			= false;
	private bool				m_IsUsingComma			= false;
	private bool				m_IsUsingMultiple		= false;
	private float				m_fUnitInterval			= 0;
	private float				m_fNumberChangeTime		= 0;
	private Transformer_Scalar	m_pScalar				= null;
	private float				m_fFitImageRate			= 0;
	private int					m_nDigits				= -1;

	private Color				m_Color					= Color.white;

	private float				m_fAlpha				= 1.0f;

	private eScreenAnchorPosition	m_eAnchorPosition	= eScreenAnchorPosition.UpperLeft;

	// List Add Order : [0,1,2...9, (IsUsingSign == true : +,-), (IsUsingComma == true : ,), (m_IsUsingMultiple == true : x)]
	public GameObject Initialize(string strGameObjectName, List<Sprite> SpriteList, int nNumber, int nHeight, bool IsUsingSign, bool IsUsingComma, bool IsUsingMultiple, float UnitInterval, 
								 float NumberChangeTime, eScreenAnchorPosition eAnchorPosition, int nDigits = -1)
	{
		if (ms_GameObject_Image == null)
		{
			GameObject ob = Resources.Load("Gui/Common/Image") as GameObject;
			ms_GameObject_Image = GameObject.Instantiate(ob);
		}

		m_pGameObject		= new GameObject(strGameObjectName);
		m_SpriteList		= SpriteList;
		m_nNumber			= nNumber;
		m_nOriginNumber		= nNumber;
		m_nHeight			= nHeight;
		m_IsUsingSign		= IsUsingSign;
		m_IsUsingComma		= IsUsingComma;
		m_IsUsingMultiple	= IsUsingMultiple;
		m_fUnitInterval		= UnitInterval;
		m_fNumberChangeTime = NumberChangeTime;
		m_eAnchorPosition	= eAnchorPosition;
		m_fFitImageRate = (float)(((float)nHeight) / ((float)SpriteList[0].rect.height));
		m_nDigits			= nDigits;
		
		m_pScalar = new Transformer_Scalar(m_nNumber);

		UpdateNumber();

		m_pGameObject.transform.parent = gameObject.transform;
		m_pGameObject.transform.localPosition = Vector3.zero;
		m_pGameObject.transform.localScale = Vector3.one;

		if (m_IsUsingSign == true && m_IsUsingMultiple == true)
		{
			Debug.LogException(new System.Exception("m_IsUsingSign == true && m_IsUsingMultiple == true"));
		}

		return m_pGameObject;
	}

	public void Update()
	{
		m_pScalar.Update(Time.deltaTime);
		int Number = (int)m_pScalar.GetCurScalar();

		if (Number != m_nNumber)
		{
			m_nNumber = Number;
			UpdateNumber();
		}
	}

	public GameObject GetGameObject()
	{
		return m_pGameObject;
	}

	public void SetNumber(int Number)
	{
		m_nOriginNumber = Number;

		m_pScalar.OnResetEvent();
		m_pScalar.OnResetCallback();
		m_pScalar.OnStop();

		if (m_fNumberChangeTime == 0)
		{
			m_nNumber = Number;

			TransformerEvent_Scalar eventValue = null;
			eventValue = new TransformerEvent_Scalar(0, m_nNumber);
			m_pScalar.AddEvent(eventValue);

			m_pScalar.OnPlay();
		}
		else
		{
			TransformerEvent_Scalar eventValue = null;
			eventValue = new TransformerEvent_Scalar(0, m_nNumber);
			m_pScalar.AddEvent(eventValue);
			eventValue = new TransformerEvent_Scalar(m_fNumberChangeTime, Number);
			m_pScalar.AddEvent(eventValue);

			m_pScalar.OnPlay();
		}

		UpdateNumber();
	}

	public int GetNumber()
	{
		return m_nOriginNumber;
	}

	private void UpdateNumber()
	{
		CalculationNumber();

		UpdateNumberInfo();
	}

	private void RemoveNumberInfoAll()
	{
		foreach (GameObject ob in m_ImageGameObjectList)
		{
			Destroy(ob);
		}

		m_ImageGameObjectList.Clear();
	}

	private void CalculationNumber()
	{
		RemoveNumberInfoAll();

		GameObject ob;
		Image pImage;
		RectTransform pRectTransform;

		int nNumber = m_nNumber;

		if (m_IsUsingSign == true)
		{
			if (nNumber > 0)
			{
				ob = GameObject.Instantiate(ms_GameObject_Image);
				ob.name = "+";
				pImage = ob.GetComponent<Image>();
				ob.transform.SetParent(m_pGameObject.transform);
				pImage.sprite = m_SpriteList[10];
				m_ImageGameObjectList.Add(ob);
				pRectTransform = ob.transform as RectTransform;
				pRectTransform.sizeDelta = new Vector2(pImage.sprite.rect.width * m_fFitImageRate, m_nHeight);
			}
			else
			{
				ob = GameObject.Instantiate(ms_GameObject_Image);
				ob.name = "-";
				pImage = ob.GetComponent<Image>();
				ob.transform.SetParent(m_pGameObject.transform);
				pImage.sprite = m_SpriteList[11];
				m_ImageGameObjectList.Add(ob);
				pRectTransform = ob.transform as RectTransform;
				pRectTransform.sizeDelta = new Vector2(pImage.sprite.rect.width * m_fFitImageRate, m_nHeight);
			}
		}

		if (m_IsUsingMultiple == true)
		{
			ob = GameObject.Instantiate(ms_GameObject_Image);
			ob.name = "x";
			pImage = ob.GetComponent<Image>();
			ob.transform.SetParent(m_pGameObject.transform);
			pImage.sprite = m_SpriteList[10];
			m_ImageGameObjectList.Add(ob);
			pRectTransform = ob.transform as RectTransform;
			pRectTransform.sizeDelta = new Vector2(pImage.sprite.rect.width * m_fFitImageRate, m_nHeight);
		}

		string strNumber = nNumber.ToString();
		int nLen = strNumber.Length;

		if (m_nDigits != -1)
		{
			int nZeroCount = m_nDigits - nLen;

			for (int i = 0; i < nZeroCount; ++i)
			{
				int num = 0;

				ob = GameObject.Instantiate(ms_GameObject_Image);
				ob.name = num.ToString();
				pImage = ob.GetComponent<Image>();
				ob.transform.SetParent(m_pGameObject.transform);
				pImage.sprite = m_SpriteList[num];
				m_ImageGameObjectList.Add(ob);
				pRectTransform = ob.transform as RectTransform;
				pRectTransform.sizeDelta = new Vector2(pImage.sprite.rect.width * m_fFitImageRate, m_nHeight);
			}
		}

		bool IsComma = false;
		if (nLen > 3)
			IsComma = true;

		for (int i = 0; i < nLen; ++i)
		{
			if (i != 0 && ((nLen - i) % 3) == 0 && m_IsUsingComma == true && IsComma == true)
			{
				ob = GameObject.Instantiate(ms_GameObject_Image);
				ob.name = ",";
				pImage = ob.GetComponent<Image>();
				ob.transform.SetParent(m_pGameObject.transform);

				if (m_IsUsingSign == true || m_IsUsingMultiple == true)	
					pImage.sprite = m_SpriteList[12];
				else							
					pImage.sprite = m_SpriteList[10];

				m_ImageGameObjectList.Add(ob);
				pRectTransform = ob.transform as RectTransform;
				pRectTransform.sizeDelta = new Vector2(pImage.sprite.rect.width * m_fFitImageRate, m_nHeight);
			}

			char sNum = strNumber[i];
			int num = sNum - '0';

			ob = GameObject.Instantiate(ms_GameObject_Image);
			ob.name = num.ToString();
			pImage = ob.GetComponent<Image>();
			ob.transform.SetParent(m_pGameObject.transform);
			pImage.sprite = m_SpriteList[num];
			m_ImageGameObjectList.Add(ob);
			pRectTransform = ob.transform as RectTransform;
			pRectTransform.sizeDelta = new Vector2(pImage.sprite.rect.width * m_fFitImageRate, m_nHeight);
		}
	}

	private void UpdateNumberInfo()
	{
		float EntireWidth = 0;

		foreach (GameObject ob in m_ImageGameObjectList)
		{
			Image pImage = ob.GetComponent<Image>();
			EntireWidth += pImage.sprite.rect.width * m_fFitImageRate;
		}

		EntireWidth += (m_ImageGameObjectList.Count - 1) * m_fUnitInterval;

		int nHalfEntireWidth = (int)(EntireWidth * 0.5f);
		int nHalfWidth = (int)((EntireWidth/m_ImageGameObjectList.Count) * 0.5f);
		int nHalfHeight = (int)(m_nHeight * 0.5f);
		Vector3 vPos = Vector3.zero;

		switch (m_eAnchorPosition)
		{
			case eScreenAnchorPosition.UpperLeft:		vPos = new Vector3(nHalfWidth, nHalfHeight, 0);						break;
			case eScreenAnchorPosition.UpperCenter:		vPos = new Vector3(-nHalfEntireWidth + nHalfWidth, nHalfHeight, 0);	break;
			case eScreenAnchorPosition.UpperRight:		vPos = new Vector3(-EntireWidth + nHalfWidth, nHalfHeight, 0);		break;
			case eScreenAnchorPosition.MiddleLeft:		vPos = new Vector3(nHalfWidth, 0, 0);								break;
			case eScreenAnchorPosition.MiddleCenter:	vPos = new Vector3(-nHalfEntireWidth + nHalfWidth, 0, 0);			break;
			case eScreenAnchorPosition.MiddleRight:		vPos = new Vector3(-EntireWidth + nHalfWidth, 0, 0);				break;
			case eScreenAnchorPosition.LowerLeft:		vPos = new Vector3(nHalfWidth, -nHalfWidth, 0);						break;
			case eScreenAnchorPosition.LowerCenter:		vPos = new Vector3(-nHalfEntireWidth + nHalfWidth, -nHalfWidth, 0); break;
			case eScreenAnchorPosition.LowerRight:		vPos = new Vector3(-EntireWidth + nHalfWidth, -nHalfWidth, 0);		break;
		}

		for( int i = 0; i < m_ImageGameObjectList.Count; ++i )
		{
			GameObject ob = m_ImageGameObjectList[i];

			ob.transform.localPosition = vPos;
			ob.transform.localScale = Vector3.one;

			Image pImage = ob.GetComponent<Image>();
			m_Color.a = m_fAlpha;
			pImage.color = m_Color;

			if (i != m_ImageGameObjectList.Count - 1)
			{
				GameObject ob_next = m_ImageGameObjectList[i + 1];
				Image pImage_Next = ob_next.GetComponent<Image>();
				vPos.x += (pImage.sprite.rect.width * m_fFitImageRate * 0.5f) + (pImage_Next.sprite.rect.width * m_fFitImageRate * 0.5f) + m_fUnitInterval;
			}
		}
	}

	public void SetNumberChangeTime(float time)
	{
		m_fNumberChangeTime = time;
	}

	public float GetNumberChangeTime()
	{
		return m_fNumberChangeTime;
	}

	public void SetAlpha(float fAlpha)
	{
		m_fAlpha = fAlpha;

		foreach (GameObject ob in m_ImageGameObjectList)
		{
			Image pImage = ob.GetComponent<Image>();
			pImage.color = new Color(1,1,1, m_fAlpha);
		}
	}

	public Vector2 GetSize()
	{
		float EntireWidth = 0;

		foreach (GameObject ob in m_ImageGameObjectList)
		{
			Image pImage = ob.GetComponent<Image>();
			EntireWidth += pImage.sprite.rect.width * m_fFitImageRate;
		}

		EntireWidth += (m_ImageGameObjectList.Count - 1) * m_fUnitInterval;

		return new Vector2(EntireWidth, m_nHeight);
	}

	public void SetColor(Color color)
	{
		m_Color = color;

		foreach (GameObject ob in m_ImageGameObjectList)
		{
			Image pImage = ob.GetComponent<Image>();
			m_Color.a = m_fAlpha;
			pImage.color = m_Color;
		}
	}
}
