using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class InGame_UI_MissionItem : MonoBehaviour
{
    private InGame_MainUI       m_pInGame_MainUI        = null;
    private eMissionType        m_eMissionType          = eMissionType.None;

    private SpriteAtlas         m_pSpriteAtlas          = null;
    private GameObject          m_pGameObject_Image     = null;
    private Text                m_pText_Count           = null;
    private GameObject          m_pGameObject_Complete  = null;
    private Transformer_Scalar  m_pScale_Complete       = new Transformer_Scalar(1.4f);

    private float               m_fScale                = 1;

    private bool                m_IsMissionComplete     = false;

    void Start()
    {
        m_pGameObject_Image = Helper.FindChildGameObject(gameObject, "Image");
        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Count");
        m_pText_Count = ob.GetComponent<Text>();

        m_pGameObject_Complete = Helper.FindChildGameObject(gameObject, "Image_Complete");
        m_pGameObject_Complete.SetActive(false);

        if (m_eMissionType != eMissionType.None && m_pSpriteAtlas != null)
        {
            ExcelData_MissionDataInfo pMissionDataInfo = ExcelDataManager.Instance.m_pExcelData_MissionData.GetMissionDataInfo(m_eMissionType);

            switch (m_eMissionType)
            {
                case eMissionType.Unit:
                    {
                        Image pImage = m_pGameObject_Image.GetComponent<Image>();
                        pImage.sprite = m_pSpriteAtlas.GetSprite(pMissionDataInfo.m_strMissionImageFileName + SavedGameDataInfo.Instance.m_eMissionUnitType.ToString());
                        pImage.SetNativeSize();
                        pImage.gameObject.transform.localScale = new Vector3(m_fScale, m_fScale, 1);
                    }
                    break;

                case eMissionType.Jelly:
                    {
                        Image pImage = m_pGameObject_Image.GetComponent<Image>();
                        pImage.sprite = m_pSpriteAtlas.GetSprite(pMissionDataInfo.m_strMissionImageFileName + "GUI");
                        pImage.SetNativeSize();
                        pImage.gameObject.transform.localScale = new Vector3(m_fScale, m_fScale, 1);
                    }
                    break;

                default:
                    {
                        Image pImage = m_pGameObject_Image.GetComponent<Image>();
                        pImage.sprite = m_pSpriteAtlas.GetSprite(pMissionDataInfo.m_strMissionImageFileName);
                        pImage.SetNativeSize();
                        pImage.gameObject.transform.localScale = new Vector3(m_fScale, m_fScale, 1);
                    }
                    break;
            }

            OnInGame_UpdateMissionCount(m_eMissionType);
        }

        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UpdateMissionCount += OnInGame_UpdateMissionCount;
    }

    private void OnDestroy()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UpdateMissionCount -= OnInGame_UpdateMissionCount;
    }

	private void Update()
	{
        m_pScale_Complete.Update(Time.deltaTime);
        float fScale = m_pScale_Complete.GetCurScalar();
        m_pGameObject_Complete.transform.localScale = new Vector3(fScale, fScale, 1);
    }

	public void SetMissionData(InGame_MainUI pInGame_MainUI, eMissionType eMissionType, SpriteAtlas pAtlas, float fScale)
    {
        m_pInGame_MainUI = pInGame_MainUI;
        m_eMissionType = eMissionType;
        m_pSpriteAtlas = pAtlas;
        m_fScale = fScale;
    }

    private void OnInGame_UpdateMissionCount(eMissionType eType)
    {
        if (eType == m_eMissionType && SavedGameDataInfo.Instance.m_MissionInfoTable.ContainsKey(m_eMissionType) == true && m_IsMissionComplete == false)
        {
            int nCount = SavedGameDataInfo.Instance.m_MissionInfoTable[m_eMissionType];
            if (nCount <= 0)
            {
                m_IsMissionComplete = true;

                Helper.OnSoundPlay(eSoundType.Mission, false);

                nCount = 0;
                m_pGameObject_Complete.SetActive(true);
                m_pText_Count.gameObject.SetActive(false);

                m_pScale_Complete.OnReset();
                TransformerEvent_Scalar eventValue;
                eventValue = new TransformerEvent_Scalar(0.12f, 0.8f);
                m_pScale_Complete.AddEvent(eventValue);
                eventValue = new TransformerEvent_Scalar(0.2f, 1.0f);
                m_pScale_Complete.AddEvent(eventValue);
                m_pScale_Complete.OnPlay();

                Update();
            }
            else
            {
                m_pText_Count.text = nCount.ToString();
            }
        }
    }

    public Vector3 GetPos()
    {
        Vector3 vPos = Camera.main.ScreenToWorldPoint(m_pGameObject_Image.gameObject.transform.position);

        Vector3 vUIPos = Helper.Get3DPosToUIPos(m_pInGame_MainUI.gameObject, Camera.main, vPos);
        return vUIPos;
    }
}
