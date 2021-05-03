using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class InGame_GetMissionMoveItem : MonoBehaviour
{
    private eMissionType        m_eMissionType  = eMissionType.None;
    private Vector3             m_v2DPos        = Vector3.zero;

    private Image               m_pImage        = null;

    private Transformer_Vector3 m_pPos          = null;
    private Transformer_Scalar  m_pScale        = new Transformer_Scalar(1);

    private eUnitType           m_eUnitType     = eUnitType.Empty;

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
                        pSpriteAtlas_Mission = pScene.m_pGameController.m_pMainUI.GetSpriteAtlas_MIssion();
                        vDestPos = pScene.m_pGameController.m_pMainUI.GetMissionItemPos(m_eMissionType);
                    }
                }
                break;

            case eSceneType.Scene_Tool:
                {
                    Scene_Tool pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_Tool;

                    if (pScene != null)
                    {
                        pSpriteAtlas_Mission = pScene.m_pGameController.m_pMainUI.GetSpriteAtlas_MIssion();
                        vDestPos = pScene.m_pGameController.m_pMainUI.GetMissionItemPos(m_eMissionType);
                    }
                }
                break;
        }

        if (pSpriteAtlas_Mission != null)
        {
            ExcelData_MissionDataInfo pMissionDataInfo = ExcelDataManager.Instance.m_pExcelData_MissionData.GetMissionDataInfo(m_eMissionType);

            switch (m_eMissionType)
            {
                case eMissionType.Jelly:
                    {
                        m_pImage.sprite = pSpriteAtlas_Mission.GetSprite(pMissionDataInfo.m_strMissionImageFileName + m_eUnitType.ToString());
                        m_pImage.SetNativeSize();
                    }
                    break;

                default:
                    {
                        m_pImage.sprite = pSpriteAtlas_Mission.GetSprite(pMissionDataInfo.m_strMissionImageFileName);
                        m_pImage.SetNativeSize();
                    }
                    break;
            }
        }

        Vector3 vUIPos = Helper.Get3DPosToUIPos(gameObject, Camera.main, m_v2DPos);
        m_pPos = new Transformer_Vector3(vUIPos);
        m_pImage.gameObject.transform.localPosition = vUIPos;

        float fDistance = Vector3.Distance(vUIPos, vDestPos);
        //float fTime = (fDistance / 160.0f) * 0.12f * (1.0f / AppInstance.Instance.m_fMainScale);
        float fTime = (fDistance / 160.0f) * 0.12f;

        TransformerEvent eventValue;

        switch (m_eMissionType)
        {
            case eMissionType.Bell:
                {
                    Helper.OnSoundPlay(eSoundType.Bell, false);

                    eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + GameDefine.ms_fGetMissionDirectionTime_Bell, vUIPos);
                    m_pPos.AddEvent(eventValue);
                    eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + GameDefine.ms_fGetMissionDirectionTime_Bell + fTime, vDestPos);
                    m_pPos.AddEvent(eventValue);
                    m_pPos.SetCallback(null, OnDone_Pos);
                    m_pPos.OnPlay();
                }
                break;

            case eMissionType.Mouse:
                {
                    eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + GameDefine.ms_fGetMissionDirectionTime_Mouse, vUIPos);
                    m_pPos.AddEvent(eventValue);
                    eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + GameDefine.ms_fGetMissionDirectionTime_Mouse + fTime, vDestPos);
                    m_pPos.AddEvent(eventValue);
                    m_pPos.SetCallback(null, OnDone_Pos);
                    m_pPos.OnPlay();
                }
                break;

            case eMissionType.Apple:
                {
                    Helper.OnSoundPlay(eSoundType.Apple, false);

                    eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + GameDefine.ms_fGetMissionDirectionTime_Apple, vUIPos);
                    m_pPos.AddEvent(eventValue);
                    eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + GameDefine.ms_fGetMissionDirectionTime_Apple + fTime, vDestPos);
                    m_pPos.AddEvent(eventValue);
                    m_pPos.SetCallback(null, OnDone_Pos);
                    m_pPos.OnPlay();

                    m_pScale.OnReset();
                    eventValue = new TransformerEvent_Scalar(0,0.85f);
                    m_pScale.AddEvent(eventValue);
                    m_pScale.OnPlay();
                }
                break;

            case eMissionType.Rock:
                {
                    eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + GameDefine.ms_fGetMissionDirectionTime_Rock, vUIPos);
                    m_pPos.AddEvent(eventValue);
                    eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + GameDefine.ms_fGetMissionDirectionTime_Rock + fTime, vDestPos);
                    m_pPos.AddEvent(eventValue);
                    m_pPos.SetCallback(null, OnDone_Pos);
                    m_pPos.OnPlay();

                    m_pScale.OnReset();
                    eventValue = new TransformerEvent_Scalar(0, 0.9f);
                    m_pScale.AddEvent(eventValue);
                    m_pScale.OnPlay();
                }
                break;

            case eMissionType.Jelly:
                {
                    eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + GameDefine.ms_fGetMissionDirectionTime_Jelly, vUIPos);
                    m_pPos.AddEvent(eventValue);
                    eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + GameDefine.ms_fGetMissionDirectionTime_Jelly + fTime, vDestPos);
                    m_pPos.AddEvent(eventValue);
                    m_pPos.SetCallback(null, OnDone_Pos);
                    m_pPos.OnPlay();
                }
                break;
        }

        Update();
    }

	private void OnDestroy()
	{
    }

	public void Init(eMissionType eMissionType, Vector3 v2DPos)
	{
        m_eMissionType = eMissionType;
        m_v2DPos = v2DPos;
    }

	public void Init(eMissionType eMissionType, Vector3 v2DPos, eUnitType eType)
    {
        m_eMissionType = eMissionType;
        m_v2DPos = v2DPos;
        m_eUnitType = eType;
    }

	void Update()
    {
        m_pPos.Update(Time.deltaTime);
        Vector3 vPos = m_pPos.GetCurVector3();
        m_pImage.gameObject.transform.localPosition = vPos;

        m_pScale.Update(Time.deltaTime);
        float fScale = m_pScale.GetCurScalar();
        m_pImage.gameObject.transform.localScale = new Vector3(fScale, fScale, 1);
    }

    private void OnDone_Pos(TransformerEvent eventValue)
    {
        --InGameInfo.Instance.m_nGetMissionMoveItemCount;

        if (SavedGameDataInfo.Instance.m_MissionInfoTable.ContainsKey(m_eMissionType) == true)
        {
            int nCount = SavedGameDataInfo.Instance.m_MissionInfoTable[m_eMissionType];
            SavedGameDataInfo.Instance.m_MissionInfoTable[m_eMissionType] = nCount - 1;
            if (SavedGameDataInfo.Instance.m_MissionInfoTable[m_eMissionType] < 0)
                SavedGameDataInfo.Instance.m_MissionInfoTable[m_eMissionType] = 0;

            AppInstance.Instance.m_pEventDelegateManager.OnInGame_GetMission(m_eMissionType);
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_UpdateMissionCount(m_eMissionType);
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_GetMissionMoveItem_Done(m_eMissionType);

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
