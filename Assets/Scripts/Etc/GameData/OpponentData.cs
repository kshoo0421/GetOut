using System;

[Serializable]
public struct OpponentData  // 1player 기준 상대
{
    public long opponent;    // 상대가 누구인가
    public bool isProposer; // 제안자 vs 수락자
}