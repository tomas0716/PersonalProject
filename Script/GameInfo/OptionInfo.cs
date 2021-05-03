using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionInfo
{
    public string   m_strCountryCode            = "None";
    public bool     m_IsBGM_On                  = true;
    public bool     m_IsEffectSound_On          = true;
    public int      m_nMissionComplete_Speed    = 1;

    public OptionInfo()
    {
        m_strCountryCode = PlayerPrefs.GetString(PlayerPrefsString.ms_CountryCode, "None");
        m_IsBGM_On = PlayerPrefs.GetInt(PlayerPrefsString.ms_Option_BGM, 1) == 1 ? true : false;
        m_IsEffectSound_On = PlayerPrefs.GetInt(PlayerPrefsString.ms_Option_EffectSount, 1) == 1 ? true : false;

        m_nMissionComplete_Speed = PlayerPrefs.GetInt(PlayerPrefsString.ms_MissionComplete_Speed, 1);
    }

    public void Save()
    {
        PlayerPrefs.SetString(PlayerPrefsString.ms_CountryCode, m_strCountryCode);
        PlayerPrefs.SetInt(PlayerPrefsString.ms_Option_BGM, m_IsBGM_On == true ? 1 : 0);
        PlayerPrefs.SetInt(PlayerPrefsString.ms_Option_EffectSount, m_IsEffectSound_On == true ? 1 : 0);

        PlayerPrefs.SetInt(PlayerPrefsString.ms_MissionComplete_Speed, m_nMissionComplete_Speed);

        if (AppInstance.Instance != null && AppInstance.Instance.m_pSoundPlayer != null)
        {
            AppInstance.Instance.m_pSoundPlayer.SetMute_BGM(!m_IsBGM_On);
            AppInstance.Instance.m_pSoundPlayer.SetMute_Sound(!m_IsEffectSound_On);
        }
    }
}
