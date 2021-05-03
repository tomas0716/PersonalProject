using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelData_AchievementDataInfo
{
	/// Table Data
	public int						m_nID						= 0;
    public eAchievementType			m_eAchievementType			= eAchievementType.None;
	public bool						m_IsActive					= false;
	public int						m_nUnlockLevel				= 1;
	public string					m_strAchievementID			= "";
	public int						m_nGoalCount				= 0;
	public int						m_nRewardCoin				= 0;
	public eItemType				m_eRewardItemType			= eItemType.None;
	public int						m_nRewardItemCount			= 0;
	public int						m_nLinkAchievementID		= 0;

	public ExcelData_AchievementDataInfo	m_pAchievementDataInfo_PrevLink = null;
	public ExcelData_AchievementDataInfo	m_pAchievementDataInfo_NextLink = null;

	/// Google Achievement Data
	public string					m_strTitle					= "";
	public int						m_nCurrCount				= 0;
	public double					m_nPercentCompleted			= 0;
	public bool						m_IsCompleted				= false;
	public bool						m_IsHidden					= false;
	public bool						m_IsIncremental				= false;

	public ExcelData_AchievementDataInfo()
    {
    }
}

public class ExcelData_AchievementData
{
	private List<ExcelData_AchievementDataInfo>									m_DataInfoList						= new List<ExcelData_AchievementDataInfo>();
	private Dictionary<int, ExcelData_AchievementDataInfo>						m_DataInfoTable_TableID				= new Dictionary<int, ExcelData_AchievementDataInfo>();
	private Dictionary<string, ExcelData_AchievementDataInfo>					m_DataInfoTable_AchievementID		= new Dictionary<string, ExcelData_AchievementDataInfo>();
	private Dictionary<eAchievementType, List<ExcelData_AchievementDataInfo>>	m_DataInfoTable_AchievementType		= new Dictionary<eAchievementType, List<ExcelData_AchievementDataInfo>>();

	public ExcelData_AchievementData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for (int i = 0; i < RowCount; ++i)
		{
			ExcelData_AchievementDataInfo Info = new ExcelData_AchievementDataInfo();
			Info.m_nID = Helper.ConvertStringToInt(Datas[index++]);

			string strData = Datas[index++];
			for (int j = (int)eAchievementType.None; j < (int)eAchievementType.Max; ++j)
			{
				eAchievementType eType = (eAchievementType)j;
				if (eType.ToString() == strData)
				{
					Info.m_eAchievementType = eType;
					break;
				}
			}

			Info.m_IsActive = Datas[index++] == "1" ? true : false;
			Info.m_nUnlockLevel = Helper.ConvertStringToInt(Datas[index++]);
			Info.m_strAchievementID = Datas[index++];
			Info.m_nGoalCount = Helper.ConvertStringToInt(Datas[index++]);
			Info.m_nRewardCoin = Helper.ConvertStringToInt(Datas[index++]);

			strData = Datas[index++];

			for (int j = 0; j < (int)eItemType.Max; ++j)
			{
				eItemType eType = (eItemType)j;

				if (eType.ToString() == strData)
				{
					Info.m_eRewardItemType = eType;
					break;
				}
			}

			Info.m_nRewardItemCount = Helper.ConvertStringToInt(Datas[index++]);
			Info.m_nLinkAchievementID = Helper.ConvertStringToInt(Datas[index++]);

			if (Info.m_nLinkAchievementID != 0)
			{
				m_DataInfoTable_TableID[Info.m_nLinkAchievementID].m_pAchievementDataInfo_NextLink = Info;
				Info.m_pAchievementDataInfo_PrevLink = m_DataInfoTable_TableID[Info.m_nLinkAchievementID];
			}

			m_DataInfoList.Add(Info);
			m_DataInfoTable_TableID.Add(Info.m_nID, Info);
			m_DataInfoTable_AchievementID.Add(Info.m_strAchievementID, Info);

			if (m_DataInfoTable_AchievementType.ContainsKey(Info.m_eAchievementType) == true)
			{
				m_DataInfoTable_AchievementType[Info.m_eAchievementType].Add(Info);
			}
			else
			{
				List<ExcelData_AchievementDataInfo> list = new List<ExcelData_AchievementDataInfo>();
				list.Add(Info);
				m_DataInfoTable_AchievementType.Add(Info.m_eAchievementType, list);
			}
		}
	}

	public int GetNumData()
	{
		return m_DataInfoList.Count;
	}

	public ExcelData_AchievementDataInfo GetData_byIndex(int nIndex)
	{
		if(nIndex < 0 || nIndex >= m_DataInfoList.Count)
			return null;

		return m_DataInfoList[nIndex];
	}

	public ExcelData_AchievementDataInfo FindAchievementDataInfo_byAchievementID(string strID)
	{
		if (m_DataInfoTable_AchievementID.ContainsKey(strID) == true)
		{
			return m_DataInfoTable_AchievementID[strID];
		}

		return null;
	}

	public ExcelData_AchievementDataInfo FindAchievementDataInfo_byTableID(int nID)
	{
		if (m_DataInfoTable_TableID.ContainsKey(nID) == true)
		{
			return m_DataInfoTable_TableID[nID];
		}

		return null;
	}

	public List<ExcelData_AchievementDataInfo> GetAchievementDataInfoList_byAchievementType(eAchievementType eType)
	{
		if (m_DataInfoTable_AchievementType.ContainsKey(eType) == true)
		{
			return m_DataInfoTable_AchievementType[eType];
		}

		return null;
	}
}
