using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eTextDataType
{
	Client_New_Ver							= 1,
	HighScore_Title							= 2,		// 최고점수
	GameStart								= 3,		// 게임시작
	NewGame_MissionChange					= 4,		// 미션바꾸기
	NewGame_MissionChange_Desc				= 5,		// 미션바꾸기 설명
	NewGame_Item_Move3						= 6,        // Item Move * 3
	NewGame_Item_Move_3_Desc				= 7,        // Item Move * 3 설명
	NewGame_Item_RainbowCat					= 8,        // Item Magician
	NewGame_Item_RainbowCat_Desc			= 9,        // Item Magician 설명
	NewGame_Item_StripeToeJelly				= 10,       // item 특수 유닛
	NewGame_Item_StripeToeJelly_Desc		= 11,		// item 특수 유닛 설명
	NewGame_Item_Movie						= 12,       // Item 광고 시청
	NewGame_Item_Movie_Desc					= 13,       // Item 광고 시청 설명
	InGame_Item_Swap						= 14,       // item 위치 교환
	InGame_Item_Swap_Desc					= 15,       // Item 위치 교환 설명
	InGame_Item_CatPunch					= 16,       // Item 정밀타격
	InGame_Item_CatPunch_Desc				= 17,       // Item 정밀타격 설명
	InGame_Item_RunningCat					= 18,       // Item 가로세로 타격
	InGame_Item_RunningCat_Desc				= 19,		// Item 가로세로 타격 설명
	InGame_Continue							= 20,       // 게임 실패시 이어하기
	InGame_Continue_Desc					= 21,       // 게임 실패시 이어하기	설명
	InGame_Continue_Title					= 22,		// 아쉬워요!
	InGame_Continue_Title_Desc				= 23,       // 아이템을 사용해 레벨을 완료하세요.

	InGame_Result_Success_GetScore			= 24,       // 인게임 성공 결과 "획득점수"
	InGame_Result_Success_TotalScore		= 25,       // 인게임 성공 결과 "최종 누적 점수"
	InGame_Result_Success_Ranking			= 26,       // 인게임 성공 결과 "랭킹"

	OK										= 27,		// 확인
	Cancel									= 28,		// 취소

	InGame_MoveCountZero					= 29,		// 이동 횟수가 바닥 났어요!
	InGame_Result_Fail_01					= 30,		// 미션 실패!
	InGame_Result_Fail_02					= 31,		// 점수를 획득하지 못했습니다.

	InGame_Option_Stop						= 32,       // 그만하기
	InGame_Option_Continue					= 33,       // 계속하기

	Shop_Title								= 34,		// 상점
	Shop_Item_Ads_Desc						= 35,		// 광고 시청후 코인 얻기 (7/7)

	Shop_FreeCoin_Receive_Title				= 36,		// 무료 보상
	Shop_FreeCoin_Receive_Desc				= 37,		// 무료 보상으로\n코인 {0}개를 받았습니다.

	RewardAdsLoadFailed_Desc				= 38,       // 보상형 광고를 로드하지 못하였습니다.\n잠시후 다시 시도해 주시기 바랍니다.

	Shop_RewardAdsCoin_Receive_Title		= 39,       // 보상
	Shop_RewardAdsCoin_Receive_Desc			= 40,       // 보상으로 코인 {0}개를 받았습니다.

	Shop_IAPBuyFailed_Title					= 41,       // 알림
	Shop_IAPBuyFailed_Desc					= 42,       // 상품 구매에 실패 하였습니다.\n나중에 다시 시도해 주세요.

	Shop_IAPBuySuccess_Title				= 43,       // 결제 성공
	Shop_IAPBuySuccess_Single_Desc			= 44,		// 코인 {0}개 구매를 성공적으로\n완료 하였습니다.
	Shop_IAPBuySuccess_PackageDesc			= 45,       // {0} 상품 구매를\n성공적으로 완료 하였습니다.

	Option_Assessment						= 46,       // 평가 하기
	Option_Share							= 47,		// 공유 하기
	Option_Follow							= 48,		// 팔로우 하기
	Option_PrivacyStatement					= 49,		// 개인정보 취급방침
	Option_ServiceCenter					= 50,		// 고객센터
	Share_Message							= 51,		// 공유하기 시 전달할 메세지
	ServiceCenterMailSubject				= 52,		// 문의 하기 메일 제목
	ServiceCentarMailText					= 53,		// 문의 하기 메일 말머리 ( 이 곳에 내용을 작성해 주세요. )

	Error									= 54,		// 에러
	InValid_Network_Desc					= 55,		// 네트워크 연결이\n 원활하지 않습니다.\n네트워크 상태를 확인해 주세요.
	InValid_Network_ReExecute				= 56,		// 앱재실행
	InValid_Network_Retry					= 57,		// 다시시도

	InValid_Internet_Desc					= 58,		// 인터넷이 연결되어 있지 않습니다.\n 인터넷 상태를 확인해 주세요.

	MissionChange_Msg_Desc					= 59,       // 현재 레벨의 미션을\n다른 미션으로 바꿔줍니다.\n스코어는 그대로 유지됩니다!

	InGame_Item_ChangeUnit_Tooltip_Desc		= 60,       // 맞닿아있는 블록 두 개의 위치를 서로 바꾸어줘요.
	InGame_Item_SlotAttack_Tooltip_Desc		= 61,		// 고양이 펀치로 블록 한개를 없애줘요.
	InGame_Item_StripeHV_Tooltip_Desc		= 62,		// 가로세로 공격해줘요.

	AchievementReward_Coin					= 63,		// 업적 보상으로 {0}코인을 획득했어요.
	AchievementReward_Item					= 64,		// 업적 보상으로 {1} {2}개를 획득했어요.

	Achievement_Button						= 65,		// 업적
	Leaderboard_Button						= 66,		// 랭킹
	Shop_Button								= 67,		// 상점
	InGame_TargetRankerPassMsg				= 68,       // 레벨 클리어시 스코어 랭킹 {0}등인 {1}님을 역전해요.

	InGame_UnitShuffle_01					= 69,		// 움직일 수 있는 블록이 없어
	InGame_UnitShuffle_02					= 70,       // 블록을 모두 섞어줍니다.

	HeartCharge_ItemName					= 71,		// 하트 충전	
	HeartCharge_ItemDesc					= 72,       // 하트를 5개 추가합니다.
	HeartCharge_RewardAds					= 73,       // 광고 시청후 하트 얻기
	Heart									= 74,
	Coin									= 75, 

	Roulette_Button							= 76,
	Attendance_Button						= 77,
	Ranking									= 78,

	Attendance_Title						= 79,		// 출석부
	Attendance_Day							= 80,		// 일
	Attendance_Desc							= 81,		// 매일 플레이하고 선물 받으세요! \n7일이 지나면 자동 갱신됩니다.
	Attendance_Recieve						= 82,		// 출석체크
	Attendance_1Day_Desc					= 83,		// 하트 1개
	Attendance_2Day_Desc					= 84,		// 무브추가+3 1개
	Attendance_3Day_Desc					= 85,		// 하트 2개
	Attendance_4Day_Desc					= 86,		// 줄무늬냥 & 솜방망이 1개
	Attendance_5Day_Desc					= 87,		// 하트 4개
	Attendance_6Day_Desc					= 88,		// 무지개냥 1개
	Attendance_7Day_Desc					= 89,		// 하트 8개

	RewardPopup_Title						= 90,		// 보상알림
	RewardPopup_Receive						= 91,		// 받기

	Roulette_Title							= 92,       // 돌려요! 행운의 룰렛
	Roulette_Free							= 93,       // 무료 돌리기
	Roulette_Ads							= 94,       // 광고보고 돌리기
	Roulette_Disable						= 95,       // 다음 보상까지 // Until the next reward
	NotEnough_HeartCharge					= 96,       // 하트가 하나도 없을때 충전할수 있어요!

	InValid_GoogleLogin_Desc				= 97,		// 구글 플레이 로그인에 실패했습니다. \n플레이 스토어 로그인을 확인해 주세요.
	ReExecute								= 98,		// 앱재실행
	Retry									= 99,		// 다시시도
	InValid_Version							= 100,		// 새로운 클라이언트 버전이 있습니다.\n플레이스토어로 이동 하시겠습니까?
	App_Quit								= 101,		// 종료
	MaxLevelArrival							= 102,      // 대단해요! 모든 레벨을 클리어했어요! \n다음 업데이트를 기다려주세요! 
    ExitGame                                = 103,      // 게임을 종료하시겠습니까?

    TimeBombExplosion                       = 104,      // 폭탄이 터져버렸어요!

    Language                                = 105,      // 언어
    LanguageSelect                          = 106,      // 언어 선택
    Country                                 = 107,      // 한국어
    CountrySelect                           = 108,      // 선택

	StarMap									= 109,		// 별 지도
	StarMap_Entire							= 110,		// 전체
	StarMap_InComplete						= 111,		// 미완성
	StarMap_Desc							= 112,		// 아래 설명

	NewUser_FiveLevelClear_Present			= 113, 
}

