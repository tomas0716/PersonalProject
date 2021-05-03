using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFPSDisplay : MonoBehaviour
{
	float deltaTime = 0.0f;

	void Update()
	{
#if _DEBUG
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
#endif
	}

	void OnGUI()
	{
#if _DEBUG
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, h - h * 2 / 100, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label(rect, text, style);
#endif
	}
}
