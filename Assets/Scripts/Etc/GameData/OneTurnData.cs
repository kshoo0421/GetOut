using System;

[Serializable]
public struct OneTurnData  // 한 턴에 한 플레이어의 데이터
{
    public PlayerTurnData player1;
    public PlayerTurnData player2;
    public PlayerTurnData player3;
    public PlayerTurnData player4;
}