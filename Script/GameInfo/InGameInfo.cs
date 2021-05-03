using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameInfo
{
    private static InGameInfo ms_pInstance = null;
    public static InGameInfo Instance { get { return ms_pInstance; } }

    public int          m_nCurrMoveCount                = 0;

    public bool         m_IsChangeSlot                  = false;
    public bool         m_IsCheckSlotMove               = false;
    public int          m_nCombo                        = 0;
    public int          m_nScore                        = 0;
    public int          m_nResultScore                  = 0;

    public eGameState   m_eGameState                    = eGameState.None;
    public eClearStep   m_eClearStep                    = eClearStep.None;

    public bool         m_IsInGameIntroing              = false;

    public bool         m_IsInGameClick                 = false;
    public bool         m_IsMissionClear                = false;

    public int          m_nClickSlotFingerID            = -1;
    public SlotUnit     m_pClickSlotUnit                = null;

    public float        m_fSlotSize                     = 72;
    public float        m_fSlotScale                    = 1;
    public Vector2      m_vSlot_LeftTopPos              = Vector2.zero;

    public int          m_nSlotStartIndex_X             = 0;
    public int          m_nSlotStartIndex_Y             = 0;
    public int          m_nSlotEndIndex_X               = 0;
    public int          m_nSlotEndIndex_Y               = 0;
    public int          m_nSlotCount_X                  = 0;
    public int          m_nSlotCount_Y                  = 0;

    public bool         m_IsDyingBlackCotton            = false;
    public bool         m_IsCreateBlackCotton           = false;

    public bool         m_IsBreakGrass                  = false;
    public bool         m_IsSpreadGrass                 = false;

    public bool         m_IsSlotCheckingRemove          = false;

    public bool         m_IsCombinationMagician_Magician = false;

    public int          m_nOctopusInkRenameCount        = 0;

    public float        m_fGameMissionDirectionTime     = 0;

    public bool         m_IsMissionNumber_Open          = false;
    public int          m_nMissionNumber_Number         = 1;

    public int          m_nGetMissionMoveItemCount      = 0;
    public int          m_nBonusPang_BubbleCount        = 0;
    public int          m_nSquareUnitProjectileCount    = 0;

    public bool         m_IsSlotLink                    = false;

    public bool         m_IsSlotDarkly                  = false;

    public int          m_nContinueCount                = 0;

    public eItemType    m_eCurrItemUse_ItemType         = eItemType.None;
    public eInGameItemUseState m_eInGameItemUseState    = eInGameItemUseState.None;

    public Slot m_pSlot_InGameItemUse_ChangeSlot_First  = null;
    public Slot m_pSlot_InGameItemUse_ChangeSlot_Second = null;

    public Slot m_pSlot_InGameItemUse_SlotAttack        = null;
    public Slot m_pSlot_InGameItemUse_StripeHV          = null;

    // Mission Info
    // Number
    public int          m_nMissionNumber_Max            = 0;

    public bool         m_IsNewGame_Item_Select_Move_3      = false;
    public bool         m_IsNewGame_Item_Select_Magician    = false;
    public bool         m_IsNewGame_Item_Select_SpecialUnit = false;

    public bool         m_IsNewGame_Item_Reward             = false;

    public int  []      m_nAchievementCounts                = new int[(int)eAchievementType.Max];

    public bool         m_IsMoveWarning                     = false;

    public int          m_nTargetRanker                     = -1;
    public string       m_nTargetRankerUserID                 = "";
    public long         m_nTargetRankerScore                = 0;

    public int          m_nInGameShuffleCount               = 0;

    public InGameInfo()
    {
        ms_pInstance = this;
    }

    public void OutGame_Item_Reset()
    {
        m_IsNewGame_Item_Select_Move_3 = false;
        m_IsNewGame_Item_Select_Magician = false;
        m_IsNewGame_Item_Select_SpecialUnit = false;
        m_IsNewGame_Item_Reward = false;

        for (int i = 0; i < (int)eAchievementType.Max; ++i)
        {
            m_nAchievementCounts[i] = 0;
        }
    }

	public void InGameStart_Reset()
	{
		m_nCurrMoveCount = 0;
		m_nCombo = 0;
		m_nScore = 0;
		m_eGameState = eGameState.None;
		m_eClearStep = eClearStep.None;

		m_IsChangeSlot = false;
		m_IsCheckSlotMove = false;

		m_IsInGameClick = false;
		m_IsMissionClear = false;
        m_IsInGameIntroing = false;

        m_nClickSlotFingerID = -1;
		m_pClickSlotUnit = null;

		m_IsDyingBlackCotton = false;
		m_IsCreateBlackCotton = false;

		m_IsBreakGrass = false;
		m_IsSpreadGrass = false;

		m_IsCombinationMagician_Magician = false;

		m_nOctopusInkRenameCount = 0;
		m_fGameMissionDirectionTime = 0;

		m_IsMissionNumber_Open = false;
		m_nMissionNumber_Number = 1;
		m_nGetMissionMoveItemCount = 0;
		m_nBonusPang_BubbleCount = 0;
		m_nSquareUnitProjectileCount = 0;
		m_IsSlotLink = false;
		m_IsSlotDarkly = false;

		m_nMissionNumber_Max = 0;

		m_nContinueCount = 0;

		m_eCurrItemUse_ItemType = eItemType.None;
		m_eInGameItemUseState = eInGameItemUseState.None;
		m_pSlot_InGameItemUse_ChangeSlot_First = null;
		m_pSlot_InGameItemUse_ChangeSlot_Second = null;

        m_IsMoveWarning = false;
        m_nTargetRanker = -1;
        m_nTargetRankerUserID = "";
        m_nTargetRankerScore = 0;

        m_nInGameShuffleCount = 0;
    }
}
