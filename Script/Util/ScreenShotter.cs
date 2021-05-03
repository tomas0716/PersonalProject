using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotter : MonoBehaviour
{
    private static ScreenShotter m_pInstance = null;
    public static ScreenShotter Instance { get { return m_pInstance; } }

    private Texture2D   m_pTexture = null;

    void Awake()
    {
        m_pInstance = this;
    }

    IEnumerator RecordFrame()
    {
        if (m_pTexture != null)
        {
            Object.Destroy(m_pTexture);
        }

        yield return new WaitForEndOfFrame();
        m_pTexture = ScreenCapture.CaptureScreenshotAsTexture();

        AppInstance.Instance.m_pEventDelegateManager.OnScreenShot(m_pTexture);
    }

    public void OnScreenShot()
    {
        if (m_pTexture != null)
        {
            Object.Destroy(m_pTexture);
        }

        StartCoroutine(RecordFrame());
    }
}