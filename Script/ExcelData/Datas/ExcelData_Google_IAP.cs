using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class ExcelData_Google_IAPDataInfo
{
	public int					m_nID						= 0;
	public string				m_strPrefabName				= "";
	public eIAPType				m_eIAPType					= eIAPType.None;
    public string               m_strProduct_ID             = "";
    public string               m_strProduct_Name           = "";
	public ProductType			m_ProductType				= ProductType.Consumable;

	public string               m_strIconFileName	        = "";
    public int                  m_nPackageName_TextTableID  = 0;

    public int					m_nCoin						= 1;
	public Dictionary<int,int>	m_IncludeItemTable			= new Dictionary<int,int>();

	public string				m_strOfflineMode_Price		= "";

	// Get IAP 
	public decimal				m_dePrice					= 0;
	public string				m_strCountryCode			= "";


	public ExcelData_Google_IAPDataInfo()
	{
	}
}

public class ExcelData_Google_IAPData
{
	private Dictionary<int, ExcelData_Google_IAPDataInfo> m_DataInfoTable = new Dictionary<int, ExcelData_Google_IAPDataInfo>();

	public ExcelData_Google_IAPData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for (int i = 0; i < RowCount; ++i)
		{
			ExcelData_Google_IAPDataInfo Info = new ExcelData_Google_IAPDataInfo();

			Info.m_nID = Helper.ConvertStringToInt(Datas[index++]);
			Info.m_strPrefabName = Datas[index++];
			string strIAPType = Datas[index++];
			for (int j = 0; j < (int)eIAPType.Max; ++j)
			{
				eIAPType eType = (eIAPType)j;
				if (strIAPType == eType.ToString())
				{
					Info.m_eIAPType = eType;
					break;
				}
			}
			Info.m_strProduct_ID = Datas[index++];
            Info.m_strProduct_Name = Datas[index++];

            string strProductType = Datas[index++];
			if(strProductType == ProductType.Consumable.ToString())				Info.m_ProductType = ProductType.Consumable;
			else if (strProductType == ProductType.NonConsumable.ToString())	Info.m_ProductType = ProductType.NonConsumable;
			else if (strProductType == ProductType.Subscription.ToString())		Info.m_ProductType = ProductType.Subscription;

            Info.m_strIconFileName = Datas[index++];
            Info.m_nPackageName_TextTableID = Helper.ConvertStringToInt(Datas[index++]);

            Info.m_nCoin = Helper.ConvertStringToInt(Datas[index++]);
			string strIncludeItemType = Datas[index++];
			if (strIncludeItemType != "0")
			{
				string[] strItems = strIncludeItemType.Split('/');

				foreach (string strItem in strItems)
				{
					string[] strDatas = strItem.Split(',');
					Info.m_IncludeItemTable.Add(Helper.ConvertStringToInt(strDatas[0]), Helper.ConvertStringToInt(strDatas[1]));
				}
			}

			Info.m_strOfflineMode_Price = Datas[index++];

			m_DataInfoTable.Add(Info.m_nID, Info);
		}
	}

	public int GetNumData()
	{
		return m_DataInfoTable.Count;
	}

	public ExcelData_Google_IAPDataInfo GetIAPDataInfo_byIndex(int nIndex)
	{
		if(nIndex < 0 || nIndex >= m_DataInfoTable.Count)
			return null;

		int i = 0;
		foreach (KeyValuePair<int, ExcelData_Google_IAPDataInfo> item in m_DataInfoTable)
		{
			if (i == nIndex)
			{
				return item.Value;
			}

			++i;
		}

		return null;
	}

	public ExcelData_Google_IAPDataInfo GetIAPDataInfo_byID(int nID)
	{
		if (m_DataInfoTable.ContainsKey(nID) == true)
		{
			return m_DataInfoTable[nID];
		}

		return null;
	}

	public ExcelData_Google_IAPDataInfo GetIAPDataInfo_byProductID(string strProductID)
	{
		foreach (KeyValuePair<int, ExcelData_Google_IAPDataInfo> item in m_DataInfoTable)
		{
			if (strProductID == item.Value.m_strProduct_ID)
			{
				return item.Value;
			}
		}

		return null;
	}
}
