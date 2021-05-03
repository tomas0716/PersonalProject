using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartChargeTimer : MonoBehaviour
{
    private static HeartChargeTimer m_pInstance = null;
    public static HeartChargeTimer Instance { get { return m_pInstance; } }

	private bool				m_IsFull					= false;
	private double				m_nAutoChargeTimerSecond	= 0;

	private Transformer_Scalar	m_pTimer_AutoCharge			= new Transformer_Scalar(0);

	private float				m_dwLastRealTime			= 0;

	private void Awake()
    {
        m_pInstance = this;
    }

	public void OnInitialize()
	{
		m_dwLastRealTime = Time.realtimeSinceStartup;

		AppInstance.Instance.m_pEventDelegateManager.OnEventInGameStartHeartMinus += OnInGameStartHeartMinus;
		AppInstance.Instance.m_pEventDelegateManager.OnEventUpdateHeartInfo += OnUpdateHeartInfo;
		AppInstance.Instance.m_pEventDelegateManager.OnEventVideoRewardHeartPlus += OnVideoRewardHeartPlus;
		AppInstance.Instance.m_pEventDelegateManager.OnEventCoinToHeartCharge += OnCoinToHeartCharge;
	}

	private void OnDestroy()
	{
		AppInstance.Instance.m_pEventDelegateManager.OnEventInGameStartHeartMinus -= OnInGameStartHeartMinus;
		AppInstance.Instance.m_pEventDelegateManager.OnEventUpdateHeartInfo -= OnUpdateHeartInfo;
		AppInstance.Instance.m_pEventDelegateManager.OnEventVideoRewardHeartPlus -= OnVideoRewardHeartPlus;
		AppInstance.Instance.m_pEventDelegateManager.OnEventCoinToHeartCharge -= OnCoinToHeartCharge;
	}

	void Update()
    {
		float fDeltaTime = Time.realtimeSinceStartup - m_dwLastRealTime;
		m_dwLastRealTime = Time.realtimeSinceStartup;

		m_pTimer_AutoCharge.Update(fDeltaTime);
		m_nAutoChargeTimerSecond = (double)m_pTimer_AutoCharge.GetCurScalar();
	}

	public bool IsFull()
	{
		return m_IsFull;
	}

	public double GetAutoChargeTimerSecond()
	{
		return m_nAutoChargeTimerSecond;
	}

	public void OnCalculate()
	{
		int nHeartCount = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart];

		if (nHeartCount >= GameDefine.ms_nMaxAutoChargeHeart)
		{
			m_IsFull = true;
		}
		else
		{
			m_IsFull = false;

			System.DateTime RecordTime = new System.DateTime((int)SavedGameDataInfo.Instance.m_sYear_Heart,
															 (int)SavedGameDataInfo.Instance.m_byMonth_Heart,
															 (int)SavedGameDataInfo.Instance.m_byDay_Heart,
															 (int)SavedGameDataInfo.Instance.m_byHour_Heart,
															 (int)SavedGameDataInfo.Instance.m_byMinute_Heart,
															 (int)SavedGameDataInfo.Instance.m_bySecond_Heart);

			System.DateTime CurrTime = TimeServer.Instance.GetNISTDate();
			TimeSpan timeSpan = CurrTime - RecordTime;
			double dwTime = timeSpan.TotalSeconds;

            if (CurrTime < RecordTime)
            {
                dwTime = 0;
            }

            int nChargeCount = (int)(dwTime / GameDefine.ms_dwAutoChargeHeartTimeSecond);
			m_nAutoChargeTimerSecond = GameDefine.ms_dwAutoChargeHeartTimeSecond - dwTime % GameDefine.ms_dwAutoChargeHeartTimeSecond;
			if (nChargeCount != 0)
			{
				int nRealChargeCount = nHeartCount + nChargeCount <= GameDefine.ms_nMaxAutoChargeHeart ? nChargeCount : GameDefine.ms_nMaxAutoChargeHeart - nHeartCount;

				if (nRealChargeCount > 0)
				{
					TimeSpan timespan = System.TimeSpan.FromSeconds(GameDefine.ms_dwAutoChargeHeartTimeSecond - m_nAutoChargeTimerSecond);
					DateTime RecordDateTime = CurrTime.Subtract(timespan);
					SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] += nRealChargeCount;
					SavedGameDataInfo.Instance.m_sYear_Heart = (short)RecordDateTime.Year;
					SavedGameDataInfo.Instance.m_byMonth_Heart = (byte)RecordDateTime.Month;
					SavedGameDataInfo.Instance.m_byDay_Heart = (byte)RecordDateTime.Day;
					SavedGameDataInfo.Instance.m_byHour_Heart = (byte)RecordDateTime.Hour;
					SavedGameDataInfo.Instance.m_byMinute_Heart = (byte)RecordDateTime.Minute;
					SavedGameDataInfo.Instance.m_bySecond_Heart = (byte)RecordDateTime.Second;
					SavedGameDataInfo.Instance.Save(false);

					AppInstance.Instance.m_pEventDelegateManager.OnUpdateHeartInfo();

					if (nHeartCount >= GameDefine.ms_nMaxAutoChargeHeart)
					{
						m_IsFull = true;
					}
				}
			}

			m_pTimer_AutoCharge.OnReset();

			if (m_IsFull == false)
			{
				TransformerEvent_Scalar eventValue = null;
				eventValue = new TransformerEvent_Scalar(0, (float)m_nAutoChargeTimerSecond);
				m_pTimer_AutoCharge.AddEvent(eventValue);
				eventValue = new TransformerEvent_Scalar((float)m_nAutoChargeTimerSecond, 0);
				m_pTimer_AutoCharge.AddEvent(eventValue);

				m_pTimer_AutoCharge.SetCallback(null, OnDone_Timer_AutoCharge);
				m_pTimer_AutoCharge.OnPlay();
			}
		}
	}

	private void OnDone_Timer_AutoCharge(TransformerEvent eventValue)
	{
		TimeServer.Instance.UpdateTime();
		OnCalculate();

		//int nHeartCount = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart];
		//if (nHeartCount + 1 <= GameDefine.ms_nMaxAutoChargeHeart)
		//{
		//	DateTime ServerTime = TimeServer.Instance.GetNISTDate();
		//	SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] += 1;
		//	SavedGameDataInfo.Instance.m_sYear_Heart = (short)ServerTime.Year;
		//	SavedGameDataInfo.Instance.m_byMonth_Heart = (byte)ServerTime.Month;
		//	SavedGameDataInfo.Instance.m_byDay_Heart = (byte)ServerTime.Day;
		//	SavedGameDataInfo.Instance.m_byHour_Heart = (byte)ServerTime.Hour;
		//	SavedGameDataInfo.Instance.m_byMinute_Heart = (byte)ServerTime.Minute;
		//	SavedGameDataInfo.Instance.m_bySecond_Heart = (byte)ServerTime.Second;
		//	SavedGameDataInfo.Instance.Save(false);

		//	AppInstance.Instance.m_pEventDelegateManager.OnUpdateHeartInfo();
		//	OnCalculate();
		//}
	}

	public void OnInGameStartHeartMinus()
	{
		OnCalculate();
	}

	public void OnUpdateHeartInfo()
	{
		OnCalculate();
	}

	public void OnVideoRewardHeartPlus()
	{
		OnCalculate();
	}

	public void OnCoinToHeartCharge()
	{
		OnCalculate();
	}
}
