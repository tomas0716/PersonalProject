using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class InGameLog
{
#if UNITY_EDITOR
	static string ms_strDirectory = "InGameLog";
	static string ms_strLogPath = "InGameLog/" + DateTime.Now.ToString("(yyyy.MM.dd) [hh.mm.ss]") + ".txt";
#else
#if UNITY_ANDROID && _DEBUG
	static string ms_strLogPath = Application.persistentDataPath + "/" + DateTime.Now.ToString("(yyyy.MM.dd) [hh.mm.ss]") + ".txt";
#endif
#endif

	public static void Log(string strMsg)
	{
#if UNITY_EDITOR
		Debug.Log(strMsg);

		if (System.IO.Directory.Exists(ms_strDirectory) == false)
		{
			System.IO.Directory.CreateDirectory(ms_strDirectory);
		}

		FileStream fs = null;

		if (System.IO.File.Exists(ms_strLogPath) == false)
		{
			fs = new FileStream(ms_strLogPath, FileMode.Create);
			fs.Close();
		}

		fs = new FileStream(ms_strLogPath, FileMode.Append);
		StreamWriter writer = new StreamWriter(fs);
		string logfrm = DateTime.Now.ToString("(yyyy.MM.dd) [hh:mm:ss.fff]	") + " " + strMsg;
		writer.WriteLine(logfrm);
		writer.Close();
		fs.Close();
#else
#if UNITY_ANDROID && _DEBUG
		string logfrm = DateTime.Now.ToString("(yyyy.MM.dd) [hh:mm:ss.fff]	") + " " + strMsg;
        StreamWriter fileWriter = new StreamWriter(ms_strLogPath, true);
        fileWriter.WriteLine(logfrm);
        fileWriter.Close();
#endif

#if UNITY_IOS && _DEBUG
#endif
#endif
	}
}
