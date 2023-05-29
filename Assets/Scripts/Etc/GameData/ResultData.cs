using System;

[Serializable]
public struct ResultData
{
    // 기본 게임 정보 - 해당 판 고유 숫자
    public string GAME_KEY;

    // 참가자 정보
    public PlayerData player1;
    public PlayerData player2;
    public PlayerData player3;
    public PlayerData player4;


    // 턴별 정보
    public OneTurnData turn1;
    public OneTurnData turn2;
    public OneTurnData turn3;
    public OneTurnData turn4;
    public OneTurnData turn5;
    public OneTurnData turn6;

    // 미션 정보
    public PlayerMissionData player1Mission;
    public PlayerMissionData player2Mission;
    public PlayerMissionData player3Mission;
    public PlayerMissionData player4Mission;

    // 각 플레이어 총합
    public TotalScores totalScores;

    // 최종 등수
    public FinalResult finalResult;
}