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