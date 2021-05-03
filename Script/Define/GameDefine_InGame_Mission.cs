using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMissionType
{
	None			= 0,
	Unit			= 1,
	Bell			= 2,		// 방울 내리기..												// 유닛 타입			// Square Target
	Mouse			= 3,        // 쥐 꺼내기.. 라이언트로피 찾기								// 유닛 타입
	Apple			= 4,        // NotUnit.. 사과 없애기.. 4단계								// 슬롯 타입			// Square Target
	Rock			= 5,        // NotUnit.. 운석에서 보석캐기.. 특수블럭으로만 타격.. 4단계	// 슬롯 타입			// Square Target
	Stripe			= 6,		
	Cross			= 7,
	Grass			= 8,        // 풀 없애기.. 못없애는 무브시 1개 번짐							// 슬롯 타입			// Square Target
	Magician		= 9,
	Bread			= 10,       // 바구니에서 고양이 꺼내기.. 6단계.. 슬롯 4개 차지				// 슬롯 타입			// Square Target
	Jelly			= 11,       // 캔에서 별 알사탕 모으기.. 8단계.. 슬롯 4개 차지				// 슬롯 타입			// Square Target
	Fish			= 12,       // 물고기 없애기.. 애니팡4 > 푸딩 없애기						// 유닛 타입			// Square Target
	Number			= 13,		// 숫자 없애기..												// Unit Shape 타입		// 제외
	Octopus			= 14,       // 꼴뚜기 먹물 없애기.. 애니팡4 > 음표제거하기					// 슬롯 타입			// Square Target
	Knit			= 15,		// 니트 채우기.. 유닛 뒤에 슬롯 타입..							// 슬롯 타입			// 제외
	Can				= 16,       // 오색별.. 애니팡4 > 바람개비 없애기							// 슬롯 타입			// Square Target
	Butterfly		= 17,       // 나비 없애기.. 애니팡4 > 테레비전 없애기						// 슬롯 타입			// Square Target

	Max,

	OctopusInk		= 1001,		
}

public enum eMissionSlotAttr
{
	None						= 0,
	Bell_Goal					= 2,
	Special_Unit_Create			= 3,
}

public partial class GameDefine
{
	static public int ms_nBreadCount = 6;
	static public int ms_nJellyCount = 8;
}