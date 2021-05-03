using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExcelData_SoundDataInfo
{
	public int 					m_nID           = 0;
	public string 		        m_strFileName   = "";
    public eSoundType			m_eSoundType    = eSoundType.Lobby;
    public eSoundKind			m_eSoundKind    = eSoundKind.eBKG;
	public float				m_fVolume		= 1.0f;

    public ExcelData_SoundDataInfo()
	{
	}
}

public class ExcelData_SoundData
{	
	private List<ExcelData_SoundDataInfo> 	m_DataInfoList = new List<ExcelData_SoundDataInfo>();
    private Dictionary<eSoundType, List<ExcelData_SoundDataInfo>> m_DataInfoList_byType = new Dictionary<eSoundType, List<ExcelData_SoundDataInfo>>();
	
	public ExcelData_SoundData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for( int i = 0; i < RowCount; ++i )
		{
			ExcelData_SoundDataInfo Info = new ExcelData_SoundDataInfo();
			
			Info.m_nID 				= Helper.ConvertStringToInt(Datas[index++]);
			Info.m_strFileName 		= Datas[index++];

			string strSoundType     = Datas[index++];

			for (int j = 0; j < (int)eSoundType.Max; ++j)
			{
				eSoundType eType = (eSoundType)j;
				if (eType.ToString() == strSoundType)
				{
					Info.m_eSoundType = eType;

					break;
				}
			}

            string strSoundKind = Datas[index++];
            if (strSoundKind == "BKG")
                Info.m_eSoundKind = eSoundKind.eBKG;
            else if (strSoundKind == "Effect")
                Info.m_eSoundKind = eSoundKind.eEffect;

			Info.m_fVolume = Helper.ConvertStringToFloat(Datas[index++]);
			m_DataInfoList.Add (Info);

            if(m_DataInfoList_byType.ContainsKey(Info.m_eSoundType))
            {
                m_DataInfoList_byType[Info.m_eSoundType].Add(Info);
            }
            else
            {
                List<ExcelData_SoundDataInfo> SoundDataInfoList = new List<ExcelData_SoundDataInfo>();
                SoundDataInfoList.Add(Info);
                m_DataInfoList_byType.Add(Info.m_eSoundType, SoundDataInfoList);
            }
		}		
	}
	
	public ExcelData_SoundDataInfo	FindSoundInfo(uint id)
	{
		foreach(ExcelData_SoundDataInfo Info in m_DataInfoList)
		{
			if( Info.m_nID == id )
			{
				return Info;
			}
		}
		
		return null;
	}
	
	public int 						GetNumSoundInfo()
	{
		return m_DataInfoList.Count;
	}
	
	public ExcelData_SoundDataInfo	GetSoundInfo_byIndex(int Index)
	{

		return m_DataInfoList[Index];
	}

    public ExcelData_SoundDataInfo  GetRandomSoundInfo_byType(eSoundType eType)
    {
        int nNumData = m_DataInfoList_byType[eType].Count;
        int nRandomValue = Random.Range(0, nNumData);

        return m_DataInfoList_byType[eType][nRandomValue];
    }
}