public class ExcelData_TextDataInfo
{
	public int 							m_nID			= 0;
	public Dictionary<string,string>	m_TextTable		= new Dictionary<string, string>();		// Key : Country Code, Value : Text

	public ExcelData_TextDataInfo()
	{
	}
}

public class ExcelData_TextData
{
	private Dictionary<int, ExcelData_TextDataInfo> m_DataInfoTable = new Dictionary<int, ExcelData_TextDataInfo>();

	public ExcelData_TextData(int ColumnCount, int RowCount, string[] Datas)
	{
		int index = 0;
		for( int i = 0; i < RowCount; ++i )
		{
			ExcelData_TextDataInfo Info = new ExcelData_TextDataInfo();
			
			Info.m_nID = Helper.ConvertStringToInt(Datas[index++]);

			Info.m_TextTable.Add("kr",Datas[index++]);
			Info.m_TextTable.Add("us", Datas[index++]);
            Info.m_TextTable.Add("jp", Datas[index++]);

            m_DataInfoTable.Add (Info.m_nID, Info);
		}
	}

	public ExcelData_TextDataInfo	FindTextInfo(eTextDataType eType)
	{
		int nID = (int)eType;
		if (m_DataInfoTable.ContainsKey(nID) == true)
		{
			return m_DataInfoTable[nID];
		}

		return null;
	}

	public ExcelData_TextDataInfo FindTextInfo(int nID)
	{
		if (m_DataInfoTable.ContainsKey(nID) == true)
		{
			return m_DataInfoTable[nID];
		}

		return null;
	}
}
