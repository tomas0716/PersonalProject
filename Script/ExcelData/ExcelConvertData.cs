using UnityEngine;
using System.Collections;

public class ExcelConvertData : ScriptableObject 
{
	public string 	[]			m_strDatas;
	public string 				m_strColumnCount;
	public string 				m_strRowCount;
		
	public ExcelConvertData()
	{
	}
	
	public void 			SetDatas(string strColumnCount, string strRowCount, string[] content)
	{
		m_strColumnCount = strColumnCount;
		m_strRowCount = strRowCount;
		
		this.m_strDatas = content;
	}
}
