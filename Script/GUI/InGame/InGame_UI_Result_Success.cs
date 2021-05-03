using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

public class InGame_UI_Result_Success : MonoBehaviour
{
    private Image []                m_pImages                   = null;
    private Text []                 m_pTexts                    = null;
    private GameObject              m_pGameObject_Parent        = null;

    private GameObject []           m_pGameObject_Star          = new GameObject[3];

    private Text                    m_pText_TotalScore_Desc     = null;
    private Text                    m_pText_TotalScore          = null;

    private Text                    m_pText_Ranking_Desc        = null;
    private Text                    m_pText_Ranking             = null;

    private GameObject              m_pGameObject_ChangeRankingMark     = null;
    private GameObject              m_pGameObejct_ChangeRankingValue    = null;
    private bool                    m_IsChangeRankingValueShow          = false;

    private Image                   m_pImage_Button_OK          = null;
    private Text                    m_pText_Button_OK           = null;
    private Outline                 m_pOutline_Button_OK        = null;

    private Transformer_Scalar      m_pAlpha                    = new Transformer_Scalar(0);
                                                    
    private ParticleSystem []       m_pParticle                 = new ParticleSystem[5];
    private Transformer_Timer       m_pTimer_Particle           = new Transformer_Timer();

    private List<ParticleSystem>    m_ParticleList              = new List<ParticleSystem>();

    private int                     m_nIndex                    = 0;

    private Transformer_Timer       m_pTimer_Info               = new Transformer_Timer();

    private Transformer_Scalar      m_pScalar_TotalScore        = new Transformer_Scalar(0);
    private Transformer_Scalar      m_pScale_TotalScore         = new Transformer_Scalar(2);

    private Transformer_Timer       m_pTimer_Star               = new Transformer_Timer();

    private Transformer_Scalar      m_pScalar_Ranking           = new Transformer_Scalar(0);
    private Transformer_Scalar      m_pScale_Ranking            = new Transformer_Scalar(3);

    private Transformer_Scalar      m_pAlpha_ButtonOK           = new Transformer_Scalar(0);

    private ParticleSystem []       m_pParticleSystem           = new ParticleSystem[3];
    private Transformer_Scalar []   m_pScale_Star               = new Transformer_Scalar[3];
    private Transformer_Scalar[]    m_pAlpha_Star               = new Transformer_Scalar[3];

