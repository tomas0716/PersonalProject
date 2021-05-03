using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelData_MapTableData
{
	private Dictionary<string, int> m_DataInfoTable = new Dictionary<string, int>();

	public ExcelData_MapTableData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for (int i = 0; i < RowCount; ++i)
		{
			string strFolderName = Datas[index++];
			int nMapCount = Helper.ConvertStringToInt(Datas[index++]);

			m_DataInfoTable.Add(strFolderName, nMapCount);
		}
	}

	public int GetMapCount(string strFolderName)
	{
		if (m_DataInfoTable.ContainsKey(strFolderName) == true)
		{
			return m_DataInfoTable[strFolderName];
		}

		return 0;
	}
}
