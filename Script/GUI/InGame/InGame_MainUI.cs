using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class InGame_MainUI : MonoBehaviour
{
    private SpriteAtlas             m_pSpriteAtlas_MIssion              = null;
    private SpriteAtlas             m_pSpriteAtlas_MissionNumber        = null;
    private SpriteAtlas             m_pSpriteAtlas_MissionCan           = null;
    private SpriteAtlas             m_pSpriteAtlas_MissionButterfly     = null;

    private Text                    m_pText_Level                       = null;
    private Text                    m_pText_Score                       = null;
    private Transformer_Scalar      m_pScalar_Score                     = new Transformer_Scalar(0);
    private int                     m_nCurrScore                        = 0;

    private int                     m_n110PercentMaxScore               = 0;
    private bool []                 m_IsFillStar                        = new bool[3];
    private GameObject []           m_pGameObject_Star                  = new GameObject[3];
    private Image                   m_pImage_ScoreGage                  = null;

    private GameObject              m_pGameObject_MissionDummy          = null;

    private Text                    m_pTextMove                         = null;
    private Color                   m_clrOriginTextMove                 = Color.white;
    private ParticleSystem          m_pParticleSystem_PlsuMove          = null;
    private float                   m_fScale_ParticleSystem_PlusMove    = 1.0f;
    private Image                   m_pImage_Move_FX                    = null;
    private Transformer_Scalar      m_pScale_Move_FX                    = new Transformer_Scalar(1.3f);
    private Transformer_Scalar      m_pAlpha_Move_FX                    = new Transformer_Scalar(0);

    private int                     m_nCurrMoveCount                    = 0;
    private Transformer_Scalar      m_pScale_PlusMoveCount              = new Transformer_Scalar(1);

    private Transformer_Timer       m_pTimer_UseItem_PlusMoveCount      = new Transformer_Timer();

    private Transformer_Color       m_pColor_MoveCount                  = new Transformer_Color(Color.white);

    private Dictionary<eMissionType, InGame_UI_MissionItem> m_MissionItemTable = new Dictionary<eMissionType, InGame_UI_MissionItem>();

    void Start()
    {
        SpriteAtlas sa = Resources.Load<SpriteAtlas>("Gui/InGame_MIssion/Atlas_Main");
        m_pSpriteAtlas_MIssion = SpriteAtlas.Instantiate(sa);
        sa = Resources.Load<SpriteAtlas>("Gui/InGame_MIssion/Atlas_Number");
        m_pSpriteAtlas_MissionNumber = SpriteAtlas.Instantiate(sa);
        sa = Resources.Load<SpriteAtlas>("Gui/InGame_MIssion/Atlas_Can");
        m_pSpriteAtlas_MissionCan = SpriteAtlas.Instantiate(sa);
        sa = Resources.Load<SpriteAtlas>("Gui/InGame_MIssion/Atlas_Butterfly");
        m_pSpriteAtlas_MissionButterfly = SpriteAtlas.Instantiate(sa);
        
        GameObject ob;
        GameObject ob_02;

        // Score
        GameObject ob_ScoreInfo = Helper.FindChildGameObject(gameObject, "ScoreInfo");
        ob = Helper.FindChildGameObject(ob_ScoreInfo, "Score");
        m_pText_Score = ob.GetComponent<Text>();
        m_pText_Score.text = "0";

        // Star
        int nMaxScore = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[2];
        m_n110PercentMaxScore = (int)(nMaxScore * 1.1f);
        ob = Helper.FindChildGameObject(ob_ScoreInfo, "ScoreGage");
        ob = Helper.FindChildGameObject(ob, "Image_Gage");
        m_pImage_ScoreGage = ob.GetComponent<Image>();
        m_pImage_ScoreGage.fillAmount = 0;
        float fScoreGageSize = m_pImage_ScoreGage.preferredWidth;
        float fGageLeftPos = -fScoreGageSize * 0.5f;

        ob = Helper.FindChildGameObject(ob_ScoreInfo, "Star");
        GameObject ob_star;
        ob_star = Helper.FindChildGameObject(ob, "Star_01");
        float fStarHeight = ob_star.transform.localPosition.y;
        int nScore = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[0];
        float fRate;
        if(m_n110PercentMaxScore != 0)
            fRate = ((float)nScore) / ((float)m_n110PercentMaxScore);
        else 
            fRate = 0;
        float fX = fGageLeftPos + fScoreGageSize * fRate;
        ob_star.transform.localPosition = new Vector3(fX, fStarHeight, 1);

        m_pGameObject_Star[0] = Helper.FindChildGameObject(ob_star, "Image_Star");
        m_pGameObject_Star[0].SetActive(false);

        ob_star = Helper.FindChildGameObject(ob, "Star_02");
        nScore = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[1];
        if (m_n110PercentMaxScore != 0)
            fRate = ((float)nScore) / ((float)m_n110PercentMaxScore);
        else
            fRate = 0;
        fX = fGageLeftPos + fScoreGageSize * fRate;
        ob_star.transform.localPosition = new Vector3(fX, fStarHeight, 1);

        m_pGameObject_Star[1] = Helper.FindChildGameObject(ob_star, "Image_Star");
        m_pGameObject_Star[1].SetActive(false);

        ob_star = Helper.FindChildGameObject(ob, "Star_03");
        nScore = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[2];
        if (m_n110PercentMaxScore != 0)
            fRate = ((float)nScore) / ((float)m_n110PercentMaxScore);
        else
            fRate = 0;
        fX = fGageLeftPos + fScoreGageSize * fRate;
        ob_star.transform.localPosition = new Vector3(fX, fStarHeight, 1);

        m_pGameObject_Star[2] = Helper.FindChildGameObject(ob_star, "Image_Star");
        m_pGameObject_Star[2].SetActive(false);

        ob = Helper.FindChildGameObject(gameObject, "MissionInfo");
        m_pGameObject_MissionDummy = Helper.FindChildGameObject(ob, "MissionDummy");
        ob = Helper.FindChildGameObject(ob, "Level");
        m_pText_Level = ob.GetComponent<Text>();

        if (GameInfo.Instance.m_IsCurrLevelPlay == false && GameInfo.Instance.m_nPrevLevelPlayLevel != -1)
        {
            m_pText_Level.text = "LEVEL " + GameInfo.Instance.m_nPrevLevelPlayLevel.ToString();
        }
        else
        {
            m_pText_Level.text = "LEVEL " + SavedGameDataInfo.Instance.m_nLevel.ToString();
        }

        ob = Helper.FindChildGameObject(gameObject, "MoveInfo");

        ob_02 = Helper.FindChildGameObject(ob, "Image_FX");
        m_pImage_Move_FX = ob_02.GetComponent<Image>();
        ob = Helper.FindChildGameObject(ob, "Move");
        m_pTextMove = ob.GetComponent<Text>();
        m_pTextMove.text = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nMoveCount.ToString();
        m_clrOriginTextMove = m_pTextMove.color;

        m_nCurrMoveCount = 0;
        ob = Helper.FindChildGameObject(ob, "FX_ApplyItem_PlusMove");
        m_pParticleSystem_PlsuMove = ob.GetComponent<ParticleSystem>();

        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UpdataMissionUI += OnInGame_UpdataMissionUI;
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UpdataStarUI += OnInGame_UpdataStarUI;
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_ChangeScore += OnInGame_ChangeScore;
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UpdateMoveCount += OnInGame_UpdateMoveCount;
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UseItem_PlusMoveCount += OnInGame_UseItem_PlusMoveCount;
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UseItem_PlusMoveParticleStart += OnInGame_UseItem_PlusMoveParticleStart;
    }

    void OnDestroy()
    {
        GameObject.Destroy(m_pSpriteAtlas_MIssion);
        GameObject.Destroy(m_pSpriteAtlas_MissionNumber);
        GameObject.Destroy(m_pSpriteAtlas_MissionCan);
        GameObject.Destroy(m_pSpriteAtlas_MissionButterfly); 

        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UpdataMissionUI -= OnInGame_UpdataMissionUI;
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UpdataStarUI -= OnInGame_UpdataStarUI;
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_ChangeScore -= OnInGame_ChangeScore;
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UpdateMoveCount -= OnInGame_UpdateMoveCount;
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UseItem_PlusMoveCount -= OnInGame_UseItem_PlusMoveCount;
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_UseItem_PlusMoveParticleStart -= OnInGame_UseItem_PlusMoveParticleStart;
    }

    void Update()
    {
        m_pScalar_Score.Update(Time.deltaTime);
        m_nCurrScore = (int)m_pScalar_Score.GetCurScalar();
        
        m_pText_Score.text = string.Format("{0:n0}", m_nCurrScore);

        m_pTimer_UseItem_PlusMoveCount.Update(Time.deltaTime);

        m_pScale_PlusMoveCount.Update(Time.deltaTime);
        float fScale = m_pScale_PlusMoveCount.GetCurScalar();
        m_pTextMove.gameObject.transform.localScale = new Vector3(fScale, fScale, 1);

        m_pScale_Move_FX.Update(Time.deltaTime);
        fScale = m_pScale_Move_FX.GetCurScalar();
        m_pImage_Move_FX.gameObject.transform.localScale = new Vector3(fScale, fScale, 1);

        m_pAlpha_Move_FX.Update(Time.deltaTime);
        float fAlpha = m_pAlpha_Move_FX.GetCurScalar();
        m_pImage_Move_FX.color = new Color(1,1,1,fAlpha);
    }

    public void OnPlay()
    {
    }

    public void OnPause()
    {
    }

    public void OnStop()
    {
    }

    public SpriteAtlas GetSpriteAtlas_MIssion()
    {
        return m_pSpriteAtlas_MIssion;
    }

    public SpriteAtlas GetSpriteAtlas_MIssionNumber()
    {
        return m_pSpriteAtlas_MissionNumber;
    }

    public SpriteAtlas GetSpriteAtlas_MIssionCan()
    {
        return m_pSpriteAtlas_MissionCan;
    }

    public SpriteAtlas GetSpriteAtlas_MIssionButterfly()
    {
        return m_pSpriteAtlas_MissionButterfly;
    }

    public Vector3 GetMissionItemPos(eMissionType eMissionType)
    {
        if (m_MissionItemTable.ContainsKey(eMissionType) == true)
        {
            return m_MissionItemTable[eMissionType].GetPos();
        }

        return Vector3.zero;
    }

    public Vector3 GetMoveCountPos()
    {
        return Camera.main.ScreenToWorldPoint(m_pTextMove.gameObject.transform.position);
    }

    private void OnInGame_UpdataMissionUI()
    {
        m_MissionItemTable.Clear();

        Helper.RemoveChildAll(m_pGameObject_MissionDummy);

		int nCount = SavedGameDataInfo.Instance.m_MissionInfoTable.Count;

		int nX = 0;
		int nInterval = 0;

		if (nCount == 2)
		{
            nX = -65;
			nInterval = 130;
        }

        foreach (KeyValuePair<eMissionType, int> item in SavedGameDataInfo.Instance.m_MissionInfoTable)
        {
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/InGame_MissionItem") as GameObject;
            ob = GameObject.Instantiate(ob);

            InGame_UI_MissionItem pInGame_UI_MissionItem = ob.GetComponent<InGame_UI_MissionItem>();
            pInGame_UI_MissionItem.SetMissionData(this, item.Key, m_pSpriteAtlas_MIssion, 1);

            ob.transform.SetParent(m_pGameObject_MissionDummy.transform);
            ob.transform.localPosition = new Vector3(nX, 0, 0);
            ob.transform.localScale = new Vector3(1, 1, 1);
            nX += nInterval;

            m_MissionItemTable.Add(item.Key, pInGame_UI_MissionItem);
        }
    }

    private void OnInGame_UpdataStarUI()
    {
        GameObject ob;
        GameObject ob_02;

        int nMaxScore = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[2];
        m_n110PercentMaxScore = (int)(nMaxScore * 1.1f);
        float fScoreGageSize = m_pImage_ScoreGage.preferredWidth;
        float fGageLeftPos = -fScoreGageSize * 0.5f;

        GameObject ob_ScoreInfo = Helper.FindChildGameObject(gameObject, "ScoreInfo");

        ob = Helper.FindChildGameObject(ob_ScoreInfo, "Star");
        GameObject ob_star;
        ob_star = Helper.FindChildGameObject(ob, "Star_01");
        float fStarHeight = ob_star.transform.localPosition.y;
        int nScore = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[0];
        float fRate;
        if (m_n110PercentMaxScore != 0)
            fRate = ((float)nScore) / ((float)m_n110PercentMaxScore);
        else
            fRate = 0;
        float fX = fGageLeftPos + fScoreGageSize * fRate;
        ob_star.transform.localPosition = new Vector3(fX, fStarHeight, 1);

        ob_star = Helper.FindChildGameObject(ob, "Star_02");
        nScore = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[1];
        if (m_n110PercentMaxScore != 0)
            fRate = ((float)nScore) / ((float)m_n110PercentMaxScore);
        else
            fRate = 0;
        fX = fGageLeftPos + fScoreGageSize * fRate;
        ob_star.transform.localPosition = new Vector3(fX, fStarHeight, 1);

        ob_star = Helper.FindChildGameObject(ob, "Star_03");
        nScore = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[2];
        if (m_n110PercentMaxScore != 0)
            fRate = ((float)nScore) / ((float)m_n110PercentMaxScore);
        else
            fRate = 0;
        fX = fGageLeftPos + fScoreGageSize * fRate;
        ob_star.transform.localPosition = new Vector3(fX, fStarHeight, 1);
    }

    private void OnInGame_ChangeScore(int nScore)
    {
        m_pScalar_Score.OnResetEvent();
        m_pScalar_Score.OnResetCallback();
        m_pScalar_Score.OnStop();

        TransformerEvent_Scalar eventValue = null;
        eventValue = new TransformerEvent_Scalar(0, m_nCurrScore);
        m_pScalar_Score.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.3f, nScore);
        m_pScalar_Score.AddEvent(eventValue);

        m_pScalar_Score.OnPlay();

        for (int i = 0; i < 3; ++i)
        {
            if (m_pGameObject_Star[i].activeSelf == false)
            {
                int nStarScore = SavedGameDataInfo.Instance.m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[i];

                if (nStarScore <= nScore)
                {
                    m_pGameObject_Star[i].SetActive(true);
                }
            }
        }

        float fAmount = ((float)nScore) / ((float)m_n110PercentMaxScore);
        m_pImage_ScoreGage.fillAmount = fAmount;
    }

	private void OnInGame_UpdateMoveCount(int nMoveCount)
    {
        m_nCurrMoveCount = nMoveCount;
        m_pTextMove.text = nMoveCount.ToString();

        if (nMoveCount <= 5)
        {
            m_pTextMove.color = new Color(1, 0, 0);
        }
        else
        {
            m_pTextMove.color = m_clrOriginTextMove;
        }
    }

    public void OnInGame_UseItem_PlusMoveCount()
    {
		++m_nCurrMoveCount;
		m_pTextMove.text = m_nCurrMoveCount.ToString();

		m_pScale_PlusMoveCount.OnReset();
        TransformerEvent eventValue;
        eventValue = new TransformerEvent_Scalar(0, 1);
        m_pScale_PlusMoveCount.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.08f, 1.5f, true);
        m_pScale_PlusMoveCount.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.16f, 1);
        m_pScale_PlusMoveCount.AddEvent(eventValue);
        m_pScale_PlusMoveCount.OnPlay();

        m_pScale_Move_FX.OnReset();
        eventValue = new TransformerEvent_Scalar(0, 1.3f);
        m_pScale_Move_FX.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.08f, 1.8f);
        m_pScale_Move_FX.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.16f, 1.3f);
        m_pScale_Move_FX.AddEvent(eventValue);
        m_pScale_Move_FX.OnPlay();

        m_pAlpha_Move_FX.OnReset();
        eventValue = new TransformerEvent_Scalar(0.01f, 0);
        m_pAlpha_Move_FX.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.01f, 1);
        m_pAlpha_Move_FX.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.15f, 1);
        m_pAlpha_Move_FX.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.15f, 0);
        m_pAlpha_Move_FX.AddEvent(eventValue);
        m_pAlpha_Move_FX.OnPlay();
    }

    public void OnInGame_UseItem_PlusMoveParticleStart()
    {
        SetParticleScale(m_pParticleSystem_PlsuMove.gameObject, AppInstance.Instance.m_fMainScale);
        m_pParticleSystem_PlsuMove.Play();
    }

    public void SetParticleScale(GameObject ob, float scale)
    {
        if (ob != null)
        {
            ParticleSystem Parent = ob.GetComponent<ParticleSystem>();

            foreach (ParticleSystem system in ob.GetComponentsInChildren<ParticleSystem>())
            {
                if (system != null && system.emission.enabled == true)
                {
                    system.Clear(true);
                    ParticleSystem.MainModule main = system.main;

                    float size = system.main.startSize.constant / m_fScale_ParticleSystem_PlusMove;
                    float size_min = system.main.startSize.constantMin / m_fScale_ParticleSystem_PlusMove;
                    float size_max = system.main.startSize.constantMax / m_fScale_ParticleSystem_PlusMove;
                    ParticleSystem.MinMaxCurve startSizeCurve = system.main.startSize;
                    startSizeCurve.constant = size * scale;
                    startSizeCurve.constantMin = size_min * scale;
                    startSizeCurve.constantMax = size_max * scale;
                    main.startSize = startSizeCurve;

                    if (system.main.startSize3D == true)
                    {
                        ParticleSystem.MinMaxCurve startSizeX = system.main.startSizeX;
                        startSizeX.constant = size * scale;
                        startSizeX.constantMin = size_min * scale;
                        startSizeX.constantMax = size_max * scale;
                        main.startSizeX = startSizeX;

                        ParticleSystem.MinMaxCurve startSizeY = system.main.startSizeY;
                        startSizeY.constant = size * scale;
                        startSizeY.constantMin = size_min * scale;
                        startSizeY.constantMax = size_max * scale;
                        main.startSizeY = startSizeY;

                        ParticleSystem.MinMaxCurve startSizeZ = system.main.startSizeZ;
                        startSizeZ.constant = size * scale;
                        startSizeZ.constantMin = size_min * scale;
                        startSizeZ.constantMax = size_max * scale;
                        main.startSizeZ = startSizeZ;
                    }

                    ParticleSystem.MinMaxCurve gravityCurve = system.main.gravityModifier;
                    float fGravity = system.main.gravityModifier.constant / m_fScale_ParticleSystem_PlusMove;
                    gravityCurve.constant = fGravity * scale;

                    float speed = system.main.startSpeed.constant / m_fScale_ParticleSystem_PlusMove;
                    float speed_min = system.main.startSpeed.constantMin / m_fScale_ParticleSystem_PlusMove;
                    float speed_max = system.main.startSpeed.constantMax / m_fScale_ParticleSystem_PlusMove;
                    ParticleSystem.MinMaxCurve startSpeedCurve = system.main.startSpeed;
                    startSpeedCurve.constant = speed * scale;
                    startSpeedCurve.constantMin = speed_min * scale;
                    startSpeedCurve.constantMax = speed_max * scale;
                    main.startSpeed = startSpeedCurve;

                    ParticleSystem.MinMaxCurve sizeOverLifetime = system.sizeOverLifetime.size;
                    float sizeoverLifetime_min = sizeOverLifetime.constantMin / m_fScale_ParticleSystem_PlusMove;
                    float sizeoverLifetime_max = sizeOverLifetime.constantMax / m_fScale_ParticleSystem_PlusMove;
                    float fsizeOverLifetime = system.sizeOverLifetime.size.constant / m_fScale_ParticleSystem_PlusMove;
                    sizeOverLifetime.constant = fsizeOverLifetime * scale;
                    startSpeedCurve.constantMin = sizeoverLifetime_min * scale;
                    startSpeedCurve.constantMax = sizeoverLifetime_max * scale;

                    ParticleSystem.ShapeModule shapeModule = system.shape;
                    float fShapeRadius = shapeModule.radius / m_fScale_ParticleSystem_PlusMove;
                    shapeModule.radius = fShapeRadius * scale;

                    Vector3 vShapePosition = shapeModule.position / m_fScale_ParticleSystem_PlusMove;
                    shapeModule.position = vShapePosition * scale;

                    Vector3 vShapeScale = shapeModule.scale / m_fScale_ParticleSystem_PlusMove;
                    shapeModule.scale = vShapeScale * scale;

                    if (Parent != system)
                    {
                        Vector3 vPos = system.transform.localPosition / m_fScale_ParticleSystem_PlusMove;
                        system.transform.localPosition = vPos * scale;
                    }
                }
            }
        }

        m_fScale_ParticleSystem_PlusMove = scale;
    }
}
