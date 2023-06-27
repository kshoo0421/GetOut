using System;

[Serializable]
public struct TurnMatchData  // ���� ����
{
    public Turn[] turn;
}

[Serializable]
public struct Turn
{
    public MatchRoom Room1;
    public MatchRoom Room2;
}

[Serializable]
public struct MatchRoom
{
    public int proposer;
    public int getter;
}