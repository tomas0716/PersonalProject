using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class NewGame_MissionItem : MonoBehaviour
{
    private eMissionType    m_eMissionType      = eMissionType.None;

    private SpriteAtlas     m_pSpriteAtlas      = null;
    private GameObject      m_pGameObject_Image = null;
    private Text            m_pText_Count       = null;

    private float           m_fScale            = 1;

    void Start()
    {
        m_pGameObject_Image = Helper.FindChildGameObject(gameObject, "Image");
        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Count");
        m_pText_Count = ob.GetComponent<Text>();

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

            int nCount = SavedGameDataInfo.Instance.m_MissionInfoTable[m_eMissionType];
            m_pText_Count.text = nCount.ToString();
        }
    }

    private void OnDestroy()
    {
    }

    public void SetMissionData(eMissionType eMissionType, SpriteAtlas pAtlas, float fScale)
    {
        m_eMissionType = eMissionType;
        m_pSpriteAtlas = pAtlas;
        m_fScale = fScale;
    }
}
