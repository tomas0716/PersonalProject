using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI_MissionComplete : MonoBehaviour
{
    private GameObject              m_pGameObject_LevelClear            = null;
    private Transformer_Scalar      m_pScale_LevelClear                 = new Transformer_Scalar(1);

    private Image              []   m_pImage_LevelClear                 = new Image[2];
    private Transformer_Scalar []   m_pScale_MissionComplete            = new Transformer_Scalar[2];
    private Transformer_Scalar []   m_pAlpha_MissionComplete            = new Transformer_Scalar[2];

    private Image                   m_pImage_Effect                     = null;
    private Transformer_Scalar      m_pRot_Effect                       = new Transformer_Scalar(0);
    private Transformer_Scalar      m_pAlpha_Effect                     = new Transformer_Scalar(0.7f);

    private GameObject []           m_pGameObject_Skip                  = new GameObject[3];

    private Vector3 []              m_vPos_Particle                     = { new Vector3(-135, 1013, -(float)(ePlaneOrder.Fx_Common)),
                                                                            new Vector3(204, 860, -(float)(ePlaneOrder.Fx_Common)),
                                                                            new Vector3(-202, 626, -(float)(ePlaneOrder.Fx_Common)),
                                                                            new Vector3(211, 427, -(float)(ePlaneOrder.Fx_Common)),
                                                                            new Vector3(-97, 656, -(float)(ePlaneOrder.Fx_Common)) };

    private Transformer_Timer       m_pTimer_Particle                   = new Transformer_Timer();

    void Start()
    {
        m_pGameObject_LevelClear = Helper.FindChildGameObject(gameObject, "LevelClear");

        Helper.OnSoundPlay(eSoundType.Clear, false);

        GameObject ob;
        ob = Helper.FindChildGameObject(m_pGameObject_LevelClear, "Image_Level");
        m_pImage_LevelClear[0] = ob.GetComponent<Image>();

        ob = Helper.FindChildGameObject(m_pGameObject_LevelClear, "Image_Clear");
        m_pImage_LevelClear[1] = ob.GetComponent<Image>();

        ob = Helper.FindChildGameObject(m_pGameObject_LevelClear, "Image_Effect");
        m_pImage_Effect = ob.GetComponent<Image>();
        m_pImage_Effect.gameObject.SetActive(false);

        m_pGameObject_Skip[0] = Helper.FindChildGameObject(gameObject, "Button_Skip_Speed_1");
        m_pGameObject_Skip[1] = Helper.FindChildGameObject(gameObject, "Button_Skip_Speed_2");
        m_pGameObject_Skip[2] = Helper.FindChildGameObject(gameObject, "Button_Skip_Speed_3");

        UpdateSkipState();

        TransformerEvent eventValue;
        
        eventValue = new TransformerEvent_Scalar(2.0f, 1);
        m_pScale_LevelClear.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(2.2f, 0);
        m_pScale_LevelClear.AddEvent(eventValue);
        m_pScale_LevelClear.SetCallback(null, OnDone_Scale_LevelClear);
        m_pScale_LevelClear.OnPlay();

        float fDelay = 0;
        for (int i = 0; i < 2; ++i)
        {
            m_pScale_MissionComplete[i] = new Transformer_Scalar(3.0f);
            eventValue = new TransformerEvent_Scalar(fDelay, 3.0f);
            m_pScale_MissionComplete[i].AddEvent(eventValue);
            eventValue = new TransformerEvent_Scalar(fDelay + 0.3f, 0.6f);
            m_pScale_MissionComplete[i].AddEvent(eventValue);
            eventValue = new TransformerEvent_Scalar(fDelay + 0.45f, 0.8f);
            m_pScale_MissionComplete[i].AddEvent(eventValue);
            m_pScale_MissionComplete[i].SetCallback(null, OnDone_Scale_Clear);
            m_pScale_MissionComplete[i].OnPlay();

            m_pAlpha_MissionComplete[i] = new Transformer_Scalar(0);
            eventValue = new TransformerEvent_Scalar(fDelay, 0.0f);
            m_pAlpha_MissionComplete[i].AddEvent(eventValue);
            eventValue = new TransformerEvent_Scalar(fDelay + 0.31f, 1.0f);
            m_pAlpha_MissionComplete[i].AddEvent(eventValue);
            m_pAlpha_MissionComplete[i].OnPlay();

            fDelay += 0.28f;
        }

        eventValue = new TransformerEvent_Scalar(0, 0.0f);
        m_pRot_Effect.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(4, -360.0f);
        m_pRot_Effect.AddEvent(eventValue);
        m_pRot_Effect.SetLoop(true);
        m_pRot_Effect.OnPlay();

        eventValue = new TransformerEvent_Scalar(0, 0.7f);
        m_pAlpha_Effect.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.3f, 1.0f);
        m_pAlpha_Effect.AddEvent(eventValue);
        eventValue = new TransformerEvent_Scalar(0.6f, 0.7f);
        m_pAlpha_Effect.AddEvent(eventValue);
        m_pAlpha_Effect.SetLoop(true);
        m_pAlpha_Effect.OnPlay();

        fDelay = 0.5f;
        eventValue = new TransformerEvent_Timer(fDelay, 0);
        m_pTimer_Particle.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fDelay + 0.4f, 1);
        m_pTimer_Particle.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fDelay + 0.7f, 2);
        m_pTimer_Particle.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fDelay + 1.1f, 3);
        m_pTimer_Particle.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fDelay + 1.45f, 4);
        m_pTimer_Particle.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fDelay + 1.45f);
        m_pTimer_Particle.AddEvent(eventValue);
        m_pTimer_Particle.SetCallback(OnOneEventDone_Timer_Particle, null);
        m_pTimer_Particle.OnPlay();

        AppInstance.Instance.m_pEventDelegateManager.OnInGame_UI_Bottom_Visible(false);

        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_MissionCompleteDestroy += OnInGame_MissionCompleteDestroy;

        Update();
    }

	private void OnDestroy()
	{
        AppInstance.Instance.m_pEventDelegateManager.OnEventInGame_MissionCompleteDestroy -= OnInGame_MissionCompleteDestroy;
    }

	void Update()
    {
        float fScale;
        float fAlpha;

        m_pScale_LevelClear.Update(Time.deltaTime);
        fScale = m_pScale_LevelClear.GetCurScalar();
        m_pGameObject_LevelClear.transform.localScale = new Vector3(fScale, fScale, 1);

        for (int i = 0; i < 2; ++i)
        {
            m_pScale_MissionComplete[i].Update(Time.deltaTime);
            fScale = m_pScale_MissionComplete[i].GetCurScalar();
            m_pImage_LevelClear[i].gameObject.transform.localScale = new Vector3(fScale, fScale, 1);

            m_pAlpha_MissionComplete[i].Update(Time.deltaTime);
            fAlpha = m_pAlpha_MissionComplete[i].GetCurScalar();
            m_pImage_LevelClear[i].color = new Color(1, 1, 1, fAlpha);
        }

        m_pRot_Effect.Update(Time.deltaTime);
        float fRot = m_pRot_Effect.GetCurScalar();
        m_pImage_Effect.gameObject.transform.localEulerAngles = new Vector3(0,0,fRot);

        m_pAlpha_Effect.Update(Time.deltaTime);
        fAlpha = m_pAlpha_Effect.GetCurScalar();
        m_pImage_Effect.color = new Color(1, 1, 1, fAlpha);

        m_pTimer_Particle.Update(Time.deltaTime);
    }

    private void UpdateSkipState()
    {
        switch (AppInstance.Instance.m_pOptionInfo.m_nMissionComplete_Speed)
        {
            case 1:
                {
                    m_pGameObject_Skip[0].SetActive(true);
                    m_pGameObject_Skip[1].SetActive(false);
                    m_pGameObject_Skip[2].SetActive(false);
                }
                break;

            case 2:
                {
                    m_pGameObject_Skip[0].SetActive(false);
                    m_pGameObject_Skip[1].SetActive(true);
                    m_pGameObject_Skip[2].SetActive(false);
                }
                break;

            case 3:
                {
                    m_pGameObject_Skip[0].SetActive(false);
                    m_pGameObject_Skip[1].SetActive(false);
                    m_pGameObject_Skip[2].SetActive(true);
                }
                break;
        }
    }

    public void OnButtonClick_Skip_Speed_1()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        AppInstance.Instance.m_pOptionInfo.m_nMissionComplete_Speed = 2;
        AppInstance.Instance.m_pOptionInfo.Save();
        UpdateSkipState();

        Time.timeScale = 2;
    }

    public void OnButtonClick_Skip_Speed_2()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        AppInstance.Instance.m_pOptionInfo.m_nMissionComplete_Speed = 1;
        AppInstance.Instance.m_pOptionInfo.Save();
        UpdateSkipState();

        Time.timeScale = 1;
    }

    public void OnButtonClick_Skip_Speed_3()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        AppInstance.Instance.m_pOptionInfo.m_nMissionComplete_Speed = 1;
        AppInstance.Instance.m_pOptionInfo.Save();
        UpdateSkipState();

        Time.timeScale = 1;
    }

    private void OnDone_Scale_Clear(TransformerEvent eventValue)
    {
        m_pImage_Effect.gameObject.SetActive(true);
    }

    private void OnDone_Scale_LevelClear(TransformerEvent eventValue)
    {
        InGameInfo.Instance.m_eClearStep = eClearStep.SpecialUnit;
        AppInstance.Instance.m_pEventDelegateManager.OnInGame_MissionCompleteActionDone();

        Time.timeScale = AppInstance.Instance.m_pOptionInfo.m_nMissionComplete_Speed;
    }

    public void OnInGame_MissionCompleteDestroy()
    {
        GameObject.Destroy(gameObject);
    }

    public void OnScreenShot(Texture2D pTex)
    {
    }

    private void OnOneEventDone_Timer_Particle(int nIndex, TransformerEvent eventValue)
    {
        if (eventValue.m_pParameta != null)
        {
            nIndex = (int)eventValue.m_pParameta;
            Vector3 vPos = m_vPos_Particle[nIndex] * AppInstance.Instance.m_fMainScale;

            AppInstance.Instance.m_pParticleManager.LoadParticleSystem("FX_LevelClear", vPos).SetScale(AppInstance.Instance.m_fMainScale);
        }
    }
}
