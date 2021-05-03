using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class InGame_UI_MissionIntroItem : MonoBehaviour
{
    private eMissionType        m_eMissionType              = eMissionType.None;

    private SpriteAtlas         m_pSpriteAtlas              = null;
    private Image               m_pImage_Icon               = null;
    private Text                m_pText_Count               = null;
    private Color               m_pColor_Count              = Color.white;

    void Start()
    {
        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Image");
        m_pImage_Icon = ob.GetComponent<Image>();

        ob = Helper.FindChildGameObject(gameObject, "Count");
        m_pText_Count = ob.GetComponent<Text>();
        m_pColor_Count = m_pText_Count.color;

        if (m_eMissionType != eMissionType.None && m_pSpriteAtlas != null)
        {
            ExcelData_MissionDataInfo pMissionDataInfo = ExcelDataManager.Instance.m_pExcelData_MissionData.GetMissionDataInfo(m_eMissionType);

            switch (m_eMissionType)
            {
                case eMissionType.Unit:
                    {
                        m_pImage_Icon.sprite = m_pSpriteAtlas.GetSprite(pMissionDataInfo.m_strMissionImageFileName + SavedGameDataInfo.Instance.m_eMissionUnitType.ToString());
                        m_pImage_Icon.SetNativeSize();
                        m_pImage_Icon.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1);
                    }
                    break;

                case eMissionType.Jelly:
                    {
                        m_pImage_Icon.sprite = m_pSpriteAtlas.GetSprite(pMissionDataInfo.m_strMissionImageFileName + "GUI");
                        m_pImage_Icon.SetNativeSize();
                        m_pImage_Icon.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1);
                    }
                    break;

                default:
                    {
                        m_pImage_Icon.sprite = m_pSpriteAtlas.GetSprite(pMissionDataInfo.m_strMissionImageFileName);
                        m_pImage_Icon.SetNativeSize();
                        m_pImage_Icon.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1);
                    }
                    break;
            }

            m_pText_Count.text = SavedGameDataInfo.Instance.m_MissionInfoTable[m_eMissionType].ToString();
        }

    }

    public void SetMissionData(eMissionType eMissionType, SpriteAtlas pAtlas)
	{
		m_eMissionType = eMissionType;
		m_pSpriteAtlas = pAtlas;
	}

	public void SetAlpha(float fAlpha)
	{
		if (m_pImage_Icon != null)
			m_pImage_Icon.color = new Color(1, 1, 1, fAlpha);

		if (m_pText_Count != null)
			m_pText_Count.color = new Color(m_pColor_Count.r, m_pColor_Count.g, m_pColor_Count.b, fAlpha);
	}
}
