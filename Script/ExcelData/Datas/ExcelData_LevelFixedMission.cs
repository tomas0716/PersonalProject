using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExcelData_LevelFixedMissionDataInfo
{
	public int				m_nLevel					= 0;
	public eMissionType		m_eMissionType				= eMissionType.None;

	public int				m_nTutorial_Title_TableID	= 0;
	public int				m_nTutorial_Desc_TableID	= 0;

	public ExcelData_LevelFixedMissionDataInfo()
	{
	}
}

public class ExcelData_LevelFixedMissionData
{
	private Dictionary<int, ExcelData_LevelFixedMissionDataInfo> m_DataInfoTable = new Dictionary<int, ExcelData_LevelFixedMissionDataInfo>();

	private Dictionary<eMissionType,int> m_FixedMissionAppearLevelTable = new Dictionary<eMissionType, int>();

	public ExcelData_LevelFixedMissionData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for( int i = 0; i < RowCount; ++i )
		{
			ExcelData_LevelFixedMissionDataInfo pInfo = new ExcelData_LevelFixedMissionDataInfo();

			pInfo.m_nLevel = Helper.ConvertStringToInt(Datas[index++]);

			string strMissionType = Datas[index++];

			for (int j = 0; j < (int)eMissionType.Max; ++j)
			{
				eMissionType eType = (eMissionType)j;

				if (eType.ToString() == strMissionType)
				{
					pInfo.m_eMissionType = eType;
					break;
				}
			}

			pInfo.m_nTutorial_Title_TableID = Helper.ConvertStringToInt(Datas[index++]);
			pInfo.m_nTutorial_Desc_TableID = Helper.ConvertStringToInt(Datas[index++]);

			m_DataInfoTable.Add (pInfo.m_nLevel, pInfo);

			if (m_FixedMissionAppearLevelTable.ContainsKey(pInfo.m_eMissionType) == false)
			{
				m_FixedMissionAppearLevelTable.Add(pInfo.m_eMissionType, pInfo.m_nLevel);
			}
		}		
	}

	public ExcelData_LevelFixedMissionDataInfo GetLevelFixedMissionDataInfo_byLevel(int nLevel)
	{
		if (m_DataInfoTable.ContainsKey(nLevel) == true)
		{
			return m_DataInfoTable[nLevel];
		}

		return null;
	}

	public int GetFixedMissionAppearLevel(eMissionType eType)
	{
		if (m_FixedMissionAppearLevelTable.ContainsKey(eType) == true)
		{
			return m_FixedMissionAppearLevelTable[eType];
		}

		return 999999;
	}
}
