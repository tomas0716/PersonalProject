using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
//public class App : MonoBehaviour
//{

//    public GPGSManager gpgsManager;
//    public Text txtUserName;

//    public Button btnAch1;
//    public Button btnAch2;
//    public Button btnStartGame;
//    public Text txtAch1;
//    public Text txtAch2;
//    public GameObject uiTitle;
//    public GameObject uiGame;
//    public Button btnAchievement;

//    private int killNormalMonsterCnt;
//    private int killEliteMonsterCnt;
//    private int achievementKillMonsterStep;



//    private void Awake()
//    {
//        this.uiTitle.SetActive(false);
//        this.uiGame.SetActive(false);

//    }
//    // Use this for initialization
//    void Start()
//    {
//        this.gpgsManager.Init();

//        this.gpgsManager.SignIn((result) => {
//            if (result)
//            {
//                this.txtUserName.text = Social.localUser.userName;

//                this.uiTitle.gameObject.SetActive(true);
//            }
//            else
//            {
//                this.txtUserName.text = "로그인 실패";
//            }
//        });

//        this.btnAch1.onClick.AddListener(() => {
//            //일반몬스터 처치 

//            this.killNormalMonsterCnt += 1;
//            this.txtAch1.text = this.killNormalMonsterCnt.ToString();

//            this.UpdateAchievement();
//        });

//        this.btnAch2.onClick.AddListener(() => {
//            //정예몬스터 처치 

//            this.killEliteMonsterCnt += 1;
//            this.txtAch2.text = this.killEliteMonsterCnt.ToString();

//            this.UpdateAchievement();

//        });

//        this.btnStartGame.onClick.AddListener(() => {
//            Social.ReportProgress(GPGSIds.achievement, 100.0f, (success) =>
//            {
//                Debug.LogFormat("success{0}", success);
//            });

//            this.uiTitle.SetActive(false);
//            this.uiGame.SetActive(true);
//        });

//        this.btnAchievement.onClick.AddListener(() => {
//            Social.ShowAchievementsUI();
//        });

//    }


//    public static class GPGSIds
//    {
//        //일반 몬스터 10마리 처치 
//        public const string achievement___10 = "CgkIkL-Tu-4XEAIQAg"; // <GPGSID>

//        //정예몬스터 10마리 처치 
//        public const string achievement___10_2 = "CgkIkL-Tu-4XEAIQBQ"; // <GPGSID>

//        //몬스터 사냥꾼 
//        public const string achievement_2 = "CgkIkL-Tu-4XEAIQBA"; // <GPGSID>

//        //게임의 시작 
//        public const string achievement = "CgkIkL-Tu-4XEAIQAw"; // <GPGSID>

//    }

//    private void UpdateAchievement()
//    {
//        var ach = PlayGamesPlatform.Instance.GetAchievement(GPGSIds.achievement_2);
//        Debug.LogFormat("{0}, {1}, {2}, {3}, {4}, {5}, {6}",
//            this.achievementKillMonsterStep,
//            ach.CurrentSteps,
//            ach.TotalSteps,
//            ach.IsIncremental,
//            ach.IsRevealed,
//            ach.IsUnlocked,
//            ach.Points);

//        if (this.killNormalMonsterCnt == 10)
//        {
//            Debug.LogFormat("killNormalMonsterCnt: {0}", killNormalMonsterCnt);

//            achievementKillMonsterStep += 1;

//            Debug.LogFormat("achievementKillMonsterStep: {0}", achievementKillMonsterStep);

//            Social.ReportProgress(GPGSIds.achievement___10, 100.0f, (success) =>
//            {
//                Debug.LogFormat("ReportProgress : {0}", success);
//            });

//            if (PlayGamesPlatform.Instance.GetAchievement(GPGSIds.achievement_2).CurrentSteps < 2)
//            {
//                PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_2, this.achievementKillMonsterStep, (success) => {
//                    Debug.Log("IncrementAchievement: {0}" + success);
//                    Debug.Log(success);
//                });
//            }

//        }
//        else if (this.killEliteMonsterCnt == 2)
//        {
//            Debug.LogFormat("killEliteMonsterCnt: {0}", killEliteMonsterCnt);

//            achievementKillMonsterStep += 1;

//            Debug.LogFormat("achievementKillMonsterStep: {0}", achievementKillMonsterStep);

//            Social.ReportProgress(GPGSIds.achievement___10_2, 100.0f, (success) =>
//            {
//                Debug.LogFormat("ReportProgress: {0}", success);
//            });

//            if (PlayGamesPlatform.Instance.GetAchievement(GPGSIds.achievement_2).CurrentSteps < 2)
//            {
//                PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_2, this.achievementKillMonsterStep, (success) => {
//                    Debug.Log("IncrementAchievement: {0}" + success);
//                });
//            }

//        }

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}

