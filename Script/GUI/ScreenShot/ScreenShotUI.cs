using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotUI : MonoBehaviour
{
    private CanvasScaler m_pCanvasScaler = null;
    private Image m_pImage_Back = null;
    private Image m_pImage_ScreenShot = null;

    void Start()
    {
        m_pCanvasScaler = gameObject.GetComponent<CanvasScaler>();
        m_pCanvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);

        GameObject ob;
        ob = Helper.FindChildGameObject(gameObject, "Image_Back");
        m_pImage_Back = ob.GetComponent<Image>();
        m_pImage_Back.gameObject.SetActive(false);

        ob = Helper.FindChildGameObject(gameObject, "Image");
        m_pImage_ScreenShot = ob.GetComponent<Image>();
        m_pImage_ScreenShot.gameObject.SetActive(false);
        ScreenShotter.Instance.OnScreenShot();

        AppInstance.Instance.m_pEventDelegateManager.OnEventScreenShot += OnScreenShot;
        AppInstance.Instance.m_pEventDelegateManager.OnEventDestroyScreenShot += OnDestroyScreenShot;
    }

	private void OnDestroy()
	{
        AppInstance.Instance.m_pEventDelegateManager.OnEventScreenShot -= OnScreenShot;
        AppInstance.Instance.m_pEventDelegateManager.OnEventDestroyScreenShot -= OnDestroyScreenShot;
    }

    public void OnScreenShot(Texture2D pTex)
    {
        m_pImage_Back.gameObject.SetActive(true);

        Sprite pSprite = Sprite.Create(pTex, new Rect(0,0,pTex.width, pTex.height), new Vector2(0.5f, 0.5f), 100.0f);
        m_pImage_ScreenShot.sprite = pSprite;
        m_pImage_ScreenShot.SetNativeSize();
        m_pImage_ScreenShot.gameObject.SetActive(true);
    }

    public void OnDestroyScreenShot()
    {
        GameObject.Destroy(gameObject);
    }
}
