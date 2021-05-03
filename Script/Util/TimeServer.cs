using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeServer : MonoBehaviour
{
    private static TimeServer m_pInstance = null;
    public static TimeServer Instance { get { return m_pInstance; } }

    private DateTime			m_ServerDataTime		= new DateTime(2020, 1, 21);

	private bool				m_IsInitialize			= false;
	private double				m_dwElpasedTime			= 0;

	private float				m_dwInitRealTime		= 0;
	private float				m_dwLastRealTime		= 0;

	private Transformer_Timer	m_pTimer_NextDay		= new Transformer_Timer();
	private float				m_fNextDayRemainTime	= 0;

    void Awake()
    {
        m_pInstance = this;
    }

    void Update()
    {
		if (m_IsInitialize == true)
		{
			m_dwElpasedTime = Time.realtimeSinceStartup - m_dwInitRealTime;

			float fDeltaTime = Time.realtimeSinceStartup - m_dwLastRealTime;
			m_pTimer_NextDay.Update(fDeltaTime);
			m_dwLastRealTime = Time.realtimeSinceStartup;
		}
	}

	public void UpdateTime()
	{
		Update();
	}

	public float GetNextDayRemainTime()
	{
		return m_fNextDayRemainTime - m_pTimer_NextDay.GetCurTime();
	}

    private DateTime GetDummyDate()
    {
		return DateTime.Now;
    }

	public DateTime GetNISTDate()
	{
		if (m_IsInitialize == true)
		{
			TimeSpan timespan = System.TimeSpan.FromSeconds(m_dwElpasedTime);
			DateTime dateTime = m_ServerDataTime;
			return dateTime.Add(timespan);
		}

		return Innter_GetNISTDate();
	}

	private DateTime Innter_GetNISTDate()
	{
        //if (Application.internetReachability == NetworkReachability.NotReachable)
        //{
        //    m_ServerDataTime = System.DateTime.Now;
        //}
        //else
        //{
        //    while (true)
        //    {
        //        m_ServerDataTime = Inner_NISTDate();
        //        if (!(m_ServerDataTime.Year == 2020 && m_ServerDataTime.Month == 1 && m_ServerDataTime.Day == 21))
        //        {
        //            break;
        //        }
        //    }
        //}

        m_ServerDataTime = System.DateTime.Now;

        return m_ServerDataTime;
	}

    public void OnNextDayCalculation(bool IsNormalTime)
    {
        m_IsInitialize = true;

        m_dwInitRealTime = Time.realtimeSinceStartup;

        m_fNextDayRemainTime = (60 * 60 * 24) - (m_ServerDataTime.Hour * 60 * 60 + m_ServerDataTime.Minute * 60 + m_ServerDataTime.Second);
        m_fNextDayRemainTime += 60;

        if (IsNormalTime == false)
            m_fNextDayRemainTime += (60 * 60 * 24);

		m_pTimer_NextDay.OnReset();

		TransformerEvent_Timer eventValue;
        eventValue = new TransformerEvent_Timer(m_fNextDayRemainTime);
        m_pTimer_NextDay.AddEvent(eventValue);
        m_pTimer_NextDay.SetCallback(null, OnDone_Timer_NextDay);
        m_pTimer_NextDay.OnPlay();

    }

    private void OnDone_Timer_NextDay(TransformerEvent eventValue)
	{
		m_pTimer_NextDay.OnReset();
		eventValue = new TransformerEvent_Timer(60 * 60 * 24);
		m_pTimer_NextDay.AddEvent(eventValue);
		m_pTimer_NextDay.SetCallback(null, OnDone_Timer_NextDay);
		m_pTimer_NextDay.OnPlay();

		m_fNextDayRemainTime = 60 * 60 * 24;

		AppInstance.Instance.OnNextDay();
		AppInstance.Instance.m_pEventDelegateManager.OnNextDay();
	}

	private DateTime Inner_NISTDate()
	{
		bool IsValid = false;
		DateTime date = GetDummyDate();
		string serverResponse = string.Empty;

		// NIST 서버 목록
		string[] servers = new string[] {
				"time.nist.gov",
				"time-a.nist.gov",
				"time-b.nist.gov",
			};

		for (int i = 0; i < 5; ++i)
		{
			for (int j = 0; j < servers.Length; j++)
			{
				try
				{
					int nServerIndex = j;

					StreamReader reader = new StreamReader(new System.Net.Sockets.TcpClient(servers[nServerIndex], 13).GetStream());
					serverResponse = reader.ReadToEnd();
					reader.Close();

					OutputLog.Log("TimeServer Index : " + nServerIndex);
					OutputLog.Log("TimeServer Time : " + serverResponse);

					if (serverResponse.Length > 47 && serverResponse.Contains("UTC(NIST)") == true)
					{
						int jd = int.Parse(serverResponse.Substring(1, 5));
						int yr = int.Parse(serverResponse.Substring(7, 2));
						int mo = int.Parse(serverResponse.Substring(10, 2));
						int dy = int.Parse(serverResponse.Substring(13, 2));
						int hr = int.Parse(serverResponse.Substring(16, 2));
						int mm = int.Parse(serverResponse.Substring(19, 2));
						int sc = int.Parse(serverResponse.Substring(22, 2));

						if (jd > 51544)
							yr += 2000;
						else
							yr += 1999;

						date = new DateTime(yr, mo, dy, hr, mm, sc);

						OutputLog.Log("TimeServer Time : " + date.ToString());

						IsValid = true;
						break;
					}
				}
				catch (Exception e)
				{
				}
			}

			if (IsValid == true)
			{
				break;
			}
		}

		if (IsValid == true)
		{
			m_ServerDataTime = date;
		}

		return m_ServerDataTime;
	}
}
