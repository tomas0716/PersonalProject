using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelData_SlotOverlap
{
	public Dictionary<int,List<eSlotOverlap>> m_DataInfoTable = new Dictionary<int, List<eSlotOverlap>>();

	public ExcelData_SlotOverlap(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for (int i = 0; i < RowCount * ColumnCount; ++i)
		{
			string strData = Datas[index++];

			if (strData != "0")
			{
				List<eSlotOverlap> OverlapList = new List<eSlotOverlap>();
				string [] strDatas = strData.Split(',');

				foreach (string strOverlap in strDatas)
				{
					if (strOverlap == "L")
					{
						OverlapList.Add(eSlotOverlap.Left);
					}
					else if (strOverlap == "R")
					{
						OverlapList.Add(eSlotOverlap.Right);
					}
					else if (strOverlap == "U")
					{
						OverlapList.Add(eSlotOverlap.Up);
					}
					else if (strOverlap == "D")
					{
						OverlapList.Add(eSlotOverlap.Down);
					}
				}

				m_DataInfoTable.Add(i, OverlapList);
			}
		}
	}

	public List<eSlotOverlap> GetOverlapList(int nSlotIndex)
	{
		if (m_DataInfoTable.ContainsKey(nSlotIndex) == true)
		{
			return m_DataInfoTable[nSlotIndex];
		}

		return null;
	}
}