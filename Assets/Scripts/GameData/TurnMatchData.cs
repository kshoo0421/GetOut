using System;

[Serializable]
public struct TurnMatchData  // 대진 배정
{
    OpponentData t1OpponentData;
    OpponentData t2OpponentData;
    OpponentData t3OpponentData;
    OpponentData t4OpponentData;
    OpponentData t5OpponentData;
    OpponentData t6OpponentData;
}

[Serializable]
public struct OpponentData  // 1player 기준 상대
{
    public long opponent;    // 상대가 누구인가
    public bool isProposer; // 제안자 vs 수락자
}