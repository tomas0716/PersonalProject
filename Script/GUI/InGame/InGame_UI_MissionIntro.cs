using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class InGame_UI_MissionIntro : MonoBehaviour
{
    private SpriteAtlas         m_pSpriteAtlas_MIssion  = null;

    private GameObject          m_pGameObject_Parent    = null;
    private Image               m_pImage_Back           = null;
    private Text                m_pText_Desc            = null;
    private Color               m_pColor_Desc           = Color.white;

    private List<InGame_UI_MissionIntroItem> m_MissionIntorItemList = new List<InGame_UI_MissionIntroItem>();

    private Transformer_Scalar  m_pScale                = new Transformer_Scalar(0);
    private Transformer_Scalar  m_pAlpha                = new Transformer_Scalar(1);

    void Start()
    {
        GameInfo.Instance.m_IsInGame_MissionIntro = true;

        m_pGameObject_Parent = Helper.FindChildGameObject(gameObject, "Parent");

        int nCount = SavedGameDataInfo.Instance.m_MissionInfoTable.Count;

        SpriteAtlas sa = Resources.Load<SpriteAtlas>("Gui/InGame_MIssion/Atlas_Main");
        m_pSpriteAtlas_MIssion = SpriteAtlas.Instantiate(sa);

        GameObject pGameObject;
        pGameObject = Helper.FindChildGameObject(gameObject, "Image");
        m_pImage_Back = pGameObject.GetComponent<Image>();

        pGameObject = Helper.FindChildGameObject(gameObject, "Text_Desc");
        m_pText_Desc = pGameObject.GetComponent<Text>();
        m_pColor_Desc = m_pText_Desc.color;

        if (nCount == 1)
        {
            int nID = (int)SavedGameDataInfo.Instance.m_MapDataMissionList[0].m_eMissionType + 200001;
            m_pText_Desc.text = Helper.GetTextDataString(nID);
        }
        if (nCount > 1)
        {
            m_pText_Desc.text = Helper.GetTextDataString(200001);
        }

        pGameObject = Helper.FindChildGameObject(gameObject, "MissionDummy");

        int nX = 0;
        int nInterval = 0;

        if (nCount == 2)
        {
            nX = -78;
            nInterval = 156;
        }

        foreach (KeyValuePair<eMissionType, int> item in SavedGameDataInfo.Instance.m_MissionInfoTable)
        {
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/InGame_MissionIntroItem") as GameObject;
            ob = GameObject.Instantiate(ob);

            InGame_UI_MissionIntroItem pInGame_UI_MissionIntoItem = ob.GetComponent<InGame_UI_MissionIntroItem>();
            pInGame_UI_MissionIntoItem.SetMissionData(item.Key, m_pSpriteAtlas_MIssion);

            ob.transform.SetParent(pGameObject.transform);
            ob.transform.localPosition = new Vector3(nX, 0, 0);
            ob.transform.localScale = new Vector3(1, 1, 1);
            nX += nInterval;

            m_MissionIntorItemList.Add(pInGame_UI_MissionIntoItem);
        }

        TransformerEvent_Scalar eventValue;
        eventValue = new TransformerEvent_Scalar(0,0);
        m_pScale.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.5f, 0);
        m_pScale.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.7f, 1.05f);
        m_pScale.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.85f, 0.975f);
        m_pScale.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(1.0f, 1.025f);
        m_pScale.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(1.125f, 1);
        m_pScale.AddEvent(eventValue);
        m_pScale.OnPlay();

        eventValue = new TransformerEvent_Scalar(3, 1);
        m_pAlpha.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(3.08f, 0);
        m_pAlpha.AddEvent(eventValue);
        m_pAlpha.SetCallback(null, OnDone_Alpha);
        m_pAlpha.OnPlay();

        Update();

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

	private void OnDestroy()
	{
        GameInfo.Instance.m_IsInGame_MissionIntro = false;

        GameObject.Destroy(m_pSpriteAtlas_MIssion);

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
    }

	void Update()
    {
        m_pScale.Update(Time.deltaTime);
        float fScale = m_pScale.GetCurScalar();
        m_pGameObject_Parent.transform.localScale = new Vector3(fScale, fScale, 1);

        m_pAlpha.Update(Time.deltaTime);
		float fAlpha = m_pAlpha.GetCurScalar();
		m_pImage_Back.color = new Color(1, 1, 1, fAlpha);
		m_pText_Desc.color = new Color(m_pColor_Desc.r, m_pColor_Desc.g, m_pColor_Desc.b, fAlpha);

		foreach (InGame_UI_MissionIntroItem pMissionIntroItem in m_MissionIntorItemList)
        {
            pMissionIntroItem.SetAlpha(fAlpha);
        }
    }

    private void OnDone_Alpha(TransformerEvent eventValue)
    {
        GameObject.Destroy(gameObject);
    }

    public void OnHardwareBackButtonClick()
    {
        if (GameInfo.Instance.m_IsHardwareBackButtonProcess == false &&
            GameInfo.Instance.m_IsShopOpen == false &&
            GameInfo.Instance.m_IsItemBuyOpen == false &&
            GameInfo.Instance.m_nMessageBoxOpenCount == 0 &&
            GameInfo.Instance.m_nRewardPopupOpenCount == 0)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = true;
        }
    }
}
