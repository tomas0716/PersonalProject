using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class InGame_GetMissionCanMoveItem : MonoBehaviour
{
    private Vector3             m_v2DPos                = Vector3.zero;

    private Image               m_pImage                = null;

    private Transformer_Vector3 m_pPos                  = null;

    private eUnitType           m_eUnitType             = eUnitType.Empty;
    private float               m_fDelayTime            = 0;

    void Start()
    {
        ++InGameInfo.Instance.m_nGetMissionMoveItemCount;

        GameObject ob;

        ob = Helper.FindChildGameObject(gameObject, "Image");
        m_pImage = ob.GetComponent<Image>();

        SpriteAtlas pSpriteAtlas_Mission = null;

        Vector3 vDestPos = Vector3.zero;

        switch (AppInstance.Instance.m_pSceneManager.GetCurrSceneType())
        {
            case eSceneType.Scene_InGame:
                {
                    Scene_InGame pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_InGame;

                    if (pScene != null)
                    {
                        pSpriteAtlas_Mission = pScene.m_pGameController.m_pMainUI.GetSpriteAtlas_MIssionCan();
                        vDestPos = pScene.m_pGameController.m_pMainUI.GetMissionItemPos(eMissionType.Can);
                    }
                }
                break;

            case eSceneType.Scene_Tool:
                {
                    Scene_Tool pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_Tool;

                    if (pScene != null)
                    {
                        pSpriteAtlas_Mission = pScene.m_pGameController.m_pMainUI.GetSpriteAtlas_MIssionCan();
                        vDestPos = pScene.m_pGameController.m_pMainUI.GetMissionItemPos(eMissionType.Can);
                    }
                }
                break;
        }

        Vector3 vUIPos = Helper.Get3DPosToUIPos(gameObject, Camera.main, m_v2DPos);
        m_pPos = new Transformer_Vector3(vUIPos);

        m_pImage.sprite = pSpriteAtlas_Mission.GetSprite("Can_" + m_eUnitType.ToString() + "_Full");
        m_pImage.SetNativeSize();

        TransformerEvent eventValue;
        eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + m_fDelayTime, vUIPos);
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + m_fDelayTime + 0.4f, vDestPos);
        m_pPos.AddEvent(eventValue);
        m_pPos.SetCallback(null, OnDone_Pos);
        m_pPos.OnPlay();
    }

	private void OnDestroy()
	{
    }

	public void Init(Vector3 v2DPos, eUnitType eType, float fDelayTime)
	{
        m_v2DPos = v2DPos;
        m_eUnitType = eType;
        m_fDelayTime = fDelayTime;
    }

	void Update()
    {
        m_pPos.Update(Time.deltaTime);
        Vector3 vPos = m_pPos.GetCurVector3();
        m_pImage.gameObject.transform.localPosition = vPos;
    }

    private void OnDone_Pos(TransformerEvent eventValue)
    {
        --InGameInfo.Instance.m_nGetMissionMoveItemCount;

        if (SavedGameDataInfo.Instance.m_MissionInfoTable.ContainsKey(eMissionType.Can) == true)
        {
            int nCount = SavedGameDataInfo.Instance.m_MissionInfoTable[eMissionType.Can];
            SavedGameDataInfo.Instance.m_MissionInfoTable[eMissionType.Can] = nCount - 1;
            if (SavedGameDataInfo.Instance.m_MissionInfoTable[eMissionType.Can] < 0)
                SavedGameDataInfo.Instance.m_MissionInfoTable[eMissionType.Can] = 0;

            AppInstance.Instance.m_pEventDelegateManager.OnInGame_GetMission(eMissionType.Can);
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_UpdateMissionCount(eMissionType.Can);
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_GetMissionMoveItem_Done(eMissionType.Can);

            if (InGameInfo.Instance.m_IsChangeSlot == false && InGameInfo.Instance.m_IsSpreadGrass == false && InGameInfo.Instance.m_nGetMissionMoveItemCount == 0)
            {
                InGameInfo.Instance.m_IsInGameClick = true;

                if (InGameInfo.Instance.m_nCurrMoveCount == 5 && InGameInfo.Instance.m_IsMoveWarning == false)
                {
                    InGameInfo.Instance.m_IsMoveWarning = true;
                    GameObject ob = Resources.Load<GameObject>("Gui/Prefabs/InGame_MoveWarning");
                    GameObject.Instantiate(ob);
                }

                AppInstance.Instance.m_pEventDelegateManager.OnInGame_CheckMissionComplete();
            }
        }

        GameObject.Destroy(gameObject);
    }
}
