using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SavedGameDataInfo
{
	private static SavedGameDataInfo ms_pInstance = null;
	public static SavedGameDataInfo Instance { get { return ms_pInstance; } }

    public bool                     m_IsNewUser                         = false;
    public bool                     m_IsExistPlayPrefs                  = false;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Game Data 
	public byte				        m_byVersion							= 1;
    public int                      m_nSavedGame_Ver                    = 0;
    public long                     m_nPrevScore                        = 0;
    public long				        m_nScore							= 0;
	public int				        m_nLevel							= 1;
	public byte				        m_byFailCount						= 0;
    public int                      m_nRanking                          = -1;

	public Dictionary<eMissionType,int> m_MissionInfoTable				= new Dictionary<eMissionType, int>();

    public List<int>                m_LevelScoreList                    = new List<int>();

    // Not Save Data
    public List<int>                m_LevelStarCountList                = new List<int>();

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Item Data
    public int  []                  m_nItemCounts                       = new int[(int)eItemType.Max];

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// SendData , RecvData
	private SendData		        m_pSendData							= new SendData();

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Save Data
	public MapDataInfo				m_pMapDataInfo						= null;
    public MapDataInfo              m_pMapDataInfo_ForInGameResult      = null;

    public List<MapDataMissionInfo> m_MapDataMissionList			    = new List<MapDataMissionInfo>();

    public eUnitType                m_eMissionUnitType                  = eUnitType.Empty;

    public bool                     m_IsBeginnerPackageBuy              = false;

    public short                    m_sYear                             = 0;
    public byte                     m_byMonth                           = 0;
    public byte                     m_byDay                             = 0;

    public bool                     m_IsGetFreeCoin                     = false;
    public byte                     m_byGetAdsCoinCount                 = 0;

    public bool                     m_IsGetAttendance                   = false;
    public byte                     m_byGetAttendanceDay                = 0;

    public bool                     m_IsGetFreeRoulette                 = false;
    public bool                     m_IsGetAdsRoulette                  = false;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Heart Save Data
    public short                    m_sYear_Heart                       = 0;
    public byte                     m_byMonth_Heart                     = 0;
    public byte                     m_byDay_Heart                       = 0;
    public byte                     m_byHour_Heart                      = 0;
    public byte                     m_byMinute_Heart                    = 0;
    public byte                     m_bySecond_Heart                    = 0;

    public byte                     m_byGetFreeHeartCountForAds         = 0;
    public short                    m_sYear_FreeHeartForAds             = 0;
    public byte                     m_byMonth_FreeHeartForAds           = 0;
    public byte                     m_byDay_FreeHeartForAds             = 0;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Heart Not Save Data
    public int                      m_nHeartAtGameStart                 = 0;

    public SavedGameDataInfo()
	{
		ms_pInstance = this;
        ExcelData_LevelFixedMissionDataInfo pLevelFixedMissionDataInfo = ExcelDataManager.Instance.m_pExcelData_LevelFixedMissionData.GetLevelFixedMissionDataInfo_byLevel(1);
        m_pMapDataInfo = new MapDataInfo("Level", "0001");
        m_pMapDataInfo.LoadFile();

        MapDataMissionInfo_Unit pMissionInfo = m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eMissionType.Unit] as MapDataMissionInfo_Unit;
        m_MissionInfoTable.Add(eMissionType.Unit, pMissionInfo.m_nCount);

		m_MapDataMissionList.Add(pMissionInfo);

		m_eMissionUnitType = eUnitType.Red;

#if _DEBUG
        m_nItemCounts[(int)eItemType.Coin] = 5000;
        m_nItemCounts[(int)eItemType.Heart] = 500;
#else
        m_nItemCounts[(int)eItemType.Coin]	= 0;
        m_nItemCounts[(int)eItemType.Heart] = 5;
#endif
    }

    public void OnMissionClear()
    {
        m_pMapDataInfo_ForInGameResult = m_pMapDataInfo;

        m_LevelScoreList.Add(InGameInfo.Instance.m_nScore);

        int nStarCount = 0;

        for (int j = 0; j < 3; ++j)
        {
            int nStarScore = m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[j];

            if (InGameInfo.Instance.m_nScore >= nStarScore)
            {
                ++nStarCount;
            }
        }

        m_LevelStarCountList.Add(nStarCount);

        m_nPrevScore = m_nScore;
        m_nScore += InGameInfo.Instance.m_nScore;
        m_byFailCount = 0;
        m_nLevel += 1;

        m_pMapDataInfo = new MapDataInfo("Level", m_nLevel.ToString("D4"));
        m_pMapDataInfo.LoadFile();

        ResetMapDataInfo(m_pMapDataInfo);
    }

    public void OnResetCurrLevel()
    {
        m_pMapDataInfo_ForInGameResult = null;

        m_pMapDataInfo = new MapDataInfo("Level", m_nLevel.ToString("D4"));
        m_pMapDataInfo.LoadFile();

        ResetMapDataInfo(m_pMapDataInfo);
    }

    public void OnPrevLevelPlay(int nLevel)
    {
        m_pMapDataInfo_ForInGameResult = null;

        m_pMapDataInfo = new MapDataInfo("Level", nLevel.ToString("D4"));
        m_pMapDataInfo.LoadFile();

        ResetMapDataInfo(m_pMapDataInfo);
    }

    public void OnPrevLevelChangeScore(int nLevel, int nScore)
    {
        m_pMapDataInfo_ForInGameResult = m_pMapDataInfo;

        if (m_LevelScoreList[nLevel - 1] < nScore)
        {
            m_nPrevScore = m_nScore;
            int nDifferScore = nScore - m_LevelScoreList[nLevel - 1];
            m_nScore += nDifferScore;

            m_LevelScoreList[nLevel - 1] = nScore;

            int nStarCount = 0;

            for (int j = 0; j < 3; ++j)
            {
                int nStarScore = m_pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[j];

                if (nScore >= nStarScore)
                {
                    ++nStarCount;
                }
            }

            m_LevelStarCountList[nLevel - 1] = nStarCount;
        }
    }

    public void ResetMapDataInfo(MapDataInfo pMapDataInfo)
    {
        m_pMapDataInfo = pMapDataInfo;

        m_MapDataMissionList.Clear();
        m_MissionInfoTable.Clear();

        foreach (KeyValuePair<eMissionType, MapDataMissionInfo> item in m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable)
        {
            if (item.Value.GetMissionCount() != 0)
            {
                m_MapDataMissionList.Add(item.Value);
                m_MissionInfoTable.Add(item.Key, item.Value.GetMissionCount());

                if (item.Key == eMissionType.Unit)
                {
                    List<eUnitType> UnitTypeList = new List<eUnitType>();
                    UnitTypeList.Add(eUnitType.Red);
                    UnitTypeList.Add(eUnitType.Blue);
                    UnitTypeList.Add(eUnitType.Yellow);
                    UnitTypeList.Add(eUnitType.White);

                    int nRandomValue = UnityEngine.Random.Range(0, UnitTypeList.Count);
                    m_eMissionUnitType = UnitTypeList[nRandomValue];
                }
            }
        }
    }

    public void OnMissionFail()
    {
        foreach (MapDataMissionInfo pMissionInfo in m_MapDataMissionList)
        {
            if (m_MissionInfoTable.ContainsKey(pMissionInfo.m_eMissionType) == true)
            {
                m_MissionInfoTable[pMissionInfo.m_eMissionType] = pMissionInfo.GetMissionCount();
            }
        }
    }

    public void OnMissionChange()
    {
        m_byFailCount = 0;
    }

    private void OnMissionChange_MissionTable()
    {
        m_MissionInfoTable.Clear();
    }

    public void Save(bool IsErrorMessageBoxActive = true)
	{
        ++m_nSavedGame_Ver;
#if UNITY_EDITOR
        Save_PlayerPrefs();
#elif UNITY_ANDROID
        Save_PlayerPrefs();
        if (GameDefine.ms_IsUseSavedGame == true)
        {
            //Save_Cloud(IsErrorMessageBoxActive);
            Save_Cloud(false);
        }
#endif
    }

    private void Save_PlayerPrefs()
    {
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Version, GameDefine.ms_byClient_Ver);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_SavedGame_Ver, m_nSavedGame_Ver);
        
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Score, (int)m_nScore);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Level, m_nLevel);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_FailCount, m_byFailCount);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Ranking, m_nRanking);

        for(int i = 0; i < m_LevelScoreList.Count; ++i)
        {
            PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_LevelScore + i.ToString(), m_LevelScoreList[i]);
        }

        for (int i = (int)eItemType.None + 1; i < (int)eItemType.Max; ++i)
        {
            eItemType eType = (eItemType)i;
            PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_ItemCount + eType.ToString(), m_nItemCounts[i]);
        }

        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_MissionUnit, (int)m_eMissionUnitType);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_IsBeginnerPackageBuy, m_IsBeginnerPackageBuy == true ? 1 : 0);

        DateTime dateTime = TimeServer.Instance.GetNISTDate();
        m_sYear = (short)dateTime.Year;
        m_byMonth = (byte)dateTime.Month;
        m_byDay = (byte)dateTime.Day;

        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Year, (int)m_sYear);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Month, (int)m_byMonth);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Day, (int)m_byDay);

        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_IsGetFreeCoin, m_IsGetFreeCoin == true ? 1 : 0);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_GetAdsCoinCount, (int)m_byGetAdsCoinCount);

        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_IsGetAttendance, m_IsGetAttendance == true ? 1 : 0);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_AttendanceDay, (int)m_byGetAttendanceDay);

        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_IsGetFreeRoulette, m_IsGetFreeRoulette == true ? 1 : 0);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_IsGetAdsRoulette, m_IsGetAdsRoulette == true ? 1 : 0);

        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Year_Heart, (int)m_sYear_Heart);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Month_Heart, (int)m_byMonth_Heart);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Day_Heart, (int)m_byDay_Heart);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Hour_Heart, (int)m_byHour_Heart);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Minute_Heart, (int)m_byMinute_Heart);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Second_Heart, (int)m_bySecond_Heart);

        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_GetFreeHeartCountForAds, (int) m_byGetFreeHeartCountForAds);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Year_FreeHeartForAds, (int) m_sYear_FreeHeartForAds);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Month_FreeHeartForAds, (int) m_byMonth_FreeHeartForAds);
        PlayerPrefs.SetInt(PlayerPrefsString_ForSavedGame.ms_Day_FreeHeartForAds, (int) m_byDay_FreeHeartForAds);

        AppInstance.Instance.m_pEventDelegateManager.OnGooglePlaySavedGameDone_Save();
    }

    private void Save_Cloud(bool IsErrorMessageBoxActive)
    {
        m_pSendData = new SendData();
        m_pSendData.Write(GameDefine.ms_byClient_Ver);
        m_pSendData.Write(m_nSavedGame_Ver);
        m_pSendData.Write(m_nScore);
        m_pSendData.Write(m_nLevel);
        m_pSendData.Write(m_byFailCount);
        m_pSendData.Write(m_nRanking);

        m_pSendData.Write(m_LevelScoreList.Count);
        for (int i = 0; i < m_LevelScoreList.Count; ++i)
        {
            m_pSendData.Write(m_LevelScoreList[i]);
        }

        for (int i = (int)eItemType.None + 1; i < (int)eItemType.Max; ++i)
        {
            m_pSendData.Write(m_nItemCounts[i]);
        }

        m_pSendData.Write((byte)m_eMissionUnitType);

        m_pSendData.Write(m_IsBeginnerPackageBuy);

        DateTime dateTime = TimeServer.Instance.GetNISTDate();
        m_sYear = (short)dateTime.Year;
        m_byMonth = (byte)dateTime.Month;
        m_byDay = (byte)dateTime.Day;

        m_pSendData.Write(m_sYear);
        m_pSendData.Write(m_byMonth);
        m_pSendData.Write(m_byDay);

        m_pSendData.Write(m_IsGetFreeCoin);
        m_pSendData.Write(m_byGetAdsCoinCount);

        m_pSendData.Write(m_IsGetAttendance);
        m_pSendData.Write(m_byGetAttendanceDay);

        m_pSendData.Write(m_IsGetFreeRoulette);
        m_pSendData.Write(m_IsGetAdsRoulette);

        m_pSendData.Write(m_sYear_Heart);
        m_pSendData.Write(m_byMonth_Heart);
        m_pSendData.Write(m_byDay_Heart);
        m_pSendData.Write(m_byHour_Heart);
        m_pSendData.Write(m_byMinute_Heart);
        m_pSendData.Write(m_bySecond_Heart);

        m_pSendData.Write(m_byGetFreeHeartCountForAds);
        m_pSendData.Write(m_sYear_FreeHeartForAds);
        m_pSendData.Write(m_byMonth_FreeHeartForAds);
        m_pSendData.Write(m_byDay_FreeHeartForAds);

        byte[] buffer = ((MemoryStream)m_pSendData.Stream).ToArray();
        GooglePlaySavedGame.Instance.SaveToCloud(buffer, IsErrorMessageBoxActive);

        m_pSendData.m_binWriter.Close();
    }

    public void OnDone_Save()
	{
	}

	public void Load()
	{
#if UNITY_EDITOR
        Load_PlayerPrefs();
        HeartChargeTimer.Instance.OnCalculate();
#elif UNITY_ANDROID
		Load_PlayerPrefs();
        if (GameDefine.ms_IsUseSavedGame == true)
        {
            GooglePlaySavedGame.Instance.LoadFromCloud();
        }
#endif
    }

    public void Load_PlayerPrefs()
    {
        m_byVersion = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Version, 0);

        if (m_byVersion == 0)
        {
            m_IsNewUser = true;
            m_byVersion = GameDefine.ms_byClient_Ver;

            m_pMapDataInfo = new MapDataInfo("Level", "0001");
            m_pMapDataInfo.LoadFile();

            m_MissionInfoTable.Clear();
            MapDataMissionInfo_Unit pMissionInfo = m_pMapDataInfo.m_pMapDataMission.m_MapDataMissionInfoTable[eMissionType.Unit] as MapDataMissionInfo_Unit;
            m_MissionInfoTable.Add(eMissionType.Unit, pMissionInfo.m_nCount);

            m_MapDataMissionList.Clear();
            m_MapDataMissionList.Add(pMissionInfo);

            m_eMissionUnitType = eUnitType.Red;

            DateTime ServerTime = TimeServer.Instance.GetNISTDate();
            m_sYear_Heart = (short)ServerTime.Year;
            m_byMonth_Heart = (byte)ServerTime.Month;
            m_byDay_Heart = (byte)ServerTime.Day;
            m_byHour_Heart = (byte)ServerTime.Hour;
            m_byMinute_Heart = (byte)ServerTime.Minute;
            m_bySecond_Heart = (byte)ServerTime.Second;

            m_byGetFreeHeartCountForAds = 0;
            m_sYear_FreeHeartForAds = (short)ServerTime.Year;
            m_byMonth_FreeHeartForAds = (byte)ServerTime.Month;
            m_byDay_FreeHeartForAds = (byte)ServerTime.Day;
        }
        else
        {
            m_IsExistPlayPrefs = true;
            m_nSavedGame_Ver = PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_SavedGame_Ver, 0);
            m_nScore = PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Score);
            m_nLevel = PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Level);
            m_byFailCount = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_FailCount);
            m_nRanking = PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Ranking);

            for (int i = 0; i < m_nLevel - 1; ++i)
            {
                int nScore = PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_LevelScore + i.ToString(), 0);

                MapDataInfo pMapDataInfo = new MapDataInfo("Level", (i + 1).ToString("D4"));
                pMapDataInfo.LoadFile();

                if (nScore == 0)
                {
                    nScore = pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[2];
                    m_LevelScoreList.Add(nScore);

                    m_LevelStarCountList.Add(3);
                }
                else
                {
                    m_LevelScoreList.Add(nScore);

                    int nStarCount = 0;

                    for (int j = 0; j < 3; ++j)
                    {
                        int nStarScore = pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[j];

                        if (nScore >= nStarScore)
                        {
                            ++nStarCount;
                        }
                    }

                    m_LevelStarCountList.Add(nStarCount);
                }
            }

            for (int i = (int)eItemType.None + 1; i < (int)eItemType.Max; ++i)
            {
                eItemType eType = (eItemType)i;
                m_nItemCounts[i] = PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_ItemCount + eType.ToString());
            }
            
            m_pMapDataInfo = new MapDataInfo("Level", m_nLevel.ToString("D4"));
            m_pMapDataInfo.LoadFile();

            ResetMapDataInfo(m_pMapDataInfo);

            m_eMissionUnitType = (eUnitType)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_MissionUnit);
            m_IsBeginnerPackageBuy = PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_IsBeginnerPackageBuy) == 1 ? true : false;

            m_sYear = (short)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Year);
            m_byMonth = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Month);
            m_byDay = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Day);

            DateTime saveDataTime = new DateTime((int)m_sYear, (int)m_byMonth, (int)m_byDay);

            m_IsGetFreeCoin = PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_IsGetFreeCoin) == 1 ? true : false;
            m_byGetAdsCoinCount = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_GetAdsCoinCount);

            m_IsGetAttendance = PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_IsGetAttendance) == 1 ? true : false;
            m_byGetAttendanceDay = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_AttendanceDay);

            m_IsGetFreeRoulette = PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_IsGetFreeRoulette) == 1 ? true : false;
            m_IsGetAdsRoulette = PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_IsGetAdsRoulette) == 1 ? true : false;

            DateTime dateTime = TimeServer.Instance.GetNISTDate();
            if (saveDataTime < dateTime && dateTime.Year != (int)m_sYear || dateTime.Month != (int)m_byMonth || dateTime.Day != (int)m_byDay)
            {
                m_sYear = (short)dateTime.Year;
                m_byMonth = (byte)dateTime.Month;
                m_byDay = (byte)dateTime.Day;

                m_IsGetFreeCoin = false;
                m_byGetAdsCoinCount = 0;

                if (m_IsGetAttendance == false)
                {
                    m_byGetAttendanceDay = 0;
                }
                else
                {
                    int nDifferDay = (dateTime - saveDataTime).Days;
                    if (nDifferDay >= 2)
                    {
                        m_byGetAttendanceDay = 0;
                    }
                    else if (nDifferDay == 1)
                    {
                        if (m_byGetAttendanceDay == 7)
                        {
                            m_byGetAttendanceDay = 0;
                        }
                    }
                }
                m_IsGetAttendance = false;

                m_IsGetFreeRoulette = false;
                m_IsGetAdsRoulette = false;
            }

            TimeServer.Instance.OnNextDayCalculation(saveDataTime < dateTime);

            m_sYear_Heart = (short)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Year_Heart);
            m_byMonth_Heart = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Month_Heart);
            m_byDay_Heart = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Day_Heart);
            m_byHour_Heart = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Hour_Heart);
            m_byMinute_Heart = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Minute_Heart);
            m_bySecond_Heart = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Second_Heart);

            m_byGetFreeHeartCountForAds = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_GetFreeHeartCountForAds);
            m_sYear_FreeHeartForAds = (short)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Year_FreeHeartForAds);
            m_byMonth_FreeHeartForAds = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Month_FreeHeartForAds);
            m_byDay_FreeHeartForAds = (byte)PlayerPrefs.GetInt(PlayerPrefsString_ForSavedGame.ms_Day_FreeHeartForAds);

            if (dateTime.Year != (int)m_sYear_FreeHeartForAds || dateTime.Month != (int)m_byMonth_FreeHeartForAds || dateTime.Day != (int)m_byDay_FreeHeartForAds)
            {
                m_sYear_FreeHeartForAds = (short)dateTime.Year;
                m_byMonth_FreeHeartForAds = (byte)dateTime.Month;
                m_byDay_FreeHeartForAds = (byte)dateTime.Day;

                m_byGetFreeHeartCountForAds = 0;
            }
        }
    }

    public void OnDone_Load(byte[] bytes)
	{
        try
        {
            string strBytes = Helper.ConvertByteToString(bytes);

            if (bytes != null && strBytes != "")
            {
                BinaryWriter binWriter = new BinaryWriter(new MemoryStream());
                binWriter.Write(bytes);

                BinaryReader binReader = new BinaryReader(binWriter.BaseStream);
                binReader.BaseStream.Seek(0, SeekOrigin.Begin);
                RecvData pRecvData = new RecvData(binReader);

                m_byVersion = pRecvData.ReadByte();

                if (m_byVersion != 0 && m_byVersion <= GameDefine.ms_byClient_Ver)
                {
                    m_IsNewUser = false;

                    if (m_byVersion >= 2)
                    {
                        int nSavedGame_Ver = pRecvData.ReadInt32();
                        if (nSavedGame_Ver <= m_nSavedGame_Ver)
                            return;

                        m_nSavedGame_Ver = nSavedGame_Ver;
                    }

                    m_nScore = pRecvData.ReadInt64();
                    m_nLevel = pRecvData.ReadInt32();
                    m_byFailCount = pRecvData.ReadByte();
                    m_nRanking = pRecvData.ReadInt32();

                    m_LevelScoreList.Clear();
                    m_LevelStarCountList.Clear();

                    if (m_byVersion < 3)
                    {
                        for (int i = 0; i < m_nLevel - 1; ++i)
                        {
                            MapDataInfo pMapDataInfo = new MapDataInfo("Level", (i + 1).ToString("D4"));
                            pMapDataInfo.LoadFile();
                            int nScore = pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[2];
                            m_LevelScoreList.Add(nScore);

                            m_LevelStarCountList.Add(3);
                        }
                    }
                    else
                    {
                        int nCount = pRecvData.ReadInt32();
                        for (int i = 0; i < nCount; ++i)
                        {
                            MapDataInfo pMapDataInfo = new MapDataInfo("Level", (i + 1).ToString("D4"));
                            pMapDataInfo.LoadFile();

                            int nScore = pRecvData.ReadInt32();
                            m_LevelScoreList.Add(nScore);

                            int nStarCount = 0;

                            for (int j = 0; j < 3; ++j)
                            {
                                int nStarScore = pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[j];

                                if (nScore >= nStarScore)
                                {
                                    ++nStarCount;
                                }
                            }

                            m_LevelStarCountList.Add(nStarCount);
                        }

                        if (nCount < m_nLevel - 1)
                        {
                            for (int i = nCount; i < m_nLevel - 1; ++i)
                            {
                                MapDataInfo pMapDataInfo = new MapDataInfo("Level", (i + 1).ToString("D4"));
                                pMapDataInfo.LoadFile();
                                int nScore = pMapDataInfo.m_pMapDataMission.m_pMapDataMissionBaseInfo.m_nStarScore[2];
                                m_LevelScoreList.Add(nScore);

                                m_LevelStarCountList.Add(3);
                            }
                        }
                    }

                    for (int i = (int)eItemType.None + 1; i < (int)eItemType.Max; ++i)
                    {
                        m_nItemCounts[i] = pRecvData.ReadInt32();
                    }

                    m_pMapDataInfo = new MapDataInfo("Level", m_nLevel.ToString("D4"));
                    m_pMapDataInfo.LoadFile();

                    ResetMapDataInfo(m_pMapDataInfo);

                    m_eMissionUnitType = (eUnitType)pRecvData.ReadByte();
                    m_IsBeginnerPackageBuy = pRecvData.ReadBoolean();

                    m_sYear = pRecvData.ReadInt16();
                    m_byMonth = pRecvData.ReadByte();
                    m_byDay = pRecvData.ReadByte();

                    DateTime saveDataTime = new DateTime((int)m_sYear, (int)m_byMonth, (int)m_byDay);

                    m_IsGetFreeCoin = pRecvData.ReadBoolean();
                    m_byGetAdsCoinCount = pRecvData.ReadByte();

                    m_IsGetAttendance = pRecvData.ReadBoolean();
                    m_byGetAttendanceDay = pRecvData.ReadByte();

                    m_IsGetFreeRoulette = pRecvData.ReadBoolean();
                    m_IsGetAdsRoulette = pRecvData.ReadBoolean();

                    DateTime dateTime = TimeServer.Instance.GetNISTDate();
                    if (saveDataTime < dateTime && dateTime.Year != (int)m_sYear || dateTime.Month != (int)m_byMonth || dateTime.Day != (int)m_byDay)
                    {
                        m_sYear = (short)dateTime.Year;
                        m_byMonth = (byte)dateTime.Month;
                        m_byDay = (byte)dateTime.Day;

                        m_IsGetFreeCoin = false;
                        m_byGetAdsCoinCount = 0;

                        if (m_IsGetAttendance == false)
                        {
                            m_byGetAttendanceDay = 0;
                        }
                        else
                        {
                            int nDifferDay = (dateTime - saveDataTime).Days;
                            if (nDifferDay >= 2)
                            {
                                m_byGetAttendanceDay = 0;
                            }
                            else if (nDifferDay == 1)
                            {
                                if (m_byGetAttendanceDay == 7)
                                {
                                    m_byGetAttendanceDay = 0;
                                }
                            }
                        }

                        m_IsGetAttendance = false;

                        m_IsGetFreeRoulette = false;
                        m_IsGetAdsRoulette = false;
                    }

                    TimeServer.Instance.OnNextDayCalculation(saveDataTime < dateTime);

                    m_sYear_Heart = pRecvData.ReadInt16();
                    m_byMonth_Heart = pRecvData.ReadByte();
                    m_byDay_Heart = pRecvData.ReadByte();
                    m_byHour_Heart = pRecvData.ReadByte();
                    m_byMinute_Heart = pRecvData.ReadByte();
                    m_bySecond_Heart = pRecvData.ReadByte();

                    m_byGetFreeHeartCountForAds = pRecvData.ReadByte();
                    m_sYear_FreeHeartForAds = pRecvData.ReadInt16();
                    m_byMonth_FreeHeartForAds = pRecvData.ReadByte();
                    m_byDay_FreeHeartForAds = pRecvData.ReadByte();

                    if (dateTime.Year != (int)m_sYear_FreeHeartForAds || dateTime.Month != (int)m_byMonth_FreeHeartForAds || dateTime.Day != (int)m_byDay_FreeHeartForAds)
                    {
                        m_sYear_FreeHeartForAds = (short)dateTime.Year;
                        m_byMonth_FreeHeartForAds = (byte)dateTime.Month;
                        m_byDay_FreeHeartForAds = (byte)dateTime.Day;

                        m_byGetFreeHeartCountForAds = 0;
                    }

                    binWriter.Close();
                    binReader.Close();

                    m_byVersion = GameDefine.ms_byClient_Ver;
                }
            }
            else if(m_IsExistPlayPrefs == false)
            {
                DateTime ServerTime = TimeServer.Instance.GetNISTDate();

                m_sYear = (short)ServerTime.Year;
                m_byMonth = (byte)ServerTime.Month;
                m_byDay = (byte)ServerTime.Day;

                m_IsGetFreeCoin = false;
                m_byGetAdsCoinCount = 0;

                m_IsGetAttendance = false;
                m_byGetAttendanceDay = 0;

                m_IsGetFreeRoulette = false;
                m_IsGetAdsRoulette = false;

                m_sYear_Heart = (short)ServerTime.Year;
                m_byMonth_Heart = (byte)ServerTime.Month;
                m_byDay_Heart = (byte)ServerTime.Day;
                m_byHour_Heart = (byte)ServerTime.Hour;
                m_byMinute_Heart = (byte)ServerTime.Minute;
                m_bySecond_Heart = (byte)ServerTime.Second;

                m_sYear_FreeHeartForAds = (short)ServerTime.Year;
                m_byMonth_FreeHeartForAds = (byte)ServerTime.Month;
                m_byDay_FreeHeartForAds = (byte)ServerTime.Day;

                m_byGetFreeHeartCountForAds = 0;
            }
        }
        catch (Exception e)
        {
            if (m_IsExistPlayPrefs == false)
            {
                DateTime ServerTime = TimeServer.Instance.GetNISTDate();

                m_sYear = (short)ServerTime.Year;
                m_byMonth = (byte)ServerTime.Month;
                m_byDay = (byte)ServerTime.Day;

                m_IsGetFreeCoin = false;
                m_byGetAdsCoinCount = 0;

                m_IsGetAttendance = false;
                m_byGetAttendanceDay = 0;

                m_IsGetFreeRoulette = false;
                m_IsGetAdsRoulette = false;

                m_sYear_Heart = (short)ServerTime.Year;
                m_byMonth_Heart = (byte)ServerTime.Month;
                m_byDay_Heart = (byte)ServerTime.Day;
                m_byHour_Heart = (byte)ServerTime.Hour;
                m_byMinute_Heart = (byte)ServerTime.Minute;
                m_bySecond_Heart = (byte)ServerTime.Second;

                m_sYear_FreeHeartForAds = (short)ServerTime.Year;
                m_byMonth_FreeHeartForAds = (byte)ServerTime.Month;
                m_byDay_FreeHeartForAds = (byte)ServerTime.Day;

                m_byGetFreeHeartCountForAds = 0;
            }
        }
    }
}
