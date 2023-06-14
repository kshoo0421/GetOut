using System;

[Serializable]
public struct ResultData
{
    // 기본 게임 정보 - 해당 판 고유 숫자
    public long gameIndex;

    // 참가자 정보
    public Players players;

    // 턴별 정보
    public Turns turns;

    // 미션 정보
    public Missions missions;

    // 각 플레이어 총합
    public TotalScores totalScores;

    // 최종 등수
    public FinalResult finalResult;
}

[Serializable]
public struct Players
{
    public PlayerData player1;
    public PlayerData player2;
    public PlayerData player3;
    public PlayerData player4;
}

[Serializable]
public struct PlayerData // 대진 배정
{
    public string playerName;
    public string playerId;
    public long curGold;
}

[Serializable]
public struct Turns
{
    public OneTurnData turn1;
    public OneTurnData turn2;
    public OneTurnData turn3;
    public OneTurnData turn4;
    public OneTurnData turn5;
    public OneTurnData turn6;
}

[Serializable]
public struct OneTurnData  // 한 턴에 한 플레이어의 데이터
{
    public PlayerTurnData player1;
    public PlayerTurnData player2;
    public PlayerTurnData player3;
    public PlayerTurnData player4;
}

[Serializable]
public struct PlayerTurnData  // 한 턴에 한 플레이어의 데이터
{
    public long value; // 제시 금액
    public bool isGet; // 거래 성사 여부
}

[Serializable]
public struct Missions
{
    public PlayerMissionData player1Mission;
    public PlayerMissionData player2Mission;
    public PlayerMissionData player3Mission;
    public PlayerMissionData player4Mission;
}

[Serializable]
public struct PlayerMissionData  // 개인 미션 데이터
{
    public MissionData low; // 난이도 하
    public MissionData mid; // 난이도 중
    public MissionData high;    // 난이도 상
}

[Serializable]
public struct MissionData  // 미션 데이터
{
    public long missionNum; // 미션 번호
    public bool isAchieved; // 성취 여부
}

[Serializable]
public struct TotalScores // 대진 배정
{
    public long player1;
    public long player2;
    public long player3;
    public long player4;
}

[Serializable]
public struct FinalResult // 대진 배정
{
    public long first;
    public long second;
    public long third;
    public long fourth;
}