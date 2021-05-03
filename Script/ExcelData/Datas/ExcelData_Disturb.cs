using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelData_DisturbDataInfo
{
	public eDisturb		m_eDisturb		= eDisturb.None;
	public bool			m_IsExistUnit	= true;

	public ExcelData_DisturbDataInfo()
	{
	}
}

public class ExcelData_DisturbData
{
	private Dictionary<eDisturb, ExcelData_DisturbDataInfo> m_DataInfoTable = new Dictionary<eDisturb, ExcelData_DisturbDataInfo>();

	public ExcelData_DisturbData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for (int i = 0; i < RowCount; ++i)
		{
			ExcelData_DisturbDataInfo Info = new ExcelData_DisturbDataInfo();

			string strData = Datas[index++];
			for (int j = (int)eDisturb.None; j < (int)eDisturb.Max; ++j)
			{
				eDisturb eDist = (eDisturb)j;
				if (eDist.ToString() == strData)
				{
					Info.m_eDisturb = eDist;
					break;
				}
			}

			Info.m_IsExistUnit = Helper.ConvertStringToInt(Datas[index++]) == 1 ? true : false;

			m_DataInfoTable.Add(Info.m_eDisturb, Info);
		}
	}

	public ExcelData_DisturbDataInfo GetDisturbDataInfo(eDisturb eDist)
	{
		if(m_DataInfoTable.ContainsKey(eDist) == true)
			return m_DataInfoTable[eDist];

		return null;
	}
}
