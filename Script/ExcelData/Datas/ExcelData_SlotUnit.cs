using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelData_SlotUnitDataInfo
{
	public int				m_nID				= 0;
	public eUnitType		m_eUnitType			= eUnitType.Empty;
	public eUnitShape		m_eUnitShape		= eUnitShape.Normal;
	public bool				m_IsUseAtlas		= true;
	public string			m_strAtlasInfo_01	= "";
	public string			m_strAtlasInfo_02	= "";
	public string			m_strAtlasInfo_Die	= "";


	public ExcelData_SlotUnitDataInfo()
	{
	}
}

public class ExcelData_SlotUnitData
{
	private List<ExcelData_SlotUnitDataInfo> m_DataInfoList = new List<ExcelData_SlotUnitDataInfo>();

	public ExcelData_SlotUnitData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for (int i = 0; i < RowCount; ++i)
		{
			ExcelData_SlotUnitDataInfo Info = new ExcelData_SlotUnitDataInfo();

			string strData = "";

			Info.m_nID = Helper.ConvertStringToInt(Datas[index++]);

			strData = Datas[index++];
			for (int j = 0; j < (int)eUnitType.Max; ++j)
			{
				if (strData == ((eUnitType)j).ToString())
				{
					Info.m_eUnitType = (eUnitType)j;
					break;
				}
			}

			strData = Datas[index++];
			for (int j = 0; j < (int)eUnitShape.Max; ++j)
			{
				if (strData == ((eUnitShape)j).ToString())
				{
					Info.m_eUnitShape = (eUnitShape)j;
					break;
				}
			}

			int nUseAtlas = Helper.ConvertStringToInt(Datas[index++]);
			Info.m_IsUseAtlas = nUseAtlas == 1 ? true : false;
			Info.m_strAtlasInfo_01 = Datas[index++];
			Info.m_strAtlasInfo_02 = Datas[index++];
			Info.m_strAtlasInfo_Die = Datas[index++];

			m_DataInfoList.Add(Info);
		}
	}

	public int GetNumData()
	{
		return m_DataInfoList.Count;
	}

	public ExcelData_SlotUnitDataInfo GetSlotUnitDataInfo_byIndex(int nIndex)
	{
		if (nIndex < 0 || nIndex >= m_DataInfoList.Count)
			return null;

		return m_DataInfoList[nIndex];
	}

	public ExcelData_SlotUnitDataInfo GetSlotUnitDataInfo_byID(int nID)
	{
		for (int i = 0; i < m_DataInfoList.Count; ++i)
		{
			if (nID == m_DataInfoList[i].m_nID)
			{
				return m_DataInfoList[i];
			}
		}

		return null;
	}

	public ExcelData_SlotUnitDataInfo GetSlotUnitDataInfo(eUnitType eUnit, eUnitShape eShape)
	{
		for (int i = 0; i < m_DataInfoList.Count; ++i)
		{
			if (m_DataInfoList[i].m_eUnitType == eUnit && m_DataInfoList[i].m_eUnitShape == eShape)
			{
				return m_DataInfoList[i];
			}
		}

		return null;
	}
}
