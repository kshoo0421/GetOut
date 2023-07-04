using System;

[Serializable]
public struct TurnMatchData  // 대진 배정
{
    public Turn[] turn;
}

[Serializable]
public struct Turn
{
    public long R1S;    // room1 suggestor
    public long R1G;   // room1 getter
    public long R2S;   // room2 suggestor
    public long R2G;   // room2 getter
}