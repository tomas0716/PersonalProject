using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelData_ItemDataInfo
{
	public int				m_nID						= 0;
	public eItemType		m_ItemType					= eItemType.None;
	public string			m_strIconFileName			= "";
	public int				m_nItemName_TextTableID		= 0;
	public int				m_nDesc_TextTableID			= 0;
	public int				m_nItemCount				= 0;
	public int				m_nNeedCoin					= 0;
	public int				m_nMoveCount				= 0;

	public ExcelData_ItemDataInfo()
	{
	}
}

public class ExcelData_ItemData
{
	private Dictionary<eItemType, ExcelData_ItemDataInfo> m_DataInfoTable = new Dictionary<eItemType, ExcelData_ItemDataInfo>();

	public ExcelData_ItemData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for (int i = 0; i < RowCount; ++i)
		{
			ExcelData_ItemDataInfo Info = new ExcelData_ItemDataInfo();
			Info.m_nID = Helper.ConvertStringToInt(Datas[index++]);
			
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

			Info.m_strIconFileName = Datas[index++];
			Info.m_nItemName_TextTableID = Helper.ConvertStringToInt(Datas[index++]);
			Info.m_nDesc_TextTableID = Helper.ConvertStringToInt(Datas[index++]);
			Info.m_nItemCount = Helper.ConvertStringToInt(Datas[index++]); 
			Info.m_nNeedCoin = Helper.ConvertStringToInt(Datas[index++]);
			Info.m_nMoveCount = Helper.ConvertStringToInt(Datas[index++]);

			m_DataInfoTable.Add(Info.m_ItemType, Info);
		}
	}

	public ExcelData_ItemDataInfo GetItemDataInfo_byItemType(eItemType eType)
	{
		if(m_DataInfoTable.ContainsKey(eType) == true)
			return m_DataInfoTable[eType];

		return null;
	}
}
