using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExcelData_LoadingDataInfo
{
	public int			m_nID						= 0;
	public string		m_strBackGround_Image		= "";
	public string		m_strImage					= "";
	public int			m_nTooltipTextTableID		= 0;
	public int			m_nStartLevel				= 1;
	public int			m_nEndLevel					= 1;

	public ExcelData_LoadingDataInfo()
	{
	}
}

public class ExcelData_LoadingData
{
	private List<ExcelData_LoadingDataInfo> m_DataInfoList = new List<ExcelData_LoadingDataInfo>();

	public ExcelData_LoadingData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for( int i = 0; i < RowCount; ++i )
		{
			ExcelData_LoadingDataInfo pInfo = new ExcelData_LoadingDataInfo();

			pInfo.m_nID = Helper.ConvertStringToInt(Datas[index++]);
			pInfo.m_strBackGround_Image = Datas[index++];
			pInfo.m_strImage = Datas[index++];
			pInfo.m_nTooltipTextTableID = Helper.ConvertStringToInt(Datas[index++]);
			pInfo.m_nStartLevel = Helper.ConvertStringToInt(Datas[index++]);
			pInfo.m_nEndLevel = Helper.ConvertStringToInt(Datas[index++]);

			m_DataInfoList.Add(pInfo);
		}		
	}

	public ExcelData_LoadingDataInfo GetLoadingDataInfo(int nLevel)
	{
		List< ExcelData_LoadingDataInfo> list = new List<ExcelData_LoadingDataInfo>();

		foreach (ExcelData_LoadingDataInfo pLoadingDataInfo in m_DataInfoList)
		{
			if (pLoadingDataInfo.m_nStartLevel <= nLevel && pLoadingDataInfo.m_nEndLevel >= nLevel)
			{
				list.Add(pLoadingDataInfo);
			}
		}

		if (list.Count > 0)
		{
			int nRandom = UnityEngine.Random.Range(0, list.Count);
			return list[nRandom];
		}

		return null;
	}
}
