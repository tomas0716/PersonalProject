using UnityEngine;
using System;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

public enum eSavedGameActionType
{
    None,
    Save,
    Load,
}

public class GooglePlaySavedGame : MonoBehaviour
{
    private static GooglePlaySavedGame m_pInstance = null;
    public static GooglePlaySavedGame Instance { get { return m_pInstance; } }

    private string                  m_strFileName               = "game_data_saved";
    private byte []                 m_byData                    = null;
    private eSavedGameActionType    m_eActionType               = eSavedGameActionType.None;

    private Transformer_Timer       m_pTimer_Waiting             = new Transformer_Timer();

    private bool                    m_IsProcess                 = false;

    private bool                    m_IsErrorMessageBoxActive   = true;

    void Awake()
    {
        m_pInstance = this;
    }

    void Update()
    {
        m_pTimer_Waiting.Update(Time.deltaTime);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Save
    /// 
    public void SaveToCloud(byte [] byData, bool IsErrorMessageBoxActive = true)
    {
#if UNITY_ANDROID
        OutputLog.Log("SaveToCloud 01");

        m_IsErrorMessageBoxActive = IsErrorMessageBoxActive;
        m_IsProcess = true;
        m_byData = byData;
        m_eActionType = eSavedGameActionType.Save;

        if (GameInfo.Instance.m_IsOfflineModeGameEnter == false)
        {
            if (Social.localUser.authenticated == true)
            {
                OutputLog.Log("SaveToCloud 02");

                ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(m_strFileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnFileOpenToSave);
            }
            else
            {
                AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlayLoginResult += OnGooglePlayLoginResult;
                GooglePlayGamesClient.Instance.Login();
            }
        }
        else
        {
        }
#else
        AppInstance.Instance.m_pEventDelegateManager.OnGooglePlaySavedGameDone_Save();
#endif
    }

    private void OnFileOpenToSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
#if UNITY_ANDROID
        OutputLog.Log("OnFileOpenToSave 01");

        if (status == SavedGameRequestStatus.Success)
        {
            OutputLog.Log("OnFileOpenToSave 02");

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

            SavedGameMetadataUpdate updatedMetadata = builder.Build();

            ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(metaData, updatedMetadata, m_byData, OnGameSave);
        }
        else
        {
            OutputLog.Log("OnFileOpenToSave 03");

            OpenMessageBox_Retry();
        }
#endif
    }

    private void OnGameSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        OutputLog.Log("OnGameSave 01");

        if (status == SavedGameRequestStatus.Success)
        {
            OutputLog.Log("OnGameSave 02");

            m_IsProcess = false;
            m_pTimer_Waiting.OnReset();
            m_eActionType = eSavedGameActionType.None;

            AppInstance.Instance.m_pSavedGameDataInfo.OnDone_Save();
        }
        else
        {
            OutputLog.Log("OnGameSave 03");

            OpenMessageBox_Retry();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Load
    ///

    public void LoadFromCloud(bool IsErrorMessageBoxActive = true)
    {
#if UNITY_ANDROID
        OutputLog.Log("LoadFromCloud 01");
        m_IsErrorMessageBoxActive = IsErrorMessageBoxActive;
        m_IsProcess = true;
        m_eActionType = eSavedGameActionType.Load;

        if (GameInfo.Instance.m_IsOfflineModeGameEnter == false)
        {
            if (Social.localUser.authenticated == true)
            {
                OutputLog.Log("LoadFromCloud 02");
                ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(m_strFileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnFileOpenToLoad);
            }
            else
            {
                OutputLog.Log("LoadFromCloud 03");
                AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlayLoginResult += OnGooglePlayLoginResult;
                GooglePlayGamesClient.Instance.Login();
            }
        }
        else
        {
            AppInstance.Instance.m_pEventDelegateManager.OnGooglePlaySavedGameDone_Load();
        }
#else
        AppInstance.Instance.m_pEventDelegateManager.OnGooglePlaySavedGameDone_Load();
#endif
    }

    private void OnFileOpenToLoad(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
#if UNITY_ANDROID
        OutputLog.Log("OnFileOpenToLoad 01");

        if (status == SavedGameRequestStatus.Success)
        {
            OutputLog.Log("OnFileOpenToLoad 02");
            ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(metaData, OnGameLoad);
        }
        else
        {
            OutputLog.Log("OnFileOpenToLoad 03");
            OpenMessageBox_Retry();
        }
#endif
    }

    private void OnGameLoad(SavedGameRequestStatus status, byte[] bytes)
    {
        OutputLog.Log("OnGameLoad 01");
        if (status == SavedGameRequestStatus.Success)
        {
            OutputLog.Log("OnGameLoad 02");

            m_IsProcess = false;
            m_pTimer_Waiting.OnReset();
            m_eActionType = eSavedGameActionType.None;

            AppInstance.Instance.m_pSavedGameDataInfo.OnDone_Load(bytes);
            HeartChargeTimer.Instance.OnCalculate();
            AppInstance.Instance.m_pEventDelegateManager.OnGooglePlaySavedGameDone_Load();
        }
        else
        {
            OutputLog.Log("OnGameLoad 03");
            OpenMessageBox_Retry();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// etc
    ///

    private void OpenMessageBox_Retry()
    {
        if (m_IsErrorMessageBoxActive == true)
        {
            //// 메시지 창을 띄워 다시 시도할수 있도록 한다.
            //GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
            //ob = GameObject.Instantiate(ob);
            //MessageBox pMessageBox = ob.GetComponent<MessageBox>();
            //pMessageBox.Initialize(MessageBox.eMessageBoxType.OKCancel,
            //                        Helper.GetTextDataString(eTextDataType.Error),
            //                        Helper.GetTextDataString(eTextDataType.InValid_Network_Desc),
            //                        Helper.GetTextDataString(eTextDataType.InValid_Network_ReExecute),
            //                        Helper.GetTextDataString(eTextDataType.InValid_Network_Retry));

            //pMessageBox.m_Callback_OK += ReExecute;
            //pMessageBox.m_Callback_Cancel += Retry;
        }
    }

    private void ReExecute()
    {
        AppInstance.Instance.ReExecute();
    }

    private void Retry()
    {
        switch (m_eActionType)
        {
            case eSavedGameActionType.Save:
                {
                    SaveToCloud(m_byData);
                }
                break;

            case eSavedGameActionType.Load:
                {
                    LoadFromCloud();
                }
                break;
        }
    }

    private void OnDone_Timer_Waiting(TransformerEvent eventValue)
    {
        if (m_IsErrorMessageBoxActive == true)
        {
            AppInstance.Instance.m_pEventDelegateManager.OnDeleteLoading();

            OpenMessageBox_Retry();
        }
    }

    public void OnGooglePlayLoginResult(bool IsResult, string strUserID, string strEmail)
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventGooglePlayLoginResult -= OnGooglePlayLoginResult;

        if (m_IsProcess == true)
        {
            //Retry();
        }
    }
}
