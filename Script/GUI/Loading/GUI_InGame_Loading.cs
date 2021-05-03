using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_InGame_Loading : MonoBehaviour
{
    private Image               m_pImage_Background     = null;
    private Image               m_pImage                = null;
    private Image               m_pImage_Loading_Text   = null;
    private Image               m_pImage_Tooltip_Back   = null;
    private Text                m_pText_Tooltip         = null;
    private Outline             m_pOutline_Tooltip      = null;

    private Transformer_Scalar  m_pAlpha                = new Transformer_Scalar(0);

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        ExcelData_LoadingDataInfo pLoadingDataInfo = ExcelDataManager.Instance.m_pExcelData_LoadingData.GetLoadingDataInfo(SavedGameDataInfo.Instance.m_nLevel);

        GameObject ob;
        Texture2D pTex;

        if (pLoadingDataInfo != null)
        {
            ob = Helper.FindChildGameObject(gameObject, "Image_Background");
            m_pImage_Background = ob.GetComponent<Image>();

            pTex = Resources.Load<Texture2D>("Gui/Loading/" + pLoadingDataInfo.m_strBackGround_Image);
            m_pImage_Background.sprite = Sprite.Create(pTex, new Rect(0, 0, pTex.width, pTex.height), new Vector2(0.5f, 0.5f), 100.0f);

            ob = Helper.FindChildGameObject(gameObject, "Image");
            m_pImage = ob.GetComponent<Image>();

            pTex = Resources.Load<Texture2D>("Gui/Loading/" + pLoadingDataInfo.m_strImage);
            m_pImage.sprite = Sprite.Create(pTex, new Rect(0, 0, pTex.width, pTex.height), new Vector2(0.5f, 0.5f), 100.0f);
            m_pImage.SetNativeSize();

            ob = Helper.FindChildGameObject(gameObject, "Image_LoadingText");
            m_pImage_Loading_Text = ob.GetComponent<Image>();

            ob = Helper.FindChildGameObject(gameObject, "Image_Tooltip_Back");
            m_pImage_Tooltip_Back = ob.GetComponent<Image>();
            ob = Helper.FindChildGameObject(ob, "Text");
            m_pText_Tooltip = ob.GetComponent<Text>();
            m_pOutline_Tooltip = ob.GetComponent<Outline>();

            if (pLoadingDataInfo.m_nTooltipTextTableID == 0)
            {
                m_pImage_Tooltip_Back.gameObject.SetActive(false);
            }
            else
            {
                m_pImage_Loading_Text.gameObject.SetActive(false);

                string strDesc = Helper.GetTextDataString(pLoadingDataInfo.m_nTooltipTextTableID);
                string strReplace = strDesc.Replace("\\n", "\n");
                m_pText_Tooltip.text = strReplace;
            }
        }

        TransformerEvent_Scalar eventValue = null;
        eventValue = new TransformerEvent_Scalar(0,0);
        m_pAlpha.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.2f, 1);
        m_pAlpha.AddEvent(eventValue);
        m_pAlpha.SetCallback(null, OnDone_FadeIn);
        m_pAlpha.OnPlay();

        Update();

        AppInstance.Instance.m_pEventDelegateManager.OnEventDeleteInGameLoading += OnDeleteInGameLoading;
    }

    private void OnDestroy()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventDeleteInGameLoading -= OnDeleteInGameLoading;
    }

    void Update()
    {
        m_pAlpha.Update(Time.deltaTime);
        float fAlpha = m_pAlpha.GetCurScalar();

        m_pImage_Background.color = new Color(1, 1, 1, fAlpha);
        m_pImage.color = new Color(1, 1, 1, fAlpha);
        m_pImage_Loading_Text.color = new Color(1, 1, 1, fAlpha);
        m_pImage_Tooltip_Back.color = new Color(1, 1, 1, fAlpha);
        m_pText_Tooltip.color = new Color(1, 1, 1, fAlpha);
        m_pOutline_Tooltip.effectColor = new Color(0, 0, 0, fAlpha);
    }

    public void OnDeleteInGameLoading()
    {
        m_pAlpha.OnReset();

        TransformerEvent_Scalar eventValue = null;
        eventValue = new TransformerEvent_Scalar(0, 1);
        m_pAlpha.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(1.5f, 1);
        m_pAlpha.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(1.9f, 0);
        m_pAlpha.AddEvent(eventValue);
        m_pAlpha.SetCallback(null, OnDone_FadeOut);
        m_pAlpha.OnPlay();
    }

    private void OnDone_FadeIn(TransformerEvent eventValue)
    {
        if (GameInfo.Instance.m_IsInGameEnter == true)
        {
            AppInstance.Instance.m_pSceneManager.ChangeScene(eSceneType.Scene_InGame, false);
        }
        else
        {
            AppInstance.Instance.m_pSceneManager.ChangeScene(eSceneType.Scene_Lobby, false);
        }
    }

    private void OnDone_FadeOut(TransformerEvent eventValue)
    {
        AppInstance.Instance.m_pEventDelegateManager.OnDestroyInGameLoading();
        Destroy(gameObject);
    }
}
