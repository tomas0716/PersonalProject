using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;

public class ExcelData_MissionDataInfo
{
	public eMissionType m_eMissionType					= eMissionType.None;
	public string		m_strMissionImageFileName		= "";
	public bool			m_IsNotUnitSlotBreakMission		= false;
	public int			m_nCreateSortNumber				= 0;
	public int			m_nLayerSortNumber				= 0;
}

public class ExcelData_MissionData
{
	private Dictionary<eMissionType, ExcelData_MissionDataInfo> m_DataInfoTable = new Dictionary<eMissionType, ExcelData_MissionDataInfo>();

	public ExcelData_MissionData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for (int i = 0; i < RowCount; ++i)
		{
			ExcelData_MissionDataInfo pInfo = new ExcelData_MissionDataInfo();

			string strMissionType = Datas[index++];
			for (int j = (int)eMissionType.None + 1; j < (int)eMissionType.Max; ++j)
			{
				eMissionType eType = (eMissionType)j;

				if (strMissionType == eType.ToString())
				{
					pInfo.m_eMissionType = eType;
					break;
				}
			}

			if (strMissionType == "OctopusInk")
			{
				pInfo.m_eMissionType = eMissionType.OctopusInk;
			}

			pInfo.m_strMissionImageFileName = Datas[index++];
			pInfo.m_IsNotUnitSlotBreakMission = Helper.ConvertStringToInt(Datas[index++]) == 0 ? false : true;
			pInfo.m_nCreateSortNumber = Helper.ConvertStringToInt(Datas[index++]);
			pInfo.m_nLayerSortNumber = Helper.ConvertStringToInt(Datas[index++]);

			m_DataInfoTable.Add(pInfo.m_eMissionType, pInfo);
		}
	}

	public ExcelData_MissionDataInfo GetMissionDataInfo(eMissionType eType)
	{
		if (m_DataInfoTable.ContainsKey(eType) == true)
		{
			return m_DataInfoTable[eType];
		}

		return null;
	}
}
