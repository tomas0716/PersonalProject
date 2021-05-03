using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Messaging;

public class CSceneManager
{
	eSceneType 			m_eCurrSceneType 	= eSceneType.None;
    CScene              m_pCurrScene		= null;

    GameObject          m_pCameraObject     = null;


    public CSceneManager ()
	{
		QualitySettings.antiAliasing = 2;
	}

	public void 	Initialize()
	{
		CreateCamera();
		CreateSoundListener();

		InitFirebase();

		SceneManager.sceneLoaded += OnSceneLoaded;
		AppInstance.Instance.m_pEventDelegateManager.OnEventCreateInGameLoading += OnCreateInGameLoading;
	}

	public void OnDestroy()
	{
		AppInstance.Instance.m_pEventDelegateManager.OnEventCreateInGameLoading -= OnCreateInGameLoading;
	}

	private void InitFirebase()
	{
		OutputLog.Log("InitFirebase");

		Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
			var dependencyStatus = task.Result;
			if (dependencyStatus == Firebase.DependencyStatus.Available)
			{
				// Create and hold a reference to your FirebaseApp,
				// where app is a Firebase.FirebaseApp property of your application class.
				AppInstance.Instance.m_pFirebaseApp = Firebase.FirebaseApp.DefaultInstance;

				OutputLog.Log("InitFirebase Complete");

				FirebaseMessaging.TokenReceived += OnTokenReceived;
				FirebaseMessaging.MessageReceived += OnMessageReceived;
				// Set a flag here to indicate whether Firebase is ready to use by your app.
			}
			else
			{
				OutputLog.Log(System.String.Format(
				  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
				// Firebase Unity SDK is not safe to use here.
			}
		});
	}

	void OnTokenReceived(object sender, TokenReceivedEventArgs e)
	{
		OutputLog.Log(string.Format("Firebase Token : {0}", e.Token));
	}

	void OnMessageReceived(object sender, MessageReceivedEventArgs e)
	{
		OutputLog.Log(string.Format("Firebase From : {0}, Title : {1}, Text : {2}", e.Message.From, e.Message.Notification.Title, e.Message.Notification.Body));
	}

	private void	CreateCamera()
	{
        if (m_pCameraObject != null)
            GameObject.DestroyImmediate(m_pCameraObject, true);

        m_pCameraObject = new GameObject("MainCamera");
		GameObject.DontDestroyOnLoad(m_pCameraObject);
		Camera camera = m_pCameraObject.AddComponent<Camera>();
		camera.tag = "MainCamera";

		camera.backgroundColor = new Color(0, 0, 0, 0);
		int layerMask = 1 << LayerMask.NameToLayer("2D");
		layerMask += 1 << LayerMask.NameToLayer("Slot");
		layerMask += 1 << LayerMask.NameToLayer("Unit");
		camera.cullingMask = layerMask;
		camera.orthographic = true;
		camera.clearFlags = CameraClearFlags.SolidColor;

		camera.orthographicSize = AppInstance.Instance.m_vCurrResolution.y * 0.5f;

		camera.nearClipPlane = 0.1f;
		camera.farClipPlane = 1000000;

		float fHeight = AppInstance.Instance.m_vBaseResolution.y * 0.5f * AppInstance.Instance.m_fMainScale;
		m_pCameraObject.transform.position = new Vector3(0, fHeight, -99990);
	}

	private void	        CreateSoundListener()
	{
		GameObject SoundListenerObject = new GameObject("SoundListenerObject");
		GameObject.DontDestroyOnLoad(SoundListenerObject);
		SoundListenerObject.transform.position = new Vector3(0, 0, 0);
		SoundListenerObject.AddComponent<AudioListener>();
	}
	
	public void 	        Update () 
	{
		if (m_eCurrSceneType != eSceneType.None) 
		{
			if (AppInstance.Instance.m_vCurrResolution.x != Screen.width || AppInstance.Instance.m_vCurrResolution.y != Screen.height) 
			{
				AppInstance.Instance.ResetResoultion ();
			}	
		}
	}
	
	public void 	        ChangeScene(eSceneType eType, bool ShowLoadScene)
	{		
		if( m_eCurrSceneType != eType )
		{
			PopupManager.Instance.ClearAll();

			m_pCurrScene = null;
            m_eCurrSceneType = eType;

			System.GC.Collect();
			Resources.UnloadUnusedAssets();

			SceneManager.LoadScene(m_eCurrSceneType.ToString(), LoadSceneMode.Single);
		}
	}

    void                    OnSceneLoaded(Scene scene, LoadSceneMode Mode)
    {
        GameObject ob = GameObject.Find("Scene");

        if (ob != null)
            m_pCurrScene = ob.GetComponent(m_eCurrSceneType.ToString()) as CScene;
    }

	public void				SetCurrScene(CScene scene)
	{
		m_pCurrScene = scene;
	}

	public CScene           GetCurrScene()
	{
		return m_pCurrScene;
	}

	public void				SetCurrSceneType(eSceneType eType)
	{
		m_eCurrSceneType = eType;
	}

	public eSceneType		GetCurrSceneType()
	{
		return m_eCurrSceneType;
	}

	public void OnCreateInGameLoading()
	{
		GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/InGame_Loading");
		GameObject.Instantiate(ob);
	}

	public void 			OnGameQuit()
	{
		Application.Quit ();
	}
}
