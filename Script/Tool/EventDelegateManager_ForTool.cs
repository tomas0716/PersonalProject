using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

public partial class EventDelegateManager_ForTool
{
	private static EventDelegateManager_ForTool ms_pInstance = null;
	public static EventDelegateManager_ForTool Instance 
	{ 
		get
		{
			if(ms_pInstance == null)
				ms_pInstance = new EventDelegateManager_ForTool();

			return ms_pInstance; 
		} 
	}

	public EventDelegateManager_ForTool()
	{
		ms_pInstance = this;
	}

	public delegate void OnEvent_UpdateMap(bool IsChangeMap);
	public event OnEvent_UpdateMap OnEventUpdateMap;
	public void OnUpdateMap(bool IsChangeMap)
	{
		OnEventUpdateMap?.Invoke(IsChangeMap);
	}

	public delegate void OnEvent_PostUpdateMap(bool IsChangeMap);
	public event OnEvent_PostUpdateMap OnEventPostUpdateMap;
	public void OnPostUpdateMap(bool IsChangeMap)
	{
		OnEventPostUpdateMap?.Invoke(IsChangeMap);
	}

	public delegate void OnEvent_Tool_Mission_Common_UnCheck();
	public event OnEvent_Tool_Mission_Common_UnCheck OnEventMission_Common_UnCheck;
	public void OnMission_Common_UnCheck()
	{
		OnEventMission_Common_UnCheck?.Invoke();
	}

	public delegate void OnEvent_Tool_Mission_UnCheck();
	public event OnEvent_Tool_Mission_UnCheck OnEventMission_UnCheck;
	public void OnMission_UnCheck()
	{
		OnEventMission_UnCheck?.Invoke();
	}

	public delegate void OnEvent_OnAllMissionRemove();
	public event OnEvent_OnAllMissionRemove OnEventOnAllMissionRemove;
	public void OnAllMissionRemove()
	{
		OnEventOnAllMissionRemove?.Invoke();
	}

	public delegate void OnEvent_DisturbPoint_Visible(bool IsVisible);
	public event OnEvent_DisturbPoint_Visible OnEventDisturbPoint_Visible;
	public void OnDisturbPoint_Visible(bool IsVisible)
	{
		OnEventDisturbPoint_Visible?.Invoke(IsVisible);
	}
}

#endif