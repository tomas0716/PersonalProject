using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutGame_UI_Option : MonoBehaviour
{
    private GameObject          m_pGameObject_Parent        = null;
    private Transformer_Scalar  m_pScale                    = new Transformer_Scalar(0);

    private GameObject          m_pGameObject_BG_Mute       = null;
    private GameObject          m_pGameObject_Sound_Mute    = null;

    public string               m_ServiceCenterEmail;   // 고객센터 이메일 주소 ( fantasticaeri@gmail.com )

    void Start()
    {
        m_pGameObject_Parent = Helper.FindChildGameObject(gameObject, "Parent");
        m_pGameObject_Parent.transform.localScale = new Vector3(0, 0, 1);

        GameObject ob;
        Text pText;

        ob = Helper.FindChildGameObject(gameObject, "Button_BG");
        m_pGameObject_BG_Mute = Helper.FindChildGameObject(ob, "Image_Mute");
        ob = Helper.FindChildGameObject(gameObject, "Button_Sound");
        m_pGameObject_Sound_Mute = Helper.FindChildGameObject(ob, "Image_Mute");

        if (AppInstance.Instance.m_pOptionInfo.m_IsBGM_On == true)
        {
            m_pGameObject_BG_Mute.SetActive(false);
        }
        else
        {
            m_pGameObject_BG_Mute.SetActive(true);
        }

        if (AppInstance.Instance.m_pOptionInfo.m_IsEffectSound_On == true)
        {
            m_pGameObject_Sound_Mute.SetActive(false);
        }
        else
        {
            m_pGameObject_Sound_Mute.SetActive(true);
        }

        ob = Helper.FindChildGameObject(gameObject, "Button_Language");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Language);

        ob = Helper.FindChildGameObject(gameObject, "Button_Assessment");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Option_Assessment);

        ob = Helper.FindChildGameObject(gameObject, "Button_Share");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Option_Share);

        ob = Helper.FindChildGameObject(gameObject, "Button_Follow");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Option_Follow);

        ob = Helper.FindChildGameObject(gameObject, "Button_PrivacyStatement");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Option_PrivacyStatement);

        ob = Helper.FindChildGameObject(gameObject, "Button_ServiceCenter ");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.Option_ServiceCenter);

        ob = Helper.FindChildGameObject(gameObject, "Text_ClientVersion");
        pText = ob.GetComponent<Text>();
        pText.text = Application.version;

        ob = Helper.FindChildGameObject(gameObject, "Text_GoogleID");
        pText = ob.GetComponent<Text>();
        pText.text = GooglePlayGamesClient.Instance.UserID;

        TransformerEvent_Scalar eventValue;
        eventValue = new TransformerEvent_Scalar(0.2f, 1.0f);
        m_pScale.AddEvent(eventValue);
        m_pScale.OnPlay();

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

    private void OnDestroy()
    {
        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
    }

    void Update()
    {
        m_pScale.Update(Time.deltaTime);
        float fScale = m_pScale.GetCurScalar();
        m_pGameObject_Parent.transform.localScale = new Vector3(fScale, fScale, 1);
    }

    public void OnButtonClick_Close()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        GameObject.Destroy(gameObject);
    }

    public void OnButtonClick_BG()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        AppInstance.Instance.m_pOptionInfo.m_IsBGM_On = !AppInstance.Instance.m_pOptionInfo.m_IsBGM_On;

        if (AppInstance.Instance.m_pOptionInfo.m_IsBGM_On == true)
        {
            m_pGameObject_BG_Mute.SetActive(false);
        }
        else
        {
            m_pGameObject_BG_Mute.SetActive(true);
        }

        AppInstance.Instance.m_pOptionInfo.Save();
    }

    public void OnButtonClick_Sound()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        AppInstance.Instance.m_pOptionInfo.m_IsEffectSound_On = !AppInstance.Instance.m_pOptionInfo.m_IsEffectSound_On;

        if (AppInstance.Instance.m_pOptionInfo.m_IsEffectSound_On == true)
        {
            m_pGameObject_Sound_Mute.SetActive(false);
        }
        else
        {
            m_pGameObject_Sound_Mute.SetActive(true);
        }

        AppInstance.Instance.m_pOptionInfo.Save();
    }

    public void OnButtonClick_Language()
    {
        GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/Language");
        GameObject.Instantiate(ob);
    }

    public void OnButtonClick_Assessment()              // 평가 하기
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        Application.OpenURL(AppInstance.Instance.m_GooglePlayUrl);
    }

    public void OnButtonClick_Share()                   // 공유 하기
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

#if UNITY_ANDROID && !UNITY_EDITOR
        StartCoroutine(Share());
#endif
    }

    IEnumerator Share()
    {
        yield return new WaitForEndOfFrame();

        string body = AppInstance.Instance.m_GooglePlayUrl + "&showAllReviews=true";

        using (AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent"))
        using (AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent"))
        {
            intentObject.Call<AndroidJavaObject>("setAction", intentObject.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>("putExtra", intentObject.GetStatic<string>("EXTRA_SUBJECT"), Helper.GetTextDataString(eTextDataType.Share_Message));
            intentObject.Call<AndroidJavaObject>("putExtra", intentObject.GetStatic<string>("EXTRA_TEXT"), body);
            using (AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via"))
                currentActivity.Call("startActivity", jChooser);
        }
    }

    public void OnButtonClick_Follow()                  // 팔로우 하기
    {
        Helper.OnSoundPlay(eSoundType.Button, false);
    }

    public void OnButtonClick_PrivacyStatement()        // 개인정보 취급방침
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        Application.OpenURL("https://blog.naver.com/kellygame/222285748069");
    }

    public void OnButtonClick_ServiceCenter()           // 고객센터
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        string subject = EscapeURL(Helper.GetTextDataString(eTextDataType.ServiceCenterMailSubject));
        string body = EscapeURL
            (
             Helper.GetTextDataString(eTextDataType.ServiceCentarMailText) + "\n\n\n\n" +
             "________________________\n\n" +
             "Device Model : " + SystemInfo.deviceModel + "\n\n" +
             "Device OS : " + SystemInfo.operatingSystem + "\n\n" +
             "________________________"
            );

        Application.OpenURL("mailto:" + m_ServiceCenterEmail + "?subject=" + subject + "&body=" + body);
    }

    private string EscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public void OnHardwareBackButtonClick()
    {
        if (GameInfo.Instance.m_IsHardwareBackButtonProcess == false &&
            GameInfo.Instance.m_IsShopOpen == false &&
            GameInfo.Instance.m_IsItemBuyOpen == false &&
            GameInfo.Instance.m_nMessageBoxOpenCount == 0 &&
            GameInfo.Instance.m_nRewardPopupOpenCount == 0)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = true;
            OnButtonClick_Close();
        }
    }
}
