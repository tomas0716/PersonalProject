using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelData_Event_AttendanceDataInfo
{
	public int					m_nDay						= 0;
	public eItemType			m_ItemType					= eItemType.None;
	public int					m_nCount					= 0;
	public int					m_nDesc						= 0;

	public ExcelData_Event_AttendanceDataInfo()
	{
	}
}

public class ExcelData_Event_AttendanceData
{
	private Dictionary<int, ExcelData_Event_AttendanceDataInfo> m_DataInfoTable = new Dictionary<int, ExcelData_Event_AttendanceDataInfo>();

	public ExcelData_Event_AttendanceData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for (int i = 0; i < RowCount; ++i)
		{
			ExcelData_Event_AttendanceDataInfo Info = new ExcelData_Event_AttendanceDataInfo();

			Info.m_nDay = Helper.ConvertStringToInt(Datas[index++]);

			string strItemType = Datas[index++];

			for (int j = 0; j < (int)eItemType.Max; ++j)
			{
				eItemType eType = (eItemType)j;

				if (eType.ToString() == strItemType)
				{
					Info.m_ItemType = eType;
					break;
				}
			}

			Info.m_nCount = Helper.ConvertStringToInt(Datas[index++]);
			Info.m_nDesc = Helper.ConvertStringToInt(Datas[index++]);

			m_DataInfoTable.Add(Info.m_nDay, Info);
		}
	}

	public ExcelData_Event_AttendanceDataInfo GetDataInfo_byDay(int nDay)
	{
		if (m_DataInfoTable.ContainsKey(nDay) == true)
		{
			return m_DataInfoTable[nDay];
		}

		return null;
	}
}
