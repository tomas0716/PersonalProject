using UnityEngine;
using System.Collections;

public abstract class CScene : MonoBehaviour
{
	protected abstract void Initialize();
    protected abstract void Destroy();
    protected abstract void Inner_Update();
	protected abstract void Inner_FixedUpdate();

	bool				m_IsInitialize	    = false;
	Transformer_Scalar	m_pTimer			= new Transformer_Scalar(0);

	public string		m_GooglePlayUrl;

	void Start () 
	{
		OutputLog.Log("CScene Start 01");

		ExcelDataManager.CreateInstance();
		AppInstance.CreateInstance();

		OutputLog.Log("CScene Start 02");

		m_pTimer.OnResetEvent();
		m_pTimer.OnResetCallback();
		m_pTimer.OnStop();

		TransformerEvent_Scalar eventValue = new TransformerEvent_Scalar(0.03f, 0);
		m_pTimer.AddEvent(eventValue);

		m_pTimer.SetCallback(null, OnDone);

		m_pTimer.OnPlay();

		AppInstance.Instance.m_pEventDelegateManager.OnEventCreateLoading += OnCreateLoading;
	}

    private void OnDestroy()
    {
		AppInstance.Instance.m_pEventDelegateManager.OnEventCreateLoading -= OnCreateLoading;

		Destroy();
	}

    void Update()
	{
		m_pTimer.Update(Time.deltaTime);

		if (m_IsInitialize == true) Inner_Update();
	}

	void FixedUpdate()
	{
		if (m_IsInitialize == true) Inner_FixedUpdate();
	}

	void OnDone(TransformerEvent eventVaue)
	{
		Initialize();
		m_IsInitialize = true;
	}

	public void OnCreateLoading()
	{
		GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/Loading");
		GameObject.Instantiate(ob);
	}
}
