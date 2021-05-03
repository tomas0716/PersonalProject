using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
{
    private GameObject          m_pGameObject_Parent    = null;
    private Transformer_Scalar  m_pScale                = new Transformer_Scalar(0);

    private eRewardSubject      m_eRewardSubject        = eRewardSubject.eAttendance;
    private eItemType           m_eItemType             = eItemType.None;
    private int                 m_nCount                = 0;

    private GameObject          m_pGameObject_Fx_Wheel  = null;
    private Transformer_Scalar  m_pRotation             = new Transformer_Scalar(0);

    private int                 m_nIndex                = 0;

    void Start()
    {
        ++GameInfo.Instance.m_nRewardPopupOpenCount;
        m_nIndex = GameInfo.Instance.m_nRewardPopupOpenCount;

        m_pGameObject_Parent = Helper.FindChildGameObject(gameObject, "Parent");
        m_pGameObject_Parent.transform.localScale = new Vector3(0, 0, 1);

        ExcelData_ItemDataInfo pItemDataInfo = ExcelDataManager.Instance.m_pExcelData_ItemData.GetItemDataInfo_byItemType(m_eItemType);

        m_pGameObject_Fx_Wheel = Helper.FindChildGameObject(gameObject, "Image_Fx_Wheel");

        GameObject ob;
        Text pText;
        Image pImage;

        ob = Helper.FindChildGameObject(gameObject, "Text_Title");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.RewardPopup_Title);

        ob = Helper.FindChildGameObject(gameObject, "Button_Get");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.OK);

        ob = Helper.FindChildGameObject(gameObject, "Image_Icon");
        pImage = ob.GetComponent<Image>();

        Texture2D pTex = Resources.Load<Texture2D>("Gui/ItemIcon/" + pItemDataInfo.m_strIconFileName);
        pImage.sprite = Sprite.Create(pTex, new Rect(0, 0, pTex.width, pTex.height), new Vector2(0.5f, 0.5f), 100.0f);

        switch (m_eRewardSubject)
        {
            case eRewardSubject.eArchievement:
                {
                    ob = Helper.FindChildGameObject(gameObject, "Text_Desc");
                    pText = ob.GetComponent<Text>();
                    string strItemName = Helper.GetTextDataString(pItemDataInfo.m_nItemName_TextTableID);
                    pText.text = string.Format(Helper.GetTextDataString(eTextDataType.AchievementReward_Item), strItemName, m_nCount);
                }
                break;
            case eRewardSubject.eNewUser_Present:
                {
                    ob = Helper.FindChildGameObject(gameObject, "Text_Desc");
                    pText = ob.GetComponent<Text>();
                    string strItemName = Helper.GetTextDataString(pItemDataInfo.m_nItemName_TextTableID);
                    pText.text = string.Format(Helper.GetTextDataString(eTextDataType.NewUser_FiveLevelClear_Present), strItemName, m_nCount);
                }
                break;
            default:
                {
                    ob = Helper.FindChildGameObject(gameObject, "Text_Desc");
                    pText = ob.GetComponent<Text>();
                    pText.text = Helper.GetTextDataString(pItemDataInfo.m_nItemName_TextTableID) + " x" + m_nCount.ToString();
                }
                break;
        }

        TransformerEvent_Scalar eventValue;
        eventValue = new TransformerEvent_Scalar(0.2f, 1.0f);
        m_pScale.AddEvent(eventValue);
        m_pScale.OnPlay();

        eventValue = new TransformerEvent_Scalar(2.5f, -360);
        m_pRotation.AddEvent(eventValue);
        m_pRotation.SetLoop(true);
        m_pRotation.OnPlay();

        Helper.OnSoundPlay(eSoundType.Reward, false);

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

    private void OnDestroy()
    {
        --GameInfo.Instance.m_nRewardPopupOpenCount;

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
    }

    void Update()
    {
        m_pScale.Update(Time.deltaTime);
        float fScale = m_pScale.GetCurScalar();
        m_pGameObject_Parent.transform.localScale = new Vector3(fScale, fScale, 1);

        m_pRotation.Update(Time.deltaTime);
        float fRot = m_pRotation.GetCurScalar();
        m_pGameObject_Fx_Wheel.transform.localEulerAngles = new Vector3(0,0,fRot);
    }

    public void SetRewardInfo(eRewardSubject eRewardSubject, eItemType eItemType, int nCount)
    {
        m_eRewardSubject = eRewardSubject;
        m_eItemType = eItemType;
        m_nCount = nCount;
    }

    public void OnButtonClick_Receive()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        AppInstance.Instance.m_pEventDelegateManager.OnRewardPopupClosed();
        GameObject.Destroy(gameObject);
    }

    public void OnHardwareBackButtonClick()
    {
        if (GameInfo.Instance.m_IsHardwareBackButtonProcess == false && 
            m_nIndex == GameInfo.Instance.m_nRewardPopupOpenCount && 
            GameInfo.Instance.m_nMessageBoxOpenCount == 0)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = true;

            OnButtonClick_Receive();
        }
    }
}
