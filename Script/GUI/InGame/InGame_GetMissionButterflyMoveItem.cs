using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class InGame_GetMissionButterflyMoveItem : MonoBehaviour
{
    private SpriteAtlas         m_pSpriteAtlas_Mission  = null;
    private Vector3             m_v2DPos                = Vector3.zero;

    private Image               m_pImage                = null;

    private Transformer_Bazier4 m_pBazier               = null;

    private eUnitType           m_eUnitType             = eUnitType.Empty;
    private Vector3             m_vPrevPos              = Vector3.zero;

    private Transformer_Timer   m_pTimer_Anim           = new Transformer_Timer(1);

    void Start()
    {
        ++InGameInfo.Instance.m_nGetMissionMoveItemCount;

        GameObject ob;

        ob = Helper.FindChildGameObject(gameObject, "Image");
        m_pImage = ob.GetComponent<Image>();

        Vector3 vDestPos = Vector3.zero;

        switch (AppInstance.Instance.m_pSceneManager.GetCurrSceneType())
        {
            case eSceneType.Scene_InGame:
                {
                    Scene_InGame pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_InGame;

                    if (pScene != null)
                    {
                        m_pSpriteAtlas_Mission = pScene.m_pGameController.m_pMainUI.GetSpriteAtlas_MIssionButterfly();
                        vDestPos = pScene.m_pGameController.m_pMainUI.GetMissionItemPos(eMissionType.Butterfly);
                    }
                }
                break;

            case eSceneType.Scene_Tool:
                {
                    Scene_Tool pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_Tool;

                    if (pScene != null)
                    {
                        m_pSpriteAtlas_Mission = pScene.m_pGameController.m_pMainUI.GetSpriteAtlas_MIssionButterfly();
                        vDestPos = pScene.m_pGameController.m_pMainUI.GetMissionItemPos(eMissionType.Butterfly);
                    }
                }
                break;
        }

        Vector3 vUIPos = Helper.Get3DPosToUIPos(gameObject, Camera.main, m_v2DPos);
        m_vPrevPos = vUIPos;

        m_pImage.sprite = m_pSpriteAtlas_Mission.GetSprite("Butterfly_" + m_eUnitType.ToString() + "_Anim_01");
        m_pImage.SetNativeSize();

        Vector3 vSecondPos = new Vector3(0, 120, 0);

        float fDistance = Vector3.Distance(vUIPos, vDestPos);
        //float fTime = (fDistance / 160.0f) * 0.3f * (1.0f / AppInstance.Instance.m_fMainScale);
        float fTime = (fDistance / 160.0f) * 0.3f;

        TransformerEvent eventValue;

        m_pBazier = new Transformer_Bazier4(vUIPos);
        eventValue = new TransformerEvent_Vector3(0, vUIPos + vSecondPos);
        m_pBazier.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(0, vUIPos + vSecondPos);
        m_pBazier.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(fTime, vDestPos);
        m_pBazier.AddEvent(eventValue);
        m_pBazier.SetCallback(null, OnDone_Pos);
        m_pBazier.OnPlay();
        
        float fAnimUnitTime = 0.03f;
        eventValue =new TransformerEvent_Timer(fAnimUnitTime, 2);
        m_pTimer_Anim.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fAnimUnitTime * 2, 3);
        m_pTimer_Anim.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fAnimUnitTime * 3, 4);
        m_pTimer_Anim.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fAnimUnitTime * 4, 3);
        m_pTimer_Anim.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fAnimUnitTime * 5, 2);
        m_pTimer_Anim.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fAnimUnitTime * 6, 1);
        m_pTimer_Anim.AddEvent(eventValue);
        eventValue = new TransformerEvent_Timer(fAnimUnitTime * 7, 1);
        m_pTimer_Anim.AddEvent(eventValue);
        m_pTimer_Anim.SetLoop(true);
        m_pTimer_Anim.SetCallback(OnOneEventDone_Timer_Anim, OnDone_Timer_Anim);
        m_pTimer_Anim.OnPlay();
    }

	private void OnDestroy()
	{
    }

	public void Init(Vector3 v2DPos, eUnitType eType)
	{
        m_v2DPos = v2DPos;
        m_eUnitType = eType;
    }

	void Update()
    {
        m_pBazier.Update(Time.deltaTime);
        Vector3 vPos = m_pBazier.GetCurVector3();
        m_pImage.gameObject.transform.localPosition = vPos;

        float fAngle = Quaternion.FromToRotation(Vector3.up, vPos - m_vPrevPos).eulerAngles.z;
        m_pImage.gameObject.transform.localEulerAngles = new Vector3(0,0, fAngle);

        m_vPrevPos = vPos;

        m_pTimer_Anim.Update(Time.deltaTime);
    }

    private void OnOneEventDone_Timer_Anim(int nIndex, TransformerEvent eventValue)
    {
        int nAnimIndex = (int)eventValue.m_pParameta;
        m_pImage.sprite = m_pSpriteAtlas_Mission.GetSprite("Butterfly_" + m_eUnitType.ToString() + "_Anim_0" + nAnimIndex.ToString());
    }

    private void OnDone_Timer_Anim(TransformerEvent eventValue)
    {
        int nAnimIndex = (int)eventValue.m_pParameta;
        m_pImage.sprite = m_pSpriteAtlas_Mission.GetSprite("Butterfly_" + m_eUnitType.ToString() + "_Anim_0" + nAnimIndex.ToString());
    }

    private void OnDone_Pos(TransformerEvent eventValue)
    {
        --InGameInfo.Instance.m_nGetMissionMoveItemCount;

        if (SavedGameDataInfo.Instance.m_MissionInfoTable.ContainsKey(eMissionType.Butterfly) == true)
        {
            int nCount = SavedGameDataInfo.Instance.m_MissionInfoTable[eMissionType.Butterfly];
            SavedGameDataInfo.Instance.m_MissionInfoTable[eMissionType.Butterfly] = nCount - 1;
            if (SavedGameDataInfo.Instance.m_MissionInfoTable[eMissionType.Butterfly] < 0)
                SavedGameDataInfo.Instance.m_MissionInfoTable[eMissionType.Butterfly] = 0;

            AppInstance.Instance.m_pEventDelegateManager.OnInGame_GetMission(eMissionType.Butterfly);
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_UpdateMissionCount(eMissionType.Butterfly);
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_GetMissionMoveItem_Done(eMissionType.Butterfly);

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
