using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinInfo : MonoBehaviour
{
    private Text    m_pText_Coin    = null;

    void Start()
    {
        GameObject ob;

        ob = Helper.FindChildGameObject(gameObject, "Text_Coin");
        m_pText_Coin = ob.GetComponent<Text>();
        m_pText_Coin.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Coin].ToString();

        AppInstance.Instance.m_pEventDelegateManager.OnEventUpdateCoinInfo += OnUpdateCoinInfo;
    }

	private void OnDestroy()
	{
        AppInstance.Instance.m_pEventDelegateManager.OnEventUpdateCoinInfo -= OnUpdateCoinInfo;
    }

	void Update()
    {
        
    }

    public void OnUpdateCoinInfo()
    {
        m_pText_Coin.text = SavedGameDataInfo.Instance.m_nItemCounts[(int)eItemType.Coin].ToString();
    }

    public void OnButtonClick_Add()
    {
        Helper.OnSoundPlay(eSoundType.Button, false);

        if (GameInfo.Instance.m_IsShopOpen == false)
        {
            GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/Shop");
            GameObject.Instantiate(ob);
        }
    }
}
