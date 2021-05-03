using System;
using UnityEngine;

public class AppInstance : MonoBehaviour
{
    private static AppInstance ms_pInstance;
    public static AppInstance Instance  { get { return AppInstance.ms_pInstance; } }

    public TextureResourceManager       m_pTextureResourceManager;
    public MaterialResourceManager      m_pMaterialResourceManager;
    public CSceneManager                m_pSceneManager;
    public ParticleManager              m_pParticleManager;
    public GameEventManager             m_pGameEventManager;
    public AtlasManager                 m_pAtlasManager;
    public DataStackManager             m_pDataStackManager;
    public EventDelegateManager         m_pEventDelegateManager;
    public GameInfo                     m_pGameInfo;
    public InGameInfo                   m_pInGameInfo;
    public OptionInfo                   m_pOptionInfo;
    public SavedGameDataInfo            m_pSavedGameDataInfo;
    public SoundPlayer                  m_pSoundPlayer;
    public SLangFilter                  m_pSLangFilter;
    public InGameRandom                 m_pInGameRandom;

    public Vector2                      m_vBaseResolution                   = new Vector2(720f, 1280f);
    public Vector2                      m_vCurrResolution                   = new Vector2(720f, 1280f);
	public float                        m_fMainScale                        = 1f;
	public float                        m_fWidthScale                       = 1f;
	public float                        m_fHeightScale                      = 1f;

    public string                       m_GooglePlayUrl                     = "";

    public Firebase.FirebaseApp         m_pFirebaseApp                      = null;

    public static void CreateInstance()
    {
        if (AppInstance.ms_pInstance == null)
        {
            AppInstance.ms_pInstance = new GameObject("AppInstance").AddComponent<AppInstance>();
            UnityEngine.Object.DontDestroyOnLoad(AppInstance.ms_pInstance);
            new GameDefine();
            AppInstance.ms_pInstance.InitTypeObject();
            AppInstance.ms_pInstance.ResetResoultion();
            AppInstance.ms_pInstance.CreateManager();
        }
    }

    public void CreateManager()
    {
        m_pTextureResourceManager   = new TextureResourceManager();
        m_pMaterialResourceManager  = new MaterialResourceManager();
        m_pSceneManager             = new CSceneManager();
        m_pParticleManager          = new ParticleManager();
        m_pGameEventManager         = new GameEventManager();
        m_pAtlasManager             = new AtlasManager();
        m_pDataStackManager         = new DataStackManager();
        m_pEventDelegateManager     = new EventDelegateManager();
        m_pGameInfo                 = new GameInfo();
        m_pInGameInfo               = new InGameInfo();
        m_pOptionInfo               = new OptionInfo();
        m_pSavedGameDataInfo        = new SavedGameDataInfo();
        m_pSoundPlayer              = new SoundPlayer();
        m_pInGameRandom             = new InGameRandom();
        m_pSLangFilter              = new SLangFilter();

        m_pSceneManager.Initialize();
    }

	private void OnDestroy()
	{
        m_pSceneManager.SetCurrSceneType(eSceneType.None);
		m_pSceneManager.OnDestroy();
	}

	public void InitTypeObject()
    {
        TypeManager.AddTypeObject("MapDataMissionInfo_Unit",        typeof(MapDataMissionInfo_Unit));
        TypeManager.AddTypeObject("MapDataMissionInfo_Bell",        typeof(MapDataMissionInfo_Bell));
        TypeManager.AddTypeObject("MapDataMissionInfo_Mouse",       typeof(MapDataMissionInfo_Mouse));
        TypeManager.AddTypeObject("MapDataMissionInfo_Apple",       typeof(MapDataMissionInfo_Apple));
        TypeManager.AddTypeObject("MapDataMissionInfo_Rock",        typeof(MapDataMissionInfo_Rock));
        TypeManager.AddTypeObject("MapDataMissionInfo_Stripe",      typeof(MapDataMissionInfo_Stripe));
        TypeManager.AddTypeObject("MapDataMissionInfo_Cross",       typeof(MapDataMissionInfo_Cross));
        TypeManager.AddTypeObject("MapDataMissionInfo_Grass",       typeof(MapDataMissionInfo_Grass));
        TypeManager.AddTypeObject("MapDataMissionInfo_Magician",    typeof(MapDataMissionInfo_Magician));
        TypeManager.AddTypeObject("MapDataMissionInfo_Bread",       typeof(MapDataMissionInfo_Bread));
        TypeManager.AddTypeObject("MapDataMissionInfo_Jelly",       typeof(MapDataMissionInfo_Jelly));
        TypeManager.AddTypeObject("MapDataMissionInfo_Fish",        typeof(MapDataMissionInfo_Fish));
        TypeManager.AddTypeObject("MapDataMissionInfo_Number",      typeof(MapDataMissionInfo_Number));
        TypeManager.AddTypeObject("MapDataMissionInfo_Octopus",     typeof(MapDataMissionInfo_Octopus));
        TypeManager.AddTypeObject("MapDataMissionInfo_Knit",        typeof(MapDataMissionInfo_Knit));
        TypeManager.AddTypeObject("MapDataMissionInfo_Can",         typeof(MapDataMissionInfo_Can));
        TypeManager.AddTypeObject("MapDataMissionInfo_Butterfly",   typeof(MapDataMissionInfo_Butterfly));
    }

