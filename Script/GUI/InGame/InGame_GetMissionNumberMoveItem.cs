using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class InGame_GetMissionNumberMoveItem : MonoBehaviour
{
    private Vector3             m_v2DPos                = Vector3.zero;

    private Image               m_pImage                = null;

    private GameObject          m_pGameObject_Number    = null;
    private GameObject          m_pGameObject_Number_In = null;

    private Transformer_Vector3 m_pPos                  = null;
    private Transformer_Scalar  m_pScale                = new Transformer_Scalar(1);

    private eUnitType           m_eUnitType             = eUnitType.Empty;
    private int                 m_nNumber               = 0;

    void Start()
    {
        ++InGameInfo.Instance.m_nGetMissionMoveItemCount;

        GameObject ob;

        ob = Helper.FindChildGameObject(gameObject, "Image");
        m_pImage = ob.GetComponent<Image>();

        m_pGameObject_Number = Helper.FindChildGameObject(gameObject, "Number");
        m_pGameObject_Number_In = Helper.FindChildGameObject(gameObject, "Number_In");

        SpriteAtlas pSpriteAtlas_Mission = null;

        Vector3 vDestPos = Vector3.zero;

        switch (AppInstance.Instance.m_pSceneManager.GetCurrSceneType())
        {
            case eSceneType.Scene_InGame:
                {
                    Scene_InGame pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_InGame;

                    if (pScene != null)
                    {
                        pSpriteAtlas_Mission = pScene.m_pGameController.m_pMainUI.GetSpriteAtlas_MIssionNumber();
                        vDestPos = pScene.m_pGameController.m_pMainUI.GetMissionItemPos(eMissionType.Number);
                    }
                }
                break;

            case eSceneType.Scene_Tool:
                {
                    Scene_Tool pScene = AppInstance.Instance.m_pSceneManager.GetCurrScene() as Scene_Tool;

                    if (pScene != null)
                    {
                        pSpriteAtlas_Mission = pScene.m_pGameController.m_pMainUI.GetSpriteAtlas_MIssionNumber();
                        vDestPos = pScene.m_pGameController.m_pMainUI.GetMissionItemPos(eMissionType.Number);
                    }
                }
                break;
        }

        Vector3 vUIPos = Helper.Get3DPosToUIPos(gameObject, Camera.main, m_v2DPos);
        m_pPos = new Transformer_Vector3(vUIPos);

        ExcelData_MissionDataInfo pMissionDataInfo = ExcelDataManager.Instance.m_pExcelData_MissionData.GetMissionDataInfo(eMissionType.Number);

        m_pImage.sprite = pSpriteAtlas_Mission.GetSprite("Number_" + m_eUnitType.ToString());
        m_pImage.SetNativeSize();

        List<Sprite> numberSpriteList = new List<Sprite>();

        for (int i = 0; i < 10; ++i)
        {
            Sprite pSprite = pSpriteAtlas_Mission.GetSprite("Number_0" + i.ToString());
            numberSpriteList.Add(pSprite);
        }

        UINumber pUINumber = m_pGameObject_Number.AddComponent<UINumber>();
        pUINumber.Initialize("Number", numberSpriteList, m_nNumber, (int)numberSpriteList[0].rect.height, false, false, false, -6, 0, eScreenAnchorPosition.MiddleCenter);

        numberSpriteList.Clear();
        for (int i = 0; i < 10; ++i)
        {
            Sprite pSprite = pSpriteAtlas_Mission.GetSprite("Number_In_0" + i.ToString());
            numberSpriteList.Add(pSprite);
        }

        pUINumber = m_pGameObject_Number_In.AddComponent<UINumber>();
        pUINumber.Initialize("Number", numberSpriteList, m_nNumber, (int)numberSpriteList[0].rect.height, false, false, false, -6, 0, eScreenAnchorPosition.MiddleCenter);
        pUINumber.SetColor(GameDefine.ms_clrNumberMission[(int)m_eUnitType]);

        TransformerEvent eventValue;
        eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + GameDefine.ms_fGetMissionDirectionTime_Number, vUIPos);
        m_pPos.AddEvent(eventValue);
        eventValue = new TransformerEvent_Vector3(GameDefine.ms_fMoveAndCreateTime + GameDefine.ms_fGetMissionDirectionTime_Number + 0.4f, vDestPos);
        m_pPos.AddEvent(eventValue);
        m_pPos.SetCallback(null, OnDone_Pos);
        m_pPos.OnPlay();
    }

	private void OnDestroy()
	{
    }

	public void Init(Vector3 v2DPos, eUnitType eType, int nNumber)
	{
        m_v2DPos = v2DPos;
        m_eUnitType = eType;
        m_nNumber = nNumber;
    }

	void Update()
    {
        m_pPos.Update(Time.deltaTime);
        Vector3 vPos = m_pPos.GetCurVector3();
        m_pImage.gameObject.transform.localPosition = vPos;
        m_pGameObject_Number.transform.localPosition = vPos;
        m_pGameObject_Number_In.transform.localPosition = vPos;
    }

    private void OnDone_Pos(TransformerEvent eventValue)
    {
        --InGameInfo.Instance.m_nGetMissionMoveItemCount;

        if (SavedGameDataInfo.Instance.m_MissionInfoTable.ContainsKey(eMissionType.Number) == true)
        {
            int nCount = SavedGameDataInfo.Instance.m_MissionInfoTable[eMissionType.Number];
            SavedGameDataInfo.Instance.m_MissionInfoTable[eMissionType.Number] = nCount - 1;
            if (SavedGameDataInfo.Instance.m_MissionInfoTable[eMissionType.Number] < 0)
                SavedGameDataInfo.Instance.m_MissionInfoTable[eMissionType.Number] = 0;

            AppInstance.Instance.m_pEventDelegateManager.OnInGame_GetMission(eMissionType.Number);
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_UpdateMissionCount(eMissionType.Number);
            AppInstance.Instance.m_pEventDelegateManager.OnInGame_GetMissionMoveItem_Done(eMissionType.Number);

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