    void Start()
    {
        Helper.OnSoundPlay(eSoundType.Result, false);

        GameObject ob;
        Text pText;

        m_pGameObject_Parent = Helper.FindChildGameObject(gameObject, "Parent");
        m_pImages = m_pGameObject_Parent.GetComponentsInChildren<Image>();
        m_pTexts = m_pGameObject_Parent.GetComponentsInChildren<Text>();

        ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Text_Level");
        pText = ob.GetComponent<Text>();

        if (GameInfo.Instance.m_IsCurrLevelPlay == false && GameInfo.Instance.m_nPrevLevelPlayLevel != -1)
        {
            pText.text = "LEVEL " + GameInfo.Instance.m_nPrevLevelPlayLevel.ToString();
        }
        else
        {
            pText.text = "LEVEL " + (SavedGameDataInfo.Instance.m_nLevel - 1).ToString();
        }

        ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Star");
        m_pGameObject_Star[0] = Helper.FindChildGameObject(ob, "Image_Star_01");
        m_pGameObject_Star[0].SetActive(false);
        m_pGameObject_Star[1] = Helper.FindChildGameObject(ob, "Image_Star_02");
        m_pGameObject_Star[1].SetActive(false);
        m_pGameObject_Star[2] = Helper.FindChildGameObject(ob, "Image_Star_03");
        m_pGameObject_Star[2].SetActive(false);

        // 최종 누적 점수
        ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Text_TotalScore_Desc");
        m_pText_TotalScore_Desc = ob.GetComponent<Text>();
        m_pText_TotalScore_Desc.text = Helper.GetTextDataString(eTextDataType.InGame_Result_Success_TotalScore);

        ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Text_TotalScore");
        m_pText_TotalScore = ob.GetComponent<Text>();
        m_pText_TotalScore.text = string.Format("{0:n0}", SavedGameDataInfo.Instance.m_nPrevScore);
        m_pText_TotalScore.gameObject.SetActive(false);

        // Ranking
        ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Text_Ranking_Desc");
        m_pText_Ranking_Desc = ob.GetComponent<Text>();
        m_pText_Ranking_Desc.text = Helper.GetTextDataString(eTextDataType.InGame_Result_Success_Ranking);
        m_pText_Ranking_Desc.gameObject.SetActive(false);

        GameObject ob_Change_Ranking = Helper.FindChildGameObject(gameObject, "Change_Ranking");
        float fLength = m_pText_Ranking_Desc.preferredWidth * 0.5f;
        Vector3 vPos = m_pText_Ranking_Desc.gameObject.transform.localPosition;
        float fHeight = ob_Change_Ranking.transform.localPosition.y;
        ob_Change_Ranking.transform.localPosition = new Vector3(vPos.x + fLength + 37, fHeight, 1);

        if (GooglePlayLeaderboard.Instance.GetScoreRanking() > GooglePlayLeaderboard.Instance.GetPrevScoreRanking())
        {
            m_IsChangeRankingValueShow = true;

            ob = Helper.FindChildGameObject(ob_Change_Ranking, "Image_Up");
            ob.SetActive(false);
            m_pGameObject_ChangeRankingMark = Helper.FindChildGameObject(ob_Change_Ranking, "Image_Down");
            m_pGameObject_ChangeRankingMark.SetActive(false);

            m_pGameObejct_ChangeRankingValue = Helper.FindChildGameObject(ob_Change_Ranking, "Text_RankingChnageValue");
            pText = m_pGameObejct_ChangeRankingValue.GetComponent<Text>();
            int nCangeValue = GooglePlayLeaderboard.Instance.GetScoreRanking() - GooglePlayLeaderboard.Instance.GetPrevScoreRanking();
            pText.text = string.Format("{0:n0}", nCangeValue);
            m_pGameObejct_ChangeRankingValue.SetActive(false);
        }
        else if (GooglePlayLeaderboard.Instance.GetScoreRanking() < GooglePlayLeaderboard.Instance.GetPrevScoreRanking())
        {
            m_IsChangeRankingValueShow = true;

            ob = Helper.FindChildGameObject(ob_Change_Ranking, "Image_Down");
            ob.SetActive(false);
            m_pGameObject_ChangeRankingMark = Helper.FindChildGameObject(ob_Change_Ranking, "Image_Up");
            m_pGameObject_ChangeRankingMark.SetActive(false);

            m_pGameObejct_ChangeRankingValue = Helper.FindChildGameObject(ob_Change_Ranking, "Text_RankingChnageValue");
            pText = m_pGameObejct_ChangeRankingValue.GetComponent<Text>();
            int nCangeValue = GooglePlayLeaderboard.Instance.GetPrevScoreRanking() - GooglePlayLeaderboard.Instance.GetScoreRanking();
            pText.text = string.Format("{0:n0}", nCangeValue);
            m_pGameObejct_ChangeRankingValue.SetActive(false);
        }
        else if (GooglePlayLeaderboard.Instance.GetScoreRanking() == GooglePlayLeaderboard.Instance.GetPrevScoreRanking())
        {
            m_IsChangeRankingValueShow = false;

            ob = Helper.FindChildGameObject(ob_Change_Ranking, "Image_Down");
            ob.SetActive(false);
            ob = Helper.FindChildGameObject(ob_Change_Ranking, "Image_Up");
            ob.SetActive(false);

            m_pGameObejct_ChangeRankingValue = Helper.FindChildGameObject(ob_Change_Ranking, "Text_RankingChnageValue");
            m_pGameObejct_ChangeRankingValue.SetActive(false);
        }

        ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Text_Ranking");
        m_pText_Ranking = ob.GetComponent<Text>();
        m_pText_Ranking.text = string.Format("{0:n0}", GooglePlayLeaderboard.Instance.GetPrevScoreRanking());
        m_pText_Ranking.gameObject.SetActive(false);

        // 확인 버튼
        ob = Helper.FindChildGameObject(m_pGameObject_Parent, "Button_OK");
        m_pImage_Button_OK = ob.GetComponent<Image>();
        m_pImage_Button_OK.color = new Color(1,1,1,0);
        ob = Helper.FindChildGameObject(ob, "Text");
        m_pText_Button_OK = ob.GetComponent<Text>();
        m_pText_Button_OK.text = Helper.GetTextDataString(eTextDataType.OK);
        m_pText_Button_OK.color = new Color(1,1,1,0);
        m_pOutline_Button_OK = ob.GetComponent<Outline>();
        m_pOutline_Button_OK.effectColor = new Color(0,0,0,0);

        GameObject[] pGameObject_Msg = new GameObject[4];
        for (int i = 0; i < 4; ++i)
        {
            string strName = "Image_Message_" + i.ToString();
            ob = Helper.FindChildGameObject(m_pGameObject_Parent, strName);
            pGameObject_Msg[i] = ob;
            pGameObject_Msg[i].SetActive(false);
        }

        int nRandomValue = UnityEngine.Random.Range(0,4);
        pGameObject_Msg[nRandomValue].SetActive(true);

        TransformerEvent eventValue;
        eventValue = new TransformerEvent_Scalar(0.3f, 0);
        m_pAlpha.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.5f, 1);
        m_pAlpha.AddEvent(eventValue);
        m_pAlpha.SetCallback(null, OnDone_Alpha);
        m_pAlpha.OnPlay();

