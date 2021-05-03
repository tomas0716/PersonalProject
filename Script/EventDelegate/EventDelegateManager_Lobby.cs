using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EventDelegateManager
{
	public delegate void OnEvent_LobbyNewGame_UpdataMissionUI();
	public event OnEvent_LobbyNewGame_UpdataMissionUI OnEventLobbyNewGame_UpdataMissionUI;
	public void OnLobbyNewGame_UpdataMissionUI()
	{
		OnEventLobbyNewGame_UpdataMissionUI?.Invoke();
	}
}
