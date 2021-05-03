using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelData_Event_RouletteDataInfo
{
	public DayOfWeek			m_DayOfWeek					= DayOfWeek.Monday;
	public string				m_strBoardImage				= "";

	public class RouletteItem
	{
		public int			m_nIndex		= 0;
		public eItemType	m_ItemType		= eItemType.None;
		public int			m_nCount		= 0;
		public int			m_nPercentage	= 0;

		public RouletteItem()
		{
		}
	}

	public RouletteItem []		m_RouletteItems				= new RouletteItem[GameDefine.ms_nMaxItemRoulette];

	public ExcelData_Event_RouletteDataInfo()
	{
	}
}

public class ExcelData_Event_RouletteData
{
	private Dictionary<DayOfWeek, ExcelData_Event_RouletteDataInfo> m_DataInfoTable = new Dictionary<DayOfWeek, ExcelData_Event_RouletteDataInfo>();

	public ExcelData_Event_RouletteData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for (int i = 0; i < RowCount; ++i)
		{
			ExcelData_Event_RouletteDataInfo Info = new ExcelData_Event_RouletteDataInfo();

			Info.m_DayOfWeek = (DayOfWeek)Helper.ConvertStringToInt(Datas[index++]);
			Info.m_strBoardImage = Datas[index++];

			for (int j = 0; j < GameDefine.ms_nMaxItemRoulette; ++j)
			{
				Info.m_RouletteItems[j] = new ExcelData_Event_RouletteDataInfo.RouletteItem();
				Info.m_RouletteItems[j].m_nIndex = j;

				string strItemType = Datas[index++];

				for (int k = 0; k < (int)eItemType.Max; ++k)
				{
					eItemType eType = (eItemType)k;

					if (eType.ToString() == strItemType)
					{
						Info.m_RouletteItems[j].m_ItemType = eType;
						break;
					}
				}

				Info.m_RouletteItems[j].m_nCount = Helper.ConvertStringToInt(Datas[index++]);
				Info.m_RouletteItems[j].m_nPercentage = Helper.ConvertStringToInt(Datas[index++]);
			}

			m_DataInfoTable.Add(Info.m_DayOfWeek, Info);
		}
	}

	public ExcelData_Event_RouletteDataInfo GetDataInfo_byDayOfWeek(DayOfWeek dayOfWeek)
	{
		if (m_DataInfoTable.ContainsKey(dayOfWeek) == true)
		{
			return m_DataInfoTable[dayOfWeek];
		}

		return null;
	}
}
