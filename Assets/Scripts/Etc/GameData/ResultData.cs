using System;

[Serializable]
public struct ResultData  
{
    // 기본 게임 정보 - 해당 판 고유 숫자
    public string GAME_KEY;

    // 참가자 정보
    PlayerData player1;
    PlayerData player2;
    PlayerData player3;
    PlayerData player4;


    // 턴별 정보
    OneTurnData turn1;
    OneTurnData turn2;
    OneTurnData turn3;
    OneTurnData turn4;
    OneTurnData turn5;
    OneTurnData turn6;

    // 미션 정보
    PlayerMissionData player1Mission;
    PlayerMissionData player2Mission;
    PlayerMissionData player3Mission;
    PlayerMissionData player4Mission;

    // 각 플레이어 총합
    TotalScores totalScores;

    // 최종 등수
    FinalResult finalResult;
}