    public void ResetResoultion()
    {
        m_vCurrResolution = new Vector2((float)Screen.width, (float)Screen.height);
        m_fWidthScale = m_vCurrResolution.x / m_vBaseResolution.x;
        m_fHeightScale = m_vCurrResolution.y / m_vBaseResolution.y;
        if (m_fWidthScale < m_fHeightScale)
        {
            m_fMainScale = m_fWidthScale;
            return;
        }
        m_fMainScale = m_fHeightScale;
        //m_fMainScale = 1;
    }

    private void Update()
    {
        m_pSceneManager.Update();
        m_pParticleManager.Update();
        m_pGameEventManager.Update();
        m_pSoundPlayer.Update();

        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = false;

            m_pEventDelegateManager.OnHardwareBackButtonClick();

            if (GameInfo.Instance.m_IsHardwareBackButtonProcess == false)
            {
                // 종료 메시지 창 출력
                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
                ob = GameObject.Instantiate(ob);
                MessageBox pMessageBox = ob.GetComponent<MessageBox>();
                pMessageBox.Initialize(MessageBox.eMessageBoxType.OKCancel, "", Helper.GetTextDataString(eTextDataType.ExitGame), Helper.GetTextDataString(eTextDataType.App_Quit), Helper.GetTextDataString(eTextDataType.Cancel));

                pMessageBox.m_Callback_OK += OnExitGame;
            }
        }

    }

    public void OnExitGame()
    {
        Application.Quit();
    }

    public void OnNextDay()
    {
        m_pSavedGameDataInfo.m_IsGetFreeCoin = false;
        m_pSavedGameDataInfo.m_byGetAdsCoinCount = 0;

        m_pSavedGameDataInfo.m_IsGetAttendance = false;

        m_pSavedGameDataInfo.m_IsGetFreeRoulette = false;
        m_pSavedGameDataInfo.m_IsGetAdsRoulette = false;

        m_pSavedGameDataInfo.m_byGetFreeHeartCountForAds = 0;
    }

    public void ReExecute()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject unity_activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass intent_class = new AndroidJavaClass("android.content.Intent");
        AndroidJavaClass pending_intnet_class = new AndroidJavaClass("android.app.PendingIntent");
        AndroidJavaObject base_context = unity_activity.Call<AndroidJavaObject>("getBaseContext");
        AndroidJavaObject intentObj = base_context.Call<AndroidJavaObject>("getPackageManager").Call<AndroidJavaObject>("getLaunchIntentForPackage", base_context.Call<string>("getPackageName"));
        AndroidJavaObject context = unity_activity.Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaObject pending_intent_obj = pending_intnet_class.CallStatic<AndroidJavaObject>("getActivity", context, 123456, intentObj, pending_intnet_class.GetStatic<int>("FLAG_CANCEL_CURRENT")); 
        AndroidJavaClass alarm_manager = new AndroidJavaClass("android.app.AlarmManager"); 
        AndroidJavaClass JavaSystemClass = new AndroidJavaClass("java.lang.System"); 
        AndroidJavaObject alarm = unity_activity.Call<AndroidJavaObject>("getSystemService", "alarm");
        //long restart_mill = JavaSystemClass.CallStatic<long>("currentTimeMillis") + 50; 
        long restart_mill = 0;
        alarm.Call("set", alarm_manager.GetStatic<int>("RTC"), restart_mill, pending_intent_obj); 
        JavaSystemClass.CallStatic("exit", 0); 
#endif
    }

	private void OnApplicationPause(bool pause)
	{
		
	}
}
