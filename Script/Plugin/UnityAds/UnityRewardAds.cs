using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using UnityEngine;

public class UnityRewardAds : MonoBehaviour, IUnityAdsListener
{
    private static UnityRewardAds m_pInstance = null;
    public static UnityRewardAds Instance { get { return m_pInstance; } }

	private void Awake()
	{
        m_pInstance = this;
        Advertisement.AddListener(this);
        Advertisement.Initialize("4087071", false);
    }

    public void Show()
    {
        StartCoroutine(ShowAdWhenReady());
    }

    IEnumerator ShowAdWhenReady()
    {
        while (!Advertisement.IsReady("rewardedVideo"))
        {
            yield return null;
        }
        Advertisement.Show("rewardedVideo");
    }


    public void OnUnityAdsDidError(string message)
    {
        AppInstance.Instance.m_pEventDelegateManager.OnGoogleRewardsAdsLoadFailed();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            AppInstance.Instance.m_pEventDelegateManager.OnGoogleRewardsAdsComplete();
        }
        else if (showResult == ShowResult.Skipped)
        {
            AppInstance.Instance.m_pEventDelegateManager.OnGoogleRewardsAdsLoadFailed();
        }
        else if (showResult == ShowResult.Failed)
        {
            AppInstance.Instance.m_pEventDelegateManager.OnGoogleRewardsAdsLoadFailed();

            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/MessageBox");
            ob = GameObject.Instantiate(ob);
            MessageBox pMessageBox = ob.GetComponent<MessageBox>();
            pMessageBox.Initialize(MessageBox.eMessageBoxType.OK, "", Helper.GetTextDataString(eTextDataType.RewardAdsLoadFailed_Desc), Helper.GetTextDataString(eTextDataType.OK), "");
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsReady(string placementId)
    {
    }
}
