using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExcelDataManager : MonoBehaviour
{
	private static ExcelDataManager  ms_pInstance = null;
	public static ExcelDataManager Instance { get{ return ms_pInstance;	} }

	public ExcelData_SoundData				m_pExcelData_SoundData				= null;
	public ExcelData_TextData				m_pExcelData_TextData				= null;
	public ExcelData_SlotUnitData			m_pExcelData_SlotUnitData			= null;

	public ExcelData_MapTableData			m_pExcelData_MapTableData			= null;
	public ExcelData_MissionData			m_pExcelData_MissionData			= null;
	public ExcelData_DisturbData			m_pExcelData_DisturbData			= null;

	public ExcelData_SlotOverlap			m_pExcelData_SlotOverlap_Mission	= null;
	public ExcelData_SlotOverlap			m_pExcelData_SlotOverlap_Disturb	= null;

	public ExcelData_LevelFixedMissionData	m_pExcelData_LevelFixedMissionData	= null;

	public ExcelData_ItemData				m_pExcelData_ItemData				= null;
	public ExcelData_Google_IAPData			m_pExcelData_Google_IAPData			= null;
	public ExcelData_AchievementData		m_pExcelData_AchievementData		= null;

	public ExcelData_Event_AttendanceData	m_pExcelData_Event_AttendanceData	= null;
	public ExcelData_Event_RouletteData		m_pExcelData_Event_RouletteData		= null;

	public ExcelData_LoadingData			m_pExcelData_LoadingData			= null;

	public void LoadExcelDatas()
	{
		ExcelConvertData ConvertData = null;

		ConvertData = (ExcelConvertData)Resources.Load("Table/Sound_Data", typeof(ExcelConvertData));
        m_pExcelData_SoundData = new ExcelData_SoundData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

        ConvertData = (ExcelConvertData)Resources.Load("Table/Text_Data", typeof(ExcelConvertData));
        m_pExcelData_TextData = new ExcelData_TextData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/Slot_Unit", typeof(ExcelConvertData));
		m_pExcelData_SlotUnitData = new ExcelData_SlotUnitData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/MapTable", typeof(ExcelConvertData));
		m_pExcelData_MapTableData = new ExcelData_MapTableData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/Mission", typeof(ExcelConvertData));
		m_pExcelData_MissionData = new ExcelData_MissionData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/Disturb", typeof(ExcelConvertData));
		m_pExcelData_DisturbData = new ExcelData_DisturbData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/MissionOverlap", typeof(ExcelConvertData));
		m_pExcelData_SlotOverlap_Mission = new ExcelData_SlotOverlap(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/DisturbOverlap", typeof(ExcelConvertData));
		m_pExcelData_SlotOverlap_Disturb = new ExcelData_SlotOverlap(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/LevelFixedMission", typeof(ExcelConvertData));
		m_pExcelData_LevelFixedMissionData = new ExcelData_LevelFixedMissionData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/Item_Data", typeof(ExcelConvertData));
		m_pExcelData_ItemData = new ExcelData_ItemData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/Google_IAP_Data", typeof(ExcelConvertData));
		m_pExcelData_Google_IAPData = new ExcelData_Google_IAPData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/Google_Achievement", typeof(ExcelConvertData));
		m_pExcelData_AchievementData = new ExcelData_AchievementData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/Event_Attendance", typeof(ExcelConvertData));
		m_pExcelData_Event_AttendanceData = new ExcelData_Event_AttendanceData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/Event_Roulette", typeof(ExcelConvertData));
		m_pExcelData_Event_RouletteData = new ExcelData_Event_RouletteData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);

		ConvertData = (ExcelConvertData)Resources.Load("Table/Loading_Data", typeof(ExcelConvertData));
		m_pExcelData_LoadingData = new ExcelData_LoadingData(Helper.ConvertStringToInt(ConvertData.m_strColumnCount), Helper.ConvertStringToInt(ConvertData.m_strRowCount), ConvertData.m_strDatas);
	}

	public static void CreateInstance()
	{
	  	if (ms_pInstance == null)
        {
			ms_pInstance = (new GameObject("ExcelDataManager")).AddComponent<ExcelDataManager>();
			GameObject.DontDestroyOnLoad(ms_pInstance);
		
			ms_pInstance.LoadExcelDatas();
        }	
	}
}
