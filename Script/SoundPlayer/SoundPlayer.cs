using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundPlayer 
{
	private GameObject 		m_pGameObject           = null;
	private AudioSource [] 	m_SoundSources;

    private bool            m_IsMute_BGM            = false;
    private bool            m_IsMute_EffectSound    = false;

    private AudioSource     m_pSoundSource_Bkg      = null;
    ExcelData_SoundDataInfo m_pBKG_SoundInfo        = null;
    private bool            m_IsCreateSound         = false;

    private Dictionary<ExcelData_SoundDataInfo, AudioSource>    m_AudioSourceTable              = new Dictionary<ExcelData_SoundDataInfo, AudioSource>();

    private Dictionary<int, Transformer_Timer>                  m_ApplyEffectSoundTable         = new Dictionary<int, Transformer_Timer>();
    private List<int>                                           m_RemoveApplyEffectSoundList    = new List<int>();

    public SoundPlayer ()
	{	
		m_pGameObject = new GameObject("SoundPlayer");
		GameObject.DontDestroyOnLoad(m_pGameObject);

        if (AppInstance.Instance.m_pOptionInfo.m_IsBGM_On == false)
        {
            m_IsMute_BGM = true;
        }

        if (AppInstance.Instance.m_pOptionInfo.m_IsEffectSound_On == false)
        {
            m_IsMute_EffectSound = true;
        }
	}

    public void OnDestroy()
    {
    }

    private void UpdateRemove()
    {
        foreach (int nID in m_RemoveApplyEffectSoundList)
        {
            if (m_ApplyEffectSoundTable.ContainsKey(nID) == true)
            {
                m_ApplyEffectSoundTable.Remove(nID);
            }
        }

        m_RemoveApplyEffectSoundList.Clear();
    }

    public void Update()
    {
        if(m_pSoundSource_Bkg != null)
        {
            if(m_pSoundSource_Bkg.loop == false && m_pSoundSource_Bkg.isPlaying == false )
            {
                AppInstance.Instance.m_pEventDelegateManager.OnBKGSoundDone();
            }
        }

        UpdateRemove();

        foreach (KeyValuePair<int, Transformer_Timer> item in m_ApplyEffectSoundTable)
        {
            item.Value.Update(Time.deltaTime);
        }
    }

	public void CreateSound()
	{
        if (m_IsCreateSound == false)
        {
            m_IsCreateSound = true;

            int NumSound = ExcelDataManager.Instance.m_pExcelData_SoundData.GetNumSoundInfo();

            for (int i = 0; i < NumSound; ++i)
            {
                ExcelData_SoundDataInfo SoundInfo = ExcelDataManager.Instance.m_pExcelData_SoundData.GetSoundInfo_byIndex(i);

                if (SoundInfo != null)
                {
                    AudioSource AS = (AudioSource)m_pGameObject.AddComponent<AudioSource>();
                    AS.clip = (AudioClip)Resources.Load("Sound/" + SoundInfo.m_strFileName);
                    AS.spatialBlend = 0;
                    AS.playOnAwake = false;
                    AS.volume = 0;
                    AS.loop = false;

                    m_AudioSourceTable.Add(SoundInfo, AS);
                }
            }
		}
	}

    public void Play(ExcelData_SoundDataInfo SoundInfo, bool IsLoop = false)
    {
        if(SoundInfo != null && m_AudioSourceTable.ContainsKey(SoundInfo) == true)
        {
            AudioSource AS = m_AudioSourceTable[SoundInfo];

            switch (SoundInfo.m_eSoundKind)
            {
                case eSoundKind.eBKG:
                    {
                        m_pBKG_SoundInfo = SoundInfo;

                        if (m_pSoundSource_Bkg != null)
                        {
                            m_pSoundSource_Bkg.Stop();
                        }

                        AS.loop = IsLoop;
                        AS.rolloffMode = AudioRolloffMode.Linear;
                        AS.ignoreListenerVolume = true;
                        AS.mute = m_IsMute_BGM;
                        AS.volume = SoundInfo.m_fVolume;
                        AS.Play();
                        

                        m_pSoundSource_Bkg = AS;
                    }
                    break;

                case eSoundKind.eEffect:
                    {
                        if (m_ApplyEffectSoundTable.ContainsKey(SoundInfo.m_nID) == false)
                        {
                            AS.loop = IsLoop;
                            AS.rolloffMode = AudioRolloffMode.Linear;
                            AS.ignoreListenerVolume = true;
                            AS.mute = m_IsMute_EffectSound;
                            AS.volume = SoundInfo.m_fVolume;
                            AS.pitch = Time.timeScale;

                            if (IsLoop == false)
                                AS.PlayOneShot(AS.clip);
                            else
                                AS.Play();

                            Transformer_Timer pTimer = new Transformer_Timer(0);
                            TransformerEvent eventValue;
                            eventValue = new TransformerEvent_Timer(0.1f, SoundInfo.m_nID);
                            pTimer.AddEvent(eventValue);
                            pTimer.SetCallback(null, OnDone_ApplayEffectSound);
                            pTimer.OnPlay();

                            m_ApplyEffectSoundTable.Add(SoundInfo.m_nID, pTimer);
                        }
                    }
                    break;
            }
        }
    }

    private void OnDone_ApplayEffectSound(TransformerEvent eventValue)
    {
        int nID = (int)eventValue.m_pParameta;
        m_RemoveApplyEffectSoundList.Add(nID);
    }

    public void StopBKG()
    {
        if (m_pSoundSource_Bkg != null)
        {
            m_pSoundSource_Bkg.Stop();
        }
    }

    public void StopEffectSound(ExcelData_SoundDataInfo SoundInfo)
    {
        if (SoundInfo != null && m_AudioSourceTable.ContainsKey(SoundInfo) == true)
        {
            AudioSource AS = m_AudioSourceTable[SoundInfo];

            if (AS != null)
            {
                AS.Stop();
            }
        }
    }

    public void SetMute_BGM(bool IsMute)
    {
        if (m_IsMute_BGM != IsMute)
        {
            m_IsMute_BGM = IsMute;

            if (m_pSoundSource_Bkg != null)
            {
                m_pSoundSource_Bkg.mute = m_IsMute_BGM;
            }
        }
    }

    public void SetMute_Sound(bool IsMute)
    {
        m_IsMute_EffectSound = IsMute;
    }
}
