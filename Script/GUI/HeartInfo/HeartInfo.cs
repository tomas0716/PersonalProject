using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartInfo : MonoBehaviour
{
    private Text    m_pText_Heart       = null;
    private Text    m_pText_HeartTiemr  = null;

    void Start()
    {
        GameObject ob;

        ob = Helper.FindChildGameObject(gameObject, "Text_Heart");
        m_pText_Heart = ob.GetComponent<Text>();
        m_pText_Heart.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart].ToString();

        ob = Helper.FindChildGameObject(gameObject, "Text_Timer");
        m_pText_HeartTiemr = ob.GetComponent<Text>();
        m_pText_HeartTiemr.text = "";

        AppInstance.Instance.m_pEventDelegateManager.OnEventUpdateHeartInfo += OnUpdateHeartInfo;
    }

	private void OnDestroy()
	{
        AppInstance.Instance.m_pEventDelegateManager.OnEventUpdateHeartInfo -= OnUpdateHeartInfo;
    }

	void Update()
    {
        if (HeartChargeTimer.Instance.IsFull() == true || SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] >= GameDefine.ms_nMaxAutoChargeHeart)
        {
            m_pText_HeartTiemr.text = "MAX";
        }
        else
        {
            float fRenameTime = (float)HeartChargeTimer.Instance.GetAutoChargeTimerSecond();
            int nMinute = (int)(fRenameTime / 60.0f);
            int nSecond = (int)(((int)fRenameTime) % 60);

            m_pText_HeartTiemr.text = string.Format("{0}:{1}", nMinute.ToString("D2"), nSecond.ToString("D2"));
        }
    }

    public void OnUpdateHeartInfo()
    {
        m_pText_Heart.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart].ToString();
    }

    public void OnButtonClick_Add()
    {
        if (GameInfo.Instance.m_IsShopOpen == false)
        {
            if (SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Heart] == 0)
            {
                Helper.OnSoundPlay(eSoundType.Button, false);

                if (SavedGameDataInfo.Instance.m_byGetFreeHeartCountForAds == GameDefine.ms_nHeartFree_Max)
                {
                    GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/HeartCharge_01");
                    GameObject.Instantiate(ob);
                }
                else
                {
                    GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/HeartCharge_02");
                    GameObject.Instantiate(ob);
                }
            }
            else
            {
                GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/HeartCharge_01");
                GameObject.Instantiate(ob);
            }
        }
    }
}
