using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI_Option : MonoBehaviour
{
    private GameObject m_pGameObject_BG_Mute        = null;
    private GameObject m_pGameObject_Sound_Mute     = null;

    void Start()
    {
        GameInfo.Instance.m_IsInGame_Option = true;

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

        ob = Helper.FindChildGameObject(gameObject, "Button_Stop");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.InGame_Option_Stop);

        ob = Helper.FindChildGameObject(gameObject, "Button_Continue");
        ob = Helper.FindChildGameObject(ob, "Text");
        pText = ob.GetComponent<Text>();
        pText.text = Helper.GetTextDataString(eTextDataType.InGame_Option_Continue);

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick += OnHardwareBackButtonClick;
    }

    private void OnDestroy()
    {
        GameInfo.Instance.m_IsInGame_Option = false;

        AppInstance.Instance.m_pEventDelegateManager.OnEventHardwareBackButtonClick -= OnHardwareBackButtonClick;
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

    public void OnButtonClick_Stop()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);
        AppInstance.Instance.m_pSoundPlayer.StopBKG();

        SavedGameDataInfo.Instance.OnMissionFail();

        GameInfo.Instance.m_IsInGameEnter = false;
        AppInstance.Instance.m_pEventDelegateManager.OnCreateInGameLoading();
    }

    public void OnButtonClick_Continue()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        InGameInfo.Instance.m_IsInGameClick = true;

        GameObject.Destroy(gameObject);
    }


    public void OnHardwareBackButtonClick()
    {
        if (GameInfo.Instance.m_IsHardwareBackButtonProcess == false &&
            GameInfo.Instance.m_IsShopOpen == false &&
            GameInfo.Instance.m_IsItemBuyOpen == false &&
            GameInfo.Instance.m_nMessageBoxOpenCount == 0 &&
            GameInfo.Instance.m_nRewardPopupOpenCount == 0 &&
            GameInfo.Instance.m_IsInGame_UseItemTooltip == false &&
            GameInfo.Instance.m_IsInGame_MissionGuide == false &&
            GameInfo.Instance.m_IsInGame_MissionIntro == false &&
            GameInfo.Instance.m_IsInGame_Tutorial == false)
        {
            GameInfo.Instance.m_IsHardwareBackButtonProcess = true;
            OnButtonClick_Continue();
        }
    }
}
