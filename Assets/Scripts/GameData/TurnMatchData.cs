using System;

[Serializable]
public struct TurnMatchData  // 대진 배정
{
    public OpponentData t1OpponentData;
    public OpponentData t2OpponentData;
    public OpponentData t3OpponentData;
    public OpponentData t4OpponentData;
    public OpponentData t5OpponentData;
    public OpponentData t6OpponentData;
}

[Serializable]
public struct OpponentData  // 1player 기준 상대
{
    public long opponent;    // 상대가 누구인가
    public bool isProposer; // 제안자 vs 수락자
}