using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInfo
{
    private static GameInfo ms_pInstance = null;
    public static GameInfo Instance { get { return ms_pInstance; } }
    
    public bool             m_IsShopOpen                            = false;
    public bool             m_IsItemBuyOpen                         = false;
    public int              m_nMessageBoxOpenCount                  = 0;
    public int              m_nRewardPopupOpenCount                 = 0;

    public bool             m_IsInGame_UseItemTooltip               = false;
    public bool             m_IsInGame_MissionGuide                 = false;
    public bool             m_IsInGame_MissionIntro                 = false;
    public bool             m_IsInGame_Tutorial                     = false;
    public bool             m_IsInGame_Option                       = false;

    public bool             m_IsFirstEnterLobby                     = true;

    public bool             m_IsInGameEnter                         = true;

    public bool             m_IsInGameSuccess                       = false;

    public bool             m_IsHardwareBackButtonProcess           = false;

    public bool             m_IsOfflineModeGameEnter                = false;

    public bool             m_IsChangeLanguage                      = false;

    public bool             m_IsInGmaeToLobby                       = false;

    public bool             m_IsCurrLevelPlay                       = true;
    public int              m_nPrevLevelPlayLevel                   = -1;

    public bool             m_IsOpenStarMapInGameStart              = false;
    public bool             m_IsStarMapEntireTab                    = true;
    public float            m_fListView_Entire_ScrollValue          = 0.0f;
    public float            m_fListView_InComplete_ScrollValue      = 0.0f;

    public GameInfo()
    {
        ms_pInstance = this;
    }
}