        ob = Helper.FindChildGameObject(gameObject, "FX_Confetti");
        UIParticle pUIParticle = ob.GetComponent<UIParticle>();
        pUIParticle.scale = AppInstance.Instance.m_fMainScale;

        for (int i = 0; i < 5; ++i)
        {
            ob = Helper.FindChildGameObject(gameObject, "FX_0" + (i + 1).ToString());
            ob = Helper.FindChildGameObject(ob, "FX_Result");
            m_pParticle[i] = ob.GetComponent<ParticleSystem>();
            pUIParticle = m_pParticle[i].gameObject.GetComponent<UIParticle>();
            pUIParticle.scale = AppInstance.Instance.m_fMainScale * 0.88f;
        }

        for (int i = 0; i < 3; ++i)
        {
            m_pScale_Star[i] = new Transformer_Scalar(1.5f);
            eventValue = new TransformerEvent_Scalar(0.35f, 1.0f);
            m_pScale_Star[i].AddEvent(eventValue);

            m_pAlpha_Star[i] = new Transformer_Scalar(0);
            eventValue = new TransformerEvent_Scalar(0.12f, 1.0f, i);
            m_pAlpha_Star[i].SetCallback(null, OnDone_Alpha_Star);
            m_pAlpha_Star[i].AddEvent(eventValue);

            ob = Helper.FindChildGameObject(gameObject, "FX_ResultStar_0" + (i+1).ToString());
            m_pParticleSystem[i] = ob.GetComponent<ParticleSystem>();
        }

        eventValue = new TransformerEvent_Timer(4);
        m_pTimer_Particle.AddEvent(eventValue);
        m_pTimer_Particle.SetCallback(null, OnDone_Timer_Particle);
        m_pTimer_Particle.OnPlay();

        AppInstance.Instance.m_pEventDelegateManager.OnInGame_UI_Bottom_Visible(true);

        eventValue = new TransformerEvent_Timer(0.5f);
        m_pTimer_Info.AddEvent(eventValue);
        m_pTimer_Info.SetCallback(null, OnDone_Timer_TotalScoreShow);
        m_pTimer_Info.OnPlay();  
        
        Update();

        int nLevel = SavedGameDataInfo.Instance.m_nLevel;
        if (nLevel < 30)
        {
            Helper.FirebaseLogEvent("InGame_Success_" + (nLevel - 1).ToString());
        }
        else
        {
            Helper.FirebaseLogEvent("InGame_Success", "Level", nLevel - 1);
        }
    }

    void Update()
    {
        m_pTimer_Star.Update(Time.deltaTime);

        m_pAlpha.Update(Time.deltaTime);
        float fAlpha = m_pAlpha.GetCurScalar();

        foreach (Image pImage in m_pImages)
        {
            Color color = pImage.color;
            pImage.color = new Color(color.r, color.g, color.b, fAlpha);
        }

        foreach (Text pText in m_pTexts)
        {
            Color color = pText.color;
            pText.color = new Color(color.r, color.g, color.b, fAlpha);
        }

        for (int i = m_ParticleList.Count - 1; i >= 0; --i)
        {
            ParticleSystem ps = m_ParticleList[i];
            if (ps.isPlaying == false)
            {
                GameObject.Destroy(ps.gameObject);
                m_ParticleList.RemoveAt(i);
            }
        }

        m_pTimer_Particle.Update(Time.deltaTime);

        m_pTimer_Info.Update(Time.deltaTime);

        m_pScalar_TotalScore.Update(Time.deltaTime);
        int nScore = (int)m_pScalar_TotalScore.GetCurScalar();
        m_pText_TotalScore.text = string.Format("{0:n0}", nScore);

        m_pScale_TotalScore.Update(Time.deltaTime);
        float fScale = m_pScale_TotalScore.GetCurScalar();
        m_pText_TotalScore.gameObject.transform.localScale = new Vector3(fScale, fScale, 1);

        m_pScalar_Ranking.Update(Time.deltaTime);
        int nRanking = (int)m_pScalar_Ranking.GetCurScalar();
        if (nRanking != -1)
        {
            m_pText_Ranking.text = Helper.ConvertToOrdinal(nRanking);
        }
        else
        {
            m_pText_Ranking.text = "-";
        }

        m_pScale_Ranking.Update(Time.deltaTime);
        fScale = m_pScale_Ranking.GetCurScalar();
        m_pText_Ranking.gameObject.transform.localScale = new Vector3(fScale, fScale, 1);

        m_pAlpha_ButtonOK.Update(Time.deltaTime);
        fAlpha = m_pAlpha_ButtonOK.GetCurScalar();
        m_pImage_Button_OK.color = new Color(1, 1, 1, fAlpha);
        m_pText_Button_OK.color = new Color(1, 1, 1, fAlpha);
        m_pOutline_Button_OK.effectColor = new Color(0, 0, 0, fAlpha * (80.0f / 255.0f));

        for (int i = 0; i < 3; ++i)
        {
            m_pScale_Star[i].Update(Time.deltaTime);
            fScale = m_pScale_Star[i].GetCurScalar();
            m_pAlpha_Star[i].Update(Time.deltaTime);
            fAlpha = m_pAlpha_Star[i].GetCurScalar();

            m_pGameObject_Star[i].transform.localScale = new Vector3(fScale, fScale, 1);
            Image pImage = m_pGameObject_Star[i].GetComponent<Image>();
            pImage.color = new Color(1, 1, 1, fAlpha);
        }
    }

    public void OnDone_Timer_TotalScoreShow(TransformerEvent eventValue)
    {
        m_pText_TotalScore.gameObject.SetActive(true);

        eventValue = new TransformerEvent_Scalar(0, SavedGameDataInfo.Instance.m_nPrevScore);
        m_pScalar_TotalScore.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.2f, SavedGameDataInfo.Instance.m_nPrevScore);
        m_pScalar_TotalScore.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(1.1f, SavedGameDataInfo.Instance.m_nScore);
        m_pScalar_TotalScore.AddEvent(eventValue);
        m_pScalar_TotalScore.SetCallback(null, OnDone_Scalar_TotalScore);
        m_pScalar_TotalScore.OnPlay();

        float fTime = 0;
        for (int i = 0; i < 3; ++i)
        {
            int nScore = SavedGameDataInfo.Instance.m_pMapDataInfo_ForInGameResult.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[i];

            if (InGameInfo.Instance.m_nResultScore >= nScore)
            {
                eventValue = new TransformerEvent_Timer(fTime, i);
                m_pTimer_Star.AddEvent(eventValue);

                fTime += 0.18f;
            }
        }

        eventValue = new TransformerEvent_Timer(fTime);
        m_pTimer_Star.AddEvent(eventValue);
        m_pTimer_Star.SetCallback(OnOneEventDone_Timer_Star, null);
        m_pTimer_Star.OnPlay();
    }

    public void OnDone_Scalar_TotalScore(TransformerEvent eventValue)
    {
        eventValue = new TransformerEvent_Scalar(0, 2);
        m_pScale_TotalScore.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.2f, 2);
        m_pScale_TotalScore.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.4f, 1);
        m_pScale_TotalScore.AddEvent(eventValue);
        m_pScale_TotalScore.SetCallback(null, OnDone_Scale_TotalScore);
        m_pScale_TotalScore.OnPlay();

        //float fTime = 0;
        //for (int i = 0; i < 3; ++i)
        //{
        //    int nScore = SavedGameDataInfo.Instance.m_pMapDataInfo_ForInGameResult.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[i];

        //    if (InGameInfo.Instance.m_nResultScore >= nScore)
        //    {
        //        eventValue = new TransformerEvent_Timer(fTime, i);
        //        m_pTimer_Star.AddEvent(eventValue);

        //        fTime += 0.18f;
        //    }
        //}

        //eventValue = new TransformerEvent_Timer(fTime);
        //m_pTimer_Star.AddEvent(eventValue);
        //m_pTimer_Star.SetCallback(OnOneEventDone_Timer_Star, null);
        //m_pTimer_Star.OnPlay();
    }

    public void OnOneEventDone_Timer_Star(int nIndex, TransformerEvent eventValue)
    {
        if (eventValue.m_pParameta != null)
        {
            int nStar = (int)eventValue.m_pParameta;
            m_pGameObject_Star[nStar].SetActive(true);
            m_pScale_Star[nStar].OnPlay();
            m_pAlpha_Star[nStar].OnPlay();
        }
    }

    public void OnDone_Alpha_Star(TransformerEvent eventValue)
    {
        if (eventValue.m_pParameta != null)
        {
            int nStar = (int)eventValue.m_pParameta;
            m_pParticleSystem[nStar].Play();
        }
    }

    public void OnDone_Scale_TotalScore(TransformerEvent eventValue)
    {
        m_pTimer_Info.OnReset();
        eventValue = new TransformerEvent_Timer(0.5f);
        m_pTimer_Info.AddEvent(eventValue);
        m_pTimer_Info.SetCallback(null, OnDone_Timer_RankingDelay);
        m_pTimer_Info.OnPlay();
    }

    public void OnDone_Timer_RankingDelay(TransformerEvent eventValue)
    {
        m_pText_Ranking_Desc.gameObject.SetActive(true);
        m_pText_Ranking.gameObject.SetActive(true);

        if (GooglePlayLeaderboard.Instance.GetPrevScoreRanking() != -1)
        {
            if (GooglePlayLeaderboard.Instance.GetPrevScoreRanking() != GooglePlayLeaderboard.Instance.GetScoreRanking())
            {
                eventValue = new TransformerEvent_Scalar(0, GooglePlayLeaderboard.Instance.GetPrevScoreRanking());
                m_pScalar_Ranking.AddEvent(eventValue);
                eventValue = new TransformerEvent_Scalar(0.4f, GooglePlayLeaderboard.Instance.GetPrevScoreRanking());
                m_pScalar_Ranking.AddEvent(eventValue);
                eventValue = new TransformerEvent_Scalar(0.6f, GooglePlayLeaderboard.Instance.GetScoreRanking());
                m_pScalar_Ranking.AddEvent(eventValue);
                m_pScalar_Ranking.SetCallback(null, OnDone_Scalar_Ranking);
                m_pScalar_Ranking.OnPlay();
            }
            else
            {
                eventValue = new TransformerEvent_Scalar(0, GooglePlayLeaderboard.Instance.GetPrevScoreRanking());
                m_pScalar_Ranking.AddEvent(eventValue);
                eventValue = new TransformerEvent_Scalar(0.2f, GooglePlayLeaderboard.Instance.GetScoreRanking());
                m_pScalar_Ranking.AddEvent(eventValue);
                m_pScalar_Ranking.SetCallback(null, OnDone_Scalar_Ranking);
                m_pScalar_Ranking.OnPlay();
            }
        }
        else
        {
            eventValue = new TransformerEvent_Scalar(0, GooglePlayLeaderboard.Instance.GetScoreRanking());
            m_pScalar_Ranking.AddEvent(eventValue);
            eventValue = new TransformerEvent_Scalar(0.3f, GooglePlayLeaderboard.Instance.GetScoreRanking());
            m_pScalar_Ranking.AddEvent(eventValue);
            m_pScalar_Ranking.SetCallback(null, OnDone_Scalar_Ranking);
            m_pScalar_Ranking.OnPlay();
        }
    }

    public void OnDone_Scalar_Ranking(TransformerEvent eventValue)
    {
        eventValue = new TransformerEvent_Scalar(0, 3);
        m_pScale_Ranking.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.2f, 3);
        m_pScale_Ranking.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.4f, 1);
        m_pScale_Ranking.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.6f, 1);
        m_pScale_Ranking.AddEvent(eventValue);
        m_pScale_Ranking.SetCallback(null, OnDone_Scale_Ranking);
        m_pScale_Ranking.OnPlay();
    }

    public void OnDone_Scale_Ranking(TransformerEvent eventValue)
    {
        m_pText_Ranking_Desc.gameObject.SetActive(true);

        if (GooglePlayLeaderboard.Instance.GetPrevScoreRanking() != -1)
        {
            if (m_pGameObject_ChangeRankingMark != null)
            {
                m_pGameObject_ChangeRankingMark.SetActive(true);
            }

            if (m_IsChangeRankingValueShow == true)
            {
                m_pGameObejct_ChangeRankingValue.SetActive(true);
            }
        }

        m_pTimer_Info.OnReset();
        eventValue = new TransformerEvent_Timer(0.5f);
        m_pTimer_Info.AddEvent(eventValue);
        m_pTimer_Info.SetCallback(null, OnDone_Timer_RankingShow);
        m_pTimer_Info.OnPlay();
    }

    public void OnDone_Timer_RankingShow(TransformerEvent eventValue)
    {
        eventValue = new TransformerEvent_Scalar(0, 0);
        m_pAlpha_ButtonOK.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.25f, 1);
        m_pAlpha_ButtonOK.AddEvent(eventValue);
        m_pAlpha_ButtonOK.OnPlay();
    }

    public void OnDone_Alpha(TransformerEvent eventValue)
    {
    }

    public void OnButtonClick_OK()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);
        AppInstance.Instance.m_pSoundPlayer.StopBKG();

        GameInfo.Instance.m_IsInGameSuccess = true;
        GameInfo.Instance.m_IsInGameEnter = false;
        GameInfo.Instance.m_IsInGmaeToLobby = true;
        AppInstance.Instance.m_pEventDelegateManager.OnCreateInGameLoading();
    }

    private void OnDone_Timer_Particle(TransformerEvent eventValue)
    {
        GameObject ob = GameObject.Instantiate(m_pParticle[m_nIndex].gameObject);
        ob.transform.SetParent(m_pParticle[m_nIndex].gameObject.transform.parent);
        ob.transform.localPosition = Vector3.zero;

        ParticleSystem ps = ob.GetComponent<ParticleSystem>();
        ps.Play();

        m_ParticleList.Add(ps);

        float fTime = UnityEngine.Random.Range(1.0f, 1.6f);

        m_pTimer_Particle.OnReset();
        eventValue = new TransformerEvent_Timer(fTime);
        m_pTimer_Particle.AddEvent(eventValue);
        m_pTimer_Particle.SetCallback(null, OnDone_Timer_Particle);
        m_pTimer_Particle.OnPlay();

        if(m_nIndex == 4)
            m_nIndex = 0;
        else
            ++m_nIndex;
    }
}
